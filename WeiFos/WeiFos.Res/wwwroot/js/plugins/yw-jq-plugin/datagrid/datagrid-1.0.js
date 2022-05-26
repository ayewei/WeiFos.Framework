/**
 * datagrid 表格/翻页插件
 * @author 叶委
 * @create date    2018-10-25
 * @param {Object}                options                 匿名参数对象
 * @param {Object}                options.tableId         表格ID，不给会自动生成
 * @param {Object}.{String}       options.url             请求地址
 * @param {Object}.{String}       options.isUnInitLoad    初始化是否加载数据
 * @param {Object}.{String}       options.trclick         行单击
 * @param {Object}.{Object}       options.before          请求前方法
 * @param {Object}.{Object}       options.enter           回车事件
 * @param {Object}.{Object}       options.type            请求类型
 * @param {Object}.{Object}       options.data            提交参数
 * @param {Object}.{Object}       options.async           同步还是异步
 * @param {Object}.{Object}       options.pager.index     当前页码
 * @param {Object}.{Object}       options.pager.pageSize  当前页码
 * @param {Object}.{Object}       options.pager.totalRow  总行数
 * @param {Object}.{Object}       options.pager.totalPage 总页数
 * @param {Object}.{Object}       options.callback        回调方法
 * @param {Object}.{Object}       options.completed       绑定完成事件方法
 * @param {Object}.{Object}       options.thead           表格列头
 * @param {Object}.{Object}       options.fixedColumn     固定列位置
 * @param {Object}.{Object}       options.column          表格列头对象
 * @param {Object}.{Object}       options.column.text     列显示标题
 * @param {Object}.{Object}       options.column.style    列样式
 * @param {Object}.{String}       options.template_id     模板ID
 * @param {Object}.{String}       options.floatThead      表头浮动
 */
