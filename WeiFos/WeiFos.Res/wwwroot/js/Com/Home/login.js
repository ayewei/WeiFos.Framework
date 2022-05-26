

$(function () {

    //防止框架在某个窗口中打开
    if (window.top != window) {
        window.top.location.href = getDomain() + "/Home/login";
    }

 
    $("#loginname").keypress(function (e) {
        if (e.keyCode == 13 || e.which == 13) {
            login();
        }
    });

    $("#password").keypress(function (e) {
        if (e.keyCode == 13 || e.which == 13) {
            login();
        }
    });

    $("#vcode").keypress(function (e) {
        if (e.keyCode == 13 || e.which == 13) {
            login();
        }
    });

    $("#regbtn").click(function () {
        window.location.href = getDomain() + "register.html";
    });

    //更换验证码
    $("#code").click(function () {
        $(this).prop("src", "/Home/GetVerifyCode?type=login&m=" + Math.random());
    });

});


//登录
function login() {

    //登录框
    var div_login = $("div.d_login");

    //登陆名输入框
    var loginname = div_login.find("#loginname");

    //密码输入框
    var psw = div_login.find("#password");

    //验证码输入框
    var vcode = div_login.find("#vcode");

    //错误提示信息
    var errormess = div_login.find("#error");

    if ($.trim(loginname.val()) == "" && $.trim(psw.val()) == "") {
        div_login.find("li.con_area").css({ "border-color": "#ED888B", "color": "#BD4247" });
        errormess.text("用户或密码错误");
        errormess.show();
        return;
    }

    if ($.trim(loginname.val()) == "") {
        div_login.find("li.con_area").css({ "border-color": "#ED888B", "color": "#BD4247" });
        errormess.text("用户或密码错误");
        errormess.show();
        return;
    }

    if ($.trim(psw.val()) == "") {
        div_login.find("li.con_area").css({ "border-color": "#ED888B", "color": "#BD4247" });
        errormess.text("用户或密码错误");
        errormess.show();
        return;
    }

    if ($.trim(vcode.val()) == "") {
        vcode.css({ "border-color": "#ED888B", "color": "#BD4247" });
        errormess.text("用户或密码错误");
        errormess.show();
        return;
    }


    $("#loginbtn").setDisable({ text: "登 录 中...", time: 2000 });
    setMaxDigits(131);
    var key = new RSAKeyPair($("#Exponent").val(), "", $("#Modulus").val());
    var data = {
        loginname: loginname.val(),
        posx: encryptedString(key, base64encode(psw.val())),
        loginpk: $("#LoginPK").val(),
        vcode: vcode.val()
    };

    $post("/Home/GetLogin", JSON.stringify(data), function (backdata) {
        if (backdata.Code == App_G.Code.Code_200) {
            div_login.find("li.con_area").attr("style", "");
            vcode.attr("style", "");
            errormess.hide();
            var url = App_G.Util.getDomain() + "/Main/Index";
            setTimeout("window.location.href ='" + url + "'", 1000);
        } else if (backdata.State == 30005) {
            div_login.find("li.con_area").attr("style", "");
            vcode.css({ "border-color": "#ED888B", "color": "#BD4247" });
            errormess.text(backdata.Message);
            errormess.show();
        } else if (backdata.State == 30030) {
            vcode.attr("style", "");
            div_login.find("li.con_area").css({ "border-color": "#ED888B", "color": "#BD4247" });
            errormess.text(backdata.Message);
            errormess.show();
        } else {
            errormess.text("网络异常");
            errormess.show();
        }
    });
}



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
