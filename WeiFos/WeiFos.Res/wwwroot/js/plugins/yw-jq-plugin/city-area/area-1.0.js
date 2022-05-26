
/// <summary>
/// area v1.0
/// @author             叶委  
/// @create date        2015-12-08 
/// @cselector          上下文选择器
/// @selector           当前元素选择器
/// @setclass           当前元素选择器
/// @attrtag            自定义标签名
/// @attr               自定义标签值
/// </summary>
yw.cityarea = function (options) {

    //克隆参数，处理统一对象共享传递
    var opts = $.extend(true, {}, options);

    var cityarea = function (opts) {

        $.extend(cityarea, {
            methods: {
                onInit: function (opts) {
                    var $this = this, context, $ele;
                    //获取上下文
                    context = (undefined == opts.cselector) ? $(document) : $(opts.cselector);

                    $ele = context.find(opts.selector);
                    $ele = (undefined == $ele || $ele.length == 0) ? null : $(opts.selector);
                    if ($ele == null) {
                        alert(opts.selector + "当前元素在页面不存在"); return;
                    }

                    //控件ID
                    opts.controlId = new Date().getTime();
                    //弹出框控件ID
                    opts.areaId = opts.controlId + "_area";
                    //弹出框隐藏域ID
                    opts.areaTmpId = opts.controlId + "_area_tmp";
                    //当前选择的 是 省 市 区
                    opts.focusId = -1;

                    $(yw.cityarea.html_tmp).attr("id", opts.controlId).appendTo($ele);
                    $(yw.cityarea.div).attr("id", opts.areaId).appendTo("body");
                    $("<div></div>").attr("id", opts.areaTmpId).appendTo("body");
                    $("#" + opts.areaTmpId).hide();

                    var inputs = $("#" + opts.controlId).find("input");

                    //是否定义标签
                    if (opts.attrtag == undefined) { opts.attrtag = "data-val"; }

                    if (App_G.Util.isObject(opts.attr)) {
                        var i = 0;
                        for (var attr in opts.attr) {
                            $(inputs[i]).attr(opts.attrtag, attr);
                            $(inputs[i]).val(opts.attr[attr]);
                            if (i > 2) { break; }
                            i++;
                        }
                    } else {
                        $(inputs[0]).attr("data-val", "Province");
                        $(inputs[1]).attr("data-val", "City");
                        $(inputs[2]).attr("data-val", "Area");
                    }

                    if (undefined != opts.setclass) {
                        if (undefined != opts.setclass.p) {
                            $(inputs[0]).attr("style", "");
                            $(inputs[0]).attr("class", opts.setclass.p);
                        }
                        if (undefined != opts.setclass.p) {
                            $(inputs[1]).attr("style", "");
                            $(inputs[1]).attr("class", opts.setclass.c);
                        }
                        if (undefined != opts.setclass.p) {
                            $(inputs[2]).attr("style", "");
                            $(inputs[2]).attr("class", opts.setclass.a);
                        }
                    }

                    //加载数据
                    $this.loaddata();
                    $this.databind(opts);
                },
                //数据绑定
                databind: function (opts) {
                    var $this = this;
                    //元素位置
                    var xy;
                    //省
                    var province = $("#" + opts.controlId).find("input:eq(0)");
                    //市
                    var city = $("#" + opts.controlId).find("input:eq(1)");
                    //区
                    var area = $("#" + opts.controlId).find("input:eq(2)");

                    //通过上下文获取
                    $("#" + opts.controlId).on("focus", "input", function (e) {
                        //当前元素
                        var input = $(this);

                        //获取当前坐标
                        xy = input.offset();
                        //移除地址元素集合
                        $("#" + opts.areaId).find("a").remove();
                        //移除临时元素集合
                        $("#" + opts.areaTmpId).find("a").remove();
                        //页面关闭按钮
                        $("#" + opts.areaId).append("<a class='btn_close' href='javascript:;'>X</a>");
                        //区分省份城市区域属性
                        switch ($(this).index()) {
                            //省
                            case 0:
                                opts.focusId = 0;
                                //获取区县列表
                                var provincedata = $(yw.cityarea.xmldata).find("address > province");
                                //省份标签集合
                                provincedata.each(function (i, o) {
                                    var ahtml = "<a>" + provincedata.eq(i).attr("name") + "</a>";
                                    $("#" + opts.areaId).append(ahtml);
                                    $("#" + opts.areaTmpId).append(ahtml);
                                });
                                break
                                //市
                            case 1:
                                opts.focusId = 1;
                                //获取城市列表数据
                                var citydata = $(yw.cityarea.xmldata).find("address > province[name='" + province.val() + "'] > city");
                                citydata.each(function (i, o) {
                                    var ahtml = "<a>" + citydata.eq(i).attr("name") + "</a>";
                                    $("#" + opts.areaId).append(ahtml);
                                    $("#" + opts.areaTmpId).append(ahtml);
                                });
                                break
                                //区
                            case 2:
                                opts.focusId = 2;
                                //获取区县列表
                                var areadata = $(yw.cityarea.xmldata).find("address > province[name='" + province.val() + "'] >  city[name='" + city.val() + "'] > country");
                                areadata.each(function (i, o) {
                                    var ahtml = "<a>" + areadata.eq(i).attr("name") + "</a>";
                                    $("#" + opts.areaId).append("<a>" + areadata.eq(i).attr("name") + "</a>");
                                    $("#" + opts.areaTmpId).append(ahtml);
                                });
                                break
                            default:
                                break;
                        }
                        if ($("#" + opts.areaId).find("a").not("a.btn_close").length > 1) {
                            $("#" + opts.areaId).css({ left: xy.left, top: xy.top + $(this).height() + 11 }).slideDown();
                        }
                    });

                    //通过上下文获取
                    $("#" + opts.controlId).on("keyup", "input", function (e) {

                        //当前元素
                        var input = $(this);

                        //是否存在结果集
                        var alist = $("#" + opts.areaId).find("a").not("a.btn_close");
                        var a_h = $("#" + opts.areaId).find("a.hover");

                        switch (e.which) {
                            //左
                            case 37:
                                if (a_h.length == 0) {
                                    a_h = alist[alist.length - 1];
                                } else {
                                    a_h = $(a_h).prev();
                                }
                                break
                                //上
                            case 38:
                                if (a_h.length == 0) {
                                    a_h = alist[alist.length - 1];
                                } else {
                                    var index = $this.getxeles(a_h, alist, opts, false);
                                    a_h = alist.eq(index);
                                }
                                break
                                //右
                            case 39:
                                if (a_h.length == 0) {
                                    a_h = alist[0];
                                } else {
                                    a_h = $(a_h).next();
                                }
                                break
                                //下
                            case 40:
                                if (a_h.length == 0) {
                                    a_h = alist[0];
                                } else {
                                    var index = $this.getxeles(a_h, alist, opts, true);
                                    a_h = alist.eq(index);
                                }
                                break
                                //回车
                            case 13:
                                $("#" + opts.areaId).hide();
                                input.val($(a_h).text());
                                input.next().focus();
                                if (opts.focusId == 0) {
                                    city.val("");
                                    area.val("");
                                } else if (opts.focusId == 1) {
                                    area.val("");
                                }
                                break
                            default:
                                //$.each($(yw.cityarea.xmldata).xpath("//address/province/@name").find("a:contains('湖')"), function (i, o) {});
                                var alist = $("#" + opts.areaTmpId).find("a:contains(" + $.trim($(this).val()) + ")");
                                $("#" + opts.areaId).find("a").not("a.btn_close").remove();
                                //必须克隆处理，否则元素会被删除再附加
                                if (alist.length == 0) {
                                    $("#" + opts.areaId).append($("#" + opts.areaTmpId).find("a").clone());
                                } else {
                                    $("#" + opts.areaId).append(alist.clone());
                                }
                                if ($("#" + opts.areaId).find("a").not("a.btn_close").length > 1) {
                                    $("#" + opts.areaId).css({ left: xy.left, top: xy.top + $(this).height() + 11 }).slideDown();
                                }
                                break;
                        }

                        if ($(a_h).length > 0) {
                            //$(input).val($(a_h).text());
                            $(a_h).addClass("hover").siblings().removeClass("hover");
                        }
                    });

                    //弹出框点击事件
                    $("#" + opts.areaId).on("click", "a", function () {
                        if ($(this).text() == "X") {
                            $("#" + opts.areaId).hide();
                        } else {
                            $("#" + opts.areaId).hide();
                            if (opts.focusId == 0) {
                                province.val($(this).text());
                                city.focus();
                                city.val("");
                                area.val("");
                            } else if (opts.focusId == 1) {
                                city.val($(this).text());
                                area.focus();
                                area.val("");
                            } else if (opts.focusId == 2) {
                                area.val($(this).text());
                                area.blur();
                            }
                        }
                    });

                    //移开事件
                    $("#" + opts.controlId).on("blur", "input", function () {
                        $("#" + opts.areaId).fadeOut();
                    });

                },
                //加载数据
                loaddata: function () {
                    //是否加载xml数据
                    if (yw.cityarea.xmldata == null) {
                        $.ajax({
                            url: App_G.Util.getDomain() + "/Config/" + "area.xml",
                            type: "GET",
                            async: false,
                            dataType: "xml",
                            error: function (xdata) { alert("加载xml数据有误！"); },
                            success: function (xdata) {
                                yw.cityarea.xmldata = xdata;
                            }
                        });
                    }
                },
                //获取相同x坐标元素
                getxeles: function (input, alist, opts, isdown) {

                    //当前元素
                    var xy = input.offset();
                    var arr = [], prevarr = [];
                    var index, sindex, eindex, cindex, previndex;
                    //当前元素位置索引
                    index = $(alist).index(input);

                    //查询同一行元素
                    $.each(alist, function (i, o) {
                        //当前行元素
                        if ($(o).offset().top == xy.top) {
                            arr.push(o);
                        }

                        //上一行元素
                        if ($(o).offset().top == (xy.top - input.height() - 6)) {
                            prevarr.push(o);
                        }
                    });

                    if (arr.length > 0) {
                        sindex = $(alist).index(arr[0]);
                        eindex = $(alist).index(arr[arr.length - 1]);
                    }

                    //当前元素在该行索引
                    cindex = $(arr).index(input);

                    //下
                    if (isdown) {
                        index = eindex + cindex + 1;
                        if (index > alist.length - 1) {
                            index = 0;
                        }
                        //上
                    } else {
                        if (prevarr.length > 0) {
                            index = alist.index(prevarr[0]) + cindex;
                        }
                        if (index < 0) {
                            index = alist.length - 1;
                        }
                    }
                    return index;
                }
            }
        });

        return cityarea.methods;
    };

    var carea = cityarea(opts);
    carea.onInit(opts);

};


/// <summary>
/// 是否存在XML数据
/// </summary>
yw.cityarea.xmldata = null;

/// <summary>
/// 弹出框html模板
/// </summary>
yw.cityarea.div = "<div class='provincecity' style='z-index:999999999;' ></div>";

/// <summary>
/// 页面元素模板
/// </summary>
yw.cityarea.html_tmp = '<span><input type="text" maxlength="10" style="width:100px;"  class="form-control form-control-sm" readonly="readonly"  placeholder="选择省份"  />\
                          <input type="text" maxlength="10" style="width:100px;"   class="form-control form-control-sm" readonly="readonly" placeholder="选择市"  />\
                          <input type="text"  maxlength="10" style="width:100px;"  class="form-control form-control-sm"  readonly="readonly" placeholder="选择区"  /></span>';





