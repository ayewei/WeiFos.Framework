using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity.Enums
{
 
    /// <summary>
    /// 平台后台权限编号
    /// @author yewei 
    /// @date 2014-02-05
    /// </summary>
    public enum SysPermissionCodes
    {
        /// <summary>
        /// 系统管理
        /// </summary>
        XT,
        /// <summary>
        /// 用户管理
        /// </summary>
        XT_001,
        /// <summary>
        /// 用户管理-添加
        /// </summary>
        XT_001_001,
        /// <summary>
        /// /用户管理-修改
        /// </summary>
        XT_001_002,
        /// <summary>
        /// 用户管理-冻结、解冻
        /// </summary>
        XT_001_003,
        /// <summary>
        /// 角色管理
        /// </summary>
        XT_002,
        /// <summary>
        /// 角色管理-添加
        /// </summary>
        XT_002_002,
        /// <summary>
        /// 角色管理-修改
        /// </summary>
        XT_002_003,
        /// <summary>
        /// 角色管理-启用、禁用
        /// </summary>
        XT_002_004,
        /// <summary>
        /// 系统菜单
        /// </summary>
        XT_003,
        /// <summary>
        /// 系统菜单-添加
        /// </summary>
        XT_003_001,
        /// <summary>
        /// 系统菜单-修改
        /// </summary>
        XT_003_002,
        /// <summary>
        /// 系统菜单-启用、禁用
        /// </summary>
        XT_003_003,
        /// <summary>
        /// 参数配置
        /// </summary>
        XT_004,
        /// <summary>
        /// 参数配置-添加
        /// </summary>
        XT_004_001,
        /// <summary>
        /// 参数配置-修改
        /// </summary>
        XT_004_002,
        /// <summary>
        /// 参数配置-启用、禁用
        /// </summary>
        XT_004_003,
        /// <summary>
        /// 权限管理
        /// </summary>
        XT_005,
        /// <summary>
        /// 权限管理-添加
        /// </summary>
        XT_005_001,
        /// <summary>
        /// 权限管理-修改
        /// </summary>
        XT_005_002,
        /// <summary>
        /// 权限管理-删除
        /// </summary>
        XT_005_003,
        /// <summary>
        /// 操作日志
        /// </summary>
        XT_006,
        /// <summary>
        /// 操作日志-搜索
        /// </summary>
        XT_006_001,
        /// <summary>
        /// 平台管理
        /// </summary>
        PT,
        /// <summary>
        /// 平台管理-平台套餐
        /// </summary>
        PT_001,
        /// <summary>
        /// 平台套餐-添加
        /// </summary>
        PT_001_001,
        /// <summary>
        /// 平台套餐-修改
        /// </summary>
        PT_001_002,
        /// <summary>
        /// 平台套餐-启用、禁用
        /// </summary>
        PT_001_003,

        /// <summary>
        /// 平台管理-平台套餐
        /// </summary>
        PT_002,
        /// <summary>
        /// 平台套餐-添加
        /// </summary>
        PT_002_001,
        /// <summary>
        /// 平台套餐-管理
        /// </summary>
        PT_002_002,
        /// <summary>
        /// 平台管理-套餐功能
        /// </summary>
        PT_003,
        /// <summary>
        /// 套餐功能-添加
        /// </summary>
        PT_003_001,
        /// <summary>
        /// 套餐功能-管理
        /// </summary>
        PT_003_002,


        /// <summary>
        /// 平台管理-主题管理
        /// </summary>
        PT_004,
        /// <summary>
        /// 套餐功能-添加
        /// </summary>
        PT_004_001,
        /// <summary>
        /// 套餐功能-管理
        /// </summary>
        PT_004_002,


        /// <summary>
        /// 平台管理-资讯类别
        /// </summary>
        PT_005,
        /// <summary>
        /// 资讯类别-添加
        /// </summary>
        PT_005_001,
        /// <summary>
        /// 资讯类别-修改
        /// </summary>
        PT_005_002,
        /// <summary>
        /// 资讯类别-删除
        /// </summary>
        PT_005_003,
        /// <summary>
        /// 资讯类别-保存排序
        /// </summary>
        PT_005_004,
        /// <summary>
        /// 资讯类别-转移资讯
        /// </summary>
        PT_005_005,

        /// <summary>
        /// 平台管理-资讯列表
        /// </summary>
        PT_006,
        /// <summary>
        /// 资讯列表-添加
        /// </summary>
        PT_006_001,
        /// <summary>
        /// 资讯列表-修改
        /// </summary>
        PT_006_002,
        /// <summary>
        /// 资讯列表-删除
        /// </summary>
        PT_006_003,
        /// <summary>
        /// 资讯列表-审核通过
        /// </summary>
        PT_006_004,
        /// <summary>
        /// 资讯列表-审核不通过
        /// </summary>
        PT_006_005,

        /// <summary>
        /// 平台推广
        /// </summary>
        TG,
        /// <summary>
        /// 平台推广-SEO类别
        /// </summary>
        TG_001,
        /// <summary>
        /// SEO类别-添加
        /// </summary>
        TG_001_001,
        /// <summary>
        /// SEO类别-修改
        /// </summary>
        TG_001_002,
        /// <summary>
        /// SEO类别-删除
        /// </summary>
        TG_001_003,
        /// <summary>
        /// SEO类别-保存排序
        /// </summary>
        TG_001_004,
        /// <summary>
        /// SEO类别-转移关键词
        /// </summary>
        TG_001_005,

        /// <summary>
        /// 平台推广-SEO关键词
        /// </summary>
        TG_002,
        /// <summary>
        /// SEO关键词-添加
        /// </summary>
        TG_002_001,
        /// <summary>
        /// SEO关键词-修改
        /// </summary>
        TG_002_002,
        /// <summary>
        /// SEO关键词-删除
        /// </summary>
        TG_002_003,

        /// <summary>
        /// 平台推广-网站SEO
        /// </summary>
        TG_004,
        /// <summary>
        /// 网站SEO-添加
        /// </summary>
        TG_004_001,
        /// <summary>
        /// 网站SEO-修改
        /// </summary>
        TG_004_002,
        /// <summary>
        /// 网站SEO-删除
        /// </summary>
        TG_004_003,

        /// <summary>
        /// 平台推广-网站地图
        /// </summary>
        TG_005,
        /// <summary>
        /// 网站地图-添加
        /// </summary>
        TG_005_001,
        /// <summary>
        /// 网站地图-修改
        /// </summary>
        TG_005_002,
        /// <summary>
        /// 网站地图-删除
        /// </summary>
        TG_005_003,
        /// <summary>
        /// 网站地图-生成
        /// </summary>
        TG_005_004,


        /// <summary>
        /// 商家信息
        /// </summary>
        SJ,
        /// <summary>
        /// 商家信息-添加
        /// </summary>
        SJ_001_001,
        /// <summary>
        /// 商家信息-修改
        /// </summary>
        SJ_001_002
        
    }

}
