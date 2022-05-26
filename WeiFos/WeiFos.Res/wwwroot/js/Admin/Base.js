
/**
  * App_G 平台脚本基类
  * @author 叶委    2015-02-11
 */
var App_G = {
    //图片类型路径
    ImgType: {
        /// apk 类型
        Apk: "Apk",
        /// 缺省图片类型
        Default: "Default",
        /// 微信账号
        WX_Account: "WX_Account",
        /// 系统用户
        Sys_User: "Sys_User",
        /// 资讯 封面图
        Informt: "Informt",
        /// 资讯 详情图
        InformtDetails: "InformtDetails",
        /// 资讯分类 封面图
        InformtCgty: "InformtCgty",
        /// 合作伙伴 封面图
        Partner: "Partner",
        /// 商品封面 封面图
        Product_Cover: "Product_Cover",
        /// 商品详情 图
        Product_Details: "Product_Details",
        ///品牌
        Product_Brand: "Product_Brand",
        /// 成功案例 封面图
        Case_Cover: "Case_Cover",
        /// 成功案例 详情图
        Case_Details: "Case_Details",
        /// 图文 封面图
        ImgTextReply_Title: "ImgTextReply_Title",
        /// 图文 内容图
        ImgTextReply_Details: "ImgTextReply_Details",
        ///广告 图
        Banner: "Banner"
    },
    //响应状态
    Code: {
        // 系统错误
        Code_500: 500,
        // 响应成功 
        Code_200: 200
    },
    //关键词类型 
    KeyWordBizType: {
        /// 微官网
        WebSite: "WebSite",
        /// 微活动
        Activity: "Activity",
        /// 文本回复
        TextReply: "TextReply",
        /// 文本回复
        ImgTextReply: "ImgTextReply",
        /// 相册
        Album: "Album",
        /// 微留言
        Message: "Message",
        /// 微预约
        WeiResv: "WeiResv",
        /// 微信会员卡
        MemberCard: "MemberCard",
        /// 商品封面图
        Product_Cover: "Product_Cover"
    },
    //是否生成缩略图
    CreateThmImg: {
        //不生成
        None: 0,
        //生成缩略小图
        CreateS: 1,
        //生成缩略中图
        CreateM: 2,
        //生成中图和小图
        CreateALL: 3
    },
    //一般处理程序动作
    HandlerAc: {
        //上传图片
        UpImg: "upimg",
        //删除图片
        DelImg: "delimg",
        //在线管理图片
        ManageImg: "manageimg"
    }, 
    //权限验证
    Auth: {
        Code: "",
        //初始化身份认证
        InitID: function () {
            var url = "/SystemModule/System/GetMyPCode?url=" + window.location.pathname + "&pcode=";
            if ($("[data-pcode]").length) url += $("[data-pcode]").val();
            $getByAsync(url, "",
                function (result) {
                    App_G.Auth.Code = result;
                });
        },
        Filter: function (pcode) {
            var hasp = false;
            if (App_G.Auth.Code != "") {
                var pcodes = App_G.Auth.Code.split(',');
                $.each(pcodes, function (i, o) {
                    if (pcode == o) {
                        hasp = true;
                        return;
                    }
                });
            }
            return hasp;
        }
    },
    //映射实体
    Mapping: {
        //加载隐藏域数据
        Load: function (selector) {
            if ($.trim($(selector).val()).length > 0) {
                var data = $.parseJSON($(selector).val());
                $(selector).remove();
                return data;
            }
            return null;
        },
        //获取绑定
        Get: function (attr, datas, selector) {
            var current;
            if (undefined == selector) {
                current = $(document);
            } else {
                current = $(selector);
            }

            var data = {};
            $.each(current.find("select[" + attr + "],input[" + attr + "],textarea[" + attr + "],span[" + attr + "],p[" + attr + "]"), function (i, v) {

                switch ($(v).prop("type")) {
                    //文本输入框.val().replace(/\n/g, '');
                    case "text":
                        data[$(v).attr(attr)] = $(v).val();
                        break;
                    //文本输入框
                    case "textarea":
                        data[$(v).attr(attr)] = $(v).val();
                        break;
                    //下拉选择框
                    case "select-one":
                        data[$(v).attr(attr)] = $(v).val();
                        break;
                    //复选框
                    case "checkbox":
                        data[$(v).attr(attr)] = $(v).is(":checked");
                        break;
                    //单选框
                    case "radio":
                        data[$(v).attr(attr)] = $("input:radio[" + attr + "=" + $(v).attr(attr) + "]:checked").val();
                        break;
                    default:
                        if (undefined == $(v).prop("type")) {
                            data[$(v).attr(attr)] = $(v).text();
                        } else {
                            data[$(v).attr(attr)] = $(v).val();
                        }
                        break;
                }
            });
            for (var _attr in datas) {
                data[_attr] = datas[_attr];
            }
            return data;
        },
        //绑定到页面
        Bind: function (attr, datas, selector) {
            var current;
            if (undefined == selector) {
                current = $(document);
            } else {
                current = $(selector);
            }

            for (var _attr in datas) {

                //遍历页面元素
                $.each(current.find("p[" + attr + "],span[" + attr + "],input[" + attr + "],select[" + attr + "],textarea[" + attr + "],label[" + attr + "],i[" + attr + "],del[" + attr + "],dt[" + attr + "],h1[" + attr + "],h2[" + attr + "],h3[" + attr + "],h4[" + attr + "],h5[" + attr + "],div[" + attr + "]"), function (i, o) {

                    var ele = $(o);
                    //通过ID关联
                    if ((ele.attr("id") != undefined && ele.attr("id") == _attr) || ele.attr(attr) == _attr) {

                        switch (ele.prop("type")) {
                            //文本输入框
                            case "text":
                                ele.val(datas[_attr]);
                                break;
                            //文本输入框
                            case "textarea":
                                ele.val(datas[_attr]);
                                break;
                            //下拉选择框
                            case "select-one":
                                if (datas[_attr] != null) {
                                    ele.val(datas[_attr].toString());
                                }
                                break;
                            //复选框
                            case "checkbox":
                                if (undefined != datas[_attr] && datas[_attr].toString().toLowerCase() == "true") {
                                    ele.attr("checked", true);
                                } else {
                                    ele.attr("checked", false);
                                }
                                break;
                            //单选框
                            case "radio":
                                if (datas[_attr].toString() == ele.val()) ele.attr("checked", true);
                                else ele.attr("checked", false);
                                break;
                            default:
                                //if (ele.is('span') || ele.is('lable')) { }
                                ele.text(datas[_attr]);
                                ele.val(datas[_attr]);
                                if (ele.attr("format-money")) {
                                    var f_str = App_G.Util.formaToMoney(datas[_attr], ele.attr("format-money"));
                                    if (ele.attr("money-symbol")) {
                                        ele.text(ele.attr("money-symbol") + f_str);
                                        ele.val(ele.attr("money-symbol") + f_str);
                                    } else {
                                        ele.text(f_str);
                                        ele.val(f_str);
                                    }
                                }
                                break;
                        }
                    }

                });
            }
        }
    },
    //工具
    Util: {
        //日期对象
        Date: {
            //获取当前日期
            getDate: function (days) {
                var d = new Date();
                if (days != undefined) {
                    var milliseconds = days * 24 * 60 * 60 * 1000;
                    var timestamp = Date.parse(d);
                    //return new Date(timestamp + milliseconds);
                    d = new Date(timestamp + milliseconds);
                    //d.setDate(days);
                }
                return d.getFullYear() + "/" + (d.getMonth() + 1) + "/" + d.getDate();
            },
            //获取当前时间
            getDateTimeNow: function (days) {
                var d = new Date();
                if (days != undefined) {
                    d.setDate(d.getDate() + days);
                }
                return d.getFullYear() + "/" + (d.getMonth() + 1) + "/" + d.getDate() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds();
            },
            //获取时间添加某个天数
            getDateTime: function (time, days) {
                var d = new Date(time);
                if (days != undefined) {
                    d.setDate(d.getDate() + days);
                }
                return d.getFullYear() + "/" + (d.getMonth() + 1) + "/" + d.getDate();
            },
            //日期格式化
            DateFormat: function (date, format) {
                var d = new Date(date);
                var o = {
                    "M+": d.getMonth() + 1,
                    "d+": d.getDate(),
                    "h+": d.getHours(),
                    "m+": d.getMinutes(),
                    "s+": d.getSeconds(),
                    "q+": Math.floor((d.getMonth() + 3) / 3),
                    "S": d.getMilliseconds()
                }
                if (/(y+)/.test(format)) {
                    format = format.replace(RegExp.$1, (d.getFullYear() + "").substr(4 - RegExp.$1.length));
                }
                for (var k in o) {
                    if (new RegExp("(" + k + ")").test(format)) {
                        format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
                    }
                }
                return format;
            },
            //json格式日期处理
            ChangeDateFormat: function (time) {
                if (time != null) {
                    if (time.indexOf('T') != -1) {
                        return time.substr(0, time.indexOf('T'));
                    }
                    var date = new Date(parseInt(time.replace("/Date(", "").replace(")/", ""), 10));
                    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
                    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
                    return date.getFullYear() + "-" + month + "-" + currentDate;
                }
                return "";
            },
            //json格式日期处理（时分秒）
            ChangeCompleteDateFormat: function (time) {
                if (time != null) {
                    if (time.indexOf('T') != -1) {
                        if (time.indexOf('.') != -1) {
                            time = time.substr(0, time.indexOf('.'));
                        }
                        return time.replace('T', ' ');
                    }
                    var date = new Date(parseInt(time.replace("/Date(", "").replace(")/", ""), 10));
                    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
                    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
                    return date.getFullYear() + "-" + month + "-" + currentDate + " " + date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();
                }
                return "";
            },
            //json格式日期处理（时分）
            ChangeComDateFormat: function (time) {
                if (time != null) {
                    if (time.indexOf('T') != -1) {
                        if (time.indexOf('.') != -1) {
                            time = time.substr(0, time.indexOf('.'));
                        }
                        return time.replace('T', ' ');
                    }
                    var date = new Date(parseInt(time.replace("/Date(", "").replace(")/", ""), 10));
                    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
                    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
                    return date.getFullYear() + "-" + month + "-" + currentDate + " " + date.getHours() + ":" + date.getMinutes()
                }
                return "";
            },
            //时间比较
            CompareDate: function (t1, t2) {
                t1 = new Date(App_G.Util.Date.ChangeDateFormat(t1));
                t2 = new Date(App_G.Util.Date.ChangeDateFormat(t2));
                return t1 > t2;
            },
            //时间比较
            CompareDateNow: function (t1) {
                return new Date(App_G.Util.Date.ChangeDateFormat(t1)) > new Date(App_G.Util.Date.getDateTimeNow());
            }
        },
        //字符串截取
        cutSubString: function (str, len) {
            if (!str || !len) { return ''; }
            var l = 0;
            var temp = '';
            for (var i = 0, m = str.length; i < m; i++) {
                l++;
                if (str.charCodeAt(i) > 255) { l++; }
                if (l > len) { return temp + '...'; }
                temp += str.charAt(i);
            }
            return str;
        },
        //获取最小值到最大值之前的整数随机数
        getRandomNum: function (Min, Max) {
            var Range = Max - Min;
            var Rand = Math.random();
            return (Min + Math.round(Rand * Range));
        },
        //金额格式
        formaToMoney: function (s, n) {
            n = n > 0 && n <= 20 ? n : 2;
            f = s < 0 ? "-" : ""; //判断是否为负数  
            s = parseFloat((Math.abs(s) + "").replace(/[^\d\.-]/g, "")).toFixed(n) + "";//取绝对值处理, 更改这里n数也可确定要保留的小数位  
            var l = s.split(".")[0].split("").reverse(),
                r = s.split(".")[1];
            t = "";
            for (i = 0; i < l.length; i++) {
                t += l[i] + ((i + 1) % 3 == 0 && (i + 1) != l.length ? "," : "");
            }
            return f + t.split("").reverse().join("") + "." + r.substring(0, 2);//保留2位小数  如果要改动 把substring 最后一位数改动就可  
        },
        //金额格式转中文大写
        arabiaToChinese: function (Num) {
            Num = Num.replace(/￥/g, "");
            Num = Num.replace(/,/g, "");
            Num = Num.replace(/^0+/, "");
            //判断输入的数字是否大于定义的数值
            if (Number(Num) > 99999999999.99) {
                alert("您输入的数字太大了");
                return;
            }
            // 初始化验证:
            var integral, decimal, outStr, parts;
            var digits, radices, bigRadices, decimals;
            var zeroCount;
            var i, p, d;
            var quotient, modulus;

            parts = Num.split(".");
            if (parts.length > 1) {
                integral = parts[0];
                decimal = parts[1];
                decimal = decimal.substr(0, 2);
            }
            else {
                integral = parts[0];
                decimal = "";
            }
            // 实例化字符大写人民币汉字对应的数字
            digits = new Array("零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖");
            radices = new Array("", "拾", "佰", "仟");
            bigRadices = new Array("", "万", "亿");
            decimals = new Array("角", "分");

            outStr = "";
            //大于零处理逻辑
            if (Number(integral) > 0) {
                zeroCount = 0;
                for (i = 0; i < integral.length; i++) {
                    p = integral.length - i - 1;
                    d = integral.substr(i, 1);
                    quotient = p / 4;
                    modulus = p % 4;
                    if (d == "0") {
                        zeroCount++;
                    }
                    else {
                        if (zeroCount > 0) {
                            outStr += digits[0];
                        }
                        zeroCount = 0;
                        outStr += digits[Number(d)] + radices[modulus];
                    }
                    if (modulus == 0 && zeroCount < 4) {
                        outStr += bigRadices[quotient];
                    }
                }
                outStr += "元";
            }

            // 包含小数部分处理逻辑
            if (decimal != "") {
                for (i = 0; i < decimal.length; i++) {
                    d = decimal.substr(i, 1);
                    if (d != "0") {
                        outStr += digits[Number(d)] + decimals[i];
                    }
                }
            }

            if (outStr == "") outStr = "零元";
            if (decimal == "") outStr += "整";
            return outStr
        },
        //数字千分符
        toThousandMoney: function (v) {
            if (isNaN(v)) { return v; }
            v = (Math.round((v - 0) * 100)) / 100;
            v = (v == Math.floor(v)) ? v + ".00" : ((v * 10 == Math.floor(v * 10)) ? v + "0" : v);
            v = String(v);
            var ps = v.split('.');
            var whole = ps[0];
            var sub = ps[1] ? '.' + ps[1] : '.00';
            var r = /(\d+)(\d{3})/;
            while (r.test(whole)) {
                whole = whole.replace(r, '$1' + ',' + '$2');
            }
            v = whole + sub;
            return v;
        },
        //数字万分符
        toTenThousandMoney: function (num) {
            if (isNaN(num)) { return num; }
            num = num * 0.0001;
            num = parseFloat(num, 0).toFixed(2);
            this.toThousandMoney(num)
            return num;
        },
        //获取原图地址
        getImgUrl: function (url, def_url) {
            if ((undefined == url || $.trim(url).length == 0) && def_url != null && undefined != def_url) return def_url;
            if (undefined == url || $.trim(url).length == 0) return App_Config.getResDomain() + "/Content/DefaultRes/Images/nopicture.jpg";
            if (url.indexOf("http://") != -1) {
                return url;
            } else {
                return App_Config.getResDomain() + url;
            }
        },
        //获取小图地址
        getImgUrl_s: function (url, def_url) {
            if ((undefined == url || $.trim(url).length == 0) && def_url != null && undefined != def_url) return def_url;
            if (undefined == url || $.trim(url).length == 0) return App_Config.getResDomain() + "/Content/DefaultRes/Images/nopicture.jpg";
            if (url.indexOf("http://") != -1) {
                return url.replace("/Image/", /Thm_Image/);
            } else {
                return App_Config.getResDomain() + url.replace("/Image/", /Thm_Image/);
            }
        },
        //获取中图地址
        getImgUrl_m: function (url, def_url) {
            if ((undefined == url || $.trim(url).length == 0) && def_url != null && undefined != def_url) return def_url;
            if (undefined == url || $.trim(url).length == 0) return App_Config.getResDomain() + "/Content/DefaultRes/Images/nopicture.jpg";
            if (url.indexOf("http://") != -1) {
                return url.replace("/Image/", /Med_Image/);
            } else {
                return App_Config.getResDomain() + url.replace("/Image/", /Med_Image/);
            }
        },
        //获取Oss图片地址
        getOssImgUrl: function (filename, param) {
            if (undefined == filename || filename == null || $.trim(filename).length == 0) {
                return App_Config.getResDomain() + "/Content/DefaultRes/Images/NoProduct.jpg";
            }
            if (undefined == param || param == null || $.trim(param).length == 0) {
                if (filename.indexOf("http://") != -1) {
                    return filename;
                }
                return App_Config.getOssDomain() + filename;
            }
            if (filename.indexOf("http://") != -1) {
                return filename + param;
            } else {
                return App_Config.getOssDomain() + filename + param;
            }
        },
        //获取页面请求参数
        getUrlParam: function (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(decodeURI(r[2])); return null;
        },
        //获取页面请求参数中的ID
        getRequestId: function (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(decodeURI(r[2])); return 0;
        },
        //获取Html前缀参数
        getHtmlId: function () {
            var url = window.location.href;
            var s_index = url.lastIndexOf('/');
            var e_index = url.lastIndexOf('.html');
            return url.substring(s_index + 1, e_index);
        },
        //获取网站域名
        getDomain: function () {
            return window.location.protocol + '//' + window.location.host;
        },
        //获取原始URL
        getRawUrl: function () {
            return window.document.location.href;
        },
        //获取自定义json字符串 
        getJsonString: function (attr, datas) {
            return JSON.stringify(App_G.Mapping.Get(attr, datas));
        },
        //判断是否是数组
        isArray: function (object) {
            return (typeof object == 'object') && object.constructor == Array;
        },
        //判断是否为函数
        isFunction: function (object) {
            return (typeof object == 'function') && object.constructor == Function;
        },
        //判断是否为字符串类型
        isString: function (object) {
            return (object != null) && (object != undefined) && (typeof object == 'string') && (object.constructor == String);
        },
        //判断是否是对象
        isJQuery: function (object) {
            return (object instanceof jQuery);
        },
        //判断是否是对象
        isObject: function (object) {
            return (typeof object == 'object') && object.constructor == Object;
        },
        //判断是否为数值类型
        isNum: function (object) {
            return !isNaN(object);
        }
    }
};


