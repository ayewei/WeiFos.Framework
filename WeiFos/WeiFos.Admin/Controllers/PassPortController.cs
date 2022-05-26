using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeiFos.Admin.Code;
using WeiFos.Admin.Code.Authorization;
using WeiFos.Core;
using Microsoft.AspNetCore.Http;
using WeiFos.Entity.Enums;
using WeiFos.Entity.SystemModule;
using WeiFos.Service;
using WeiFos.NetCore.Extensions;
using WeiFos.Service.SystemModule;
using WeiFos.Service.LogsModule;

namespace WeiFos.Admin.Controllers
{

    /// <summary>
    /// Copyright (c) 2013-2018 深圳微狐信息科技有限公司
    /// 描 述：登录控制器 
    /// 创建人：叶委
    /// 日 期：2019.01.10
    /// </summary>
    public class PassPortController : BaseController
    {

        #region 登录功能模块

        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        { 
            if (LoginUser.Instance.IsLogin(HttpContext)) return LocalRedirect("~/Home/Index");

            //生成公私钥对
            (string, string) p = AlgorithmHelper.CreateKeyPair();

            //公钥
            ViewBag.publicKey = p.Item1;

            //私钥存储在绘话状态中
            HttpContext.Session.SetString("privateKey", p.Item2);

            return View();
        }



        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="posx"></param>
        /// <param name="loginpk"></param>
        /// <param name="verifyvode"></param>
        /// <returns></returns>
        public JsonResult LoginIn([FromBody] dynamic data)
        {
            //图形验证码
            //if (!LoginUser.VerificationCode.ToString().Equals(vcode))  return Json(GetResult(StateCode.State_104), JsonRequestBehavior.AllowGet);
          
            //当前私钥
            string private_key = HttpContext.Session.GetString("privateKey").Replace("\r\n", "");
             
            //解密登录信息
            string posx = AlgorithmHelper.Decrypt(private_key, data.posx.ToString());

            //用户密码
            string username = posx.Split("\\")[0];  
            string password = posx.Split("\\")[1];

            //登录
            SysUser user = ServiceIoc.Get<SysUserService>().Login(username, password, HttpContext.GetClientIp());
            if (user.login_code == StateCode.State_200)
            {
                //登录操作
                LoginUser.Instance.LoginIn(user, this.HttpContext);
                return Json(GetResult(user.login_code));
            }
    
            return Json(GetResult(user.login_code));
        }
         


        /// <summary>
        /// 加载用户登录菜单
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [LoginAuth]
        public JsonResult LoadUserMenus(SysUser user)
        {
            List<SysModelMenu> menus = null;
            if ((bool)user.is_manager)
            {
                menus = AppGlobal.Instance.Menus;
            }
            else
            {
                foreach (SysModelMenu menu in AppGlobal.Instance.Menus)
                {
                    foreach (SysPermission p in LoginUser.Instance.Permissions)
                    {
                        if (p.code != null && p.code.Equals(menu.serial_no))
                        {
                            menus.Add(menu);
                        }
                    }
                }
            }

            return Json(GetResult(StateCode.State_200, menus));
        }


        /// <summary>
        /// 获取当前用户权限集合
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [LoginAuth]
        public JsonResult GetMyPCode(string url)
        {
            return Json(LoginUser.Instance.GetArrayCodeByUrl(url));
        }


        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOut()
        {
            LoginUser.Instance.LoginOut(this.HttpContext);
            return Redirect(AppGlobal.Admin);
        }



        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult GetVerifyCode(string type)
        {
            string code = VerifyCode.CreateRandomCode(4).ToLower();
            LoginUser.Instance.VerificationCode = code;
            byte[] bytes = VerifyCode.CreateImage(code);
            return File(bytes, @"image/jpeg");
        }




        #endregion


    }
}