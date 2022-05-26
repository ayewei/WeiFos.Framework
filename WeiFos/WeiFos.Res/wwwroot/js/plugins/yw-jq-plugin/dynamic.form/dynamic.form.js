
/**
 * Jquery 动态创建表单    
 * @author 叶委        2015-04-21
 * @formId             表单ID
 * @url                请求地址
 * @type               请求类型
 * @data               提交参数
*/
yw.dynamicform = {
    //创建表单
    createForm: function (options) {
        var form = $("<form></form>");
        form.attr('method', undefined == options.type ? "post" : options.type);
        form.attr('action', options.url);

        if(undefined != options.id){
            form.css('id', options.id);
        }

        form.css('display', 'none');
        //设置控件ID
        if (options.formId == undefined) {
            options.formId = new Date().getTime();
        }
        form.attr("id", options.formId);

        //提交参数
        for (var attr in options.data) {
            var input = $("<input type='hidden' name='" + attr + "' value='" + options.data[attr] + "' />");
            form.append(input);
        }
        return form;
    }
}
