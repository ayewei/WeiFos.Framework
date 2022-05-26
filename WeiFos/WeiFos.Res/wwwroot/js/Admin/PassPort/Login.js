
$(function () {

    var bg_num = App_G.Util.getRandomNum(1, 20); 
    $("body").css("background", "url(" + App_Config.getResDomain() + "/css/Admin/Images/Login/bg" + bg_num + ".jpg) 0% 0% / 100% 100% no-repeat");


    //防止框架在某个窗口中打开
    if (window.top != window) {
        window.top.location.href = "/PassPort/Login";
    }

    $("#login_name").keydown(function (e) {
        if (e.keyCode == 13) {
            Login();
        }
    });

    $("#psw").keypress(function (e) {
        if (e.keyCode == 13) {
            Login();
        }
    });

    //点击登录
    $("[name=login_btn]").click(function () {
        Login();
    });
});


function Login() {
    if ($.trim($("#login_name").val()) == "") {
        $("p.login-box-msg").hide();
        $("div.alert-danger").show();
        $("div.alert-danger").find("span").text("请输入用户名");
        shake();
        return;
    }

    if ($.trim($("#psw").val()) == "") {
        $("p.login-box-msg").hide();
        $("div.alert-danger").show();
        $("div.alert-danger").find("span").text("请输入密码").show();
        shake();
        return;
    }

    $("p.login-box-msg").show();
    $("div.alert-danger").hide();

    var encrypt = new JSEncrypt();
    encrypt.setPublicKey($("#publicKey").val());
    var encrypted_str = encrypt.encrypt($("input[type=text]").val() + "\\" + $("input[type=password]").val());

    var data = { posx: encrypted_str };

    $post("/PassPort/LoginIn", JSON.stringify(data),

        function (result) {
            if (result.Code == App_G.Code.Code_200) {
                $("div.alert-danger").hide();
                $("div.alert-success").show();
                window.location.href = "/Home/Index";
            } else {
                shake();
                $("p.login-box-msg").hide();
                $("div.alert-success").hide();
                $("div.alert-danger").show();
                $("div.alert-danger").find("span").text(result.Message).show();
            }
        });
}

 
//摇一摇
function shake() {
    var $panel = $("div.card");
    var box_left = ($(window).width() - $panel.width()) / 2;
    var box_top = $panel.offset().top;
    $panel.css({ 'left': box_left, 'top': box_top, 'position': 'absolute' });
    for (var i = 1; 4 >= i; i++) {
        $panel.animate({ left: box_left - (40 - 10 * i) }, 50);
        $panel.animate({ left: box_left + (40 - 10 * i) }, 50);
    }
}


 
