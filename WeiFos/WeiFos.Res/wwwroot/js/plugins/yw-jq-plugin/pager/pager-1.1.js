/**
 * jquery 翻页插件
 * @author 叶委
 * @param before           请求前方法
 * @param url              当前页码
 * @param type             请求类型
 * @param data             提交参数
 * @param async            同步还是异步
 * @param currentPage      当前页码
 * @param pageSize         每页大小
 * @param totalRow         总行数
 * @param pageTotal        总页数 
 * @param callback         回调方法 
 */
(function ($) {
    $.extend($.fn, {
        pager: function (options) { 
            var jq_pager;
            this.each(function () {
                //获取当前元素
                var $this = $(this);
                //克隆参数，处理统一对象共享传递，
                //Boolean类型 指示是否深度合并对象，默认为false。如果该值为true，
                //且多个对象的某个同名属性也都是对象，则该"属性对象"的属性也将进行合并
                var opts = $.extend(true, {}, options); 
                //初始控件
                jq_pager = pagerExtend(opts, $(this));
                jq_pager.onInit(opts, $(this));
            });
            return jq_pager;
        }
    });

    /// <summary>
    /// 翻页控件
    /// </summary>
    var pagerExtend = function (options, obj) {
        $.extend(pagerExtend, {
            methods: {
                //初始化
                onInit: function (options, obj) {
                    var $this = this;
                    //如果是数组类型
                    if (App_G.Util.isArray(options.pageSize)) {
                        options.exPageSize = options.pageSize[0];
                    } else {
                        options.exPageSize = options.pageSize;
                    }

                    $this.pagination.currentPage = options.currentPage; 
                    $this.onLoad(options, obj);
                },
                //加载数据
                onLoad: function (options, obj) {
                    var $this = this;

                    var param = {};
                    param.currentPage = $this.pagination.currentPage;
                    param.pageSize = options.exPageSize;

                    for (var attr in options.data) {
                        param[attr] = options.data[attr];
                    }

                    $.ajax({
                        url: options.url,
                        beforeSend: undefined == options.before ? function () { } : options.before,
                        type: (options.type == null || options.type == undefined) ? "GET" : options.type,
                        dataType: "json",
                        async: (options.async == undefined) ? false : options.async,//默认为同步
                        cache: false,
                        data: param
                    }).done(function (data) {
                        $this.dataBind(options, data, obj);
                        options.callback(data);
                    }).fail(function (data) {
                        if (undefined != data && "XMLHttpRequest.LoginOut" == data.responseText) {
                            alert("登录超时，请重新登录");
                            window.top.location.href = App_G.Util.getDomain();
                        } else {
                            alert("处理出现异常!");
                        }
                    });
                },
                //数据绑定 
                dataBind: function (options, data, obj) {
                    var $this = this, pageSize;
                    if (App_G.Util.isArray(options.pageSize)) {
                        pageSize = obj.find("#pagesize_ddl").length == 0 ? options.pageSize[0] : obj.find("#pagesize_ddl").val();
                    } else {
                        pageSize = options.pageSize;
                    }

                    $this.pagination.data = data.data;
                    $this.pagination.totalRow = data.totalRow;
                    $this.pagination.pageTotal = parseInt($this.pagination.totalRow / pageSize) + ($this.pagination.totalRow % pageSize == 0 ? 0 : 1);

                    //暂无数据时候当前页设置为-1
                    var curPage = $this.pagination.currentPage + 1;
                    if (data.data.length == 0) {
                        curPage = 0;
                    }

                    //删除当前元素
                    obj.find("div.pageBar").remove();
                    $(page_temp.pageBar).appendTo(obj);

                    obj.find("#current_page").text(curPage);
                    obj.find("#total_page").text($this.pagination.pageTotal);
                    obj.find("#total_row").text($this.pagination.totalRow);

                    var go_text = obj.find("[name=go_page_text]");
                    go_text.keyup(function () {
                        if (/[^\d]/g.test(go_text.val()) || go_text.val() <= 1) {
                            go_text.val('1');
                        }
                    });
                    go_text.blur(function () {
                        if (/[^\d]/g.test(go_text.val()) || go_text.val() <= 1) {
                            go_text.val('1');
                        }
                    });

                    var pagesize_ddl = obj.find("#pagesize_ddl");
                    //如果是数组类型
                    if (App_G.Util.isArray(options.pageSize) && options.pageSize.length > 0) {
                        var opts = "";
                        //遍历数组
                        $.each(options.pageSize, function (i, o) {
                            opts += "<option value=\"" + o + "\" >" + o + "条</option>";
                        });

                        pagesize_ddl.find("option").remove();
                        $(opts).appendTo(pagesize_ddl);
                    }

                    pagesize_ddl.change(function (i, o) {
                        options.currentPage = 0;
                        options.exPageSize = pagesize_ddl.val();
                        $this.executePage(options.currentPage, options, obj);
                    });
                    pagesize_ddl.val(options.exPageSize);

                    var pageNumber = "";
                    var start10Page = parseInt($this.pagination.currentPage / 10) * 10;
                    for (var i = 0; i < 10; i++) {
                        if (start10Page + i + 1 > $this.pagination.pageTotal) break;
                        if ($this.pagination.currentPage == start10Page + i) {
                            pageNumber += $(page_temp.pageNumber.replace("{0}", start10Page + i + 1)).css({
                                "color": "red",
                                "font-weight": "800",
                                "text-decoration": "underline "
                            })[0].outerHTML;
                        } else {
                            pageNumber += page_temp.pageNumber.replace("{0}", start10Page + i + 1);
                        }
                    }

                    //翻页码
                    $(pageNumber).insertAfter(obj.find("#priv_btn_ten"));

                    $this.clickBind(options, data, obj);

                    $this.is_enable(obj);
                    //单击事件绑定
                }, clickBind: function (options, data, obj) {
                    var $this = this;
                    obj.find("[name=page_btn]").click(function () {

                        switch ($(this).attr("id")) {
                            case "first_btn":
                                if ($this.pagination.currentPage == 0) return;
                                $this.pagination.currentPage = 0;
                                break;
                            case "priv_btn":
                                if ($this.pagination.currentPage == 0) return;
                                $this.pagination.currentPage -= 1;
                                break;
                            case "priv_btn_ten":
                                if ($this.pagination.currentPage <= 0) return;

                                if ($this.pagination.currentPage - 10 < 0) {
                                    $this.pagination.currentPage = 0;
                                } else {
                                    $this.pagination.currentPage -= 10;
                                }
                                break;
                            case "next_btn_ten":
                                if ($this.pagination.currentPage >= $this.pagination.pageTotal - 1) return;

                                if ($this.pagination.currentPage + 10 > $this.pagination.pageTotal) {
                                    $this.pagination.currentPage = $this.pagination.pageTotal;
                                } else {
                                    $this.pagination.currentPage += 10;
                                }
                                break;
                            case "next_btn":
                                if ($this.pagination.currentPage == $this.pagination.pageTotal - 1) return;
                                $this.pagination.currentPage += 1;
                                break;
                            case "last_btn":
                                if ($this.pagination.currentPage == $this.pagination.pageTotal - 1) return;
                                $this.pagination.currentPage = $this.pagination.pageTotal - 1;
                                break;
                            case "go_page_btn":
                                var gopage = $.trim(obj.find("[name=go_page_text]").val()) == "" ? 0 : parseInt(obj.find("[name=go_page_text]").val() - 1);
                                if ($this.pagination.pageTotal == 1 || $this.pagination.currentPage == gopage) return;
                                $this.pagination.currentPage = gopage;
                                break;
                            default:
                                if ($this.pagination.currentPage == parseInt($(this).text() - 1)) return;
                                $this.pagination.currentPage = parseInt($(this).text() - 1);
                                break;
                        }

                        $this.executePage($this.pagination.currentPage, options, obj);
                    });

                    //执行查询页面
                }, executePage: function (currentPage, options, obj) {
                    var $this = this;
                    if (currentPage >= $this.pagination.pageTotal) {
                        $this.pagination.currentPage = ($this.pagination.pageTotal - 1) < 0 ? 0 : ($this.pagination.pageTotal - 1);
                    }
                    if (currentPage < 0) {
                        $this.pagination.currentPage = 0;
                    }
                    $this.onLoad(options, obj);
                    //查询事件
                }, execute: function (data, url, currentPage) {
                    var $this = this;
                    $this.options.data = data;
                    if (undefined != url) { options.url = url; }

                    if (undefined == currentPage) $this.executePage($this.pagination.currentPage, $this.options, obj);
                    else $this.executePage(currentPage, $this.options, obj);

                }, is_enable: function (obj) {
                    var $this = this;
                    //一页
                    if ($this.pagination.pageTotal == 1 || $this.pagination.pageTotal == 0) {
                        obj.find("#first_btn").addClass("disabled");
                        obj.find("#priv_btn").addClass("disabled");
                        obj.find("#priv_btn_ten").addClass("disabled");

                        obj.find("#next_btn_ten").addClass("disabled");
                        obj.find("#next_btn").addClass("disabled");
                        obj.find("#page_last").addClass("disabled");

                        obj.find("[name=page_btn]").addClass("disabled");
                    }

                    //当前要页为零
                    if ($this.pagination.currentPage == 0 || $this.pagination.pageTotal == 0) {
                        obj.find("#first_btn").addClass("disabled");
                        obj.find("#priv_btn").addClass("disabled");
                        obj.find("#priv_btn_ten").addClass("disabled");
                    }

                    //当前要等于总页数
                    if ($this.pagination.currentPage == $this.pagination.pageTotal - 1 || $this.pagination.pageTotal == 0) {
                        obj.find("#next_btn_ten").addClass("disabled");
                        obj.find("#next_btn").addClass("disabled");
                        obj.find("#last_btn").addClass("disabled");
                    }
                },
                //翻页信息
                getPager: function () {
                    var $this = this;
                    return $this.pagination;
                },
                options: options,
                pagination: {
                    data: null,
                    currentPage: 0,
                    pageSize: [25, 50, 100],
                    totalRow: 0,
                    pageTotal: 0
                }
            }
        });
        return pagerExtend.methods;
    }


    /// <summary>
    /// 变量
    /// </summary>
    var page_temp = {
        pageBar: '<div class="pageBar" >\
        <a name=\"page_btn\" id="first_btn" title="首页" class="first page_first"  >首页</a>\
        <a name=\"page_btn\" id="priv_btn" title="上一页" >上一页</a>\
        <a name=\"page_btn\" id="priv_btn_ten" title="上十页" >&lt;&lt;</a>\
        <a name=\"page_btn\" id="next_btn_ten" title="下十页" >&gt;&gt;</a>\
        <a name=\"page_btn\" id="next_btn" href="javascript:;" title="下一页"  >下一页</a>\
        <a name=\"page_btn\" id="last_btn" href="javascript:;" title="尾页" class="page_last" >尾页</a>\
        <span class="info_label">当前 <span id="current_page"></span>/<span id="total_page"></span> 页 总共 <span id="total_row"></span> 条</span>\
        <input name="go_page_text" type="text" style="width:30px;"  maxlength="5" name="goPageText">\
        <input name=\"page_btn\" id="go_page_btn" type="submit" class="btn btn-small btn-primary btn-go" value="Go" >\
        <select id="pagesize_ddl" style="width:70px;"><option value="10">10条</option><option value="50">50条</option>\
        </select>\
        </div>',
        pageNumber: "<a href=\"javascript:;\" name=\"page_btn\" class=\"current\" >{0}</a>"
    }

})(jQuery)







