
 
$(function () {

    $("[data-original]").lazyload({
        effect: "fadeIn"
    });

    //翻页
    var pager = $("#PagerDiv").pager({
        url: '/home/GetInformts?rnd=' + Math.random(),
        data: { cgty_id: 1 },
        currentPage: 0,
        pageSize: 15,
        callback: function (data) {
            $("ul.msg-list-ul.shadow-blue").html(template("template", data));
        }
    });

});

