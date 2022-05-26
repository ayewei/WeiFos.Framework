using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Core.EnumHelper;

namespace WeiFos.Entity.Enums
{
    /// <summary>
    /// WeiFos 全局状态返回码
    /// @author yewei 
    /// @date 2015-02-11
    /// </summary>
    public enum StateCode
    {

        #region 常用状态

        /// <summary>
        /// 操作失败
        /// </summary>
        [EnumAttribute("操作失败")]
        State_500 = 500,

        /// <summary>
        /// 操作成功
        /// </summary>
        [EnumAttribute("操作成功")]
        State_200 = 200,

        /// <summary>
        /// 验证通过
        /// </summary>
        [EnumAttribute("验证通过")]
        State_0 = 0,

        /// <summary>
        /// 验证未通过
        /// </summary>
        [EnumAttribute("验证未通过")]
        State_1 = 1,

        /// <summary>
        /// 数据不存在
        /// </summary>
        [EnumAttribute("数据不存在")]
        State_2 = 2,

        /// <summary>
        /// 签名失败
        /// </summary>
        [EnumAttribute("签名失败")]
        State_3 = 3,

        /// <summary>
        /// 用户或密码错误
        /// </summary>
        [EnumAttribute("用户或密码错误")]
        State_4 = 4,

        /// <summary>
        /// 签名失败
        /// </summary>
        [EnumAttribute("签名失败")]
        State_5 = 5,

        /// <summary>
        /// 未知请求
        /// </summary>
        [EnumAttribute("未知请求")]
        State_6 = 6,

        /// <summary>
        /// 为空的令牌
        /// </summary>
        [EnumAttribute("为空的令牌")]
        State_7 = 7,

        #endregion


        #region 短信模块状态码 范围（50~100）

        /// <summary>
        /// 手机号码不能为空
        /// </summary>
        [EnumAttribute("手机号码不能为空")]
        State_50 = 50,

        /// <summary>
        /// 验证码不存在
        /// </summary>
        [EnumAttribute("验证码不存在")]
        State_51 = 51,

        /// <summary>
        /// 验证码已过期
        /// </summary>
        [EnumAttribute("验证码已过期")]
        State_52 = 52,

        /// <summary>
        /// 验证码错误
        /// </summary>
        [EnumAttribute("验证码错误")]
        State_53 = 53,

        /// <summary>
        /// 请点击获取验证
        /// </summary>
        [EnumAttribute("请点击获取验证")]
        State_54 = 54,

        /// <summary>
        /// 验证码不能为空
        /// </summary>
        [EnumAttribute("验证码不能为空")]
        State_55 = 55,

        /// <summary>
        /// 验证码已经被使用过了
        /// </summary>
        [EnumAttribute("验证码已经被使用过了")]
        State_56 = 56,

        /// <summary>
        /// 手机号码格式有误
        /// </summary>
        [EnumAttribute("手机号码格式有误")]
        State_57 = 57,

        #endregion


        #region 系统用户状态 范围（101~150）

        /// <summary>
        /// 用户或密码错误
        /// </summary>
        [EnumAttribute("用户或密码错误")]
        State_101 = 101,

        /// <summary>
        /// 登录超时
        /// </summary>
        [EnumAttribute("登录超时")]
        State_102 = 102,

        /// <summary>
        /// 密码错误
        /// </summary>
        [EnumAttribute("密码错误")]
        State_103 = 103,

        /// <summary>
        /// 验证码错误
        /// </summary>
        [EnumAttribute("验证码错误")]
        State_104 = 104,


        #endregion


        #region 微信公众号状态码 范围（151~200）

        /// <summary>
        /// 微信公众号类型验证失败
        /// </summary>
        [EnumAttribute("微信公众号类型验证失败,请核对输入是否有误! 错误代码")]
        State_151 = 151,

        /// <summary>
        /// 微信菜单设置失败
        /// </summary>
        [EnumAttribute("微信菜单设置失败")]
        State_152 = 152,

        #endregion


        #region 平台用户状态 （201~250）

        /// <summary>
        /// 登录名或登录密码不正确
        /// </summary>
        [EnumAttribute("用户或密码不正确")]
        State_201 = 201,

        /// <summary>
        /// 该用户已经被注册
        /// </summary>
        [EnumAttribute("该用户已经被注册")]
        State_202 = 202,

        /// <summary>
        /// 两次密码不一致
        /// </summary>
        [EnumAttribute("两次密码不一致")]
        State_203 = 203,

        /// <summary>
        /// 无效的令牌token
        /// </summary>
        [EnumAttribute("无效的令牌token。")]
        State_204 = 204,

        /// <summary>
        /// 登录信息已失效
        /// </summary>
        [EnumAttribute("登录信息已失效")]
        State_205 = 205,

        /// <summary>
        /// 手机号码已经被注册
        /// </summary>
        [EnumAttribute("手机号码已经被注册")]
        State_206 = 206,

