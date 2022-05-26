
$(function () {

    //防止框架在某个窗口中打开
    if (window.top != window) {
        window.top.location.href = "/Home/Index";
    }

    $("input[type=text]").keydown(function (e) {
        if (e.keyCode == 13) {
            Login();
        }
    });

    $("input[type=text]").keyup(function () {
        if ($(this).val().length && $("input[type=password]").val().length > 0) {
            $("#sign-in").removeClass("disabled");
        } else {
            $("#sign-in").addClass("disabled");
        }
    });

    $("input[type=password]").keypress(function (e) {
        if (e.keyCode == 13) {
            Login();
        }
    });

    $("input[type=password]").keyup(function (e) {
        if ($(this).val().length && $("input[type=text]").val().length > 0) {
            $("#sign-in").removeClass("disabled");
        } else {
            $("#sign-in").addClass("disabled");
        }
    });

    //点击登录
    $("#sign-in").click(function () {
        Login();
    });

});


function Login() {
    if ($.trim($("input[type=text]").val()) == "") {
        $("#errMsg").parent().parent().removeClass("hide");
        $("#errMsg").text("请输入用户名");
        shake();
        return;
    }

    if ($.trim($("input[type=password]").val()) == "") {
        $("#errMsg").parent().parent().removeClass("hide");
        $("#errMsg").text("请输入密码");
        shake();
        return;
    }

    if (!$("#errMsg").parent().parent().hasClass("hide")) {
        $("#errMsg").parent().parent().addClass("hide")
    }
    $("#sign-in i").hide();
    $("#sign-in img").show();

    setMaxDigits(131);
    var key = new RSAKeyPair($("#Exponent").val(), "", $("#Modulus").val());
    var posx = encryptedString(key, base64encode($("input[type=password]").val()));
    var posx = encryptedString(key, base64encode($.trim($("input[type=text]").val())) + "\\" + base64encode($("input[type=password]").val()));

    $("input[type=password]").val(hex_sha1($("input[type=password]").val()).substring(0, 30));
    var data = { loginname: $.trim($("input[type=text]").val()), posx: posx, loginpk: $("#LoginPK").val() };

    $post("/Passport/GoLogin", JSON.stringify(data), 
        function (result) {
            if (result.Code == App_G.Code.Code_200) {
                window.location.href = "/Home/Index";
            } else if (result.State == 101 || result.State == 103 || result.State == 104) {
                $("#errMsg").parent().parent().removeClass("hide");
                $("#errMsg").text(result.Message);
                shake();
                $("input[type=password]").val("");
                $("#sign-in i").show();
                $("#sign-in img").hide();
            } else {
                $("#errMsg").parent().parent().removeClass("hide");
                $("#errMsg").text("操作失败");
                shake();
                $("input[type=password]").val("");
                $("#sign-in i").show();
                $("#sign-in img").hide();
            }
        });
}


//摇一摇
function shake() {
    var $panel = $("#login_div");
    var box_left = ($(window).width() - $panel.width()) / 2;
    var box_top = $panel.offset().top;
    $panel.css({ 'left': box_left, 'top': box_top, 'position': 'absolute' });
    for (var i = 1; 4 >= i; i++) {
        $panel.animate({ left: box_left - (40 - 10 * i) }, 50);
        $panel.animate({ left: box_left + (40 - 10 * i) }, 50);
    }
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
