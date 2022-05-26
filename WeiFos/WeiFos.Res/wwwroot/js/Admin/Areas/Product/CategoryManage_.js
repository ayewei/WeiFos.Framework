 
//材料分类数据
var t, znode = [];

$(function () {
     
    //加载框  
    var index = layer.load(1, {
        shade: [0.3, '#000']  
    });

    $get("/ProductModule/Product/LoadProductCgtys", "",
    function (result) {

        if (result.Code == App_G.Code.Code_200) {
            $.each(result.Data, function (i, o) {
                var tmp = {
                    id: o.id,
                    pId: o.parent_id,
                    name: o.name,
                    open: true
                };
                znode.push(tmp);
            });

            t = $("#tree");
            //初始化控件树
            t = $.fn.zTree.init(t, setting, znode);
        }

        layer.close(index);
    });

    //关闭全部
    $("#close_all").click(function () {
        t.expandAll(false);
    });

    //展开全部
    $("#open_all").click(function () {
        t.expandAll(true);
    });


});
 


var setting = {
    view: {
        dblClickExpand: false,
        showIcon: true,
        showLine: true,
        selectedMulti: false
    },
    edit: {
        enable: true,
        removeTitle: "删除",
        //显示修改按钮
        showRenameBtn: false,
        //显示删除按钮
        showRemoveBtn: true
    },
    data: {
        simpleData: {
            enable: true,
            idKey: "id",
            pIdKey: "pId",
            rootPId: ""
        }
    },
    callback: { 
        beforeRemove: beforeRemove,
        beforeClick: function (treeId, treeNode) {
            window.location.href = "CategoryForm?bid=" + treeNode.id;
        }
    }
};


//删除之前方法
function beforeRemove(treeId, treeNode) {
    var is_remove = false;
    //删除数据
    $("[name=deleteBtn]").digbox({
        IsAuto: true,
        Title: "提示信息",
        Context: "确定删除 " + treeNode.name + " 该节点吗？",
        CallBack: function (b, c, p) {

            $get("/ProductModule/Product/DeleteProductCgty?bid=" + treeNode.id, "",
                function (data) {
                    if (data.Status == App_G.Code.Code_200) {
                        App_G.MsgBox.success_digbox();
                        is_remove = true;
                    } else {
                        App_G.MsgBox.error_digbox("操作失败！");
                    }
                });
        }
    });
    return is_remove;
}


 