        /// <summary>
        /// 不存在的手机号码
        /// </summary>
        [EnumAttribute("不存在的手机号码")]
        State_207 = 207,

        /// <summary>
        /// 请绑定新手机号码
        /// </summary>
        [EnumAttribute("请绑定新手机号码")]
        State_208 = 208,

        /// <summary>
        /// 用户已被冻结
        /// </summary>
        [EnumAttribute("用户已被冻结")]
        State_209 = 209,

        /// <summary>
        /// 无效的令牌token
        /// </summary>
        [EnumAttribute("无效的令牌token。")]
        State_210 = 210,

        /// <summary>
        /// 令牌Token不能为空
        /// </summary>
        [EnumAttribute("请登录后再操作")]
        State_211 = 211,

        /// <summary>
        /// 登录信息已失效
        /// </summary>
        [EnumAttribute("登录信息已失效")]
        State_212 = 212,

 
        /// <summary>
        /// 手机号码已经被注册
        /// </summary>
        [EnumAttribute("手机号码已经被注册")]
        State_214 = 214,
        /// <summary>
        /// 邮箱已经被注册
        /// </summary>
        [EnumAttribute("邮箱已经被注册")]
        State_215 = 215,
        /// <summary>
        /// 证件已经被注册
        /// </summary>
        [EnumAttribute("证件已经被注册")]
        State_216 = 216,
        /// <summary>
        /// 请绑定新手机号码
        /// </summary>
        [EnumAttribute("请绑定新手机号码")]
        State_217 = 217,
        /// <summary>
        /// 邮箱不能为空
        /// </summary>
        [EnumAttribute("邮箱不能为空")]
        State_218 = 218,
        /// <summary>
        /// 请不要绑定当前邮箱
        /// </summary>
        [EnumAttribute("请不要绑定当前邮箱")]
        State_219 = 219,
        /// <summary>
        /// 邮件已发送
        /// </summary>
        [EnumAttribute("邮件已发送")]
        State_220 = 220,
        /// <summary>
        /// 邮件发送失败
        /// </summary>
        [EnumAttribute("邮件发送失败")]
        State_221 = 221,
        /// <summary>
        /// 请绑定手机号码
        /// </summary>
        [EnumAttribute("请绑定手机号码")]
        State_222 = 222,

        /// <summary>
        /// 不存在该用户
        /// </summary>
        [EnumAttribute("用户信息不存在")]
        State_223 = 223,

        /// <summary>
        /// 暂未配送邮件模板
        /// </summary>
        [EnumAttribute("暂未配送邮件模板")]
        State_224 = 224,

        /// <summary>
        /// 邮件发送失败
        /// </summary>
        [EnumAttribute("邮件发送失败")]
        State_225 = 225,

        #endregion


        #region 物流配送 模块 （251~300）

        /// <summary>
        /// 系统当前未设置配送
        /// </summary>
        [EnumAttribute("系统当前未设置配送")]
        State_251 = 251,

        /// <summary>
        /// 不存在的用户收货地址
        /// </summary>
        [EnumAttribute("不存在的用户收货地址")]
        State_252 = 252,

        #endregion


        #region 资源上传模板状态码 范围（351~400）

        /// <summary>
        /// 上传参数异常
        /// </summary>
        [EnumAttribute("上传参数异常")]
        State_351 = 351,

        /// <summary>
        /// 上传票据校验失败
        /// </summary>
        [EnumAttribute("上传票据校验失败")]
        State_352 = 352,

        #endregion


        #region 平台商品模块 范围（501~550）

        /// <summary>
        /// 该商品不存在
        /// </summary>
        [EnumAttribute("该商品不存在")]
        State_501 = 501,

        /// <summary>
        /// 商品库存不足
        /// </summary>
        [EnumAttribute("商品库存不足")]
        State_502 = 502,

        /// <summary>
        /// 该商品存在数据引用
        /// </summary>
        [EnumAttribute("该商品存在数据引用")]
        State_503 = 503,

        /// <summary>
        /// 退订的商品数量不能大于订单商品数量
        /// </summary>
        [EnumAttribute("退订的商品数量不能大于订单商品数量")]
        State_504 = 504,

        /// <summary>
        /// 商品已下架
        /// </summary>
        [EnumAttribute("商品已下架")]
        State_505 = 505,

        /// <summary>
        /// 该分类存在子分类不能删除
        /// </summary>
        [EnumAttribute("该分类存在子分类不能删除")]
        State_506 = 506,

        #endregion


        #region 平台商品订单模块 范围（551~600）

        /// <summary>
        /// 不存在的商品订单
        /// </summary>
        [EnumAttribute("不存在的商品订单")]
        State_551 = 551,

        /// <summary>
        /// 该商品订单未支付
        /// </summary>
        [EnumAttribute("该商品订单未支付")]
        State_552 = 552,

        /// <summary>
        /// 该商品订单未申请退款
        /// </summary>
        [EnumAttribute("该商品订单未申请退款")]
        State_553 = 553,

