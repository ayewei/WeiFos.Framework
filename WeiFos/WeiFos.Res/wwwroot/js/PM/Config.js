/**
  * App_Config 全局变量配置 
  * @author 叶委  2015-05-18
 */
App_Config = {
    //获取网站域名
    getCrossRes: function () {
        var arr = window.location.host.split(".");
        return arr[arr.length - 2] + "." + arr[arr.length - 1];
    },
    //获取网站域名
    getMainDomain: function () {
        var dms = window.location.host.split(".");
        return dms[dms.length - 2] + "." + dms[dms.length - 1];
    },
    //获取资源站点域名
    getResDomain: function () {
        var domain = window.location.protocol + '//' + "res." + App_Config.getCrossRes();

        return domain;
    },
    //获取OSS域名
    getOssDomain: function () {
        return "http://test.oss.jd-soft.cn/";
    }
}
