/**
 * 城市身份联动      
 * @author 叶委          2013-04-17
 * @province_id          省份ID
 * @city_id              城市ID
 * @area_id              区县ID
*/
(function ($) {
    $.extend($.fn, {
           area: function (options) {
            //相关参数配置
            options = $.extend(options, {
                province_id: (options.province_id == null || $.trim(options.province_id) == "") ? "province_id" : options.province_id,
                city_id: (options.city_id == null || $.trim(options.city_id) == "") ? "city_id" : options.city_id,
                area_id: (options.area_id == null || $.trim(options.area_id) == "") ? "area_id" : options.area_id,
                pval: options.pval,
                cval: options.cval,
                aval: options.aval,
                xmldata: null
            });

            var selects;

            this.each(function () {

                //获取当前元素
                var $this = $(this);

                var _options = {};
                _options["province_id"] = options.province_id;
                _options["city_id"] = options.city_id;
                _options["area_id"] = options.area_id;

                //初始控件
                selects = selector(options, $this);
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
                dataBind: function () {

                    $this = this;

                    var province_DefaultOption = "<option value=''>-选择省份-</option>";
                    var city_DefaultOption = "<option value=''>-选择城市-</option>";
                    var area_DefaultOption = "<option value=''>-选择区县-</option>";

                    var p_selects = "<span><select id=" + options.province_id + ">" + province_DefaultOption + "</select></span>";
                    $(p_selects).appendTo(obj);
                    $this.bindPselect(options.pval);

                    if (undefined != options.cval && $('#' + options.province_id).val() != "") {
                        $this.bindCselect(options.cval);
                    } else {
                        var c_selects = "<span><select id='" + options.city_id + "' >" + city_DefaultOption + "</select></span>";
                        $(c_selects).appendTo(obj);
                    }

                    if (undefined != options.aval && $('#' + options.city_id).val() != "") {
                        $this.bindAselect($('#' + options.area_id).val());
                    } else {
                        var a_selects = "<span><select id='" + options.area_id + "'>" + area_DefaultOption + "</select></span>";
                        $(a_selects).appendTo(obj);
                    }

                    var select_province = $("#" + options.province_id);  //省份
                    var select_city = $("#" + options.city_id);          //城市
                    var select_area = $("#" + options.area_id);          //区县

                    /*********省份触发城市区县事件*********/
                    select_province.bind("change", function () {
                        $this.bindCselect();
                        $this.bindAselect();
                    });

                    /*********城市触发区县事件*********/
                    select_city.bind("change", function () {
                        $this.bindAselect();
                    });
                  
                },
                //绑定省份
                bindPselect: function (val) {
                    $("#" + options.province_id).find("option").remove();
                    var provincedata = $(options.xmldata).find("address > province");
                    var p_options = "";
                    p_options = "<option value=''>-选择省份-</option>";
                    provincedata.each(function (i, o) {
                        p_options += "<option>" + provincedata.eq(i).attr("name") + "</option>";
                    });
                    $(p_options).appendTo($("#" + options.province_id));

                    //如果存在值
                    if (val != undefined && val != null) {
                        $("#" + options.province_id).val(val);
                    }

                },//绑定城市
                bindCselect: function (val) {
                    var c_options = "";
                    $("#" + options.city_id).find("option").remove();

                    c_options = "<option value=''>-选择城市-</option>";
                    var citydata = $(options.xmldata).find("address > province[name='" + $("#" + options.province_id).val() + "'] > city");
                    citydata.each(function (i, o) {
                        c_options += "<option>" + citydata.eq(i).attr("name") + "</option>";
                    });
                    $(c_options).appendTo($("#" + options.city_id));

                    if (val == 0) {
                        $("#" + options.area_id).find("option").remove();
                        $("<option value=''>-选择区县-</option>").appendTo($("#" + options.area_id));
                    }

                    //如果存在值
                    if (val != undefined && val != null) {
                        $("#" + options.city_id).val(val);
                    }

                },//绑定区县
                bindAselect: function (val) {
                    var a_options = "";

                    $("#" + options.area_id).find("option").remove();
                    a_options = "<option value=''>-选择区县-</option>";

                    var areadata = $(options.xmldata).find("address > province[name='" + $("#" + options.province_id).val() + "'] >  city[name='" + $("#" + options.city_id).val() + "'] > country");
                    areadata.each(function (i, o) {
                        a_options += "<option>" + areadata.eq(i).attr("name") + "</option>";
                    });
                    $(a_options).appendTo($("#" + options.area_id));
                    //如果存在值
                    if (val != undefined && val != null) {
                        $("#" + options.area_id).val(val);
                    }
                },
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

                    $this.dataBind();
                }
            }
        });
        return selector.methods;
    }
})(jQuery)

