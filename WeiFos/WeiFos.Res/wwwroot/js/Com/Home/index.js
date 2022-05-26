$(function () {

    $('.footer-copyright').find('a').each(function (i, el) {
        $(el).attr('target', '_blank');
    })

    $(window).scroll(function () { 
        if ($(window).scrollTop() > 200) {
            $(".side-box").fadeIn(500);
        } else {
            $(".side-box").fadeOut(500);
        } 
    });

    //图片懒加载
    $("img").lazyload({
        effect: "fadeIn"
    });

    $('.zixun').on('click', function () {
        $('.popup').css('display', 'block');
    });
    $('.popup .close').on('click', function () {
        $(this).parents('.popup').css('display', 'none');
    });


    var timeout1, timeout2, timeout3;
    $('.index-server-list .item-list li').on('mouseenter', function () {
        var obj = $(this);
        // $(this).find('.prop-server').css('display','block');
        $(this).find('.prop-server').show(500);
        timeout1 = setTimeout(function () {
            // console.log(obj.find('.prop-server .jiedian span.t1')[0]);
            obj.find('.prop-server .jiedian .dian.t1')
            .addClass('block');
            obj.find('.prop-server p').css('display', 'block');
        }, 500);
        timeout2 = setTimeout(function () {
            // console.log(2)
            obj.find('.prop-server .jiedian .dian.t2')
            .addClass('block');
        }, 800);
        timeout3 = setTimeout(function () {
            // console.log(3)
            obj.find('.prop-server .jiedian .dian.t3')
            .addClass('block');
        }, 800);
    }).on('mouseleave', function (event) {
        clearTimeout(timeout1);
        timeout1 = null;
        clearTimeout(timeout2);
        timeout2 = null;
        clearTimeout(timeout3);
        timeout3 = null;
        $(this).find('.prop-server h2').css('display', 'none');
        $(this).find('.prop-server').css('display', 'none');
        $(this).find('.prop-server .jiedian .dian')
            .removeClass('block');
    });


    //è§†é¢‘æ’­æ”¾å¼¹å‡º
    $('.click-shade').click(function () {
        $('.video-popup').fadeIn(400);
        $('.video').get(0).play();   //æ’­æ”¾

    });

    $('.video-close').click(function (event) {
        $('.video-popup').fadeOut(400);
        $('.video').get(0).pause();  // æš‚åœ
    });

    //é¦–é¡µè¡Œä¸šè§£å†³æ–¹æ¡ˆ
    $('.index-jieJue li .hangYe').on('mouseenter', function () {
        $(this).addClass('hover').find('.hangye-shade').addClass('blocks');
        $(this).find('.max-icon').animate({
            'top': '50%',
            'opacity': 1
        }, 500)
    }).on('mouseleave', function () {
        $(this).removeClass('hover').find('.hangye-shade').removeClass('blocks');
        $(this).find('.max-icon').animate({
            'top': 0,
            'opacity': 0
        }, 500)
    })

    //é¦–é¡µåˆä½œä¼™ä¼´éšæœºæ—‹è½¬&&æ»‘åŠ¨
    $('.index-heZuo li').on('mouseenter', function () {
        $(this).removeClass('rotate');
    }).on('mouseleave', function () {
        $(this).addClass('rotate');
    })

    setInterval(function () {
        var random = Math.floor(Math.random() * (35));//0-34
        // console.log(random);
        var rotate_li = $('.index-heZuo .rotate').eq(random);
        if (rotate_li) {
            rotate_li.addClass('ro_cur');
        } else {
            return;
        }
        var remove_timear = setTimeout(function () {
            rotate_li.removeClass('ro_cur');
        }, 1000, function () {
            clearTimeout(remove_timear);
        })
    }, 1500)
    var app_old_hover_i = 0;
    $('.app-img li').on('mouseenter', function () {
        var i = $(this).index();
        var li_left = $(this)[0].offsetLeft + 43;
        if (i - app_old_hover_i >= 0) {
            $('.app-item .app-item-txt').eq(i).siblings('.active').animate({ 'left': '100%', 'opacity': 0 }, 1000);
        } else {
            $('.app-item .app-item-txt').eq(i).siblings('.active').animate({ 'left': '-100%', 'opacity': 0 }, 1000);
        }
        var app_timer = setTimeout(function () {
            $('.app-item .app-item-txt').eq(i).addClass('active').siblings('.active').removeClass('active').animate({ 'left': 0, 'opacity': 1 });
        }, 800, function () {
            clearTimeout(app_timer);
        })
        // $('.app-item .app-item-txt').eq(i).addClass('active').siblings('.active').removeClass('active');
        $('.min-curBg').css('left', li_left + 'px');
        app_old_hover_i = i;
    })

    //çƒ­é—¨èµ„è®¯å‘¨æ¦œ/æœˆæ¦œåˆ‡æ¢
    $('.paihang-idex li').on('click', function () {
        var i = $(this).index();
        $('.paihang-item-box .paihang-item').eq(i).addClass('active').siblings('.active').removeClass('active');
        $(this).addClass('active').siblings('.active').removeClass('active');
    })

    //èµ„è®¯åˆ†ç±»ç‚¹å‡»åˆ‡æ¢
    $('.zixun-type').on('mouseenter', 'li', function () {
        var i = $(this).index();
        $('#catekey').html($(this).find('span').html());
        $(this).addClass('active').siblings('.active').removeClass('active');
        $('.left-msg-box .left-tab-box').eq(i).addClass('active').siblings('.active').removeClass('active');
    })

    //å…³äºŽå¯æ±‡æµè§ˆåˆ‡æ¢è§†å›¾
    $('.click_index_list li').on('click', function () {
        var i = $(this).index();
        $(this).addClass('active').siblings('.active').removeClass('active');
        $('.about_slide_bg').eq(i).fadeIn(1000).addClass('blocks').siblings('.blocks').removeClass('blocks').css('display', 'none');
        $('.about-box').eq(i).addClass('blocks').siblings('.blocks').removeClass('blocks');
        // console.log(i);
    })

    //å…³äºŽå¯æ±‡-æœ€æ–°èµ„è®¯ æ–°é—»ç±»åž‹åˆ‡æ¢
    $('.about-new-list').on('click', 'li', function () {
        var i = $(this).index();
        $(this).addClass('active').siblings('.active').removeClass('active');
        $('.cur-line').css('left', i * 120 + 'px');
        cur_line_i = i;
        //å¼€å§‹è¯·æ±‚æ•°æ® æ›´æ–°æ–°é—»åˆ—è¡¨
        //..............
    })
    var cur_line_i = 0;
    $('.about-new-list').on('mouseenter', 'li', function () {
        var i = $(this).index();
        $('.cur-line').css('left', i * 120 + 'px');
    }).on('mouseleave', 'li', function () {
        $('.cur-line').css('left', cur_line_i * 120 + 'px');
    })

    //è”ç³»æˆ‘ä»¬-æ‹›è˜
    $('.join-body').on('click', 'li', function () {
        console.log(1);
        $(this).addClass('shadow-blue3').find('.job-detail').slideDown(300);
        $(this).siblings('.shadow-blue3').removeClass('shadow-blue3').find('.job-detail').slideUp(300);
    })

    //è”ç³»æˆ‘ä»¬-åœ°å›¾
    $('.map-head li').on('click', function () {
        var i = $(this).index();
        $(this).addClass('active').siblings('.active').removeClass('active');
        $('.map-box li').eq(i).addClass('active').siblings('.active').removeClass('active');
    })

    //é¡µé¢å†…æ»‘åŠ¨åˆ°å…·ä½“ä½ç½®
    $('.click_index_list_scroll li').on('click', function () {
        var id_txt = $(this).attr('data-scroll');
        var scrollTop = $('#' + id_txt).offset().top;
        $('body,html').animate({
            scrollTop: scrollTop
        }, 500);
        return false;
        // console.log(scrollTop,$('#'+id_txt))
    })

    //APPå¼€å‘æµç¨‹hoveræ•ˆæžœ
    //95+(130-40/2)-30
    $('.buzhou-list .buzhou-item').on('mouseenter', function () {
        var top_item = parseInt($(this).css('top'));
        var top = top_item + 15.5;
        $('.bg-div .line-img .img').css({
            'top': top + 'px'
        });
        $(this).css({ 'transform': 'scale(1.2)' });
    }).on('mouseleave', function () {
        $(this).css({ 'transform': 'scale(1)' });
    })

    //è‡ªå®šä¹‰å¹´ä»½é€‰æ‹©å™¨
    $('.new-select-box .select-v').click(function () {
        $('.new-select').slideDown(300);
    })
    $('.new-select').on('click', 'li', function () {
        var v_str = $(this).text();
        $('.new-select-box .select-v').text(v_str);
        $(this).parent('.new-select').slideUp(300);
    })

    //ä¾§è¾¹æ å¼¹å‡ºç”µè¯
    $('.tel-btn').on('mouseenter', function () {
        $(this).css('border-radius', '0 4px 4px 0').find('.phone-side').slideDown(300);
    }).on('mouseleave', function () {
        $(this).css('border-radius', '4px').find('.phone-side').slideUp(300);
    })



    var mySwiper = new Swiper('.main-swiper .swiper-container', {
        pagination: '.main-swiper .swiper-pagination',
        nextButton: '.main-swiper .swiper-button-next',
        prevButton: '.main-swiper .swiper-button-prev',
        paginationClickable: true,
        autoplayDisableOnInteraction: false,
        loop: true,
        autoplay: 5000,
        lazyLoading: true,
        lazyLoadingInPrevNext: true,
        lazyLoadingInPrevNextAmount: 1,
        // autoHeight:true,
    });
    var swiper = new Swiper('.less-swiper .swiper-container', {
        pagination: '.less-swiper .swiper-pagination',
        paginationClickable: true,
        nextButton: '.less-swiper .swiper-button-next',
        prevButton: '.less-swiper .swiper-button-prev',
        autoHeight: true, //enable auto height
        speed: 500,
        effect: 'fade',
        autoplay: 5000,
        loop: true,
        onSlideChangeStart: function (swiper) {
            $('.less-swiper .swiper-slide').each(function (index, el) {
                $(el).find('.l-phone').css('left', '-100px');
                $(el).find('.r-phone').css('right', '-100px');
            });
            var this_slide = $('.less-swiper .swiper-slide-active');
            // console.log(swiper.activeIndex)
            this_slide.find('.l-phone').animate({ 'left': '0px' }, 2000, 'swing');
            this_slide.find('.r-phone').animate({ 'right': '0px' }, 2000, 'swing');
        }
    });

})