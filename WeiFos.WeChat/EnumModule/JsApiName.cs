using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.WeChat.EnumModule
{
    //JS接口名称
    public enum JsApiName
    {
        /// <summary>
        /// 判断当前客户端版本是否支持指定JS接口
        /// </summary>
        checkJsApi,
        /// <summary>
        /// 微信公众号拉起发票列表接口
        /// </summary>
        chooseInvoice,
        /// <summary>
        /// 微信公众号拉起发票抬头列表接口
        /// </summary>
        chooseInvoiceTitle, 
        /// <summary>
        /// 拍照或从手机相册中选图接口
        /// </summary>
        chooseImage,
        /// <summary>
        /// 微信扫一扫接口
        /// </summary>
        scanQRCode,
        /// <summary>
        /// 分享给朋友
        /// </summary>
        onMenuShareAppMessage,
        /// <summary>
        /// 分享到QQ
        /// </summary>
        onMenuShareQQ,
        /// <summary>
        /// 分享到微博
        /// </summary>
        onMenuShareWeibo,
        /// <summary>
        /// 分享到朋友圈
        /// </summary>
        onMenuShareTimeline,
        /// <summary>
        /// 微信支付
        /// </summary>
        chooseWXPay,
        /// <summary>
        /// 播放语音接口
        /// </summary>
        playVoice,
        /// <summary>
        /// 暂停播放接口
        /// </summary>
        pauseVoice,
        /// <summary>
        /// 停止播放接口
        /// </summary>
        stopVoice,
        /// <summary>
        /// 监听语音播放完毕接口
        /// </summary>
        onVoicePlayEnd,
        /// <summary>
        /// 上传语音接口
        /// </summary>
        uploadVoice,
        /// <summary>
        /// 下载语音接口
        /// </summary>
        downloadVoice,
        /// <summary>
        /// 识别音频并返回识别结果接口
        /// </summary>
        translateVoice,
        /// <summary>
        /// 获取网络状态接口
        /// </summary>
        getNetworkType,
        /// <summary>
        /// 使用微信内置地图查看位置接口
        /// </summary>
        openLocation,
        /// <summary>
        /// 获取地理位置接口
        /// </summary>
        getLocation,

        #region 微信右上角功能接口

        /// <summary>
        /// 隐藏所有非基础按钮接口
        /// </summary>
        hideAllNonBaseMenuItem,

        /// <summary>
        /// 显示所有功能按钮接口
        /// </summary>
        showAllNonBaseMenuItem,

        /// <summary>
        /// 批量显示功能按钮接口
        /// </summary>
        showMenuItems,

        /// <summary>
        /// 批量隐藏功能按钮接口
        /// </summary>
        hideMenuItems,

        /// <summary>
        /// 显示右上角菜单接口
        /// </summary>
        showOptionMenu,

        /// <summary>
        /// 隐藏右上角菜单接口
        /// </summary>
        hideOptionMenu,

        #endregion

        /// <summary>
        /// 批量添加卡券接口
        /// </summary>
        addCard,
        /// <summary>
        /// 拉取适用卡券列表并获取用户选择信息
        /// </summary>
        chooseCard,
        /// <summary>
        /// 查看微信卡包中的卡券接口
        /// </summary>
        openCard,

        /// <summary>
        /// 关闭当前网页窗口接口
        /// </summary>
        closeWindow,
    }
}