/**
  * String format方法扩展    
  * @author 叶委   2013-10-20
 */
if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
                ? args[number]
                : match
                ;
        });
    };
}

/**
  * 数组方法扩展    
  * @author 叶委   2013-10-20
 */
Array.prototype.indexOf = function (val) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == val) return i;
    }
    return -1;
};

Array.prototype.remove = function (val) {
    var index = this.indexOf(val);
    if (index > -1) {
        this.splice(index, 1);
    }
};

/**
  * Jquery 光标位置扩展    
  * @author 叶委   2013-10-20
 */
(function ($) {
    $.extend($.fn, {
        unselectContents: function () {
            if (window.getSelection)
                window.getSelection().removeAllRanges();
            else if (document.selection)
                document.selection.empty();
        },
        selectContents: function () {
            $(this).each(function (i) {
                var node = this;
                var selection, range, doc, win;
                if ((doc = node.ownerDocument) && (win = doc.defaultView) && typeof win.getSelection != 'undefined' && typeof doc.createRange != 'undefined' && (selection = window.getSelection()) && typeof selection.removeAllRanges != 'undefined') {
                    range = doc.createRange();
                    range.selectNode(node);
                    if (i == 0) {
                        selection.removeAllRanges();
                    }
                    selection.addRange(range);
                } else if (document.body && typeof document.body.createTextRange != 'undefined' && (range = document.body.createTextRange())) {
                    range.moveToElementText(node);
                    range.select();
                }
            });
        },
        setCaret: function () {
            if (!$.browser.msie) return;
            var initSetCaret = function () {
                var textObj = $(this).get(0);
                textObj.caretPos = document.selection.createRange().duplicate();
            };
            $(this).click(initSetCaret).select(initSetCaret).keyup(initSetCaret);
        },
        insertAtCaret: function (textFeildValue) {
            var textObj = $(this).get(0);
            if (document.all && textObj.createTextRange && textObj.caretPos) {
                var caretPos = textObj.caretPos;
                caretPos.text = caretPos.text.charAt(caretPos.text.length - 1) == '' ?
                    textFeildValue + '' : textFeildValue;
            } else if (textObj.setSelectionRange) {
                var rangeStart = textObj.selectionStart;
                var rangeEnd = textObj.selectionEnd;
                var tempStr1 = textObj.value.substring(0, rangeStart);
                var tempStr2 = textObj.value.substring(rangeEnd);
                textObj.value = tempStr1 + textFeildValue + tempStr2;
                textObj.focus();
                var len = textFeildValue.length;
                textObj.setSelectionRange(rangeStart + len, rangeStart + len);
                textObj.blur();
            } else {
                textObj.value += textFeildValue;
            }
        }
    });
})(jQuery);


