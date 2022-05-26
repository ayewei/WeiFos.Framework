$(function () {

    //加载数据
    InitData();

    //商品类别选择
    $("ul.clearfix.list_sc").on("click", "ul.list_item li", function (event) {
        var li = $(this);
        if (li.hasClass("item_hs")) {

            $get("/ProductModule/Product/GetCgtyChildrens?bid=" + li.attr("id"), "",
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        ClearLi(li);
                        $("ul.clearfix.list_sc").append($(template("c_template", { data: result.Data })));
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });
        } else {
            ClearLi(li);
        }
        //阻止冒泡
        return false;
    });

    //下一步
    $("#next_Btn").click(function () {
        if ($("li.f_item").length == $("li.seleted").length) {
            var id = $($("li.f_item")[$("li.f_item").length - 1]).find("li.seleted").attr("id");
            if (undefined == id) return;
            window.location.href = "/ProductModule/Product/ProductForm?cid=" + id + "&bid=" + App_G.Util.getRequestId('pid');
        } else {
            layer.msg("请选择最底层类别做为商品类别！", { icon: 2 });
        }
    });

});


//初始化数据
function InitData() {
    $getByAsync("/ProductModule/Product/LoadCgtyChildren?bid=" + App_G.Util.getRequestId('bid'), "",
        function (data) {
            if (data.Code == App_G.Code.Code_200) {
                if ($("ul.clearfix.list_sc").find("li").length > 0) {
                    $("ul.clearfix.list_sc").find("li").remove();
                }
                $("ul.clearfix.list_sc").append(template("template", $.parseJSON(data.Data)));
                //绑定选中
                bindSelect(App_G.Util.getRequestId('bid'));
            } else {
                layer.msg(result.Message, { icon: 2 });
            }
        });
}


//递归绑定选中
function bindSelect(id, text) {
    var li = $("ul.list_item").find("li#" + id);
    var li_text = "";

    if (li.length) {
        //选中样式
        li.addClass("seleted");
        //选中文字
        li_text = li.find("a").text() + (text == undefined ? "" : " >> " + text);
        $("#select_text").text(li_text);
        //递归
        bindSelect(li.attr("pid"), li_text);
    }
}


function ClearLi(li) {
    //设置选中样式
    li.addClass("seleted").siblings().removeClass("seleted");
    li.parents("li.f_item").nextAll().remove();
    bindSelect(li.attr("id"));
}
