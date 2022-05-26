/**
  * App_Config 全局变量配置 
  * @author 叶委  2015-05-18
 */
App_Config = {
    //获取网站域名
    getCrossRes: function () {
        var arr = window.location.host.split(".");
        var com = arr[arr.length - 1];
        return arr[arr.length - 2] + "." + (com.indexOf(':') != -1 ? com.split(':')[0] : com);
    },
    //当前网站完整域名
    getDomain: function () {
        return window.location.protocol + '//' + window.location.host;
    },
    //获取资源站点域名
    getResDomain: function () {
        var domain = window.location.protocol + '//' + "a.netcore.res." + App_Config.getCrossRes();
        return domain;
    },
    //获取OSS域名
    getOssDomain: function () {
        return "http://test.oss.jd-soft.cn/";
    }
}
