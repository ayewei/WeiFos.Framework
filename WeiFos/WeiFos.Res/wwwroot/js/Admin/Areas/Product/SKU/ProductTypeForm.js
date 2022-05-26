$(function () {

    //初始化验证插件
    yw.valid.config({
        submitelements: "#SaveBtn", vsuccess: function () {

            $("#SaveBtn").setDisable({ text: "保 存 中..." });

            var data = {
                productType: App_G.Util.getJson("data-val", { id: App_G.Util.getRequestId('bid') })
            };

            $post("/ProductModule/SKU/SaveProductType", JSON.stringify(data),
                function (data) {
                    if (data.Code == App_G.Code.Code_200) {
                        App_G.MsgBox.success_digbox();
                        var url = App_G.Util.getDomain() + "/ProductModule/SKU/AttrName?bid=" + data.Data;
                        setTimeout("window.location.href ='" + url + "'", 1000);
                    } else {
                        App_G.MsgBox.error_digbox(data.Message);
                    }
                    $("#SaveBtn").val("保 存");
                    $("#SaveBtn").prop("disabled", false);
                });

        }
    });

    var v = yw.valid.getValidate();

    //商品类型名称
    v.valid("#name", {
        focusmsg: "请输入商品类型名称", errormsg: "请输入商品类型名称", vtype: verifyType.anyCharacter,
        othermsg: "该商品类型名称已经存在", repeatvalidate: { url: App_G.Util.getDomain() + "/ProductModule/SKU/ExistTypeName?bid=" + App_G.Util.getRequestId('bid') }
    });

    //备注信息
    v.valid("#remarks", { focusmsg: "请输入备注信息，长度在100字符以内", errormsg: "请输入备注信息，长度在100字符以内", vtype: verifyType.anyCharacter });
 
    //返回
    $("#BackBtn").click(function () {
        window.location.href = "/ProductModule/SKU/ProductTypeManage";
    });

     
    //页面数据绑定
    if ($.trim($("#cur_entity").val()).length > 0) {
         
        //当前用户对象
        var cur_entity = $.parseJSON($("#cur_entity").val());

        //页面绑定用户对象
        App_G.Util.bindJson("data-val", cur_entity);
    }
});