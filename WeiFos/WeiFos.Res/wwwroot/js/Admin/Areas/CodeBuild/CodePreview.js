var datagrid, nodes = [], selected_node = null, code = {};

function getSearchData(link_id, name) {
    return { link_id: link_id, tb_name: name };
}


$(function () {

    //加载数据
    var code = loadData(); 

    //主题
    var themes = [
        ['#fff', 'Default']
    ];

    $.each(themes, function (index) {

        //实体iframe
        var $iframe_entity = $('<iframe class="test-wrap" src="about:blank" />'), background = this[0], themeName = this[1];
        //业务逻辑iframe
        var $iframe_service = $('<iframe class="test-wrap" src="about:blank" />'), background = this[0], themeName = this[1];
        //表单页iframe
        var $iframe_form = $('<iframe class="test-wrap" src="about:blank" />'), background = this[0], themeName = this[1];
        //表单页js iframe
        var $iframe_form_js = $('<iframe class="test-wrap" src="about:blank" />'), background = this[0], themeName = this[1];
        //列表管理页 iframe
        var $iframe_manage = $('<iframe class="test-wrap" src="about:blank" />'), background = this[0], themeName = this[1];
        //列表管理页JS iframe
        var $iframe_manage_js = $('<iframe class="test-wrap" src="about:blank" />'), background = this[0], themeName = this[1];
        //控制器Action iframe
        var $iframe_action = $('<iframe class="test-wrap" src="about:blank" />'), background = this[0], themeName = this[1];

        //输出实体
        $('#output_entity').append($iframe_entity);
        //业务逻辑
        $('#output_service').append($iframe_service);
        //表单页
        $('#output_form').append($iframe_form);
        //表单页js
        $('#output_form_js').append($iframe_form_js);

        //列表管理页
        $('#output_manage').append($iframe_manage);
        //列表管理页JS
        $('#output_manage_js').append($iframe_manage_js);
        //控制器Action
        $('#output_action').append($iframe_action);

        //代码渲染插件目录
        var src_url = App_Config.getResDomain() + '/js/plugins/syntaxhighlighter_3.0.83/';

        /*** 加载实体 ***/
        $iframe_entity.ready(function () {

            //实体代码
            var code_str = code.entity.replace(/</g, '&lt;');

            var doc = $iframe_entity[0].contentDocument;
            $iframe_entity.css('background', background);
            $iframe_entity.css('scrollbars', 0);

            doc.write(''
                + '<link type="text/css" rel="stylesheet" href="' + src_url + '/styles/shCore' + themeName + '.css"/>'
                + '<style type="text/css">'
                + '.syntaxhighlighter { overflow: hidden;}'
                + 'html{height:200px;overflow:hidden}'
                + '</style>'
                + '<script type="text/javascript" src="' + src_url + '/scripts/shCore.js"></scr' + 'ipt>'
                + '<script type="text/javascript" src="' + App_Config.getResDomain() + '/js/plugins/syntaxhighlighter_3.0.83/scripts/shBrushXml.js"></scr' + 'ipt>'
                + '<script type="text/javascript" src="' + App_Config.getResDomain() + '/js/plugins/syntaxhighlighter_3.0.83/scripts/shBrushCSharp.js"></scr' + 'ipt>'

                + '<pre type="syntaxhighlighter" class="brush: csharp;toolbar:false;" >'// highlight: [5]
                + code_str
                + '</pre>'
                + '<pre type="syntaxhighlighter" class="brush: csharp; collapse: true;toolbar:false;">'
                + code_str
                + '</pre>'
                + '<script type="text/javascript">'
                + 'SyntaxHighlighter.highlight();'
                + '</script>'
            );

            doc.close();
            resetIframeHeight($iframe_entity);
        });

        /*** 加载Service ***/
        $iframe_service.ready(function () {

            var code_str = code.service.replace(/</g, '&lt;');
            var doc = $iframe_service[0].contentDocument;
            $iframe_service.css('background', background);
            $iframe_service.css('scrollbars', 0);

            doc.write(''
                + '<link type="text/css" rel="stylesheet" href="' + src_url + '/styles/shCore' + themeName + '.css"/>'
                + '<style type="text/css">'
                + '.syntaxhighlighter { overflow: hidden;}'
                + 'html{height:200px;overflow:hidden}'
                + '</style>'
                + '<script type="text/javascript" src="' + src_url + '/scripts/shCore.js"></scr' + 'ipt>'
                + '<script type="text/javascript" src="' + App_Config.getResDomain() + '/js/plugins/syntaxhighlighter_3.0.83/scripts/shBrushXml.js"></scr' + 'ipt>'
                + '<script type="text/javascript" src="' + App_Config.getResDomain() + '/js/plugins/syntaxhighlighter_3.0.83/scripts/shBrushCSharp.js"></scr' + 'ipt>'

                + '<pre type="syntaxhighlighter" class="brush: csharp;toolbar:false;" >'// highlight: [5]
                + code_str
                + '</pre>'
                + '<pre type="syntaxhighlighter" class="brush: csharp; collapse: true;toolbar:false;">'
                + code_str
                + '</pre>'
                + '<script type="text/javascript">'
                + 'SyntaxHighlighter.highlight();'
                + '</script>'
            );

            doc.close(); 
            resetIframeHeight($iframe_service);
        });

        /*** 加载表单页 ***/
        $iframe_form.ready(function () {

            var code_str = code.view_form.replace(/</g, '&lt;');
            var doc = $iframe_form[0].contentDocument;
            $iframe_form.css('background', background);
            $iframe_form.css('scrollbars', 0);

            doc.write(''
                + '<link type="text/css" rel="stylesheet" href="' + src_url + '/styles/shCoreEclipse.css"/>'
                + '<style type="text/css">'
                + '.syntaxhighlighter { overflow: hidden;}'
                + 'html{height:200px;overflow:hidden}'
                + '</style>'
                + '<script type="text/javascript" src="' + src_url + '/scripts/shCore.js"></scr' + 'ipt>'
                + '<script type="text/javascript" src="' + App_Config.getResDomain() + '/js/plugins/syntaxhighlighter_3.0.83/scripts/shBrushXml.js"></scr' + 'ipt>'
                + '<script type="text/javascript" src="' + App_Config.getResDomain() + '/js/plugins/syntaxhighlighter_3.0.83/scripts/shBrushCSharp.js"></scr' + 'ipt>'

                + '<pre type="syntaxhighlighter" class="brush: csharp;html-script: true;toolbar:false;" >'// highlight: [5]
                + code_str
                + '</pre>'
                + '<pre type="syntaxhighlighter" class="brush: csharp;html-script: true; collapse: true;toolbar:false;">'
                + code_str
                + '</pre>'
                + '<script type="text/javascript">'
                + 'SyntaxHighlighter.highlight();'
                + '</script>'
            );

            doc.close(); 
            resetIframeHeight($iframe_form);
        });

        /*** 加载表单页JS ***/
        $iframe_form_js.ready(function () {

            var code_str = code.js_form.replace(/</g, '&lt;');
            var doc = $iframe_form_js[0].contentDocument;
            $iframe_form_js.css('background', background);
            $iframe_form_js.css('scrollbars', 0);

            doc.write(''
                + '<link type="text/css" rel="stylesheet" href="' + src_url + '/styles/shCore' + themeName + '.css"/>'
                + '<style type="text/css">'
                + '.syntaxhighlighter { overflow: hidden;}'
                + 'html{height:200px;overflow:hidden}'
                + '</style>'
                + '<script type="text/javascript" src="' + src_url + '/scripts/shCore.js"></scr' + 'ipt>'
                + '<script type="text/javascript" src="' + App_Config.getResDomain() + '/js/plugins/syntaxhighlighter_3.0.83/scripts/shBrushXml.js"></scr' + 'ipt>'
                + '<script type="text/javascript" src="' + App_Config.getResDomain() + '/js/plugins/syntaxhighlighter_3.0.83/scripts/shBrushJScript.js"></scr' + 'ipt>'

                + '<pre type="syntaxhighlighter" class="brush: js;toolbar:false;" >'// highlight: [5]
                + code_str
                + '</pre>'
                + '<pre type="syntaxhighlighter" class="brush: js; collapse: true;toolbar:false;">'
                + code_str
                + '</pre>'
                + '<script type="text/javascript">'
                + 'SyntaxHighlighter.highlight();'
                + '</script>'
            );

            doc.close();
            resetIframeHeight($iframe_form_js);
        });

        /*** 加载管理页 ***/
        $iframe_manage.ready(function () {

            var code_str = code.view_manage.replace(/</g, '&lt;');
            var doc = $iframe_manage[0].contentDocument;
            $iframe_manage.css('background', background);
            $iframe_manage.css('scrollbars', 0);

            doc.write(''
                + '<link type="text/css" rel="stylesheet" href="' + src_url + '/styles/shCoreEclipse.css"/>'
                + '<style type="text/css">'
                + '.syntaxhighlighter { overflow: hidden;}'
                + 'html{height:200px;overflow:hidden}'
                + '</style>'
                + '<script type="text/javascript" src="' + src_url + '/scripts/shCore.js"></scr' + 'ipt>'
                + '<script type="text/javascript" src="' + App_Config.getResDomain() + '/js/plugins/syntaxhighlighter_3.0.83/scripts/shBrushXml.js"></scr' + 'ipt>'
                + '<script type="text/javascript" src="' + App_Config.getResDomain() + '/js/plugins/syntaxhighlighter_3.0.83/scripts/shBrushCSharp.js"></scr' + 'ipt>'

                + '<pre type="syntaxhighlighter" class="brush: csharp;html-script: true;toolbar:false;" >'// highlight: [5]
                + code_str
                + '</pre>'
                + '<pre type="syntaxhighlighter" class="brush: csharp;html-script: true; collapse: true;toolbar:false;">'
                + code_str
                + '</pre>'
                + '<script type="text/javascript">'
                + 'SyntaxHighlighter.highlight();'
                + '</script>'
            );

            doc.close();
            resetIframeHeight($iframe_manage);
        });

        /*** 加载管理页JS ***/
        $iframe_manage_js.ready(function () {

            var code_str = code.js_manage.replace(/</g, '&lt;');
            var doc = $iframe_manage_js[0].contentDocument;
            $iframe_manage_js.css('background', background);
            $iframe_manage_js.css('scrollbars', 0);

            doc.write(''
                + '<link type="text/css" rel="stylesheet" href="' + src_url + '/styles/shCore' + themeName + '.css"/>'
                + '<style type="text/css">'
                + '.syntaxhighlighter { overflow: hidden;}'
                + 'html{height:200px;overflow:hidden}'
                + '</style>'
                + '<script type="text/javascript" src="' + src_url + '/scripts/shCore.js"></scr' + 'ipt>'
                + '<script type="text/javascript" src="' + App_Config.getResDomain() + '/js/plugins/syntaxhighlighter_3.0.83/scripts/shBrushXml.js"></scr' + 'ipt>'
                + '<script type="text/javascript" src="' + App_Config.getResDomain() + '/js/plugins/syntaxhighlighter_3.0.83/scripts/shBrushJScript.js"></scr' + 'ipt>'

                + '<pre type="syntaxhighlighter" class="brush: js;toolbar:false;" >'// highlight: [5]
                + code_str
                + '</pre>'
                + '<pre type="syntaxhighlighter" class="brush: js; collapse: true;toolbar:false;">'
                + code_str
                + '</pre>'
                + '<script type="text/javascript">'
                + 'SyntaxHighlighter.highlight();'
                + '</script>'
            );

            doc.close(); 
            resetIframeHeight($iframe_manage_js);
        });

        /*** 加载管理页JS ***/
        $iframe_action.ready(function () {

            var code_str = code.action.replace(/</g, '&lt;');
            var doc = $iframe_action[0].contentDocument;
            $iframe_action.css('background', background);
            $iframe_action.css('scrollbars', 0);

            doc.write(''
                + '<link type="text/css" rel="stylesheet" href="' + src_url + '/styles/shCore' + themeName + '.css"/>'
                + '<style type="text/css">'
                + '.syntaxhighlighter { overflow: hidden;}'
                + 'html{height:200px;overflow:hidden}'
                + '</style>'
                + '<script type="text/javascript" src="' + src_url + '/scripts/shCore.js"></scr' + 'ipt>'
                + '<script type="text/javascript" src="' + App_Config.getResDomain() + '/js/plugins/syntaxhighlighter_3.0.83/scripts/shBrushXml.js"></scr' + 'ipt>'
                + '<script type="text/javascript" src="' + App_Config.getResDomain() + '/js/plugins/syntaxhighlighter_3.0.83/scripts/shBrushCSharp.js"></scr' + 'ipt>'

                + '<pre type="syntaxhighlighter" class="brush: csharp;toolbar:false;" >'// highlight: [5]
                + code_str
                + '</pre>'
                + '<pre type="syntaxhighlighter" class="brush: csharp; collapse: true;toolbar:false;">'
                + code_str
                + '</pre>'
                + '<script type="text/javascript">'
                + 'SyntaxHighlighter.highlight();'
                + '</script>'
            );

            doc.close();
            resetIframeHeight($iframe_action);
        });

        //$("div#myTabContent").niceScroll("iframe > html", { bouncescroll: false });
    });

});



//加载数据
function loadData() {
    var data = "";
    var link_id = App_G.Util.getRequestId("link_id");
    var tb_name = App_G.Util.getUrlParam("tb_name");
    var mid = App_G.Util.getUrlParam("mid");

    //加载服务器
    $getByAsync("/CodeBuildModule/CodeBuild/LoadCode?link_id=" + link_id + "&mid=" + mid + "&tb_name=" + tb_name, "", function (result) {
        if (result.Code == App_G.Code.Code_200) {
            data = result.Data;
        } else {
            layer.msg(result.Message, { icon: 2 });
        }
    });

    return data;
}


function resetIframeHeight(iframe) {
    setTimeout(function () {
        var height = iframe.contents().find("div.container:first").children("div.line").length * 17.5;
        $(iframe).css("height", height + 30);
    }, 1000);
}