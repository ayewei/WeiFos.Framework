
/**
  * 注册页面脚本
  * @author 叶委    
  * date 2016-01-16
 */
$(function () {

    //初始化验证插件
    yw.valid.config({
        submitelements: "#registerBtn", vsuccess: function () {

            $("#registerBtn").setDisable({ text: "注 册 中...", time: 2000 });

            setMaxDigits(131);
            var key = new RSAKeyPair($("#Exponent").val(), "", $("#Modulus").val());
            var posx = encryptedString(key, base64encode($("#pass_word").val()));
            //var posx = encryptedString(key, base64encode($("#pass_word").val()) + "\\" + base64encode($("#rpass_word").val()));
           
            var data = {
                user: App_G.Util.getJson("data-val"),
                vcode: $("#code").val(),
                loginpk: $("#LoginPK").val(),
                posx: posx
            };

            $postByAsync("/Home/GetRegister", JSON.stringify(data), function (backdata) {
                if (backdata.Code == App_G.Code.Code_200) {
                    App_G.MsgBox.success_digbox();
                    var url = App_G.Util.getDomain() + "/Main/Index";
                    setTimeout("window.location.href ='" + url + "'", 1000);
                } else {
                    App_G.MsgBox.error_digbox(backdata.Message);
                    $("#SaveBtn").val("保 存");
                    $("#SaveBtn").prop("disabled", false);
                }
            });
        }
    });

    //获取验证对象
    var v = yw.valid.getValidate();

    //登陆账号 
    v.valid("#login_name", {
        focusmsg: "长度为6~16位字符，可以为“数字/字母/中划线/下划线”组成",
        errormsg: "长度为6~16位字符，可以为“数字/字母/中划线/下划线”组成",
        othermsg: "该用户名已经存在",
        repeatvalidate: { url: App_G.Util.getDomain() + "/Home/ExistLoginName" },
        vtype: verifyType.isSerialNumber
    });

    //密码 
    v.valid("#pass_word", {
        focusmsg: "请输新的密码",
        errormsg: "长度在6~16 之间，任意字符",
        othermsg: "两次密码不一致", vtype: verifyType.isPassword, isrepeatelement: true
    });

    //确定密码 
    v.valid("#rpass_word", {
        focusmsg: "确定密码",
        errormsg: "长度在6~16 之间，任意字符",
        othermsg: "两次密码不一致", vtype: verifyType.isPassword, isrepeatelement: true
    });

    //手机号码 
    v.valid("#mobile_number", {
        focusmsg: "请输入您的手机号码",
        errormsg: "请输入正确的手机号码",
        othermsg: "该邮箱已经存在",
        repeatvalidate: { url: App_G.Util.getDomain() + "/Home/ExistMobile" },
        vtype: verifyType.isMoblie
    });

    //QQ号码 
    v.valid("#qq", {
        focusmsg: "请输入您的QQ号码（选填）",
        errormsg: "请输入正确的QQ号码",
        vtype: verifyType.isNumber,
        selectvalidate: true
    });
  
    //邮箱 
    v.valid("#email", {
        focusmsg: "邮箱将与支付及优惠相关，请填写正确的邮箱",
        errormsg: "请输入正确的邮箱",
        othermsg: "该邮箱已经存在",
        vtype: verifyType.isEmail,
        repeatvalidate: { url: App_G.Util.getDomain() + "/Home/ExistEmail" },
    });

    //验证码
    v.valid("#vcode", {
        focusmsg: "请输入验证码",
        afterelementId: "code",
        errormsg: "长度必须是4位",
        vtype: verifyType.validateCode
    });

    //更换验证码
    $("#code").click(function () {
        $(this).prop("src", "/Home/GetVerifyCode?type=register&m=" + Math.random());
    });

});


function base64encode(str) {
    var base64EncodeChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
    var base64DecodeChars = new Array(
    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 62, -1, -1, -1, 63,
    52, 53, 54, 55, 56, 57, 58, 59, 60, 61, -1, -1, -1, -1, -1, -1,
    -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
    15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, -1, -1, -1, -1, -1,
    -1, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
    41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, -1, -1, -1, -1, -1);
    var out, i, len;
    var c1, c2, c3;

    len = str.length;
    i = 0;
    out = "";
    while (i < len) {
        c1 = str.charCodeAt(i++) & 0xff;
        if (i == len) {
            out += base64EncodeChars.charAt(c1 >> 2);
            out += base64EncodeChars.charAt((c1 & 0x3) << 4);
            out += "==";
            break;
        }
        c2 = str.charCodeAt(i++);
        if (i == len) {
            out += base64EncodeChars.charAt(c1 >> 2);
            out += base64EncodeChars.charAt(((c1 & 0x3) << 4) | ((c2 & 0xF0) >> 4));
            out += base64EncodeChars.charAt((c2 & 0xF) << 2);
            out += "=";
            break;
        }
        c3 = str.charCodeAt(i++);
        out += base64EncodeChars.charAt(c1 >> 2);
        out += base64EncodeChars.charAt(((c1 & 0x3) << 4) | ((c2 & 0xF0) >> 4));
        out += base64EncodeChars.charAt(((c2 & 0xF) << 2) | ((c3 & 0xC0) >> 6));
        out += base64EncodeChars.charAt(c3 & 0x3F);
    }
    return out;
}
 