
/// <summary>
/// rtsearch v1.1
/// @author             叶委  
/// @update date        2017-08-22
/// @cselector          上下文选择器
/// @selector           当前元素选择器
/// @event              触发事件
/// @callback           回传事件
/// @enter              回车事件
/// </summary>
yw.rtsearch = function (options) {
    //克隆参数，处理统一对象共享传递
    var opts = $.extend(true, {}, options);
    //是否触发
    opts.trigger = true;

    var rtsearch = function (opts) {
        $.extend(rtsearch, {
            methods: {
                onInit: function () {
                    var $this = this, context, $ele;
                    //获取上下文
                    context = (undefined == opts.cselector) ? $(document) : $(opts.cselector);

                    $ele = context.find(opts.selector);
                    $ele = (undefined == $ele || $ele.length == 0) ? null : $(opts.selector);
                    if ($ele == null) {
                        alert(opts.selector + "当前元素在页面不存在");
                        return;
                    }

                    //设置控件ID
                    opts.controlId = new Date().getTime();

                    //绑定事件
                    context.on(undefined == opts.event ? "keyup" : options.event, opts.selector, function (e) {

                        switch (e.which) {
                            //上
                            case 38:
                                $this.selectli(false, this);
                                break;
                            //下
                            case 40:
                                $this.selectli(true, this);
                                break;
                            //回车 
                            case 13:
                                var li = $("div.search_suggest li.hover");
                                if (li.length == 0) {
                                    li = null;
                                }
                                opts.enter($this, this, li);
                                $this.reset();
                                break;
                            default:
                                if (rtsearch.trigger) {
                                    rtsearch.trigger = false
                                    setTimeout(rtsearch.trigger = true, 500)
                                    opts.callback($this, this, e);
                                }
                                break;
                        }
                    });

                    //绑定事件 
                    context.on("blur", opts.selector, function (e) {
                        $("#" + opts.controlId).fadeOut();
                    });

                    //绑定事件 
                    context.on("click", opts.selector, function (e) {
                        $this.databind(yw.rtsearch.data, $(this));
                    });

                    //点击事件
                    context.on("click", "div.search_suggest li", function (e) {

                        var li = $(this);
                        var input = $(opts.selector);

                        li.addClass("hover").siblings().removeClass("hover");

                        input.val(li.text());
                        $(input).focus();

                        //opts.callback($this, input);
                        $this.reset();
                        opts.enter($this, input, li);

                    });

                    //鼠标移动事件
                    context.on("mousemove", "div.search_suggest li", function (e) {
                        $(this).addClass("hover").siblings().removeClass("hover");
                    });

                },
                //数据绑定
                databind: function (data, ele) {
                    var $this = this;
                    $this.reset();

                    yw.rtsearch.data = data;
                    if (undefined != data && data.length > 0) {
                        //当前元素位置
                        var xy = $(ele).offset();
                        var lis = "";
                        $.each(data, function (i, o) {
                            //如果是对象
                            if ((typeof o == 'object') && o.constructor == Object) {
                                var attr_str = "";
                                for (var attr in o) {
                                    attr_str += "rt-" + attr + " = '" + o[attr] + "'";
                                }
                                lis += "<li " + attr_str + " >" + o["name"] + "</li>";
                            } else {
                                lis += "<li>" + o + "</li>";
                            }
                        });

                        $(yw.rtsearch.result_tmp).attr("id", opts.controlId).appendTo("body");
                        $(lis).appendTo($("#" + opts.controlId).find("ul"));
                        $("#" + opts.controlId).css({ left: xy.left, top: xy.top + $(ele).height() + 11, width: $(ele).outerWidth(), position: "absolute" }).show();//relative absolute position
                    }
                },
                //数据绑定
                selectli: function (isdown, input) {
                    //是否存在结果集
                    var lis = $("#" + opts.controlId).find("li");
                    if (lis != null) {
                        var h_li = $("#" + opts.controlId).find("li.hover");

                        if (isdown) {
                            if (h_li.length == 0) {
                                h_li = lis[0];
                            } else {
                                h_li = $(h_li).next();
                            }
                        } else {
                            if (h_li.length == 0) {
                                h_li = lis[lis.length - 1];
                            } else {
                                h_li = $(h_li).prev();
                            }
                        }
                        if (h_li != null) {
                            $(input).val($(h_li).text());
                            $(h_li).addClass("hover").siblings().removeClass("hover");
                        }
                        $(input).focus();
                    }
                },
                //重置
                reset: function () {
                    $("#" + opts.controlId).remove();
                }
            },//是否触发
            trigger: true
        });

        return rtsearch.methods;
    };

    var search = rtsearch(opts);
    search.onInit(opts);

    return search;
};


//是否触发
yw.rtsearch.trigger = true;

//查询临时存放数据
yw.rtsearch.data = [];

/// <summary>
/// 搜索结果模板
/// </summary>
yw.rtsearch.result_tmp = '<div class="search_suggest" style="height:314px;overflow-y:auto;" ><ul></ul></div>';




