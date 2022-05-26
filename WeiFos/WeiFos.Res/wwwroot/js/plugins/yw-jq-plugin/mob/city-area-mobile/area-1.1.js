/**
 * 城市身份联动      
 * @author 叶委          2013-04-17
 * 参数数据格式 
   {   省
     opt_p:{ 
        id : "",
        text : "",
        val : "",
        style : "",
        attr_name: "data-val",
        attr_val: "province"
     },市
     opt_c:{ 
        id : "",
        text : "",
        val : "",
        style : "",
        attr_name: "data-val",
        attr_val: "city"
     },区
     opt_a:{ 
        id : "",
        text : "",
        val : "",
        style : "",
        attr_name: "data-val",
        attr_val: "area"
     },
     xmldata:null
   }
*/
(function ($) {
    $.extend($.fn, {
        area: function (options) {

            var selects;
            var opt = $.extend(true, {}, options);

            this.each(function () {

                //获取当前元素
                var $this = $(this);

                //设置默认ID
                var id = new Date().getTime();
                opt.opt_p.id = (undefined == options.opt_p.id ? id + "p" : options.opt_p.id);
                opt.opt_c.id = (undefined == options.opt_c.id ? id + "c" : options.opt_c.id);
                opt.opt_a.id = (undefined == options.opt_a.id ? id + "a" : options.opt_a.id);

                //设置默认文本
                opt.opt_p.text = (undefined == options.opt_p.text ? "-选择省份-" : options.opt_p.text);
                opt.opt_c.text = (undefined == options.opt_c.text ? "-选择城市-" : options.opt_c.text);
                opt.opt_a.text = (undefined == options.opt_a.text ? "-选择区县-" : options.opt_a.text);

                //初始控件
                selects = selector(opt, $this);
                selects.onload();
            });

            return selects;
        }
    });

    /// <summary>
    /// 创建下拉元素
    /// </summary>
    var selector = function (options, obj) {
        $.extend(selector, {
            methods: {
                //数据绑定 
                dataBind: function (options) {

                    $this = this;

                    obj.children().remove();

                    var p_option = "<option value=''>" + options.opt_p.text + "</option>";
                    var c_option = "<option value=''>" + options.opt_c.text + "</option>";
                    var a_option = "<option value=''>" + options.opt_a.text + "</option>";

                    /*********省份初始化 -s*********/
                    var p_tag = "";
                    if (options.opt_p.attr_name != undefined && options.opt_p.attr_val != undefined) { p_tag = " " + options.opt_p.attr_name + "=\"" + options.opt_p.attr_val + "\" " }

                    var p_style = (undefined == options.opt_p.style ? "" : options.opt_p.style);
                    var p_selects = "<span><select id=" + options.opt_p.id + " style='" + p_style + "' " + p_tag + ">" + p_option + "</select></span>";
                    $(p_selects).appendTo(obj);

                    $this.bindPselect(options);
                    /*********省份初始化 -e*********/


                    /*********城市初始化 -s*********/
                    var c_tag = "";
                    if (options.opt_c.attr_name != undefined && options.opt_c.attr_val != undefined) { c_tag = " " + options.opt_c.attr_name + "= \"" + options.opt_c.attr_val + "\" " }

                    var c_style = (undefined == options.opt_c.style ? "" : options.opt_c.style);
                    var c_selects = "<span><select id='" + options.opt_c.id + "' style='" + c_style + "' " + c_tag + " >" + c_option + "</select></span>";
                    $(c_selects).appendTo(obj);

                    $this.bindCselect(options);
                    /*********城市初始化 -e*********/


                    /*********区县初始化 -s*********/
                    var a_tag = "";
                    if (options.opt_a.attr_name != undefined && options.opt_a.attr_val != undefined) { a_tag = " " + options.opt_a.attr_name + "= \"" + options.opt_a.attr_val + "\" " }

                    var a_style = (undefined == options.opt_a.style ? "" : options.opt_a.style);
                    var a_selects = "<span><select id='" + options.opt_a.id + "' style='" + a_style + "' " + a_tag + " >" + a_option + "</select></span>";
                    $(a_selects).appendTo(obj);

                    $this.bindAselect(options);
                    /*********区县初始化 -e*********/


                    var select_province = $("#" + options.opt_p.id);      //省份
                    var select_city = $("#" + options.opt_c.id);          //城市
                    var select_area = $("#" + options.opt_a.id);          //区县

                    /*********省份触发城市区县事件*********/
                    select_province.bind("change", function () {
                        $this.bindCselect(options, true);
                        $this.bindAselect(options, true);
                    });

                    /*********城市触发区县事件*********/
                    select_city.bind("change", function () {
                        $this.bindAselect(options, true);
                    });

                },
                //绑定省份
                bindPselect: function (options) {

                    //删除所有option
                    $("#" + options.opt_p.id).find("option").remove();

                    //默认option
                    var p_options = "<option value=''>" + options.opt_p.text + "</option>";

                    var provincedata = $(options.xmldata).find("address > province");
                    provincedata.each(function (i, o) {
                        p_options += "<option value='" + provincedata.eq(i).attr("name") + "' >" + provincedata.eq(i).attr("name") + "</option>";
                    });
                    $(p_options).appendTo($("#" + options.opt_p.id));

                    //如果存在值
                    if (options.opt_p.val != undefined && options.opt_p.val != null) {
                        $("#" + options.opt_p.id).val(options.opt_p.val);
                    }

                },
                //绑定城市
                bindCselect: function (options, ischange) {

                    //删除所有option
                    $("#" + options.opt_c.id).find("option").remove();

                    //默认option
                    var c_options = "<option value=''>" + options.opt_c.text + "</option>";

                    //是否存在城市初始化数据
                    if ($('option:selected', "#" + options.opt_p.id).index() > 0) {
                        var citydata = $(options.xmldata).find("address > province[name='" + $("#" + options.opt_p.id).val() + "'] > city");
                        citydata.each(function (i, o) {
                            c_options += "<option value='" + citydata.eq(i).attr("name") + "' >" + citydata.eq(i).attr("name") + "</option>";
                        });

                        $(c_options).appendTo($("#" + options.opt_c.id));

                        if (!ischange && options.opt_c.val != undefined && options.opt_c.val != null) {
                            $("#" + options.opt_c.id).val(options.opt_c.val);
                        }
                    } else {
                        $(c_options).appendTo($("#" + options.opt_c.id));
                    }
                },
                //绑定区县
                bindAselect: function (options, ischange) {

                    //删除所有option
                    $("#" + options.opt_a.id).find("option").remove();

                    //默认option
                    var a_options = "<option value=''>" + options.opt_a.text + "</option>";

                    //是否存在区县初始化数据
                    if ($('option:selected', "#" + options.opt_c.id).index() > 0) {
                        var areadata = $(options.xmldata).find("address > province[name='" + $("#" + options.opt_p.id).val() + "'] >  city[name='" + $("#" + options.opt_c.id).val() + "'] > country");
                        areadata.each(function (i, o) {
                            a_options += "<option value='" + areadata.eq(i).attr("name") + "' >" + areadata.eq(i).attr("name") + "</option>";
                        });

                        $(a_options).appendTo($("#" + options.opt_a.id));

                        if (!ischange && options.opt_a.val != undefined && options.opt_a.val != null) {
                            $("#" + options.opt_a.id).val(options.opt_a.val);
                        }

                    } else {
                        $(a_options).appendTo($("#" + options.opt_a.id));
                    }

                },
                //加载数据
                onload: function () {
                    $this = this;

                    //加载xml数据
                    if (options.xmldata == null) {
                        $.ajax({
                            url: "/Content/XmlData/area.xml",
                            type: "GET",
                            async: false,
                            dataType: "xml",
                            error: function (xdata) { alert("加载xml数据有误！"); },
                            success: function (xdata) {
                                options.xmldata = xdata;
                            }
                        });
                    }

                    $this.dataBind(options);
                }
            }
        });
        return selector.methods;
    }
})(jQuery)