(function ($) {
    $.extend($.fn, {
        datagrid: function (options) {
            var jq_datagrid;
            this.each(function () {
                //获取当前元素
                var $this = $(this);
                //克隆参数，处理统一对象共享传递，
                //Boolean类型 指示是否深度合并对象，默认为false。如果该值为true，
                //且多个对象的某个同名属性也都是对象，则该"属性对象"的属性也将进行合并
                var opts = $.extend(true, {}, options);
                //初始控件
                jq_datagrid = datagridExtend(opts, $this);
                jq_datagrid.onInit(opts, $(this));
            });
            return jq_datagrid;
        }
    });

    /// <summary>
    /// datagrid翻页控件
    /// </summary>
    var datagridExtend = function (options, obj) {
        $.extend(datagridExtend, {
            methods: {
                //初始化
                onInit: function (options, obj) {
                    var $this = this;
                    //设置ID
                    if (undefined == options.tableId) {
                        options.tableId = new Date().getTime();
                    }

                    //设置表格ID
                    $(grid_tmp.table).attr("id", options.tableId).appendTo(obj);

                    var tr_head = "";
                    var tr_tot = "";
                    //列头
                    if (options.thead) {
                        tr_head = options.thead;
                        //添加列头
                        obj.find("#" + options.tableId).find("thead").html(tr_head);
                    } else {
                        //处理表格列
                        $.each(options.column, function (i, o) {
                            tr_head += "<th style='" + (undefined == o.style ? "" : o.style) + "; overflow:hidden;'" + ">";
                            if (o.html != undefined) {
                                if (!o.order_by) tr_head += o.html;
                                else tr_head += grid_tmp.orderBy.replace('-', o.html).replace('data-filed=""', 'data-filed="' + o.order_by.filed + '"');
                            } else {
                                if (!o.order_by) tr_head += o.text;
                                else tr_head += grid_tmp.orderBy.replace('-', o.text).replace('data-filed=""', 'data-filed="' + o.order_by.filed + '"');
                            }
                            tr_head += "</th>";
                        });

                        //添加列头
                        $(tr_head).appendTo(obj.find("#" + options.tableId).find("thead > tr"));
                        obj.find("#" + options.tableId).find("i[name=orderby]").css({ "cursor": "pointer" });
                    }

                    //是否创建合计行
                    var create_tot_row = false;
                    $.each(options.column, function (i, o) {
                        if (o.tot) {
                            create_tot_row = true;
                            return false;
                        }
                    });

                    //合计行
                    if (create_tot_row) {
                        //处理合计行列
                        $.each(options.column, function (i, o) {
                            tr_tot += "<td style='" + (undefined == o.style ? "" : o.style) + "; overflow:hidden;'" + ">";
                            if (o.tot) tr_tot += grid_tmp.totTd.replace('data-filed=""', 'data-filed="' + o.tot.filed + '"');
                            tr_tot += "</td>";
                        });
                        //生成合计模板
                        options.tot_obj = $("<tr></tr>").html(tr_tot);
                    }

                    //处理翻页翻页
                    if (options.pager != undefined) {
                        //如果是数组类型
                        if (App_G.Util.isArray(options.pager.pageSize)) {
                            options.exPageSize = options.pager.pageSize[0];
                        } else {
                            options.exPageSize = options.pager.pageSize;
                        }

                        $this.pagination.index = options.pager.index;
                    }

                    $this.onLoad(options, obj);
                },
                //加载数据
                onLoad: function (options, obj) {
                    var $this = this;

                    var param = {};
                    //翻页参数
                    if (options.pager != undefined) {
                        param.pageIndex = $this.pagination.index;
                        param.pageSize = options.exPageSize;
                    }

                    //数据参数
                    for (var attr in options.data) {
                        param[attr] = options.data[attr];
                    }

                    //如果有排序方式
                    var i_order_by = $("#" + options.tableId + ">thead" + ",#_" + options.tableId + ">thead").find("i[name=orderby][data-asc]");
                    if (i_order_by.length) {
                        param["asc_filed"] = $(i_order_by[0]).attr("data-filed");
                        param["asc_value"] = $(i_order_by[0]).attr("data-asc");
                    }

                    //查询参数
                    $this.pagination.searchData = param;

                    if (!options.isUnInitLoad) {
                        var layer_box = layer.load(1, { shade: [0.6, '#fff'] });
                        //这里50mm延迟加载，防止弹出框无效果
                        setTimeout(function () {
                            $.ajax({
                                url: options.url,
                                beforeSend: undefined == options.before ? function () { } : options.before,
                                type: (options.type == null || options.type == undefined) ? "GET" : options.type,
                                dataType: "json",
                                async: (options.async == undefined) ? false : options.async,//async：true（异步）或 false（同步）
                                cache: false,
                                data: param
                            }).done(function (result) {
                                layer.close(layer_box);
                                if (options.callback != undefined) options.callback(result);
                                $this.dataBind(options, result, obj);
                            }).fail(function (data) {
                                layer.close(layer_box);
                                if (undefined != data && "XMLHttpRequest.LoginOut" == data.responseText) {
                                    alert("登录超时，请重新登录");
                                    window.top.location.href = App_G.Util.getDomain();
                                } else {
                                    alert("处理出现异常!");
                                }
                            });
                        }, 50);

                    } else {
                        options.isUnInitLoad = false;
                        $(grid_tmp.noData).insertAfter(obj.find("#" + options.tableId));
                    }

                },
                //数据绑定 
                dataBind: function (options, result, obj) {
                    var $this = this;

                    //绑定数据到模板
                    var data = null;
                    if (options.pager != undefined) {
                        data = result.Data.pageData
                    } else {
                        data = result.Data
                    }

                    //数据源
                    $this.pagination.data = data;

                    //绑定数据
                    $(obj.find("#" + options.tableId).find("tbody")).html(template(options.template_id == undefined ? "template" : options.template_id, { data: data }));
                    $this.eventBindTable();

                    //是否绑定翻页
                    if (options.pager != undefined) {
                        $this.bindPage(options, result, obj);
                    }

                    //处理暂无数据
                    if (data == null || !data.length) {
                        var has_data = false;
                        //判断子对象是否存在值
                        if (data.length == undefined) {
                            $.each(data, function (i, o) {
                                if (data[i] != null && data[i].length) {
                                    has_data = true;
                                    return;
                                }
                            })
                        }

                        if (!has_data) {
                            obj.find("div.pageBar").hide();
                            if (!obj.find("[ele-id=nodata]").length) {
                                $(grid_tmp.noData).insertAfter(obj.find("#" + options.tableId));
                            }
                        } else {
                            obj.find("[ele-id=nodata]").remove();
                        }
                    } else {
                        obj.find("div.pageBar").show();
                        obj.find("[ele-id=nodata]").remove();
                    }

                    if (options.floatThead) {
                        obj.css({ "max-height": options.floatThead.height + "px", "overflow-x": "auto" });
                        obj.scroll(function () {
                            var scrollTop = this.scrollTop;
                            obj.find("thead").attr("style", "transform: translateY(" + scrollTop + "px);background-color:#ffffff;")
                        });
                    }

                    //执行完成事件
                    if (options.completed != undefined) {
                        options.completed(result);
                    }

                    //左侧列固定
                    if (options.fixedColumn && options.fixedColumn.index) {
                        //当前数据table
                        var table = obj.find("#" + options.tableId);

                        //删除固定的列头
                        obj.parent().find("div.firstcol").remove();
                        //处理再次查询时候隐藏会导致宽、高丢失
                        obj.find("table").find("thead tr:eq(0)>th:lt(" + parseInt(options.fixedColumn.index + 1) + ")").show();
                        obj.find("table").find("tbody tr").each(function (i, o) { $(o).find("td:lt(" + parseInt(options.fixedColumn.index + 1) + ")").show(); });
                        obj.find("table").find("tfoot tr").each(function (i, o) { $(o).find("td:lt(" + parseInt(options.fixedColumn.index + 1) + ")").show(); });

                        //固定的列集合宽高
                        var width = 0, head_height = 0;
                        var thead = table.find("thead>tr:eq(0)>th:lt(" + parseInt(options.fixedColumn.index + 1) + ")");
                        var tb = obj.find("table").clone().css('width', '').attr("id", "_" + options.tableId).hide()[0].outerHTML;
                        //添加second样式
                        obj.addClass("second");
                        //将first div添加到页面
                        obj.before(grid_tmp.firstTb);
                        $(tb).appendTo(obj.parent().find("div.firstcol"));

                        //如果定义了方法
                        if (options.fixedColumn.func) {
                            var w = options.fixedColumn.func(obj.parent().find("div.second"));
                            if (w) table.width(w);
                        }

                        //设置固定table的宽和头部的高
                        head_height = thead.outerHeight();
                        thead.each(function (i, o) { width += $(o).outerWidth(); });

                        //固定的table
                        var f_table = $("#_" + options.tableId);
                        //保留固定的列
                        f_table.find("tr>th:gt(" + options.fixedColumn.index + ")").remove();
                        //删除body里面多余的列
                        f_table.find("tbody>tr").each(function (i, o) { $(o).find("td:gt(" + options.fixedColumn.index + ")").remove(); });
                        //删除tfoot里面多余的列
                        f_table.find("tfoot>tr").each(function (i, o) { $(o).find("td:gt(" + options.fixedColumn.index + ")").remove(); });

                        //显示列头不然会丢失宽度和高度
                        f_table.find("thead tr>th").show();

                        //当前元素位置 
                        f_table.css({ background: "#fff", width: width }).show().find("thead").height(head_height - 1);
                        //重新设置翻页插件位置
                        obj.parent().find("div.second").css({ "padding-left": width, "min-height": "500px" }).find("div.pageBar").css({ "position": "absolute", "left": 15 });
                        table.find("thead tr:eq(0)>th:lt(" + parseInt(options.fixedColumn.index + 1) + ")").hide();
                        table.find("tbody tr").each(function (i, o) { $(o).find("td:lt(" + parseInt(options.fixedColumn.index + 1) + ")").hide(); });
                        table.find("tfoot tr").each(function (i, o) { $(o).find("td:lt(" + parseInt(options.fixedColumn.index + 1) + ")").hide(); });
                        table.find("tfoot tr>td:last").html("&nbsp;");
                    }

                    //回车事件
                    if (options.enter != undefined) {
                        $("body").keyup(function (e) {
                            //选中索引 
                            var index = obj.find("tr.selected").index();
                            switch (e.which) {
                                //上
                                case 38:
                                    if (index == -1) obj.find("tbody>tr:eq(0)").addClass("selected").siblings().removeClass("selected");
                                    else {
                                        if (index > 0) {
                                            obj.find("tbody>tr:eq(" + (index - 1).toString() + ")").addClass("selected").siblings().removeClass("selected");
                                        }
                                    }
                                    break;
                                //下
                                case 40:
                                    if (index == -1) obj.find("tbody>tr:eq(0)").addClass("selected").siblings().removeClass("selected");
                                    else {
                                        if (index < obj.find("tbody>tr").length) {
                                            obj.find("tbody>tr:eq(" + (index + 1).toString() + ")").addClass("selected").siblings().removeClass("selected");
                                        }
                                    }
                                    break;
                                //回车
                                case 13:
                                    //selected
                                    options.enter(e, index);
                                    break
                                default:
                                    break;
                            }
                        });
                    }

                    //是否绑定排序
                    $this.bindSort(options, result, obj);

                    //是否绑定合计行
                    $this.bindTot(options, result, obj);
                },
                //排序绑定
                bindSort: function (options, result, obj) {
                    var $this = this;
                    //绑定单击事件，此处有浮动列情况，所以要考虑两处页面交互状态 data-filed
                    $("#" + options.tableId + ">thead" + ",#_" + options.tableId + ">thead").off("click").on("click", "i[name=orderby]", function () {
                        var cur_btn = $(this);
                        var order_by = cur_btn.attr("data-asc");
                        //当前排序btn
                        cur_btn.parents("tr").find("i[name=orderby]").removeAttr("data-asc").text("--").removeClass("fa").removeClass("fa-sort-amount-desc").removeClass("fa-sort-amount-asc").removeClass("mt3");
                        if (order_by == 0) {
                            cur_btn.attr("data-asc", 1).text("").removeClass("fr fa fa-sort-amount-asc mt3").addClass("fr fa fa-sort-amount-desc mt3");
                        } else if (order_by == 1) {
                            cur_btn.attr("data-asc", 0).text("").removeClass("fr fa fa-sort-amount-desc mt3").addClass("fr fa fa-sort-amount-asc mt3");
                        } else {
                            cur_btn.attr("data-asc", 0).text("").removeClass("fr fa fa-sort-amount-desc mt3").addClass("fr fa fa-sort-amount-asc mt3");
                        }

                        var to_up_btn = null;
                        //当前元素对应的最近的父级div
                        var closest_div = cur_btn.closest("div");
                        //当前排序发生在clone出来的列头上
                        if (closest_div.hasClass("firstcol")) {
                            to_up_btn = closest_div.next().find("i[data-filed=" + cur_btn.attr("data-filed") + "]");
                        } else if (closest_div.hasClass("second")) {
                            to_up_btn = closest_div.prev().find("i[data-filed=" + cur_btn.attr("data-filed") + "]");
                        }

                        //联动将要更新的
                        if (to_up_btn != null && to_up_btn.length) {
                            to_up_btn.parents("tr").find("i[name=orderby]").removeAttr("data-asc").text("--").removeClass("fa").removeClass("fa-sort-amount-desc").removeClass("fa-sort-amount-asc").removeClass("mt3");
                            if (order_by == 0) {
                                to_up_btn.attr("data-asc", 1).text("").removeClass("fr fa fa-sort-amount-asc mt3").addClass("fr fa fa-sort-amount-desc mt3");
                            } else if (order_by == 1) {
                                to_up_btn.attr("data-asc", 0).text("").removeClass("fr fa fa-sort-amount-desc mt3").addClass("fr fa fa-sort-amount-asc mt3");
                            } else {
                                to_up_btn.attr("data-asc", 0).text("").removeClass("fr fa fa-sort-amount-desc mt3").addClass("fr fa fa-sort-amount-asc mt3");
                            }
                        }

                        //执行查询
                        $this.executePage(options.pager.index, options, obj);
                    });
                },
                //合计行绑定
                bindTot: function (options, result, obj) {
                    var $this = this;
                    //合计行填充数据
                    if (options.tot_obj != undefined && $this.pagination.totalData != undefined) {
                        //遍历列
                        $.each($this.pagination.totalData[0], function (i, o) {

                            options.tot_obj.find("[data-filed='" + i + "']").text(o);
                        });
                        //添加到表格
                        $(options.tot_obj.html()).insertAfter(obj.find("#" + options.tableId + " tr:last"));
                    }

                },
                //数据绑定 
                bindPage: function (options, result, obj) {
                    var $this = this, pageSize;
                    if (App_G.Util.isArray(options.pager.pageSize)) {
                        pageSize = obj.find("[name=select_size]").length == 0 ? options.pager.pageSize[0] : obj.find("[name=select_size]").val();
                    } else {
                        pageSize = options.pager.pageSize;
                    }

                    $this.pagination.totalRow = result.Data.totalRow;
                    $this.pagination.totalPage = parseInt($this.pagination.totalRow / pageSize) + ($this.pagination.totalRow % pageSize == 0 ? 0 : 1);
                    $this.pagination.totalData = result.Data.totalData;
                    //暂无数据时候当前页设置为-1
                    var curPage = $this.pagination.index + 1;
                    if (result.Data.pageData == null) {
                        alert("加载失败");
                    } else if (result.Data.pageData.length == 0) {
                        curPage = 0;
                    }

                    //删除当前元素
                    obj.find("div.pageBar").remove();
                    $(grid_tmp.pageBar).appendTo(obj);

                    obj.find("[name=page_index_input]").text(curPage);
                    obj.find("[name=total_page]").text($this.pagination.totalPage);
                    obj.find("[name=total_row]").text($this.pagination.totalRow);

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

                    var pagesize_ddl = obj.find("[name=select_size]");
                    //如果是数组类型
                    if (App_G.Util.isArray(options.pager.pageSize) && options.pager.pageSize.length > 0) {
                        var opts = "";
                        //遍历数组
                        $.each(options.pager.pageSize, function (i, o) {
                            opts += "<option value=\"" + o + "\" >" + o + "条</option>";
                        });

                        pagesize_ddl.find("option").remove();
                        $(opts).appendTo(pagesize_ddl);
                    }

                    pagesize_ddl.change(function (i, o) {
                        options.pager.index = 0;
                        options.exPageSize = pagesize_ddl.val();
                        $this.executePage(options.pager.index, options, obj);
                    });
                    pagesize_ddl.val(options.exPageSize);

                    var pageNumber = "";
                    var start10Page = parseInt($this.pagination.index / 10) * 10;
                    for (var i = 0; i < 10; i++) {
                        if (start10Page + i + 1 > $this.pagination.totalPage) break;
                        if ($this.pagination.index == start10Page + i) {
                            pageNumber += $(grid_tmp.pageNumber.replace("{0}", start10Page + i + 1)).css({
                                "color": "red"
                            })[0].outerHTML;
                        } else {
                            pageNumber += grid_tmp.pageNumber.replace("{0}", start10Page + i + 1);
                        }
                    }

                    //翻页码
                    $(pageNumber).insertAfter(obj.find("[ele-id=priv_btn_ten]"));
                    //翻页条事件绑定
                    $this.eventBindPage(options, result, obj);
                    //控制页码状态是否可用
                    $this.is_enable(obj);
                },
                //表格事件绑定
                eventBindTable: function () {

                    //单击事件
                    if (options.trclick == undefined) {
                        //绑定单击事件
                        $("#" + options.tableId + " tbody").on("click", "tr", function () {
                            $(this).addClass("selected").siblings().removeClass("selected");
                        });
                    }

                    //绑定双击事件
                    if (options.dblclick != undefined) {
                        $("#" + options.tableId + " tbody").unbind('dblclick').on("dblclick", "tr", function (e) {
                            e.stopPropagation();
                            $(this).addClass("selected").siblings().removeClass("selected");
                            if (options.dblclick != undefined) {
                                options.dblclick($(this));
                            }
                        });
                    }

                },
                //翻页事件绑定
                eventBindPage: function (options, data, obj) {
                    var $this = this;
                    obj.find("[name=page_btn]").click(function () {

                        switch ($(this).attr("ele-id")) {
                            case "first_btn":
                                if ($this.pagination.index == 0) return;
                                $this.pagination.index = 0;
                                break;
                            case "priv_btn":
                                if ($this.pagination.index == 0) return;
                                $this.pagination.index -= 1;
                                break;
                            case "priv_btn_ten":
                                if ($this.pagination.index <= 0) return;

                                if ($this.pagination.index - 10 < 0) {
                                    $this.pagination.index = 0;
                                } else {
                                    $this.pagination.index -= 10;
                                }
                                break;
                            case "next_btn_ten":
                                if ($this.pagination.index >= $this.pagination.totalPage - 1) return;

                                if ($this.pagination.index + 10 > $this.pagination.totalPage) {
                                    $this.pagination.index = $this.pagination.totalPage;
                                } else {
                                    $this.pagination.index += 10;
                                }
                                break;
                            case "next_btn":
                                if ($this.pagination.index == $this.pagination.totalPage - 1) return;
                                $this.pagination.index += 1;
                                break;
                            case "last_btn":
                                if ($this.pagination.index == $this.pagination.totalPage - 1) return;
                                $this.pagination.index = $this.pagination.totalPage - 1;
                                break;
                            case "go_page_btn":
                                var gopage = $.trim(obj.find("[name=go_page_text]").val()) == "" ? 0 : parseInt(obj.find("[name=go_page_text]").val() - 1);
                                if ($this.pagination.totalPage == 1 || $this.pagination.index == gopage) return;
                                $this.pagination.index = gopage;
                                break;
                            default:
                                if ($this.pagination.index == parseInt($(this).text() - 1)) return;
                                $this.pagination.index = parseInt($(this).text() - 1);
                                break;
                        }

                        $this.executePage($this.pagination.index, options, obj);
                    });
                },
                //翻页执行
                executePage: function (pageIndex, options, obj) {
                    var $this = this;
                    if (pageIndex >= $this.pagination.totalPage) {
                        $this.pagination.index = ($this.pagination.totalPage - 1) < 0 ? 0 : ($this.pagination.totalPage - 1);
                    }
                    if (pageIndex <= 0) {
                        $this.pagination.index = 0;
                    }

                    $this.onLoad(options, obj);
                },
                //再次执行，通过switch (arguments.length)长度方式实现重载
                execute: function (data, url, pageIndex) {
                    var $this = this;
                    $this.options.data = data;
                    $this.pagination.index = 0;

                    switch (arguments.length) {
                        case 1:
                            $this.executePage($this.pagination.index, $this.options, obj);
                            break;
                        case 2:
                            options.url = url;
                            $this.executePage($this.pagination.index, $this.options, obj);
                            break;
                        case 3:
                            options.url = url;
                            $this.pagination.index = pageIndex;
                            $this.executePage($this.pagination.index, $this.options, obj);
                            break;
                        default:
                            break;
                    }
                },
                //翻页按钮是否可用
                is_enable: function (obj) {
                    var $this = this;
                    //一页
                    if ($this.pagination.totalPage == 1 || $this.pagination.totalPage == 0) {
                        obj.find("[name=first_btn]").addClass("disabled");
                        obj.find("[name=priv_btn]").addClass("disabled");
                        obj.find("[name=priv_btn_ten]").addClass("disabled");

                        obj.find("[name=next_btn_ten]").addClass("disabled");
                        obj.find("[name=next_btn]").addClass("disabled");
                        obj.find("[name=page_last]").addClass("disabled");

                        obj.find("[name=page_btn]").addClass("disabled");
                    }

                    //当前要页为零
                    if ($this.pagination.index == 0 || $this.pagination.totalPage == 0) {
                        obj.find("[name=first_btn]").addClass("disabled");
                        obj.find("[name=priv_btn]").addClass("disabled");
                        obj.find("[name=priv_btn_ten]").addClass("disabled");
                    }

                    //当前要等于总页数
                    if ($this.pagination.index == $this.pagination.totalPage - 1 || $this.pagination.totalPage == 0) {
                        obj.find("[name=next_btn_ten]").addClass("disabled");
                        obj.find("[name=next_btn]").addClass("disabled");
                        obj.find("[name=last_btn]").addClass("disabled");
                    }
                },
                //翻页信息
                getPager: function () {
                    var $this = this;
                    return $this.pagination;
                },
                //获取grid
                getDataGrid: function () {
                    return $("#" + options.tableId)
                },
                //获取选中行
                getSeleteTr: function () {
                    return $("#" + options.tableId).find("tr.selected")
                },
                options: options,
                pagination: {
                    searchData: null,
                    data: null,
                    index: 0,
                    pageSize: [25, 50, 100],
                    totalRow: 0,
                    totalPage: 0
                }
            }
        });
        return datagridExtend.methods;
    }


    /// <summary>
    /// 变量
    /// </summary>
    var grid_tmp = {
        //排序的列 <p>门店编号<i name="orderby" class="fr fa fa-sort-amount-desc mt3"></i></p>
        orderBy: '<p><span>-</span><i name="orderby" data-filed="" class="fr">--</i></p>',
        //汇总的列
        totTd: '<span name="totTd" data-filed="" style="font-size:16px;color:red;">-</span>',
        //固定列的时候的模板
        firstTb: '<div class="firstcol"></div>',
        //固定列的时候的模板
        table: '<table class="table table-bordered table-hover dataTable b_catg_m"  >\
                            <thead><tr role="row"></tr></thead>\
                            <tbody ></tbody>\
                        </table>',
        //页面条
        pageBar: '<div class="pageBar" >\
        <a name=\"page_btn\" ele-id="first_btn" title="首页" class="first page_first"  >首页</a>\
        <a name=\"page_btn\" ele-id="priv_btn" title="上一页" >上一页</a>\
        <a name=\"page_btn\" ele-id="priv_btn_ten" title="上十页" >&lt;&lt;</a>\
        <a name=\"page_btn\" ele-id="next_btn_ten" title="下十页" >&gt;&gt;</a>\
        <a name=\"page_btn\" ele-id="next_btn" href="javascript:;" title="下一页"  >下一页</a>\
        <a name=\"page_btn\" ele-id="last_btn" href="javascript:;" title="尾页" class="page_last" >尾页</a>\
        <span class="info_label">当前 <span name="page_index_input"></span>/<span name="total_page"></span> 页 总共 <span name="total_row"></span> 条</span>\
        <input name="go_page_text" type="text" style="width:30px;" maxlength="5" name="goPageText">\
        <input name=\"page_btn\" ele-id="go_page_btn" name="go_page_btn" type="button" class="btn btn-primary btn-go" value="Go"  >\
        <select name="select_size" style="width:70px;"><option value="10">10条</option><option value="50">50条</option>\
        </select>\
        </div>',
        //页码
        pageNumber: "<a href=\"javascript:;\" name=\"page_btn\" class=\"current\" >{0}</a>",
        //搜索无数据
        noData: "<div class=\"nodata-img\" ele-id=\"nodata\" style=\"display: block;\" ></div>"
    }

})(jQuery)
