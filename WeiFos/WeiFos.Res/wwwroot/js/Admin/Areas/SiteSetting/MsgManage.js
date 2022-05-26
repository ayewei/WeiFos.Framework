
/*****获取查询数据******/
function getSearchData() {
    return { name: $("#name").val() };
}

$(function () {
    //初始化权限编号
    App_G.Auth.InitID();

    $post("/SiteSetting/GetMsgs?type=0", "",
    function (result) {
        $("#listTable tbody").html(template("template", { data: result }));
    });

    //查询事件
    $("#search_btn").click(function () {
        pager.execute(getSearchData());
    });

});


template.helper('GetMsgTypeValue', function (type) {

    switch (type) {
        case "AboutUs":
            return "关于我们";
        case "Contact":
            return "联系我们";
        case "AntCollege":
            return "蚂蚁学院";
        case "BarterProcess":
            return "蚂蚁学院-易货流程";
        case "RegisterProcess":
            return "蚂蚁学院-注册流程";
        case "BarterKnowledge":
            return "蚂蚁学院-易货知识";
        case "BarterRule":
            return "蚂蚁学院-易货规则";
        case "AntQa":
            return "蚂蚁学院-蚂蚁问答";
        case "EmailTmp":
            return "蚂蚁易货邮箱发送模板";
        default:
            return "";
    }
 
});