/**
  * 按钮禁用扩展 
  * @param  text(显示文本)
  * @param  time(为毫秒)
  * @author 叶委 2015-06-23
*/
(function ($) {
    $.extend($.fn, {
        setDisable: function (options) {
            options = $.extend(options = (options == undefined ? {} : options), {
                text: options.text,
                time: options.time
            });

            this.each(function () {

                var $o = $(this), t = $.trim($o.text()) == "" ? $o.val() : $o.text();

                //未设置文本
                if (undefined != options.text && $.trim(options.text).length != 0) {
                    $o.text(options.text);
                    $o.val(options.text);
                } else {
                    $o.text($o.text() + "...");
                    $o.val($o.val() + "...");
                }

                //禁用按钮
                $o.prop("disabled", true);

                //未设置时间
                if (undefined == options.time || $.trim(options.time).length == 0) {
                    setTimeout(function () { $o.prop("disabled", false); $o.text(t); $o.val(t); }, 1000);
                } else {
                    setTimeout(function () { $o.prop("disabled", false); $o.text(t); $o.val(t); }, options.time);
                }

            });

        }
    });
})(jQuery);


/**
  * Jquery.digbox    
  * @author 叶委   2013-05-09
 */

/// <summary>
/// Author   yewei  
/// Creation date    2013/05/29 
/// Updated  date    2019/02/13
/// Jquery.digbox
/// @param  {String} ---                                         
/// @param  {Object}                      options                匿名参数对象
/// @param  {Object}.{String}             options.Title          标题
/// @param  {Object}.{String}             options.Context        内容
/// @param  {Object}.{String}             options.Backdrop       验证类型
/// @param  {Object}.{String}             options.Event          事件类型
/// @param  {Object}.{function}           options.Before         弹出之前
/// @param  {Object}.{Object}.{String}    options.Cencel         取消事件
/// @param  {Object}.{Object}.{function}  options.Show           是否显示
/// @param  {Object}.{function}           options.CallBack       回调方法
/// @param  {Object}.{Object}.{String}    options.Selector       选择器
/// @param  {Object}.{Object}.{function}  options.IsAuto         是否自动弹出
/// </summary>
(function ($) {
    $.extend($.fn, {
        digbox: function (options) {
            options = $.extend(options, {
                Title: options.Title,
                Content: options.Content,
                Backdrop: options.Backdrop,
                Event: options.Event,
                Before: options.Before,
                Cencel: options.Cencel,
                Show: options.Show,
                CallBack: options.CallBack,
                Selector: options.Selector,
                Context: options.Context,
                IsAuto: options.IsAuto
            });
             
            var current;
            if (undefined == options.Context) {
                current = this;
            } else {
                current = $(options.Context);
            }

            if (options.IsAuto) {
                var dig = digboxer(options, $(this));
                return dig.show($(this));
            } else {
                current.on(undefined == options.Event ? "click" : options.Event, options.Selector, function (e) {
                    e.stopPropagation();
                    var dig = digboxer(options, $(this));
                    return dig.show($(this));
                });
            }

        }
    });

    var digboxer = function (options, obj) {
        $.extend(digboxer, {
            methods: {
                //显示模态框
                show: function (obj) {
                    var $this = this, width = 0, height = 0;

                    if (App_G.Util.isFunction(options.Before) && !options.Before(obj)) return false;

                    if ($("#" + options.controlId).length) {
                        $("#" + options.controlId).remove();
                    }

                    if (obj.attr("id") == undefined) {
                        options.controlId = new Date().getTime();
                        $(digbox_tmp).attr("id", options.controlId);
                    } else {
                        options.controlId = obj.attr("id") + "_digbox";
                    }

                    //当前模态窗口
                    var current_dig = $(digbox_tmp).attr("id", options.controlId);

                    //如果在iframe
                    if (self.frameElement && self.frameElement.tagName == "IFRAME") {

                        width = $(parent.document).width();
                        height = $(parent.document).height();
                        $(current_dig, parent.document).appendTo($(parent.document).find("body"));

                        //监听隐藏事件 
                        $("#" + options.controlId, parent.document).on('hidden.bs.modal', function () {
                            $("#" + options.controlId, parent.document).remove();
                        })

                    } else {
                        width = $(window).width();
                        height = $(window).height();

                        $(current_dig).appendTo('body');
                        $("#" + options.controlId).on('hidden.bs.modal', function () {
                            $("#" + options.controlId).remove();
                        })
                    }

                    //标题
                    current_dig.find("h4").text(options.Title);

                    //标题
                    current_dig.find("div.modal-body").html(options.Content);

                    //X按钮
                    current_dig.find(".close").unbind('click').click(function (e) {
                        $this.backmethod(obj, options.Cencel);
                    });

                    //确定
                    current_dig.find("button[name=dialog_confirm_btn]").attr("id", options.controlId + "_submit");
                    current_dig.find("button[name=dialog_confirm_btn]").unbind('click').click(function () {
                        $this.backmethod(obj, options.CallBack(current_dig.find("button:eq(2)").attr("id"), obj, current_dig));
                    });

                    //取消
                    current_dig.find("button[name=dialog_cancel_btn]").unbind('click').click(function () {
                        $this.backmethod(obj, options.Cencel);
                    });

                    //自适应宽
                    $(current_dig).modal((undefined == options.Backdrop ? "show" : { backdrop: 'static', keyboard: false })).css({ "width": "auto", "margin-left": function () { return (width - $(current_dig).width()) / 2; } });
                    //自适高
                    $(current_dig).modal((undefined == options.Backdrop ? "show" : { backdrop: 'static', keyboard: false })).css({ "height": "auto", "margin-top": function () { return (height - $(current_dig).height()) / 2; } });

                    if (self.frameElement && self.frameElement.tagName == "IFRAME") {
                        $(parent.document).find("#" + options.controlId).modal((undefined == options.Backdrop ? "show" : { backdrop: 'static', keyboard: false }));
                    } else {
                        $(current_dig).modal((undefined == options.Backdrop ? "show" : { backdrop: 'static', keyboard: false }));
                    }

                    //当前元素，提交按钮，当前用户定义html
                    if (undefined != options.Show) options.Show(current_dig.find("button:eq(2)").attr("id"), obj, current_dig);

                    return false;
                },
                backmethod: function (obj, method) {

                    var $this = this;
                    if (App_G.Util.isFunction(method)) {
                        var cfct = method(obj);
                        if (undefined == cfct || cfct) {
                            $this.hidden(obj);
                        }
                    } else {
                        if (undefined == method || method) {
                            $this.hidden(obj);
                        }
                    }
                },
                //隐藏模态框
                hidden: function (obj) {
                    //如果在iframe
                    if (self.frameElement && self.frameElement.tagName == "IFRAME") {
                        $("#" + options.controlId, parent.document).modal('hide');
                        setTimeout(function () {
                            $("#" + options.controlId, parent.document).remove();
                        }, 500);
                    } else {
                        $("#" + options.controlId).modal('hide');
                        setTimeout(function () {
                            $("#" + options.controlId).remove();
                        }, 500);
                    }
                }
            }
        });
        return digboxer.methods;
    };

    //弹出窗模板
    var digbox_tmp = '<div class="modal fade in" >\
        <div class="modal-dialog" >\
            <div class="modal-content">\
                <div class="modal-header">\
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">\
                        <span aria-hidden="true">&times;</span></button>\
                    <h4 class="modal-title"></h4>\
                </div>\
                <div class="modal-body">\
                    <p>One fine body&hellip;</p>\
                </div>\
                <div class="modal-footer">\
                    <button type="button" name="dialog_cancel_btn" class="btn btn-default pull-left" data-dismiss="modal">关&nbsp;闭</button>\
                    <button type="button" name="dialog_confirm_btn" class="btn btn-primary">确&nbsp;定</button>\
                </div>\
            </div>\
          </div>\
        </div>';

})(jQuery);


