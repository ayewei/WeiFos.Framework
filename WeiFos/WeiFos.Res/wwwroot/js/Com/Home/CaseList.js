
 
$(function () {
     
    var bid = App_G.Util.getHtmlId();
    
    $("div.hangye-list a[data-id=" + bid + "]").find("span").addClass("active").siblings().parent().find("span").removeClass("active");

    $("[data-original]").lazyload({
        effect: "fadeIn"
    });

    //翻页
    var pager = $("#PagerDiv").pager({
        url: '/Home/GetCases?rnd=' + Math.random(),
        data: { cgty_id: App_G.Util.getHtmlId() },
        currentPage: 0,
        pageSize: 15,
        callback: function (data) {
            $("ul.succ-anli-list").html(template("template", data));
        }
    });


});

