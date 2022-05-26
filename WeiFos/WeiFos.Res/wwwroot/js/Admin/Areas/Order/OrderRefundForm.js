
$(function () {


    yw.valid.config({
        submiteles: "#ConfirmBtn", vsuccess: function () {

            $("#ConfirmBtn").setDisable();

            var data = {
                no: App_G.Util.getRequestId('no'),
                refund_amount: $("#refund_actual_amount").val()
            };

            $post("/Order/SubmitRefundOrder", JSON.stringify(data),
                function (result) {
                    if (result.State == App_G.BackState.State_200) {
                        App_G.MsgBox.success_digbox();
                        var url = App_G.Util.getDomain() + "/Order/OrderRefundManage";
                        setTimeout("window.location.href ='" + url + "'", 1000);
                    } else {
                        App_G.MsgBox.error_digbox(result.Data);
                    }
                });

        }
    });

    //权限名称
    var v = yw.valid.getValidate();

    //退款最大金额
    v.valid("#refund_actual_amount", {
        vtype: function () {
            var $this = this;
            $this.blur.msg = "请输入正确的退款金额";
            //如果格式正确
            if (validateType.isLGZeroPrice($("#refund_actual_amount").val())) {
                //最大金额
                var max_amount = parseFloat($("#refund_actual_amount").attr("data-val"));
                //输入金额
                var amount = parseFloat($("#refund_actual_amount").val());
                if (amount > max_amount) {
                    $this.blur.msg = "退款金额不能大于订单金额";
                    return false;
                } else {
                    return true;
                }
            } else {
                return false;
            }
            return false;
        },
        focus: { msg: "请输入退款金额" },
        blur: { msg: "请输入正确的退款金额" },
        othermsg:"3333"
    });


});