/**
  * textarea 长度限制    
  * @author 叶委   2013-08-31
 */
(function ($) {
    $.extend($.fn, {
        maxlength: function (options) {
            options = $.extend(options, {
                MaxLength: (undefined == options.MaxLength || options.MaxLength == null || $.trim(options.MaxLength) == "") ? 100 : options.MaxLength
            });

            this.each(function () {
                //获取当前元素
                var $thistextarea = $(this);

                $thistextarea.keyup(function () {
                    if (options.MaxLength - $thistextarea.val().length < 0) {
                        $thistextarea.val($thistextarea.val().substr(0, options.MaxLength));
                    }
                });

                $thistextarea.keydown(function () {
                    if (options.MaxLength - $thistextarea.val().length < 0) {
                        $thistextarea.val($thistextarea.val().substr(0, options.MaxLength));
                    }
                });
            })
        }
    });
})(jQuery);


/*ajax 简易封装*/
function $get(url, data, datatype, callback) {
    if (App_G.Util.isFunction(datatype)) {
        ajax_send("get", url, data, "json", datatype);
    } else {
        ajax_send("get", url, data, datatype, callback);
    }
};

function $getByAsync(url, data, datatype, callback) {
    if (App_G.Util.isFunction(datatype)) {
        ajax_send("get", url, data, "json", datatype, false);
    } else {
        ajax_send("get", url, data, datatype, callback, false);
    }
};

