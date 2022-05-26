/**
 * art-template帮助类  
 * @author 叶委          
 * add by date 2015-01-15
**/

/**
* 验证权限
* @param pcode 权限编码
* @return String
**/
template.defaults.imports.VerifyPermission = function (pcode) {
    return App_G.Auth.Filter(pcode);
}; 

 
/**
* josn字符串截取
* @param str 被截取的字符串
* @param len 截取的字符串长度
* @return String
**/
template.defaults.imports.cutSubString = function (str, len) {
    return App_G.Util.cutSubString(str, len);
}; 
 
/**
* josn日期格式化
* @param datetime json序列化后的日期
* json序列化格式日期 例如：/Date(1410192000000)/
* @return String
**/
template.defaults.imports.ChangeDateFormat = function (datetime) {
    return App_G.Util.Date.ChangeDateFormat(datetime);
}; 
 
/**
*显示Json完整时间
**/
template.defaults.imports.ChangeCompleteDateFormat = function (datetime) {
    return App_G.Util.Date.ChangeCompleteDateFormat(datetime);
}; 
 
/**
*显示Json完整时间 不含毫秒
**/
template.defaults.imports.ChangeComDateFormat = function (datetime) {
    return App_G.Util.Date.ChangeComDateFormat(datetime);
};

 
/**
* josn日期格式化
* @param datetime 当前值
* @return String
**/
template.defaults.imports.DateFormat = function (date, format){
    return App_G.Util.Date.DateFormat(date, format);
};
 

/**
* 隐藏名字
**/
template.defaults.imports.HiddenStr = function (tel) {
    if (tel == '' || tel == null) {
        return null;
    }
    var reg = /^(\w{1})\w+$/;
    return tel.replace(reg, "$1***");
}; 


/**
* 金额格式转换
* @param s 需要格式的字符串
* @param n 保留小数点位数
* @return String
**/
template.defaults.imports.formaToMoney = function (s, n) {
    return App_G.Util.formaToMoney(s, n);
}; 
  

/**
* 获取原图地址
* @param s 原图片地址
* @return String
**/
template.defaults.imports.getImgUrl = function (s, defurl) {
    return App_G.Util.getImgUrl(s, defurl);
}; 
 
/**
* 获取小图地址
* @param s 原图片地址
* @return String
**/
template.defaults.imports.getImgUrl_s = function (s, defurl) {
    return App_G.Util.getImgUrl_s(s, defurl);
};

 
/**
* 获取中图地址
* @param s 原图片地址
* @return String
**/
template.defaults.imports.getImgUrl_m = function (s, defurl) {
    return App_G.Util.getImgUrl_m(s, defurl);
};

 
/**
* 获取OSS图片地址
* @param s 原图片名称，OSS图片此存参数
* @return String
**/
template.defaults.imports.getOssImgUrl = function (s, p)  {
    return App_G.Util.getOssImgUrl(s, p);
};

  
/**
* int转换
**/
template.defaults.imports.parseInt = function (value) {
    return App_G.Util.parseInt(value);
};

 