        /// <summary>
        /// 退款金额不能大于订单金额
        /// </summary>
        [EnumAttribute("退款金额不能大于订单金额")]
        State_554 = 554,

        /// <summary>
        /// 该订单已发货
        /// </summary>
        [EnumAttribute("该订单已发货")]
        State_555 = 555,

        /// <summary>
        /// 订单金额不能为零
        /// </summary>
        [EnumAttribute("订单金额不能为零")]
        State_556 = 556,


        #endregion


        #region 资源模块 状态范围[9000至10000]

        /// <summary>
        /// 参数验证失败
        /// </summary>
        [EnumAttribute("参数验证失败")]
        State_9000 = 9000,


        /// <summary>
        /// 参数验证失败
        /// </summary>
        [EnumAttribute("参数验证失败")]
        State_9001 = 9001,

        #endregion


        #region 关键词模块 状态码范围[10001至 11000]

        /// <summary>
        /// 关键词不能为空
        /// </summary>
        [EnumAttribute("关键词不能为空")]
        State_10005 = 10005,

        /// <summary>
        /// 关键词已存在
        /// </summary>
        [EnumAttribute("关键词已存在")]
        State_10010 = 10010,

        #endregion


        #region 微信菜单绑定 状态码范围[11001至 12000]

        /// <summary>
        /// 微信菜单设置失败
        /// </summary>
        [EnumAttribute("微信菜单设置失败")]
        State_11001 = 11001,

        /// <summary>
        /// 微信菜单设置失败
        /// </summary>
        [EnumAttribute("微信菜单设置失败")]
        State_11002 = 11002,

        #endregion


        #region 微信活动模块 状态码范围[20005至 21000]

        /// <summary>
        /// 活动奖品总数量为0
        /// </summary>
        [EnumAttribute("活动奖品总数量为0")]
        State_20005 = 20005,

        /// <summary>
        /// 投票选项为0
        /// </summary>
        [EnumAttribute("投票选项为0")]
        State_20010 = 20010,

        /// <summary>
        /// 调研题目为0
        /// </summary>
        [EnumAttribute("投票选项为0")]
        State_20015 = 20015,

        /// <summary>
        /// 调研题目为0
        /// </summary>
        [EnumAttribute("投票选项为0")]
        State_20020 = 20020,

        #endregion


        #region 用户模块 状态码范围[30005至 31000]

        /// <summary>
        /// 验证码错误
        /// </summary>
        [EnumAttribute("验证码错误")]
        State_30005 = 30005,

        /// <summary>
        /// 用户注册证码错误
        /// </summary>
        [EnumAttribute("用户注册证码错误")]
        State_30010 = 30010,

        /// <summary>
        /// 用户登录名存在
        /// </summary>
        [EnumAttribute("用户登录名存在")]
        State_30015 = 30015,

        /// <summary>
        /// 用户邮箱存在
        /// </summary>
        [EnumAttribute("用户邮箱存在")]
        State_30020 = 30020,

        /// <summary>
        /// 手机号码存在
        /// </summary>
        [EnumAttribute("手机号码存在")]
        State_30025 = 30025,

        /// <summary>
        /// 用户或密码错误
        /// </summary>
        [EnumAttribute("用户或密码错误")]
        State_30030 = 30030,

        /// <summary>
        /// 用户或密码错误
        /// </summary>
        //[EnumAttribute("用户或密码错误")]
        //State_30035 = 30035,

        /// <summary>
        /// 用户未登陆
        /// </summary>
        [EnumAttribute("用户未登陆")]
        State_30040 = 30040,

        /// <summary>
        /// 非法请求，请操作当前账号
        /// </summary>
        [EnumAttribute("非法请求，请操作当前账号")]
        State_30045 = 30045,

        /// <summary>
        /// 官网栏目的上级分类不能是该栏目子类
        /// </summary>
        [EnumAttribute("用户账号匹配不成功")]
        State_30050 = 30050,

        /// <summary>
        /// 已达到定义上限
        /// </summary>
        [EnumAttribute("已达到定义上限")]
        State_30060 = 30060,


        #endregion  
          

        #region 商品模块 状态码范围[50000至 51000]

        /// <summary>
        /// 商品已经下架
        /// </summary>
        [EnumAttribute("商品已经下架")]
        State_50000 = 50000,

        /// <summary>
        /// 商品库存不足
        /// </summary>
        [EnumAttribute("商品库存不足")]
        State_50001 = 50001,

        /// <summary>
        /// 商品定单未完成
        /// </summary>
        [EnumAttribute("商品定单未完成")]
        State_50010 = 50010,

        #endregion


        #region 商户后台模块 状态码范围[50000至 51000]

        /// <summary>
        /// 代理商开单 套餐日期不能小于当天
        /// </summary>
        [EnumAttribute("代理商开单 套餐日期不能小于当天")]
        State_30065 = 30065,

        #endregion


    }
}
