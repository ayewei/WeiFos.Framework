/**
 * Jquery.Upload    
 * @author 叶委          2015-01-15
 * @url                  服务器地址
 * @title                显示标题
 * @defimgurl            缺省图片路劲
 * @imgurl               初始化图片路劲
 * @fileType             限制图片类型
 * @bizType              业务类型
 * @bizID                业务ID
 * @imgHeight            图片默认高度
 * @imgWidth             图片默认宽度
 * @maxSize              图片上传最大限制单位M
 * @createThmImg         生成缩略图，0不生成、1小图、2中图
 * @callBack             上传成功回传方法
*/
(function ($) {
    $.extend($.fn, {
        upload: function (options) {
            //相关参数配置
            options = $.extend(true, options, {
                url: (options.url == null || $.trim(options.url) == "") ? App_Config.getResDomain() + "/Up/UploadFile" : options.url,
                title: options.title,
                imgurl: options.imgurl,
                cData: options.cData,
                fileType: "gif|jpg|jpeg|png|bmp",
                imgHeight: options.imgHeight,
                imgWidth: options.imgWidth,
                maxSize: options.maxSize,
                callBack: options.callBack
            });

            //上传主函数
            this.each(function () {
                //获取当前元素
                var $this = $(this);

                //克隆参数，处理统一对象共享传递
                var _options = $.extend(true, {}, options);

                //初始化上传控件
                var uploador = uploader(_options, $this);
                uploador.onInit(options, $this);

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

                    //图片
                    var img = wrap.find("img.thumb_img");

                    //图片缺省路径
                    img.attr("src", options.imgurl == undefined || $.trim(options.imgurl).length == 0 ? options.defimgurl : options.imgurl);
                    img.attr("default-src", options.defimgurl);

                    //避免重复绑定
                    wrap.find(".btn_delPic").unbind('click').click(function () {
                        if (img.attr("src") != img.attr("default-src")) {
                            if (confirm("确定删除该图片")) {
                                $this.delImage(options, wrap);
                            }
                        } else {
                            alert("没有上传的图片");
                        }
                    });

                    //if (options.imgHeight != null) img.css("height", options.imgHeight);
                    //if (options.imgWidth != null) img.css("width", options.imgWidth);
                    wrap.find("div.picThumbBar.disBlock").css("height", options.imgHeight);
                    wrap.find("div.picThumbBar.disBlock").css("width", options.imgWidth);

                    obj.find("[type=button]").bind("click", function () {
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
                    if (typeof val2 !== 'string' || val2 === "") { val2 = "gif|jpg|jpeg|png|bmp"; }
                    return new RegExp("\.(" + val2 + ")$").test(val1);
                },
                //创建上传表单
                createForm: function () {
                    var uploadform = document.createElement("form");
                    uploadform.action = options.url + "?action=uploadimage&r=" + Math.random();
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

                    //绑定的图片
                    var img = wrap.find("img.thumb_img");

                    //获取上传图片
                    var file = wrap.find("input:file");

                    //获取隐藏域
                    var hiddenfile = wrap.find("input:hidden");

                    //判断是否选择图片
                    if (file.val() == null || $.trim(file.val()) == "") {
                        alert("请选择要上传的图片！");
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

                    img.hide();

                    //设置进度条
                    $("<div class='loader' ></div>").appendTo(wrap.find("div.picThumbBar.disBlock"));

                    var params = {};
                    params.ticket = options.ticket;
                    params.maxSize = options.maxSize;
                    params.createThmImg = options.createThmImg;
                    for (var attr in options.cData) {
                        params[attr] = options.cData[attr];
                    }

                    try {
                        //开始ajax提交表单
                        form.ajaxSubmit({
                            type: "POST",
                            data: params,
                            success: function (data) {

                                if (data.state == 200) {
                                    $this.showImage(wrap, options, data);
                                    options.callback(data);
                                }
                                wrap.find(".divUpload.clearfix.ml10").append('<input name = "upfile"  class="fileUpload" type="file" >');
                                wrap.find("input:file").bind("change", function () { wrap.find("span.button-text").text(file.val()); });
                                upbtn.attr("disabled", false);
                            }, error: function (xhr, XMLHttpResponse) {
                                var ddd = xhr;
                                alert(xhr);
                            }
                        });
                    } catch (e) {
                        alert(e.message);
                    }
                },
                //上传后显示图片 pictureId
                showImage: function (wrap, options, data) {
                    $this = this;

                    var img = wrap.find("img.thumb_img");
                    img.attr("src", data.url);
                    img.show();
                    wrap.find("div.loader").hide();

                    //设置对应图片的隐藏域Id
                    wrap.find("input:hidden").val(data.data);

                    //if (options.imgHeight != null) {
                    //    img.css("height", options.imgHeight);
                    //}

                    //if (options.imgWidth != null) {
                    //    img.css("width", options.imgWidth);
                    //}

                    //显示标题
                    var title = wrap.find("span.button-text");
                    if (options.title != null && options.title != "") {
                        title.text(options.title);
                    } else {
                        title.text("选择图片");
                    }

                    //绑定查看原图事件
                    img.bind("dblclick", function () {
                        window.open(App_G.Util.getDomain() + img.attr("src"), "_blank");
                    });
                },
                //删除图片
                delImage: function (options, wrap) {
                    var msg, i_length, index;
                    var imgsrc = wrap.find("img.thumb_img").attr("src");
                    index = imgsrc.lastIndexOf('/');
                    i_length = imgsrc.length - (imgsrc.length - imgsrc.lastIndexOf('.'));
                    msg = imgsrc.substring(index + 1, i_length);

                    if ($.trim(msg) != "" && msg != null) {

                        var url = App_Config.getResDomain() + "/Up/DeleteFile" + "?bizType=" + options.cData.bizType + "&imgmsg=" + msg + "&ticket=" + options.cData.ticket + "&bizId=" + options.cData.bizId;
                        $get(url, {}, function (result) {

                            if (result == App_G.Code.Code_200) {
                                var img = wrap.find("img.thumb_img");
                                img.attr("src", img.attr("default-src"));//options.defimgurl
                                wrap.find("input:hidden").val("");
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
    var upload_temp = '<div><div class="picThumbBar disBlock">\
        <a href="javascript:void(0);" class="btn_delPic fa fa-trash-o fs14" ></a>\
        <img class="thumb_img" src="" default-src="" />\
        </div><div class="divUpload clearfix ml10"><input type="file" class="fileUpload" name="upfile" />\
        <a class="qbutton button button-upload clearfix" onclick="return false;" href="javascript:;">\
        <span class="button-left"></span><span class="button-text" >选择图片</span>\
        <span class="button-right"></span></a><input type="hidden" /></div>\
        <input type="button" class="btn btn-small btn-primary btn_unpload btn_mbottom" value="上传"   />\
        </div>';

})(jQuery);
