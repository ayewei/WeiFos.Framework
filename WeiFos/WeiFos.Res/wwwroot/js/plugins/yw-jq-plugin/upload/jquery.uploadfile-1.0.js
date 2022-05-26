/**
 * Jquery.Upload    
 * @author 叶委          2015-01-15
 * @update date          2020-07-25
 * @url                  服务器地址
 * @title                显示标题
 * @isImport             是否导入数据
 * @fileType             限制图片类型
 * @bizType              业务类型
 * @bizID                业务ID
 * @maxSize              上传最大限制单位M
 * @callBack             上传成功回传方法
*/
(function ($) {
    $.extend($.fn, {
        upload: function (options) {
            //相关参数配置
            options = $.extend(options, {
                url: (options.url == null || $.trim(options.url) == "") ? App_Config.getResDomain() + "/Api/Up/UploadFile" : options.url,
                title: options.title,
                isImport: options.isImport,
                resurl: options.resurl,
                bizType: options.bizType,
                bizId: options.bizId,
                maxSize: options.maxSize,
                callBack: options.callBack
            });

            //上传主函数
            this.each(function () {
                //获取当前元素
                var $this = $(this);
                //克隆参数，处理统一对象共享传递，
                //Boolean类型 指示是否深度合并对象，默认为false。如果该值为true，
                //且多个对象的某个同名属性也都是对象，则该"属性对象"的属性也将进行合并
                var _options = $.extend(true, {}, options)
                //初始化上传控件
                var uploador = uploader(_options, $this);
                uploador.onInit(_options, $this);
            });
        }
    });

    /// <summary>
    /// 初始化上传控件
    /// </summary>
    /// <param name="options" type="Object">参数对象</param>
    /// <param name="wrap" type="Object">上传体(整个上传体)</param>
    var uploader = function (options, wrap) {
        $.extend(uploader, {
            methods: {
                //控件初始化
                onInit: function (options, obj) {
                    var $this = this;

                    if (obj.attr("id") == undefined) {
                        options.controlId = new Date().getTime();
                        $(upload_temp).attr("id", options.controlId).appendTo(obj);
                    } else {
                        options.controlId = obj.attr("id");
                        $(upload_temp).appendTo(obj);
                    }

                    //查找图片上传file
                    var file = wrap.find("input:file");

                    //显示标题
                    var title = wrap.find("span.button-text");
                    title.text(options.text)

                    //绑定file的change事件
                    file.bind("change", function () { title.text(file.val()); });

                    //避免重复绑定
                    wrap.find("#delete_btn").unbind('click').click(function () {
                        if ($.trim(wrap.find("a#file_text").text()).length > 0) {
                            if (confirm("确定删除该资源")) {
                                $this.delFiles(options, wrap);
                            }
                            return;
                        } else {
                            alert("没有上传的资源");
                        }
                    });

                    if (undefined != options.resurl && options.resurl.indexOf("http://") != -1) {
                        var index = options.resurl.lastIndexOf(".");
                        var len = options.resurl.lastIndexOf("/");
                        var file_title = options.resurl.substr(len + 1, options.resurl.length - len);
                        wrap.find("a#file_text").text(file_title);
                        wrap.find("a").attr("href", options.resurl);
                        wrap.find("#delete_btn").show();
                    }

                    obj.find("#up_btn").bind("click", function () {
                        //获取当前元素
                        var upbtn = $(this);
                        $this.upload(options, obj, upbtn);
                    });
                },
                //检查文件后缀 
                checkFile: function (val1, val2) {
                    //获得文件后缀
                    val1 = val1.substring(val1.lastIndexOf("."), val1.length)
                    val1 = val1.toLowerCase();
                    //if (typeof val2 !== 'string' || val2 === "") { val2 = "gif|jpg|jpeg|png|bmp"; }
                    //return new RegExp("\.(" + val2 + ")$").test(val1);
                    return true;
                },
                //创建上传表单
                createForm: function () {
                    var uploadform = document.createElement("form");
                    uploadform.action = options.url + (options.url.indexOf('?') != -1 ? "&m=" + Math.random() : "");
                    uploadform.method = "post";
                    uploadform.enctype = "multipart/form-data";
                    uploadform.style.display = "none";
                    //将表单加当document上，
                    //创建表单后一定要加上这句否则得到的form不能上传。document后要加上body,否则火狐下不行。
                    document.body.appendChild(uploadform);
                    return $(uploadform);
                },
                //图片上传
                upload: function (options, obj, upbtn) {
                    var $this = this;

                    //按钮不可用
                    upbtn.attr("disabled", true);

                    //控件
                    var wrap = $("#" + options.controlId);

                    //获取上传图片
                    var file = wrap.find("input:file");

                    //判断是否选择图片
                    if (file.val() == null || $.trim(file.val()) == "") {
                        alert("请选择要上传的文件！");
                        upbtn.attr("disabled", false);
                        return false;
                    }

                    //验证图片类型
                    if (!$this.checkFile(file.val(), options.fileType)) {
                        alert("文件格式不正确，只能上传格式为：" + options.fileType + "的文件。");
                        upbtn.attr("disabled", false);
                        return false;
                    }

                    //创建表单
                    var form = $this.createForm();
                    file.appendTo(form);

                    try {
                        var layer_box = layer.load(1, { shade: [0.6, '#fff'] });

                        //提交的参数
                        var param = {};
                        //将初始化的参数合并
                        if (options.data != undefined) {
                            for (var attr in options.data) {
                                param[attr] = options.data[attr];
                            }
                        }

                        //将动态的参数合并
                        if (App_G.Util.isFunction(options.setData)) {
                            var data = options.setData();
                            for (var attr in data) {
                                param[attr] = data[attr];
                            }
                        }

                        //开始ajax提交表单
                        form.ajaxSubmit({
                            type: "POST",
                            data: param,
                            success: function (backdata) {
                                layer.close(layer_box);
                                //如果不是导入数据情况
                                if (!options.isImport) {
                                    //采用百度Ueditor hander 处理
                                    data = backdata;
                                    //兼容老版本百度编辑器
                                    var data = data.replace(/(<\/?(pre)[^>\/]*)\/?>/gi, "");
                                    data = eval("(" + data + ")");
                                    if (data.state == "success") {
                                        //显示标题
                                        var title = wrap.find("span.button-text");
                                        if (options.title != null && options.title != "") {
                                            title.text(options.title);
                                        } else {
                                            title.text("选择文件");
                                        }
                                        wrap.find("a#file_text").text(data.data + "." + data.original.split(".")[1]);
                                        wrap.find("a").attr("href", data.url);
                                        wrap.find("#delete_btn").show();
                                    }
                                } else {
                                    var res = eval("(" + backdata + ")");
                                    //显示标题
                                    var title = wrap.find("span.button-text");
                                    if (options.title != null && options.title != "") {
                                        title.text(options.title);
                                    } else {
                                        title.text("选择文件");
                                    }

                                    if (res.Code != 200) alert("上传失败!");
                                    options.callback(res);
                                }

                                wrap.find(".divUpload.clearfix").append('<input name = "upfile"  class="fileUpload" type="file" >');
                                wrap.find("input:file").bind("change", function () { wrap.find("span.button-text").text(file.val()); });
                                upbtn.attr("disabled", false);
                            }, error: function (xhr, XMLHttpResponse) {
                                var ddd = xhr;
                                alert(xhr);
                            }
                        });
                    } catch (e) {
                        layer.close(layer_box);
                        alert(e.message);
                    }
                },
                //删除资源
                delFiles: function (options, wrap) {
                    var msg, i_length, index;

                    var resurl = wrap.find("a#file_text").text();
                    if (undefined != resurl && resurl.length > 0) {
                        index = resurl.lastIndexOf('/');
                        i_length = resurl.length - (resurl.length - resurl.lastIndexOf('.'));
                        msg = resurl.substring(index + 1, i_length);
                    }

                    if ($.trim(msg) != "" && msg != null) {

                        var data = "?action=deletehandle&bizType=" + options.bizType + "&htype=delimg&imgmsg=" + msg;
                        $post(options.url + data, {}, function (backdata) {

                            if (backdata == App_G.Code.Code_200) {
                                wrap.find("a#file_text").text("");
                                wrap.find("a#file_text").attr("href", "javascript:;");
                            } else {
                                alert("操作失败");
                            }

                        });
                    }
                }
            }
        });
        return uploader.methods;
    };



    /// <summary>
    /// 上传模板
    /// </summary>
    var upload_temp = '<div>\
        <div class="divUpload clearfix"><input type="file" class="fileUpload" name="upfile" />\
        <a class="qbutton button button-upload clearfix" onclick="return false;" href="javascript:;">\
        <span class="button-left"></span><span class="button-text" >选择文件</span>\
        <span class="button-right"></span></a><input type="hidden" /></div>\
        <a id="file_text"></a>\
        <input type="button" class="btn btn-small btn-primary btn_unpload btn_mbottom" value="删除" id="delete_btn" style="display:none;"   />\
        <input type="button" class="btn btn-small btn-primary btn_unpload btn_mbottom" value="上传" id="up_btn"  />\
        </div>';

})(jQuery);
