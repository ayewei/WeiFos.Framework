
/**
  * 城市身份联动        
  * @author 叶委
  * @provinceId         省份文本框ID
  * @cityId             城市文本框ID
  * @areaId             区县文本框ID
 */
function province_city_area(provinceId, cityId, areaId) {

    //省份
    var province = $("#" + provinceId);

    //城市
    var city = $("#" + cityId);

    //区县
    var area = $("#" + areaId);

    //添加到body 不然找不到该元素
    province_city_area.div.appendTo('body');

    //省份
    province.click(function () {
        //当前元素位置
        var xy = $(this).offset();

        //加载数据
        province_city_area.prototype.loaddata();

        //省份列表
        var provincedata = $(province_city_area.prototype.xmldata).find("address > province");
        province_city_area.div.find("a").remove();
        province_city_area.div.append("<a class='btn_close' href='javascript:void(0);'>X</a>");
        provincedata.each(function (i, o) {
            province_city_area.div.append("<a>" + provincedata.eq(i).attr("name") + "</a>");
        });

        province_city_area.div.find("a").click(function () {
            if ($(this).text() == "X") {
                province_city_area.div.hide();
            } else {
                province_city_area.div.find("a").remove();
                province_city_area.div.hide();
                province.val($(this).text());
                if (typeof cityId === "string" && cityId.length == 0) {
                    province.focus();
                }
                city.val("");
                area.val("");
            }
        });
        province_city_area.div.css({ left: xy.left, top: xy.top + $(this).height() + 11 }).slideDown();
    });


    //城市
    city.click(function () {

        //当前元素位置
        var xy = $(this).offset();

        //加载数据
        province_city_area.prototype.loaddata();

        if ($.trim(province.val()).length > 0) {
            //获取城市列表数据
            var citydata = $(province_city_area.prototype.xmldata).find("address > province[name='" + province.val() + "'] > city");
            province_city_area.div.find("a").remove();
            province_city_area.div.append("<a class='btn_close' href='#'>X</a>");
            citydata.each(function (i, o) {
                province_city_area.div.append("<a>" + citydata.eq(i).attr("name") + "</a>");
            });

            province_city_area.div.find("a").click(function () {
                if ($(this).text() == "X") {
                    province_city_area.div.hide();
                } else {
                    province_city_area.div.find("a").remove();
                    province_city_area.div.hide();
                    city.val($(this).text());

                    if (typeof areaId === "string" && areaId.length==0) {
                        city.focus();
                    }
                    area.val("");
                }
            });

            province_city_area.div.css({ left: xy.left, top: xy.top + $(this).height() + 11 }).slideDown();
        }
    });


    //加载县区数据
    area.click(function () {

        //当前元素位置
        var xy = $(this).offset();

        //加载数据
        province_city_area.prototype.loaddata();

        if ($.trim(city.val()).length > 0) {
            //获取区县列表
            var areadata = $(province_city_area.prototype.xmldata).find("address > province[name='" + province.val() + "'] >  city[name='" + city.val() + "'] > country");
            province_city_area.div.find("a").remove();
            province_city_area.div.append("<a class='btn_close' href='#'>X</a>");
            areadata.each(function (i, o) {
                province_city_area.div.append("<a>" + areadata.eq(i).attr("name") + "</a>");
            });

            province_city_area.div.find("a").click(function () {
                if ($(this).text() == "X") {
                    province_city_area.div.hide();
                } else {
                    province_city_area.div.find("a").remove();
                    province_city_area.div.hide();
                    area.val($(this).text());
                    area.focus();
                }
            });
            province_city_area.div.css({ left: xy.left, top: xy.top + $(this).height() + 11 }).slideDown();
        }
    });

};

/**
 * 城市省份数据
 * @data 
 */
province_city_area.prototype.xmldata = null;

//省份城市区县div
province_city_area.div = $("<div class='provincecity'></div>");

/**
 * 加载城市省份数据
 * @return data
 */
province_city_area.prototype.loaddata = function () {
    if (province_city_area.prototype.xmldata == null) {
        //$.ajax({
        //    type: "get",
        //    dataType: 'jsonp',
        //    url: App_G.Util.getDomain() + "/Content/Resources/Script/plugin/city-area/" + "area.xml",
        //    //jsonpCallback: "GetJsonExamPaper",
        //    success: function (xdata) {
        //        province_city_area.prototype.xmldata = xdata;
        //    }
        //});
        $.ajax({
            url: App_G.Util.getDomain() + "/Config/" + "area.xml",
            type: "GET",
            async: false,
            dataType: "xml",
            error: function (xdata) { alert("加载xml数据有误！"); },
            success: function (xdata) {
                province_city_area.prototype.xmldata = xdata;
            }
        });
    }
};

