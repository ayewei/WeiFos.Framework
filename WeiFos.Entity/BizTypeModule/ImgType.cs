using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 

namespace WeiFos.Entity.BizTypeModule
{
    /// <summary>
    /// 图片路径配置实体类
    /// @author yewei 
    /// @date 2013-09-22
    /// </summary>
    public static class ImgType
    {

        //公用图片
        #region

        /// <summary>
        /// 缺省图片路径
        /// </summary>
        public const string Default = "Default";

        /// <summary>
        /// 公共图片文件夹
        /// </summary>
        public const string Commom = "Commom";

        //公用图片
        #endregion

        //微狐平台
        #region

        /// <summary>
        /// Banner图
        /// </summary>
        public const string Banner = "Banner";

        /// <summary>
        /// 平台用户 图
        /// </summary>
        public const string User = "User";

        /// <summary>
        /// 微信账号 图
        /// </summary>
        public const string WX_Account = "WX_Account";


        /// <summary>
        /// 官网logo 图
        /// </summary>
        public const string WebSite_Logo = "WebSite_Logo";

        /// <summary>
        /// 广告封面图片
        /// </summary>
        public const string Advertise = "Advertise";

        /// <summary>
        /// 图文封面 图
        /// </summary>
        public const string LbsReply_Title = "LbsReply_Title";

        /// <summary>
        /// 图文封面 图
        /// </summary>
        public const string ImgTextReply_Title = "ImgTextReply_Title";

        /// <summary>
        /// 图文详情 图
        /// </summary>
        public const string ImgTextReply_Details = "ImgTextReply_Details";

        /// <summary>
        /// 资讯分类
        /// </summary>
        public const string InformtCgty = "InformtCgty";

        /// <summary>
        /// 资讯信息
        /// </summary>
        public const string Informt = "Informt";

        /// <summary>
        /// 资讯详情信息
        /// </summary>
        public const string InformtDetails = "InformtDetails";

        /// <summary>
        /// 成功案例封面图
        /// </summary>
        public const string Case_Cover = "Case_Cover";

        /// <summary>
        /// 成功案例封详情图
        /// </summary>
        public const string Case_Details = "Case_Details";

        /// <summary>
        /// 活动开始 图
        /// </summary>
        public const string Activity_Start = "Activity_Start";
        
        /// <summary>
        /// 微商城商品封面图
        /// </summary>
        public const string Product_Cover = "Product_Cover";

        /// <summary>
        /// 微商城商品详情图
        /// </summary>
        public const string Product_Details = "Product_Details";

        /// <summary>
        /// 微商城商品导购类别图
        /// </summary>
        public const string GuideProduct_Cgty = "GuideProduct_Cgty";

        /// <summary>
        /// 商品品牌图
        /// </summary>
        public const string Product_Brand = "Product_Brand";

        /// <summary>
        /// 合作品牌图
        /// </summary>
        public const string Partner = "Partner";


        #endregion

        //微狐平台管理后台
        #region

        /// <summary>
        /// 后台系统用户图
        /// </summary>
        public const string SysUser = "SysUser";

        /// <summary>
        /// 资讯标题图
        /// </summary>
        public const string Info_Title = "Info_Title";

        /// <summary>
        /// 资讯内容图
        /// </summary>
        public const string Info_Content = "Info_Content";

        /// <summary>
        /// 平台主题案例图
        /// </summary>
        public const string Theme_Case = "Theme_Case";

        #endregion


        /// <summary>
        /// 图片业务类集合
        /// </summary>
        public static List<string> ImgTypeList = new List<string>(){
            ImgType.Default,
            ImgType.Commom,

            #region 微狐平台 
            ImgType.User,
            ImgType.Banner,
            ImgType.WX_Account,
            ImgType.WebSite_Logo,
            ImgType.Advertise,
            ImgType.InformtCgty,
            ImgType.Informt,
            ImgType.InformtDetails,
            ImgType.Case_Cover,
            ImgType.Case_Details,
            ImgType.ImgTextReply_Title,
            ImgType.ImgTextReply_Details,
            ImgType.Product_Cover,
            ImgType.Product_Details,
            ImgType.Product_Brand,
            ImgType.GuideProduct_Cgty,
            ImgType.Partner,
            #endregion

            #region 微狐平台管理后台
            ImgType.SysUser,
            ImgType.Info_Title,
            ImgType.Info_Content,
            ImgType.Theme_Case
            #endregion
             
        };


        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="imgtype"></param>
        /// <returns></returns>
        public static bool exist(string imgtype)
        {
            return ImgTypeList.Contains(imgtype);
        }


    }

}
