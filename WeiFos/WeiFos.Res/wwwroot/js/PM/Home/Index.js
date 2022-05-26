


jQuery.cookie = function (n, t, i) {
    var f, r, e, o, u, s;
    if (typeof t != "undefined") {
        i = i || {},
        t === null && (t = "", i = $.extend({},
        i), i.expires = -1),
        f = "",
        i.expires && (typeof i.expires == "number" || i.expires.toUTCString) && (typeof i.expires == "number" ? (r = new Date, r.setTime(r.getTime() + i.expires * 864e5)) : r = i.expires, f = "; expires=" + r.toUTCString());
        var h = i.path ? "; path=" + i.path : "",
        c = i.domain ? "; domain=" + i.domain : "",
        l = i.secure ? "; secure" : "";
        document.cookie = [n, "=", encodeURIComponent(t), f, h, c, l].join("")
    } else {
        if (e = null, document.cookie && document.cookie != "") for (o = document.cookie.split(";"), u = 0; u < o.length; u++) if (s = jQuery.trim(o[u]), s.substring(0, n.length + 1) == n + "=") {
            e = decodeURIComponent(s.substring(n.length + 1));
            break
        }
        return e
    }
}

//防止框架在某个窗口中打开
if (window.top != window) {
    window.top.location.href = "/Home/Index";
}

$(document).ready(function () {

    function setCookie(name, value, expireday) {
        var exp = new Date();
        exp.setTime(exp.getTime() + expireday * 24 * 60 * 60 * 1000);
        document.cookie = name + "=" + escape(value) + "; expires" + "=" + exp.toGMTString();
    }
    P.skn();

    $("div#sidebar-wrapper li ul").hide();
    $("div#sidebar-wrapper li a.current").parent().find("ul").slideToggle("slow");
    $("div#sidebar-wrapper li a[top_item]").click(
        function () {
            $(this).parent().siblings().find("ul").slideUp("normal");
            $(this).next().slideToggle("normal");
            return false;
        }
    );

    $("div#sidebar-wrapper li a.no-submenu").click(
        function () {
            window.open(this.href, "RightFrame")
            //window.location.href = (this.href);
            return false;
        }
    );

    $("div#sidebar-wrapper li a[top_item]").hover(function () {
        $(this).stop().animate({ paddingLeft: "25px" }, 1000);
    }, function () {
        $(this).stop().animate({ paddingLeft: "15px" }, 1000);
    });


    //$('#sidebar-wrapper').rollbar({ zIndex: 80000 });

    $("a.hidden-xs").click(function () {
        tab_toggle();
    });

    $("a.btn_collapse").click(function () {
        tab_toggle();
    });


    //退出
    $("[name=SignOut]").digbox({
        Selector: ".dropdown-menu.pull-right",
        Title: "信息提示框",
        Context: "确认退出吗？",
        CallBack: function (submit_btnid, current, panel) {
            window.location.href = "/Home/LoginOut";
        }
    });


    //顶部bar
    var topbar = $("div.topBar");
    //菜单效果
    topbar.find("ul>li").mouseover(function () {
        $(this).find("div.t_panel").show();
    });

    topbar.find("ul>li").find("div.t_panel").mouseover(function () {
        $(this).find("div.t_panel").show();
    });

    //菜单效果
    topbar.find("ul>li").mouseout(function () {
        $(this).find("div.t_panel").hide();
    });

    topbar.find("ul>li").find("div.t_panel").mouseout(function () {
        $(this).find("div.t_panel").hide();
    });

    //点击选择
    $(".nav-sub").find("li").click(function () {
        $(".nav-sub").find("li").removeClass("current");
        $(this).addClass("current");
    });



    //test();
});

var tab_index = true;
function tab_toggle() {
    if (tab_index) {
        $("#sidebar").addClass("minified");
        $(".btn_collapse span").text(">>");
        tab_index = false;
    } else {
        $("#sidebar").removeClass("minified");
        $(".btn_collapse span").text("<<");
        tab_index = true;
    }
}
