/**
 * Jquery.Upload    
 * @author 叶委          2015-01-15
 * @url                  服务器地址
 * @fileCount            上传最多数量
 * @auto                 是否是自动上传
 * @imgurl               初始化图片路劲
 * @defimgurl            缺省图片路劲
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
        flashUp: function (options) {
            //相关参数配置
            options = $.extend(true, {}, {
                url: (options.url == null || $.trim(options.url) == "") ? App_Config.getResDomain() + "/Up/UploadFile" : options.url,
                fileCount: options.fileCount,
                auto: undefined == options.auto ? false : options.auto,
                imgurl: options.imgurl,
                defimgurl: options.defimgurl,
                cData: options.cData,
                fileType: "gif|jpg|jpeg|png|bmp",
                imgHeight: options.imgHeight,
                imgWidth: options.imgWidth,
                maxSize: options.maxSize,
                startUp: options.startUp,
                upSuccess: options.upSuccess,
                upError: options.upError,
                upComplete: options.upComplete,
                upProgress: options.upProgress
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
                    } else {
                        options.controlId = obj.attr("id");
                    }

                    //控件最外围ID
                    options.wrapId = "wrap_" + options.controlId;
                    options.upId = "up_" + options.controlId;
                    //控件外围包裹元素
                    obj.wrap($(upload_temp).attr("id", options.wrapId)).parent().addClass("uploadImgBar");
                    $("#" + options.controlId).addClass("cp_img_jia");
                    $("#" + options.wrapId).prepend("<div class='filelist'></div>");
                    $("#" + options.wrapId).append("<input type='button' value='上传' class='btn btn-submit' />");

                    if (App_G.Util.isArray(options.imgurl)) {
                        for (var i = 0; i < options.imgurl.length; i++) {
                            $this.bindImg(options.imgurl[i], options.wrapId, options.cData.bizType);
                        }
                    } else {
                        $this.bindImg(options.imgurl, options.wrapId, options.cData.bizType);
                    }

                    //缺省图
                    $this.bindDefImg(options.defimgurl, options.wrapId);

                    //第三方上传插件
                    var uploader = WebUploader.create({
                        fileNumLimit: (App_G.Util.isNum(options.fileCount) ? options.fileCount : 1),
                        //根据文件名字、文件大小和最后修改时间来生成hash Key.
                        duplicate: true,
                        // 选完文件后，是否自动上传。
                        auto: options.auto,
                        disableGlobalDnd: true,
                        // swf文件路径
                        swf: App_Config.getResDomain() + "/Content/Resources/Script/plugin/webuploader/Uploader.swf",
                        // 文件接收服务端。
                        server: options.url,
                        //定义上传file名称
                        fileVal: undefined == options.fileVal ? "upfile" : options.fileVal,
                        // 选择文件的按钮。可选。
                        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
                        pick: {
                            id: "#" + options.controlId,
                            innerHTML: ""
                        },
                        //只允许选择图片
                        accept: {
                            title: 'Images',
                            extensions: 'gif,jpg,jpeg,bmp,png,pdf,xls,xlsx,docx',
                            mimeTypes: 'image/*,.pdf,.xls,.xlsx,.docx',
                        },
                        formData: options.cData
                    });

                    /*****上传按钮******/
                    var upbtn = $("#" + options.wrapId).find("[type=button]");
                    if (options.auto) {
                        upbtn.hide();
                    }  

                    //在优化retina下,ratio这个值是2。缩略图大小.
                    var ratio = window.devicePixelRatio || 1, thumbnailWidth = 90 * ratio, thumbnailHeight = 90 * ratio;

                    // 当有文件添加进来的时候
                    uploader.on('fileQueued', function (file) {
                        var $li = $('<div data-fileid="' + file.id + '" class="cp_img"><img><div class="cp_img_jian">删除</div></div>');
                        var $img = $li.find('img');

                        if ($("#" + options.wrapId).find("div.filelist>div").length < (App_G.Util.isNum(options.fileCount) ? options.fileCount : 1)) {
                            //添加到图片集合
                            $("#" + options.wrapId).find("div.filelist").append($li);

                            // 创建缩略图 如果为非图片文件，可以不用调用此方法。
                            uploader.makeThumb(file, function (error, src) {
                                if (error) { $img.replaceWith('<span>不能预览</span>'); return; }
                                $img.attr('src', src);
                            }, thumbnailWidth, thumbnailHeight);

                            upbtn.val("待上传");
                            upbtn.bind("click", function () {
                                uploader.upload();
                            });
                        } else {
                            $("#" + options.wrapId).find("div.filelist>li img").attr("src", "");
                            uploader.removeFile(uploader.getFile(file.id, true));
                        }

                        //绑定操作
                        $this.bindOp(options, uploader);
                    });

                    /*****开始上传事件******/
                    uploader.on('startUpload', function (file) {
                        undefined != options.startUp ? options.startUp() : "";
                        if (undefined != file) {
                            upbtn.val("上传中...");
                        }
                    });

                    /*****上传进度******/
                    uploader.on('uploadProgress', function (file, Number) {
                        upbtn.val("上传中...");
                    });

                    /*****上传成功******/
                    uploader.on('uploadSuccess', function (file, result) {
                        if (result.state == 200) {
                            upbtn.attr("disabled", false);
                            undefined != options.upSuccess ? options.upSuccess(result) : "";
                            $('[data-fileid=' + file.id + ']').addClass('upload-state-done');
                            $('[data-fileid=' + file.id + ']').attr('id', $this.getFileNmae(result.url));
                            $('[data-fileid=' + file.id + ']').attr('biz-type', options.cData.bizType);
                        } else {
                            alert(result.data);
                        }
                        upbtn.val("上传");

                        //缺省图
                        if (undefined != options.defimgurl && $("[biz-type=" + options.cData.bizType + "]").length == 0) {
                            $("#" + options.wrapId).find("[data-defurl=defurl]").show();
                        } else {
                            $("#" + options.wrapId).find("[data-defurl=defurl]").hide();
                        }
                    });

                    /*****上传完成******/
                    uploader.on('uploadComplete', function (file) {
                        undefined != options.upComplete ? options.upComplete() : "";
                    });

                    /*****上传失败******/
                    uploader.on('uploadError', function (file) {
                        alert("上传失败");
                        undefined != options.upError ? options.upError() : "";
                    });

                    //绑定操作
                    $this.bindOp(options, uploader);

                },
                getFileNmae: function (url) {
                    var msg, i_length, index;
                    index = url.lastIndexOf('/');
                    i_length = url.length - (url.length - url.lastIndexOf('.'));
                    return url.substring(index + 1, i_length);
                },
                //绑定操作
                bindOp: function (options, uploader) {

                    //显示删除按钮
                    $("#" + options.wrapId).find(".cp_img").on("mouseover", function () {
                        $(this).children(".cp_img_jian").css('display', 'block');
                    });
                    //隐藏删除按钮
                    $("#" + options.wrapId).find(".cp_img").on("mouseout", function () {
                        $(this).children(".cp_img_jian").css('display', 'none');
                    });

                    //执行删除方法
                    $("#" + options.wrapId).on("click", ".cp_img_jian", function () {
                        var Id = $(this).parent().attr("data-fileid");
                        if (undefined != Id) {
                            uploader.removeFile(uploader.getFile(Id, true));
                        }
                        $(this).parent().remove();

                        //缺省图
                        if (undefined != options.defimgurl && $("[biz-type=" + options.cData.bizType + "]").length == 0) {
                            $("#" + options.wrapId).find("[data-defurl=defurl]").show();
                        } else {
                            $("#" + options.wrapId).find("[data-defurl=defurl]").hide();
                        }
                    });
                    
                },
                //绑定图片
                bindImg: function (imgurl, wrapId, bizType) {
                    var $this = this;
                    if (undefined != imgurl) {
                        var $li = $('<div id="' + $this.getFileNmae(imgurl) + '" biz-type="' + bizType + '" class="cp_img" ><img><div class="cp_img_jian">删除</div></div>');
                        var $img = $li.find('img');
                        //添加到图片集合
                        $("#" + wrapId).find("div.filelist").append($li);

                        //设置文件地址 
                        if (/[^\s]+\.(jpg|gif|png|jpeg|bmp)/i.test(imgurl.toLowerCase())) {
                            $img.attr('src', imgurl);
                        } else {
                            $img.replaceWith('<span>不能预览</span>');
                        }
                    }

                },
                //绑定缺省图片
                bindDefImg: function (defimgurl, wrapId) {
                    var $this = this;
                    if (undefined != defimgurl) {
                        var $li = $('<div data-defurl="defurl" class="cp_img" style="display:none;" ><img></div>');
                        var $img = $li.find('img');
                        //添加到图片集合
                        $("#" + wrapId).find("div.filelist").append($li);
                        $img.attr('src', defimgurl);
                    }
                }



            }
        });
        return uploader.methods;
    };



    /// <summary>
    /// 上传模板
    /// </summary>
    var upload_temp = '<div></div>';

})(jQuery);
