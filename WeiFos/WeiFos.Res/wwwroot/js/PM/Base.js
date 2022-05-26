
/**
  * App_G 平台脚本基类
  * @author 叶委    2015-02-11
 */
var App_G = {
    //图片类型路径
    ImgType: {
        /// 缺省图片类型
        Default: "Default",
        /// 公共图片类型
        Commom: "Commom",
        /// 用户账号图片
        User_Account: "User_Account",
        /// 官网封面图片
        WebSite_Title: "WebSite_Title",
        /// 官网背景图片
        WebSite_Bg: "WebSite_Bg",
        /// 官网广告图片
        WebSite_Ad: "WebSite_Ad",
        /// 官网logo图片
        WebSite_Logo: "WebSite_Logo",
        /// 图文 封面图
        ImgTextReply_Title: "ImgTextReply_Title",
        /// 图文 内容图
        ImgTextReply_Details: "ImgTextReply_Details",
        ///官网栏目封面图片
        WebSite_CgtyTitle: "WebSite_CgtyTitle",
        ///相册空间头部图片
        AlbumZone: "AlbumZone",
        ///相册消息封面图片
        AlbumTitle: "AlbumTitle",
        ///相册照片
        AlbumPhoto: "AlbumPhoto",
        ///活动开始图片
        Activity_Start: "Activity_Start",
        ///活动中奖图片
        Activity_Win: "Activity_Win",
        ///活动结束图片
        Activity_End: "Activity_End",
        ///活动内容图
        Activity_Details: "Activity_Details",
        ///投票封面图
        Vote_Title: "Vote_Title",
        ///投票选项图片
        Vote_Option: "Vote_Option",
        ///调研封面图
        Survey_Title: "Survey_Title",
        ///LBS 封面图
        LbsReply_Title: "LbsReply_Title",
        ///LBS 内容图
        LbsReply_Details: "LbsReply_Details",
        ///留言板 封面图
        Message_Title: "Message_Title",
        ///留言板 头部图
        Message_Header: "Message_Header",
        ///微酒店 全景图
        Pano_Hotel: "Pano_Hotel",
        ///微预约 封面图
        Resv_Header: "Resv_Header",
        ///微预约订单页 顶部图
        Resv_Title: "Resv_Title",
        ///微信息会员卡 封面图
        Member_Card_Cover: "Member_Card_Cover",
        ///微信会员卡正面背景图
        Member_Card_Positive: "Member_Card_Positive",
        ///微信会员卡反面背景图
        Member_Card_Negative: "Member_Card_Negative",
        ///微信会员卡Logo图
        Member_Card_Logo: "Member_Card_Logo"
    },
    //响应状态
    BackState: {
        // 系统错误
        State_500: 500,
        // 响应成功 
        State_200: 200,
        // 验证通过 
        State_0: 0,
        // 验证未通过
        State_1: 1,
        // 数据不存在
        State_30030: 30030,
        // 用户未登陆
        State_30045: 30045,
        // 已达到定义上限
        State_30060: 30060
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
        MemberCard: "MemberCard"
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
    //权限集合
    AuthCode: "",
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
                    return new Date(timestamp + milliseconds);
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
            //json格式日期处理（时分秒）
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
        //获取原图地址
        getImgUrl: function (url) {
            if (undefined == url || $.trim(url).length == 0) return App_Config.getResDomain() + "/Content/DefaultRes/Image/NoProduct.jpg";
            if (url.indexOf("http://") != -1) {
                return url;
            } else {
                return App_Config.getResDomain() + url;
            }
        },
        //获取小图地址
        getImgUrl_s: function (url) {
            if (undefined == url || $.trim(url).length == 0) return App_Config.getResDomain() + "/Content/DefaultRes/Image/NoProduct.jpg";
            if (url.indexOf("http://") != -1) {
                return url.replace("/Image/", /Thm_Image/);
            } else {
                return App_Config.getResDomain() + url.replace("/Image/", /Thm_Image/);
            }
        },
        //获取中图地址
        getImgUrl_m: function (url) {
            if (undefined == url || $.trim(url).length == 0) return App_Config.getResDomain() + "/Content/DefaultRes/Image/NoProduct.jpg";
            if (url.indexOf("http://") != -1) {
                return url.replace("/Image/", /Med_Image/);
            } else {
                return App_Config.getResDomain() + url.replace("/Image/", /Med_Image/);
            }
        },
        //获取Oss图片地址
        getOssImgUrl: function (filename, param) {
            if (undefined == filename || filename == null || $.trim(filename).length == 0) {
                return App_Config.getResDomain() + "/Content/DefaultRes/Image/NoProduct.jpg";
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
            return JSON.stringify(App_G.Util.getJson(attr, datas));
        },
        //获取自定义json 对象
        getJson: function (attr, datas) {
            var data = {};
            $.each($("select[" + attr + "],input[" + attr + "],textarea[" + attr + "]"), function (i, v) {

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
                        data[$(v).attr(attr)] = $("input:radio[" + attr + "]:checked").val();
                        break;
                    default:
                        data[$(v).attr(attr)] = $(v).val();
                        break;
                }
            });
            for (var _attr in datas) {
                data[_attr] = datas[_attr];
            }
            return data;
        },
        //绑定json对象到页面
        bindJson: function (attr, datas) {

            for (var _attr in datas) {

                //遍历页面元素
                $.each($("input[" + attr + "],select[" + attr + "],textarea[" + attr + "],label[" + attr + "]"), function (i, o) {

                    //通过ID关联
                    if (($(o).attr("id") != undefined && $(o).attr("id") == _attr) || $(o).attr(attr) == _attr) {

                        switch ($(o).prop("type")) {
                            //文本输入框
                            case "text":
                                $(o).val(datas[_attr]);
                                break;
                                //文本输入框
                            case "textarea":
                                $(o).val(datas[_attr]);
                                break;
                                //下拉选择框
                            case "select-one":
                                if (datas[_attr] != null) {
                                    $(o).val(datas[_attr].toString());
                                }
                                break;
                                //复选框
                            case "checkbox":
                                if (undefined != datas[_attr] && datas[_attr].toString().toLowerCase() == "true") {
                                    $(o).attr("checked", true);
                                } else {
                                    $(o).attr("checked", false);
                                }
                                break;
                                //单选框
                            case "radio":
                                if (datas[_attr].toString() == $(o).val()) $(o).attr("checked", true);
                                else $(o).attr("checked", false);
                                break;
                            case "hidden":
                                $(o).val(datas[_attr]);
                                break;
                            default:
                                $(o).text(datas[_attr]);
                                $(o).val(datas[_attr]);
                                break;
                        }
                    }

                });
            }
        },
        //获取权限编号
        getPCodes: function () {
            var url = "/System/GetMyPCode?url=" + window.location.pathname + "&pcode=";
            if ($("[data-pcode]").length) url += $("[data-pcode]").val();
            $getByAsync(url, "",
            function (backdata) {
                //var data = $.parseJSON(backdata);
                App_G.AuthCode = backdata;
            });
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
        },
        parseInt: function (value) {
            return Number(value);
        }
    },
    //消息提示框
    MsgBox: {
        //操作成功提示信息框
        success_digbox: function (msg, time) {
            if (msg == undefined || msg == null) { msg = "操作成功"; }
            if (!$('.msgbox_layer_wrap').length) {
                var remindhtml = "<div class='msgbox_layer_wrap' style='display:none;' >"
                + "<span style='z-index: 10000;' class='msgbox_layer'><span class='gtl_ico_succ'></span>"
                + msg + "<span class='gtl_end'></span></span></div>";
                $(remindhtml).appendTo('body');
            }
            var remind = $('.msgbox_layer_wrap');
            remind.fadeIn('slow');

            if (undefined != time && App_G.Util.isNum(time)) {
                setTimeout("$('.msgbox_layer_wrap').fadeOut('slow');", time);
                setTimeout("$('.msgbox_layer_wrap').remove();", time);
            } else {
                setTimeout("$('.msgbox_layer_wrap').fadeOut('slow');", 1500);
                setTimeout("$('.msgbox_layer_wrap').remove();", 1500);
            }
        },
        //操作失败提示框
        error_digbox: function (msg, time) {
            if (msg == undefined || msg == null) { msg = "操作失败"; }
            if (!$('.msgbox_layer_wrap_e').length) {
                var remindhtml = "<div class='msgbox_layer_wrap_e' style='display:none;' >"
                + "<span style='z-index: 10000;' class='msgbox_layer_e'><span class='gtl_ico_succ_e'></span>"
                + msg + "<span class='gtl_end_e'></span></span></div>";
                $(remindhtml).appendTo('body');
            }

            var remind = $('.msgbox_layer_wrap_e');
            remind.fadeIn('slow');

            if (undefined != time && App_G.Util.isNum(time)) {
                setTimeout("$('.msgbox_layer_wrap_e').fadeOut('slow');", time);
                setTimeout("$('.msgbox_layer_wrap_e').remove();", time);
            } else {
                setTimeout("$('.msgbox_layer_wrap_e').fadeOut('slow');", 1500);
                setTimeout("$('.msgbox_layer_wrap_e').remove();", 1500);
            }

        }
    }
}


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
                var _options = {};
                _options["text"] = options.text;
                _options["time"] = options.time;

                var $o = $(this), t = $.trim($o.text()) == "" ? $o.val() : $o.text();

                //未设置文本
                if (undefined != options.text && $.trim(options.text).length != 0) {
                    $o.text(options.text);
                    $o.val(options.text);
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
(function ($) {
    $.extend($.fn, {
        digbox: function (options) {
            options = $.extend(options, {
                Title: options.Title,
                Context: options.Context,
                Backdrop: options.Backdrop,
                Selector: options.Selector,
                Event: options.Event,
                Before: options.Before,
                Cencel: options.Cencel,
                Show: options.Show,
                CallBack: options.CallBack,
                IsAuto: options.IsAuto
            });

            var current;
            if (undefined == options.Selector) {
                current = $(document);
            } else {
                current = $(options.Selector);
            }

            if (options.IsAuto) {
                var dig = digboxer(options, $(this));
                return dig.show($(this));
            } else {
                current.on(undefined == options.Event ? "click" : options.Event, this.selector, function (e) {
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
                    var $this = this;

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

                    $(current_dig).appendTo('body');

                    //标题
                    current_dig.find("h3").text(options.Title);

                    //标题
                    current_dig.find("div.modal-body").html(options.Context);

                    //监听隐藏事件
                    $("#" + options.controlId).on('hidden', function () {
                        setTimeout("$('#" + options.controlId + "').remove()", 500);
                    });

                    //X按钮
                    current_dig.find(".close").unbind('click').click(function (e) {
                        $this.backmethod(obj, options.Cencel);
                    });

                    //确定
                    current_dig.find("button:eq(2)").attr("id", options.controlId + "_submit");
                    current_dig.find("button:eq(2)").unbind('click').click(function () {
                        $this.backmethod(obj, options.CallBack(current_dig.find("button:eq(2)").attr("id"), obj, current_dig));
                    });

                    //取消
                    current_dig.find("button:eq(1)").unbind('click').click(function () {
                        $this.backmethod(obj, options.Cencel);
                    });


                    $(current_dig).modal((undefined == options.Backdrop ? "show" : { backdrop: 'static', keyboard: false })).css({ "width": "auto", "margin-left": function () { return -($(this).width() / 2); } });


                    $(current_dig).modal((undefined == options.Backdrop ? "show" : { backdrop: 'static', keyboard: false })).css({ "height": "auto", "margin-top": function () { return -($(this).height() / 2); } });

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
                    $("#" + options.controlId).modal('hide');
                }
            }
        });
        return digboxer.methods;
    };

    //弹出窗模板
    var digbox_tmp = '<div class="modal hide fade" role="dialog" style="min-width:540px;"  aria-labelledby="myModalLabel" aria-hidden="true">\
                     <div class="modal-header">\
                     <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>\
                     <h3></h3>\
                     </div>\
                     <div class="modal-body"></div>\
                         <div class="modal-footer">\
                         <button class="btn" aria-hidden="true"  >关&nbsp;闭</button>\
                         <button class="btn btn-primary" >确&nbsp;定</button>\
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

//日历控件js
function selected(cal, date) {
    cal.sel.value = date;
    if (cal.dateClicked && cal.sel.id == "sel1")
        cal.callCloseHandler();
}

function closeHandler(cal) {
    cal.hide();
    _dynarch_popupCalendar = null;
}

function showCalendar(id, format, showsTime, showsOtherMonths) {

    var el = document.getElementById(id);
    if (_dynarch_popupCalendar != null) {
        _dynarch_popupCalendar.hide();
    } else {
        var cal = new Calendar(1, null, selected, closeHandler);
        if (typeof showsTime == "string") {
            cal.showsTime = true;
            cal.time24 = (showsTime == "24");
        }
        if (showsOtherMonths) {
            cal.showsOtherMonths = true;
        }
        _dynarch_popupCalendar = cal;
        cal.setRange(1900, 2070);
        cal.create();
    }
    _dynarch_popupCalendar.setDateFormat(format);    // set the specified date format
    _dynarch_popupCalendar.parseDate(el.value);      // try to parse the text in field
    _dynarch_popupCalendar.sel = el;                 // inform it what input field we use
    _dynarch_popupCalendar.showAtElement(el, "Br");        // show the calendar
    return false;
}
//日历控件js

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


$(function () {
    var hover = function () {
        $("#listTable,#d_listTable tbody").on("mouseenter", "tr", function () {
            $(this).addClass("catehover");
        })
        $("#listTable,#d_listTable tbody").on("mouseleave", "tr", function () {
            $(this).removeClass("catehover");
        })
    }();
    var click = function () {
        $("#listTable,#d_listTable tbody").on("click", "tr", function () {
            $(this).addClass("cateclick").siblings().removeClass("cateclick");
        })
    }();
});