function $post(url, data, datatype, callback) {
    if (App_G.Util.isFunction(datatype)) {
        ajax_send("post", url, data, "json", datatype);
    } else {
        ajax_send("post", url, data, datatype, callback);
    }
};

function $postByAsync(url, data, datatype, callback) {
    if (App_G.Util.isFunction(datatype)) {
        ajax_send("post", url, data, "json", datatype, false);
    } else {
        ajax_send("post", url, data, datatype, callback, false);
    }
};

function ajax_send(requestType, url, data, datatype, callback, isAsc) {
    $.ajax({
        type: requestType,
        contentType: "application/json; charset=utf8",
        url: (url.indexOf('http://') != -1 ? "" : App_G.Util.getDomain()) + url + (url.indexOf('?') != -1 ? "&m=" + Math.random() : ""),
        data: data,
        async: (isAsc == null || isAsc),
        dataType: datatype
    }).done(function (data) {
        if (callback != null && callback != 'undefined') {
            callback(data);
        }
    }).fail(function (data) {
        if (undefined != data && "XMLHttpRequest.LoginOut" == data.responseText) {
            alert("登录超时，请重新登录");
            window.location.href = App_G.Util.getDomain();
        } else if (undefined != data && "XMLHttpRequest.InvalidAccount" == data.responseText) {
            alert("非法的请求,请操作当前账号！");
            window.location.href = App_G.Util.getDomain() + "/Main/Index";
        } else {
            alert("处理出现异常!");
        }
    });
}

