'use strict'
$(function () {

    /*判断当前浏览器是否是IE浏览器*/
    if ($('body').hasClass('IE') || $('body').hasClass('InternetExplorer')) {
        $('#weifos_loadbg').append('<img data-img="imgdw" src="' + top.$.rootUrl + '/Content/images/ie-loader.gif" style="position: absolute;top: 0;left: 0;right: 0;bottom: 0;margin: auto;vertical-align: middle;">');
        Pace.stop();
    }
    else {
        Pace.on('done', function () {
            $('#weifos_loadbg').fadeOut();
        });
    }

    //iframe 滚动条优化
    //$("div.content-wrapper").niceScroll("iframe", { bouncescroll: false });
    //document.addEventListener('touchstart', function (event) {
    //    // 判断默认行为是否可以被禁用
    //    if (event.cancelable) {
    //        // 判断默认行为是否已经被禁用
    //        if (!event.defaultPrevented) {
    //            event.preventDefault();
    //        }
    //    }
    //}, false);

    //左菜单滚动条
    $("div.sidebar").niceScroll({
        cursorcolor: "#424242"
    }).onResize();

    //个人信息
    $("li.nav-item.dropdown.user.user-menu").mouseover(function () {
        $(this).find('ul.dropdown-menu').show();
    }).mouseout(function () {
        $(this).find('ul.dropdown-menu').hide();
    });

    //退出登录
    $("#login_out").digbox({ 
        Title: "信息提示框",
        Content: "确认退出吗？",
        CallBack: function (submit_btnid, current, panel) {
            window.location.href = "/PassPort/LoginOut";
        }
    });

    //顶部窗口标签切换关闭
    $("ul.navbar-nav").on("click", "i.nav-icon.fa.fa-close", function () {
        let li = $(this).parent("li");
        //获取对应ID
        let id = li.attr("tag-tab");
        //默认缺省ID
        let todo_id = -1;

        //下一元素
        let next_li = li.next();
        //上一元素
        let prev_li = li.prev();
        if (next_li.length) {
            todo_id = next_li.attr("tag-tab");
        } else {
            todo_id = prev_li.attr("tag-tab");
        }

        //移除标签
        $("[tag-tab=" + id + "]").remove();
        //移除iframe
        $("[tag-iframe=" + id + "]").remove();

        //处理tab样式
        $("div.frame-tabs-wrap > ul.navbar-nav").find("[tag-tab=" + todo_id + "]").addClass("active").siblings().removeClass("active");
        //处理iframe样式
        $("div.content-wrapper").find("[tag-iframe=" + todo_id + "]").addClass("active").siblings().removeClass("active");

        //顶部tab滚动条
        $("div.frame-tabs-wrap").niceScroll({
            cursorcolor: "#424242"
        }).onResize();
    });

    //顶部窗口标签
    $("ul.navbar-nav").on("click", "li.frame-tabItem", function () {
        let id = $(this).attr("tag-tab");
        //处理tab样式
        $("div.frame-tabs-wrap > ul.navbar-nav").find("[tag-tab=" + id + "]").addClass("active").siblings().removeClass("active");
        //处理iframe样式
        $("div.content-wrapper").find("[tag-iframe=" + id + "]").addClass("active").siblings().removeClass("active");
    });

    //左侧菜单点击事件
    $("ul.nav.nav-pills.nav-sidebar.flex-column").on("click", "li[data-id],[name=menu]", function () {

        let li = $(this);
        let id = li.attr("data-id");

        //是否点击过标签
        if (!$("[tag-tab=" + id + "]").length) {
            //添加tab
            $("div.frame-tabs-wrap > ul.navbar-nav").append(template("tab_template", { id: id, icon_class: li.find("i").attr("class"), name: li.find("p").text() }));
            //添加iframe
            $("div.content-wrapper").append(template("iframe_template", { id: id, src: li.attr("data-src") }));
        }

        //处理菜单样式
        li.find("a").addClass("active").parent().siblings().find("a").removeClass("active");
        //处理tab样式
        $("div.frame-tabs-wrap > ul.navbar-nav").find("[tag-tab=" + id + "]").addClass("active").siblings().removeClass("active");
        //处理iframe样式
        $("div.content-wrapper").find("[tag-iframe=" + id + "]").addClass("active").siblings().removeClass("active");

        //顶部tab滚动条
        $("div.frame-tabs-wrap").niceScroll({
            cursorcolor: "#424242"
        }).onResize();

        //$("div.content-wrapper").niceScroll("iframe[tag-iframe=" + id + "]", { bouncescroll: true }).onResize();
        //iframe 滚动条优化
        //$("div.content-wrapper").niceScroll("iframe", { bouncescroll: true }).onResize();

    });

    //左侧菜单点击事件
    $("body").on("click", "[name=menu]", function () {

        let ele = $(this);
        let id = ele.attr("data-id");

        //是否点击过标签
        if (!$("[tag-tab=" + id + "]").length) {
            //添加tab
            $("div.frame-tabs-wrap > ul.navbar-nav").append(template("tab_template", { id: id, icon_class: '', name: ele.attr("title") }));
            //添加iframe
            $("div.content-wrapper").append(template("iframe_template", { id: id, src: ele.attr("data-src") }));
        }

        //处理tab样式
        $("div.frame-tabs-wrap > ul.navbar-nav").find("[tag-tab=" + id + "]").addClass("active").siblings().removeClass("active");
        //处理iframe样式
        $("div.content-wrapper").find("[tag-iframe=" + id + "]").addClass("active").siblings().removeClass("active");

        //顶部tab滚动条
        $("div.frame-tabs-wrap").niceScroll({
            cursorcolor: "#424242"
        }).onResize();

    });



    /**
     * 自定义主题脚本
     * author yewei
     * add by @date 2014-12-09 
     */
    var $sidebar = $('.control-sidebar')
    var $container = $('<div />', {
        class: 'p-3'
    })

    $sidebar.append($container)

    var navbar_dark_skins = [
        'bg-primary',
        'bg-info',
        'bg-success',
        'bg-danger'
    ]

    var navbar_light_skins = [
        'bg-warning',
        'bg-white',
        'bg-gray-light'
    ]

    //设置title
    $container.append('<h5>WeiFos 自定义主题</h5><hr class="mb-2"/>' + '<h6>Navbar Variants</h6>')

    //动态创建元素
    var $navbar_variants = $('<div />', { 'class': 'd-flex' })

    var navbar_all_colors = navbar_dark_skins.concat(navbar_light_skins)
    var $navbar_variants_colors = createSkinBlock(navbar_all_colors, function (e) {
        var color = $(this).data('color')
        console.log('Adding ' + color)
        var $main_header = $('.main-header')
        $main_header.removeClass('navbar-dark').removeClass('navbar-light')
        navbar_all_colors.map(function (color) {
            $main_header.removeClass(color)
        })

        if (navbar_dark_skins.indexOf(color) > -1) {
            $main_header.addClass('navbar-dark')
            console.log('AND navbar-dark')
        } else {
            console.log('AND navbar-light')
            $main_header.addClass('navbar-light')
        }
        $main_header.addClass(color)
    })

    $navbar_variants.append($navbar_variants_colors)
    $container.append($navbar_variants)

    var $checkbox_container = $('<div />', { 'class': 'mb-4' })
    var $navbar_border = $('<input />', {
        type: 'checkbox',
        value: 1,
        checked: $('.main-header').hasClass('border-bottom'),
        'class': 'mr-1'
    }).on('click', function () {
        if ($(this).is(':checked')) {
            $('.main-header').addClass('border-bottom')
        } else {
            $('.main-header').removeClass('border-bottom')
        }
        console.log($(this).is(':checked'), 'hi')
    })
    $checkbox_container.append($navbar_border)
    $checkbox_container.append('<span>Navbar border</span>')
    $container.append($checkbox_container)

    var sidebar_colors = [
        'bg-primary',
        'bg-warning',
        'bg-info',
        'bg-danger',
        'bg-success'
    ]
    var sidebar_skins = [
        'sidebar-dark-primary',
        'sidebar-dark-warning',
        'sidebar-dark-info',
        'sidebar-dark-danger',
        'sidebar-dark-success',
        'sidebar-light-primary',
        'sidebar-light-warning',
        'sidebar-light-info',
        'sidebar-light-danger',
        'sidebar-light-success'
    ]

    $container.append('<h6>Dark Sidebar Variants</h6>')
    var $sidebar_variants = $('<div />', {
        'class': 'd-flex'
    })
    $container.append($sidebar_variants)
    $container.append(createSkinBlock(sidebar_colors, function () {
        var color = $(this).data('color')
        var sidebar_class = 'sidebar-dark-' + color.replace('bg-', '')
        var $sidebar = $('.main-sidebar')
        sidebar_skins.map(function (skin) {
            $sidebar.removeClass(skin)
        })

        $sidebar.addClass(sidebar_class)
        console.log('added ' + sidebar_class)
    }))

    $container.append('<h6>Light Sidebar Variants</h6>')
    var $sidebar_variants = $('<div />', {
        'class': 'd-flex'
    })
    $container.append($sidebar_variants)
    $container.append(createSkinBlock(sidebar_colors, function () {
        var color = $(this).data('color')
        var sidebar_class = 'sidebar-light-' + color.replace('bg-', '')
        var $sidebar = $('.main-sidebar')
        sidebar_skins.map(function (skin) {
            $sidebar.removeClass(skin)
        })

        $sidebar.addClass(sidebar_class)
        console.log('added ' + sidebar_class)
    }))

    var logo_skins = navbar_all_colors
    $container.append('<h6>Brand Logo Variants</h6>')
    var $logo_variants = $('<div />', {
        'class': 'd-flex'
    })
    $container.append($logo_variants)
    var $clear_btn = $('<a />', {
        href: 'javascript:;'
    }).text('clear').on('click', function () {
        var $logo = $('.brand-link')
        logo_skins.map(function (skin) {
            $logo.removeClass(skin)
        })
    })
    $container.append(createSkinBlock(logo_skins, function () {
        var color = $(this).data('color')
        var $logo = $('.brand-link')
        logo_skins.map(function (skin) {
            $logo.removeClass(skin)
        })
        $logo.addClass(color)
    }).append($clear_btn))

    function createSkinBlock(colors, callback) {
        var $block = $('<div />', {
            'class': 'd-flex flex-wrap mb-3'
        })

        colors.map(function (color) {
            var $color = $('<div />', {
                'class': (typeof color === 'object' ? color.join(' ') : color) + ' elevation-2'
            })
            $block.append($color)
            $color.data('color', color)
            $color.css({
                width: '40px',
                height: '20px',
                borderRadius: '25px',
                marginRight: 10,
                marginBottom: 10,
                opacity: 0.8,
                cursor: 'pointer'
            })
            $color.hover(function () {
                $(this).css({ opacity: 1 }).removeClass('elevation-2').addClass('elevation-4')
            }, function () {
                $(this).css({ opacity: 0.8 }).removeClass('elevation-4').addClass('elevation-2')
            })
            if (callback) {
                $color.on('click', callback)
            }
        })

        return $block
    }

});

