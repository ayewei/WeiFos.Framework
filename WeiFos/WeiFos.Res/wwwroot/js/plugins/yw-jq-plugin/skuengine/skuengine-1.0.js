
/// <summary>
/// 商品SKU组件
/// @author 叶委     2015-04-05
/// @param {Object}  speccustoms         编辑初始化规格值
/// @param {Object}  disablehead         sku头是否可以编辑
/// @param {Object}  skudataset          编辑初始化sku数据
/// @param {Object}  specnames           规格名称集合
/// @param {Object}  specvalues          规格值集合
/// @param {Object}  skuresult           初始化sku
/// @param {Object}  skucustom           自定义别名
/// @param {Object}  skucolumn           自定义sku列头
/// @param {Object}  skucolumn.isauto    编号生成规则（0：编号后面自动加-1 -2等,1：EAN-13规则替换掉前面3位）
/// @param {Object}  options.completed   绑定完成事件方法
/// </summary>
(function ($) {
    $.extend($.fn, {
        skuengine: function (options) {
            var skuer;
            this.each(function () {
                var $this = $(this);
                var opts = $.extend(true, {}, options);
                skuer = skuenginer(opts, $this);
                skuer.onInit(options, $this);
            });
            return skuer;
        }
    });

    /**
    ///SKU插件对象
    */
    var skuenginer = function (options, wrap) {
        $.extend(skuenginer, {
            methods: {
                //初始化
                onInit: function (options, obj) {
                    sku_tmp.sku.skuelement = obj;
                    sku_tmp.sku.spec_names = options.specnames;
                    sku_tmp.sku.spec_values = options.specvalues;
                    this.onLoad(options.specnames, options.specvalues, options.speccustoms, options.skudataset);
                },
                //加载数据
                onLoad: function (specnames, specvalues, speccustoms, skudataset) {
                    sku_tmp.sku.spec_names = specnames;
                    sku_tmp.sku.spec_values = specvalues;
                    sku_tmp.sku.speccustoms = speccustoms;
                    sku_tmp.sku.skudataset = skudataset;
                    //生成SKU
                    this.createSpecModule(options, sku_tmp.sku.skuelement);
                    //执行完成事件
                    if (options.completed != undefined) options.completed();
                },
                /// <summary>
                /// SKU组合算法引擎
                /// a母集，b子集
                /// </summary>
                skuEngine: function (a, b) {
                    //存放组合分组
                    var ab = [];
                    //存放结果集合
                    var rt = [];
                    //生成sku结果集合变量
                    var hasone = true;

                    //组合分组
                    $.each(a, function (i, o) {
                        ab[i] = new Array();
                        $.each(b, function (j, v) {
                            //此处根据自己业务参数判断
                            if (v.specname_id == o.id) {
                                ab[i].push(v);
                            }
                        });
                    });

                    //计算出sku集合数组
                    $.each(ab, function (ii, o) {
                        if (hasone) {
                            $.each(ab[ii], function (j, k) {
                                sku_tmp.sku.tmp_result[j] = new Array(k);
                            });
                            hasone = false;
                        } else {

                            //临时数据对象
                            var t = new Array();
                            var index = 0;

                            $.each(sku_tmp.sku.tmp_result, function (i, o) {
                                $.each(ab[ii], function (j, v) {

                                    var sp = new Array();
                                    sp.push(v);

                                    $.each(sku_tmp.sku.tmp_result[i], function (l, n) {
                                        sp.push(n);
                                    });

                                    t[index] = sp;
                                    index++;
                                });
                            });
                            sku_tmp.sku.tmp_result = t;
                        }
                    });

                    //处理规格列排序
                    var tmp = new Array();
                    $.each(sku_tmp.sku.tmp_result, function (i, o) {
                        var sp = new Array();
                        $.each(a, function (k, l) {
                            $.each(sku_tmp.sku.tmp_result[i], function (j, v) {
                                if (l.id == v.specname_id) {
                                    sp[k] = v;
                                }
                            });
                        });
                        tmp[i] = sp;
                    });

                    //重置临时对象
                    sku_tmp.sku.tmp_result = [];
                    //处理规格列排序
                    return tmp;
                },
                //获取规格名称
                getSepcName: function (id) {
                    var val = {};
                    $.each(sku_tmp.sku.spec_names, function (i, o) {
                        if (o.id == id) {
                            val = o;
                            return;
                        }
                    });
                    return val;
                },
                //获取规格值
                getSepcValue: function (id) {
                    var val = {};
                    $.each(sku_tmp.sku.spec_values, function (i, o) {
                        if (o.id == id) {
                            val = o;
                            return;
                        }
                    });
                    return val;
                },
                //是否能够创建
                hasCreateSku: function () {
                    var has_create = true;
                    $.each($("#" + sku_tmp.sku.options.controlId).find("div.right_s"), function (i, o) {
                        if (!$(this).next().find("input[type=checkbox]:checked").length) {
                            has_create = false;
                            return;
                        }
                    });
                    return has_create;
                },
                //获取选中值
                getSelectValue: function (options) {
                    var $this = this;
                    //集合
                    var a = [], b = [];
                    //存放结果集合
                    var rt = [];

                    $.each($("#" + options.controlId).find("div[data-sku-spid]"), function (i, o) {
                        var parent = $(this);
                        var has_id = true;

                        //复选框集合
                        var checkedboxs = $(this).find("input[type=checkbox]:checked");
                        if (checkedboxs.length) {
                            $.each(checkedboxs, function (j, k) {
                                b.push($this.getSepcValue($(k).val()));
                            });
                            if (has_id) {
                                a.push($this.getSepcName(parent.attr("data-sku-spid")));
                            }
                            has_id = false;
                        }
                    });
                    rt = $this.skuEngine(a, b);
                    return rt;
                },
                //获取已存在sku行
                getCurrentSku: function (specset) {
                    var tr = null;
                    //遍历当前行
                    $.each(sku_tmp.sku.sku_trs, function (i, o) {
                        if ($(o).attr("data-set") == specset) {
                            tr = o;
                        }
                    });
                    return tr;
                },
                //自定义规格名
                coustomSpecName: function (options) {
                    $.each($("#" + options.controlId).find("input[type=checkbox]:checked"), function (i, o) {
                        var title = $(this).attr("title");
                        //自定义名称
                        var cname = $("#" + "c_" + $(this).val()).find("input");
                        if (cname.length) {
                            var td = $("td[" + "data-vid=" + $(this).val() + "]");
                            if (td.length) {
                                if ($.trim(cname.val()).length == 0) {
                                    td.text(title);
                                } else {
                                    td.text(cname.val());
                                }
                            }
                        }
                    });
                },
                //自动增长字段
                autofield: function (options) {
                    var that = this;
                    $.each(options.skucolumn, function (i, o) {
                        //结尾 加-1 -2的方式 
                        if (o.isauto == 0) {
                            //遍历自动编码字段
                            $.each($("#" + options.skuId).find("tr[data-set]").find("input[name=" + o.no + "]"), function (ii, oo) {
                                //如果原来存在编号则不修改 
                                $.each(options.skudataset, function (iii, ooo) {
                                    //如果在初始化sku集合里面不存在，则自动生成
                                    if (!that.compareSku($(oo).parent().parent().attr("data-set"), ooo.specset)) {
                                        var val = o.value + "-" + (ii + 1);
                                        $(oo).val(val);
                                        $(oo).attr("value", val);
                                        return false;
                                    }
                                });
                            });

                            //ISBN替换前三位的方式 
                        } else if (o.isauto == 1 || o.isauto == 2 || o.isauto == 3 || o.isauto == 4 || o.isauto == 5 || o.isauto == 10 || o.isauto == 20) {
                            //遍历自动编码字段ean13
                            $.each($("#" + options.skuId).find("tr[data-set]").find("input[name=" + o.no + "]"), function (ii, oo) {

                                //此处存在修改初始化和新增点击增加sku行情况，不能覆盖修改初始化sku编号的值
                                var index = -1;
                                //如果原来存在编号则不修改 
                                $.each(options.skudataset, function (iii, ooo) {
                                    //如果在初始化sku集合里面不存在，则自动生成
                                    if (!that.compareSku($(oo).parent().parent().attr("data-set"), ooo.specset)) {
                                        index = iii;
                                        return false;
                                    }
                                });

                                if (o.selector != undefined) {
                                    var val = $(o.selector).val();
                                    //修改情况，并且是新创建的行
                                    if ((options.skudataset != undefined && !$(oo).attr("is_edit") && val.length > 3) || (options.skudataset == undefined && val.length > 3)) {
                                        var val = val.substring(3, val.length - 1);
                                        switch (o.isauto) {
                                            //1图书,2咖啡餐饮，3好物多营，4课堂，5原材料，6专柜租金，10图书好物 其它商品20，参照BizModule
                                            case 1:
                                                val = (ii + 100) + val;
                                                break;
                                            //好物
                                            case 2:
                                                val = (ii + 300) + val;
                                                break;
                                            //其他
                                            case 20:
                                                val = (ii + 700) + val;
                                                break;
                                        }

                                        //EAN-13编码
                                        val = that.ean13(val);
                                        $(oo).val(val);
                                        $(oo).attr("value", val);
                                    }
                                }

                            });

                        }
                    });
                },
                //创建规格模块
                createSpecModule: function (options, obj) {
                    var $this = this;

                    //规格头
                    if ($("#" + options.controlId).length) {
                        $("#" + options.controlId).children().remove();
                    }

                    //sku列表
                    if ($("#" + options.skuId).length) {
                        $("#" + options.skuId).children().remove();
                    }

                    //设置控件ID
                    if (obj.attr("id") == undefined) {
                        options.controlId = new Date().getTime().toString() + App_G.Util.getRandomNum(1, 1000000);
                        $(sku_tmp.skumodule_tmp).attr("id", options.controlId).appendTo(obj);
                    } else {
                        options.controlId = obj.attr("id");
                    }

                    var skumodule = $(sku_tmp.skumodule_tmp);
                    $.each($(sku_tmp.sku.spec_names), function (i, o) {
                        var name = $(sku_tmp.specname_tmp);
                        name.find("label").text(o.name);
                        name.appendTo(skumodule.find("div:eq(1)"));
                        //规格值集合
                        var vals = $(sku_tmp.specvals_tmp);
                        //设在复选框
                        vals.attr("data-sku-spid", o.id);
                        $.each(sku_tmp.sku.spec_values, function (j, k) {
                            if (k.specname_id == o.id) {
                                //规格模板
                                var val = $(sku_tmp.specval_tmp);

                                //复选框是否可用 
                                if (options.disablehead == 2) val.find("[type=checkbox]").prop("disabled", true);

                                //设置val
                                val.find("[type=checkbox]").val(k.id).attr("title", k.val);

                                //设在文本
                                val.find("label").text(k.val);
                                val.appendTo(vals);
                            }
                        });

                        vals.appendTo(skumodule.find("div:eq(1)"));
                    });

                    //创建页面元素
                    skumodule.appendTo(obj);

                    //规格头初始化绑定
                    if (undefined != sku_tmp.sku.speccustoms && sku_tmp.sku.speccustoms.length) {
                        $.each($("#" + options.controlId).find("input[type=checkbox]"), function (i, o) {
                            $.each(sku_tmp.sku.speccustoms, function (j, v) {
                                if ($(o).val() == v.specvalue_id) {
                                    $(o).prop("checked", true);

                                    $(o).parent().find("#c_" + $(o).val()).find("input").val(v.custom_value);
                                    var cname = $(sku_tmp.custom_name);
                                    cname.attr("id", "c_" + $(o).val()).find("input").attr("name", "cname");
                                    cname.find("input").attr("data-id", $(o).parent().parent().attr("data-sku-spid") + "_" + $(o).val()).val(v.custom_value);
                                    //等于1的时候修改数据时候不能去掉选中
                                    if (options.disablehead == 1) {
                                        $(o).prop("disabled", true);
                                        //cname.find("input").prop("disabled", true);
                                    }

                                    cname.insertAfter($(o).next());
                                }
                            });
                        });
                    }

                    //sku集合初始化绑定
                    if (undefined != sku_tmp.sku.skudataset && sku_tmp.sku.skudataset.length) {
                        $this.createSku(sku_tmp.sku.skudataset, options, obj);
                    }

                    $("i.fa.fa-chevron-up").hide();
                    $("i.fa.fa-chevron-down").hide();
                    //sku头部
                    var div_title = $("#" + options.controlId).find("div.m_opBar");
                    //sku头部对应内容框 height: 60px;overflow: hidden;
                    var div_title_box = div_title.next();
                    //获取当前高度
                    var h = div_title_box.height();
                    //只有一组SKU情况，并且高度超出
                    if (div_title_box.find(">div").length == 2 && h > 50) {
                        $("i.fa.fa-chevron-down").show();
                        div_title_box.css({ "height": "75px", "overflow": "hidden" });
                    }

                    //展开
                    $("i.fa.fa-chevron-down").click(function () {
                        $(this).hide();
                        $("i.fa.fa-chevron-up").show();
                        div_title_box.css({ "height": "auto", "overflow": "unset" });
                    });

                    //折叠
                    $("i.fa.fa-chevron-up").click(function () {
                        $(this).hide();
                        $("i.fa.fa-chevron-down").show();
                        div_title_box.css({ "height": "75px", "overflow": "hidden" });
                    });

                    //选择选中生成
                    $("#" + options.controlId).off().on("click", "[type=checkbox]", (function () {
                        //自定义名称
                        var cname;
                        //选中
                        if ($(this).prop("checked")) {
                            cname = $(sku_tmp.custom_name);
                            cname.attr("id", "c_" + $(this).val()).find("input").attr("name", "cname");
                            cname.find("input").attr("data-id", $(this).parent().parent().attr("data-sku-spid") + "_" + $(this).val());
                            cname.insertAfter($(this).next());
                        } else {
                            $("#c_" + $(this).val()).remove();
                        }

                        //将生成后的行存放在
                        sku_tmp.sku.sku_trs = $("#" + options.skuId).find("tr[data-set]");

                        //移除原有sku层
                        $("#" + options.skuId).remove();

                        //是否能生成
                        if (!$this.hasCreateSku()) return;

                        //获取生成的值
                        var skuresult = $this.getSelectValue(options);

                        //生成SKU
                        $this.createSku(skuresult, options, obj);

                        //绑定SKU文本框输入值
                        $("#" + options.skuId).on("keyup", "input.input_wr", (function () {
                            $(this).attr("value", $(this).val());
                        }));

                        //自定义规格名称
                        $this.coustomSpecName(options);

                    }));

                    //规格名按键事件
                    $this.bindSepcNameKey(options);
                    //设置相关设置
                    sku_tmp.sku.options = options;
                },
                //创建sku模块
                createSku: function (skuresult, options, obj) {
                    var $this = this;

                    //设置sku控件ID
                    options.skuId = options.controlId + "_sku";
                    $(sku_tmp.sku_div).attr("id", options.skuId).insertAfter($("#" + options.controlId));

                    //获取sku 模板
                    var sku_div = $("#" + options.skuId);

                    //生成表头
                    if (sku_tmp.sku.spec_names.length > 0) {
                        var tr_head = "";

                        //sku 列头
                        $.each(sku_tmp.sku.spec_names, function (i, o) {
                            tr_head += "<th role='row'>" + o.name + "</th>";
                        });

                        //自定义列头
                        $.each(options.skucolumn, function (i, o) {
                            tr_head += "<th>" + o.name + "</th>";
                        });

                        tr_head += "<th>操作</th>";
                        //填充表头
                        $(tr_head).appendTo(sku_div.find($("thead")));
                    }

                    var tr_data = "";
                    var td_data = "";

                    $.each(skuresult, function (i, o) {
                        var sset = "";
                        var u_id = "";

                        //非编辑状态
                        if (undefined == o.specset) {
                            $.each(skuresult[i], function (j, v) {
                                td_data += "<td name = 'attr_spec_td' data-vid='" + skuresult[i][j].id + "' id='" + v.specname_id + "_" + v.id + "' >" + skuresult[i][j].val + "</td>";
                                if (sset.length > 0) {
                                    sset += "," + v.specname_id + "_" + v.id;
                                } else {
                                    sset += v.specname_id + "_" + v.id;
                                }
                                u_id += v.specname_id + "_" + v.id;
                            });
                        } else {

                            //重新排序
                            $.each(sku_tmp.sku.spec_names, function (ii, oo) {
                                sset = o.specset;
                                var specArray = o.specset.split(',');
                                $.each(specArray, function (j, v) {
                                    var spec_name_id = v.split('_')[0];
                                    var spec_value_id = v.split('_')[1];

                                    if (spec_name_id == oo.id) {
                                        td_data += "<td name = 'attr_spec_td' data-vid='" + spec_value_id + "' id='" + spec_name_id + "_" + spec_value_id + "' >"
                                            + $this.getSepcValue(spec_value_id).val + "</td>";

                                        u_id += spec_name_id + "_" + spec_value_id;
                                        return;
                                    }
                                });
                            });
                        }

                        //sku自定义列
                        $.each(options.skucolumn, function (j, v) {
                            var input = "", val = "", is_edit = false;
                            //根据选择器选择生成初始化的值
                            if (v.selector != undefined) val = $(v.selector).val();
                            else val = undefined == v.value ? "" : v.value;

                            $.each(o, function (ii, oo) {
                                if (v.no == ii) {
                                    val = oo;
                                    //此处理编辑状态下的sku等信息不可用，新增情况可以输入
                                    if (!v.isenable) {
                                        o.isenable = false;
                                    } else {
                                        o.isenable = true;
                                    }

                                    is_edit = true;
                                    return false;
                                }
                            });

                            //自定义格式输出
                            if (App_G.Util.isFunction(v.stringformt)) {
                                val = v.stringformt(val);
                            }

                            //是否是只读
                            if (undefined == v.isenable || v.isenable != false) {
                                input = "<input style='" + (undefined == v.ip_style ? "" : v.ip_style) + ";' class='input_wr' type='text' " + (is_edit ? "is_edit='1'" : "") + " "
                                    + ((undefined != o.is_enable && !o.is_enable) || (undefined != o.isenable && !o.isenable) ? "disabled='disabled'" : "") + " name='" + v.no + "' id='" + v.no + "_" + u_id + "' value='" + val + "' />";
                            } else {
                                input = "<label name='" + v.no + "'> " + val + "</label>";
                            }

                            td_data += "<td style='" + (undefined == v.td_style ? "" : v.td_style)
                                + "; overflow:hidden;'>" + input + "</td>";
                        });

                        td_data += "<td style='width:200px;' name='operation_td' >" + ((undefined != o.is_enable && !o.is_enable) ? sku_tmp.sku_enable_btn : sku_tmp.sku_dis_btn);
                        if (options.print != undefined) {
                            td_data += "<i name='print_btn' class='fa fa-print ml10'>" + options.print.text + "</i></td>";
                        }
                        td_data += "</td>";

                        //根据sku集合获取当前
                        var cur_tr = $this.getCurrentSku(sset);
                        if (cur_tr == null) {
                            tr_data += "<tr data-set='" + sset + "' " + (undefined == o.id ? "" : "id='" + o.id + "'") + ((undefined != o.is_enable && !o.is_enable) ? "disabled='disabled'" : "") + ">" + td_data + "</tr>";
                        } else {
                            tr_data += "<tr data-set='" + sset + "' " + (undefined == o.id ? "" : "id='" + o.id + "'") + ((undefined != o.is_enable && !o.is_enable) ? "disabled='disabled'" : "") + ">" + $(cur_tr).html() + "</tr>";
                        }
                        td_data = "";
                    });

                    //添加SKU行到页面
                    $(tr_data).appendTo(sku_div.find($("tbody")));

                    //充值规格名
                    $this.coustomSpecName(options);

                    //设置禁用按钮
                    $("a[name=disable_btn]").digbox({
                        Selector: "a[name=disable_btn]",
                        Context: "div.mpContent.mt10",
                        Title: "提示信息",
                        Content: "确定禁用该条数据吗？",
                        CallBack: function (s, c, p) {
                            c.parent().parent().attr("disabled", true);
                            c.parent().parent().find("input").prop("disabled", true);
                            $(sku_tmp.sku_enable_btn).appendTo(c.parent());
                            c.remove();
                        }
                    });

                    //设置启用按钮
                    $("[name=enable_btn]").digbox({
                        Selector: "a[name=enable_btn]",
                        Context: "div.mpContent.mt10",
                        Title: "提示信息",
                        Content: "确定启用该条数据吗？",
                        CallBack: function (s, c, p) {
                            c.parent().parent().attr("disabled", false);
                            c.parent().parent().find("input").prop("disabled", false);
                            $(sku_tmp.sku_dis_btn).appendTo(c.parent());
                            c.remove();
                        }
                    });

                    //打印方法
                    $("[name=print_btn]").click(function () {
                        options.print.fun($(this));
                    });

                    //自动增长字段
                    $this.autofield(options);

                    //绑定验证
                    $this.bindValidate(options);

                    //设置相关设置
                    sku_tmp.sku.options = options;
                },
                //自定义规格名按键事件
                bindSepcNameKey: function (options) {
                    var $this = this;
                    //绑定文本框
                    $("#" + options.controlId).on("keyup", "[name=cname]", (function () {
                        if (!$this.hasCreateSku()) return;
                        $this.coustomSpecName(options);
                    }));
                    //绑定文本框
                    $("#" + options.controlId).on("blur", "[name=cname]", (function () {
                        if (!$this.hasCreateSku()) return;
                        $this.coustomSpecName(options);
                    }));
                },
                //绑定验证
                bindValidate: function () {
                    $.each(options.skucolumn, function (j, v) {
                        if (undefined != v.validate) {
                            v.validate();
                            //v.valid("input[name=" + v.no + "]",
                            //{ selector: $(sku_tmp.sku_div).attr("id"), tabindex: $("#base_panel"), focusmsg: "", errormsg: "", vtype: verifyType.isLGZeroPrice });
                        }

                    });
                },
                //获取设置自定义规格值
                getSpecCustoms: function () {

                    var params = [];
                    $.each($("#" + options.controlId).find("input[type=checkbox]:checked"), function (i, o) {
                        //自定义数据
                        var data = {};
                        //自定义名称
                        var cname = $("#" + "c_" + $(this).val()).find("input");
                        data["specname_id"] = cname.attr("data-id").split('_')[0];
                        data["specvalue_id"] = cname.attr("data-id").split('_')[1];
                        data["custom_value"] = cname.val();
                        params.push(data);
                    });

                    return params;
                },
                //获取sku数据
                getSkuData: function () {
                    var params = [];
                    if (sku_tmp.sku.options == null) return params;
                    $.each($("#" + sku_tmp.sku.options.skuId).find("tr[data-set]"), function (i, o) {
                        var data = {};
                        var specSet = "";
                        var specName = "";
                        $.each($(o).find("td").not("[name=operation_td]"), function (j, v) {

                            if ($(v).attr("name") == "attr_spec_td") {
                                if (specSet == "") {
                                    specSet += $(v).attr("id");
                                    specName += $(v).text();
                                } else {
                                    specSet += "," + $(v).attr("id");
                                    specName += " " + $(v).text();
                                }
                            } else {
                                if ($(v).find("input").length) {
                                    data[$(v).find("input").attr("name")] = $(v).find("input").val();
                                } else {
                                    data[$(v).find("label").attr("name")] = null;
                                }
                            }
                        });

                        data["sku_name"] = specName;
                        data["is_enable"] = $(o).attr("disabled") == undefined;
                        data["specset"] = specSet;
                        data["id"] = (App_G.Util.isNum($(o).attr("id")) ? $(o).attr("id") : 0);

                        params.push(data);
                    });
                    return params;
                },
                //比对sku字符串
                compareSku: function (sku1, sku2) {
                    let exist = 0
                    let arr = sku1.split(',')
                    let arr1 = sku2.split(',')
                    //sku集合是否一致
                    if (arr.length != arr1.length) {
                        return false
                    }

                    $.each(arr, function (i, o) {
                        $.each(arr1, function (ii, oo) {
                            if (oo == o) {
                                exist++
                            }
                        });
                    });

                    return arr.length == exist
                },
                //EAN-13
                ean13: function (CodeString) {
                    if (CodeString == "") return "";
                    if (CodeString.length > 12) CodeString = CodeString.substring(0, 12);
                    Code = CodeString.split("");
                    var A = 0;
                    var B = 0;
                    for (i = 0; i < Code.length; i++) {
                        if (i % 2 == 1) {
                            A += parseInt(Code[i]);
                        }
                        else {
                            B += parseInt(Code[i]);
                        }
                    }
                    var C1 = B;
                    var C2 = A * 3;
                    var CC = (C1 + C2) % 10;
                    var CheckCode = (10 - CC) % 10;
                    return CodeString + CheckCode;
                }
            }
        });
        return skuenginer.methods;
    };


    /// <summary>
    /// 变量
    /// </summary>
    var sku_tmp = {
        //SKU集合模板
        skumodule_tmp: '<div class="m_pFormat mt20">\
                        <div class="m_opBar"><a href="javascript:;">商品规格</a><i class="fa fa-chevron-down fr mt-1 fr mt-1" ></i><i class="fa fa-chevron-up fr mt-1" ></i></div>\
                        <div style="border:1px solid #ddd;padding-bottom:10px;" ></div></div>',
        //规格名称模版
        specname_tmp: '<div class="right_s" data-sku-specname="" ><label  style="vertical-align: middle;" ></label></div>',
        //规格值集合模版
        specvals_tmp: '<div data-sku-spid="" style="text-align:left;"></div>',
        //规格值模版
        specval_tmp: '<div class="disBlock" style="min-width:160px;text-indent: 28px;" ><input type="checkbox" style="vertical-align: middle;" ><label style="vertical-align: middle;font-size:13px;font-weight: 400;margin:0px;padding:0px;display: inline;"></label></div>',
        //自定义别名
        custom_name: '<div style="border:1px solid #ddd;line-height: 24px;margin-left:28px;"><span class="color-note-text" style="visibility: visible;line-height: 18px;margin-left:-25px">别名</span>\
                      <input type="text" maxlength="30" style="width:90px;line-height: 17px;border-top:1px;border-bottom:0px;border-right:0px;border:0px;" /></div>',

        //sku模板
        sku_div: '<div class="mpContent mt10" >\
                            <div style="height: auto;overflow-x: auto; overflow-y: hidden;">\
                                <table id="DataTables_Table_0" class="table table-bordered">\
                                    <thead></thead>\
                                    <tbody role="alert" aria-live="polite" aria-relevant="all"></tbody>\
                                </table>\
                            </div>\
                        </div>',
        //禁用按钮
        sku_dis_btn: '<a href="javascript:;" name="disable_btn"  class="nl"  ><i class="fa fa-toggle-on"></i></a>',
        //启用按钮
        sku_enable_btn: '<a href="javascript:;" name="enable_btn"  class="nl" ><i class="fa fa-toggle-off"></i></a>',

        //sku数据集合
        sku: {
            //当前sku控件对象
            skuelement: null,
            //返回sku集合对象
            options: null,
            //选中的规格名集合
            spec_names: null,
            //选中的规格值集合
            spec_values: null,
            //生成后的规格集合
            spec_array: new Array(),
            //生成后的SKU行集合
            sku_trs: new Array(),
            //由于js算法不能引用递归传递参数
            //所有定义临时全局变量结果集合 计算完后清空
            tmp_result: new Array(),
            //存放初始化sku集合
            skudataset: new Array()
        }
    }


})(jQuery);
