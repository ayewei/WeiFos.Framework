var entity = null;

var isEnter = false;
$(function () {

    $("#ticket_text").focus();
    $("#listTable tbody").html(template("template", { data: [{ orderCourse: { ticket: "" } }] }));

    $("#ticket_text").keyup(function (event) {
        if (event.keyCode == 13) {
            serach($(this).val())
        }
    });

    //初始化验证插件
    yw.valid.config({
        submiteles: "#SaveBtn", vsuccess: function () {

            $("#SaveBtn").setDisable({ text: "保 存 中..." });

            var data = {
                entity: App_G.Util.getJson("data-val", {
                    ticket: entity.orderCourseDetails.ticket,
                    mobile: $("#mobile").val(),
                    contact: $("#name").val()
                })
            };

            $post("/Order/SubmitApply", JSON.stringify(data),
                function (data) {
                    if (data.State == App_G.BackState.State_200) {
                        App_G.MsgBox.success_digbox();
                        setTimeout("window.location.reload();", 1000);
                    } else {
                        App_G.MsgBox.error_digbox("操作失败");
                    }
                });

        }
    });

    var v = yw.valid.getValidate();

    //学员姓名
    v.valid("#name", {
        vtype: validateType.anyCharacter,
        focus: { msg: "请输入学员姓名" },
        blur: { msg: "请输入学员姓名" }
    });

    //手机号码
    v.valid("#mobile", {
        vtype: validateType.isMoblie,
        focus: { msg: "请输入手机号码" },
        blur: { msg: "请输入手机号码" }
    });

    //查询
    $("#search_btn").click(function () {
        serach($("#ticket_text").val());
    });

  
});


//查询
function serach(ticket) {
    $get("/Order/ScanQRCodeOrderCourse?ticket=" + ticket, "", function (data) {
        if (data.State == 200) {
            entity = data;
            $("#listTable tbody").html(template("template", { data: [data] }));
        } else {
            $("#listTable tbody").html(template("template", { data: [{ orderCourse: { ticket: "" } }] }));
        }
    });
}

//获取时间
template.helper('GetTime', function (time) {
    let time1 = time.split(":")[0];
    let time2 = time.split(":")[1];
    if (time1.length == 1) {
        time1 = "0" + time1;
    }
    if (time2.length == 1) {
        time2 = "0" + time2;
    }
    return time1 + ":" + time2;
});
