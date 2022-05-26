using Newtonsoft.Json;
using WeiFos.Entity.Common;
using WeiFos.Entity.LogsModule;
using WeiFos.Entity.OrgModule;
using WeiFos.Entity.UserModule;
using WeiFos.Entity.WeChatModule.EntModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data;
using WeiFos.SDK.Model;
using WeiFos.WeChat.Helper;
using WeiFos.WeChat.Helper.WeChatEnt;
using WeiFos.WeChat.Models.OrgEntity;
using WeiFos.Service;
using WeiFos.Entity.Enums;
using WeiFos.Service.LogsModule;

namespace Solution.Service.WeChatModule.EntModule
{

    /// <summary>
    /// 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
    /// Copyright (c) 2013-2018 深圳微狐信息技术有限公司
    /// 创 建：叶委
    /// 日 期：2019-03-15 14:38:04
    /// 描 述：企业号员工信息业务逻辑
    /// </summary>
    public class WeChatQYEmployeeService : BaseService<WeChatQYEmployee>
    {

        /// <summary>
        /// 根据用户ID获取
        /// </summary>
        /// <param name="userid">企业账号</param>
        /// <returns></returns>
        public WeChatQYEmployee Get(string userid)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<WeChatQYEmployee>("where userid = @0 ", userid);
            }
        }



        /// <summary>
        /// 微信自动登录
        /// 业务逻辑和登录有直接关系
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="signPackage"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public dynamic Login(string user_id, SignPackage signPackage, out StateCode code)
        { 
            code = StateCode.State_223;
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //员工信息
                WeChatQYEmployee employee = s.Get<WeChatQYEmployee>("where userid = @0", user_id);
                if (employee == null) return null;

                //用户信息
                User user = s.Get<User>("where id = @0", employee.user_id);
                if (user == null) return null;

                //重置当前用户所有令牌状态
                s.ExcuteUpdate("update tb_user_token set is_enable = @0 where user_id = @1", false, employee.id);

                int borrwIng = 0;
                int overdue = 0;
                int complete = 0;
                int apptIng = 0;
                //借阅中
                object _borrwIng = s.ExecuteScalar("select count(1) from tb_mbr_borrow_record where user_id = @0 and is_repay = @1 ", employee.user_id, false);
                //已逾期
                object _overdue = s.ExecuteScalar("select count(1) from v_borrow_log where is_repay = @0 and user_id = @1 and is_overdue = 1", false, employee.user_id);
                //已完成
                object _complete = s.ExecuteScalar("select count(1) from tb_mbr_borrow_record where user_id = @0 and is_repay = @1", employee.user_id, true);
                //预约中
                object _apptIng = s.ExecuteScalar("select count(1) from tb_mbr_appt_record where user_id = @0 and status = @1", employee.user_id, 0);

                borrwIng = int.Parse(_borrwIng.ToString());
                overdue = int.Parse(_overdue.ToString());
                complete = int.Parse(_complete.ToString());
                apptIng = int.Parse(_apptIng.ToString());

                signPackage.Token = Guid.NewGuid().ToString("N");
                UserToken token = new UserToken();
                token.last_time = DateTime.Now;
                token.created_date = DateTime.Now;
                token.os = signPackage.OS;
                token.user_id = user.id;
                token.imei = signPackage.IMEI;
                token.imsi = signPackage.IMSI;
                token.token = signPackage.Token;
                token.is_enable = true;
                s.Insert(token);

                //登录成功
                code = StateCode.State_200;
                //返回数据
                return new
                {
                    //Token
                    token.token,
                    //图像地址
                    headimg = employee.avatar,
                    //邮箱
                    email = user.email ?? "",
                    //手机号码
                    mobile = user.mobile ?? "",
                    //员工昵称
                    employee.name,
                    //企业用户ID
                    employee_id = employee.userid,
                    //用户ID
                    employee.user_id,
                    //统计
                    stats = new
                    {
                        //借阅中
                        borrwIng,
                        //已逾期
                        overdue,
                        //已完成
                        complete,
                        //预约中
                        apptIng
                    }
                };
            }
        }



        /// <summary>
        /// 微信自动登录
        /// 业务逻辑和登录有直接关系
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public dynamic refreshUser(long user_id, out StateCode code)
        {
            code = StateCode.State_500;
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //员工信息
                WeChatQYEmployee employee = s.Get<WeChatQYEmployee>("where user_id = @0", user_id);
                if (employee == null) return null;

                //用户信息
                User user = s.Get<User>("where id = @0", employee.user_id);
                if (user == null) return null;

                int borrwIng = 0;
                int overdue = 0;
                int complete = 0;
                int apptIng = 0;
                //借阅中
                object _borrwIng = s.ExecuteScalar("select count(1) from tb_mbr_borrow_record where user_id = @0 and is_repay = @1 ", employee.user_id, false);
                //已逾期
                object _overdue = s.ExecuteScalar("select count(1) from v_borrow_log where is_repay = @0 and user_id = @1 and is_overdue = 1", false, employee.user_id);
                //已完成
                object _complete = s.ExecuteScalar("select count(1) from tb_mbr_borrow_record where user_id = @0 and is_repay = @1", employee.user_id, true);
                //预约中
                object _apptIng = s.ExecuteScalar("select count(1) from tb_mbr_appt_record where user_id = @0 and status = @1", employee.user_id, 0);

                borrwIng = int.Parse(_borrwIng.ToString());
                overdue = int.Parse(_overdue.ToString());
                complete = int.Parse(_complete.ToString());
                apptIng = int.Parse(_apptIng.ToString());

                code = StateCode.State_200;
                //返回数据
                return new
                {
                    //图像地址
                    headimg = employee.avatar,
                    //邮箱
                    email = user.email ?? "",
                    //手机号码
                    mobile = user.mobile ?? "",
                    //员工昵称
                    employee.name,
                    //企业用户ID
                    employee_id = employee.userid,
                    //用户ID
                    employee.user_id,
                    //借阅中
                    borrwIng,
                    //已逾期
                    overdue,
                    //已完成
                    complete,
                    //预约中
                    apptIng
                };
            }
        }




        #region 自动同步员工接口

        /// <summary>
        /// 同步微信数据
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public StateCode SyncWeChatDept(string token)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    //通过微信接口获取部门信息
                    //s.StartTransaction();

                    //获取本地数据库微信同步部门数据
                    List<Department> depts = s.List<Department>("where wx_dept_id > @0", 0);

                    //获取本地数据库微信同步部门数据
                    List<WeChatQYEmployee> employees = s.List<WeChatQYEmployee>("", "");

                    if (depts.Count > 0)
                    {
                        //循环部门信息
                        foreach (var dept in depts)
                        {
                            //根据部门获取员工信息
                            string employeeJson = WeChatEntHelper.GetEmployeeByDeptId(token, dept.wx_dept_id, 1);
                            WeChatEmployeeResult employeeResult = JsonConvert.DeserializeObject<WeChatEmployeeResult>(employeeJson);

                            //成功获取到部门员工信息
                            if (employeeResult.errcode == 0)
                            {
                                //序列化成可直接可以写入数据库
                                WeChatQYEmployeeResult dbEmployeeResult = JsonConvert.DeserializeObject<WeChatQYEmployeeResult>(employeeJson);
                                //如果不存在微信企业员工信息
                                if (employees.Count == 0 && employeeResult.userlist.Count > 0)
                                {
                                    foreach (var e in dbEmployeeResult.userlist)
                                    {
                                        //写入对应用户信息
                                        User user = new User();
                                        user.email = e.email;
                                        user.mobile = e.mobile;
                                        user.login_name = e.mobile;
                                        user.created_date = DateTime.Now;
                                        s.Insert(user);

                                        //写入对应员工信息
                                        e.user_id = user.id;
                                        e.created_date = DateTime.Now;
                                        e.created_user_id = -1;
                                        s.Insert(e);

                                        WeChatEmployee weChatEmployee = employeeResult.userlist.Where(ee => ee.userid.Equals(e.userid)).SingleOrDefault();
                                        if (weChatEmployee != null)
                                        {
                                            for (int i = 0; i < weChatEmployee.department.Count(); i++)
                                            {
                                                //清空当前员工部门关系
                                                s.ExcuteUpdate("delete tb_wx_ent_empl_dept where user_id = @0", e.userid);

                                                WeChatEmplDept weChatEmplDept = new WeChatEmplDept();
                                                weChatEmplDept.dept_id = weChatEmployee.department[i];
                                                weChatEmplDept.user_id = e.userid;
                                                weChatEmplDept.order_index = weChatEmployee.order[i];
                                                weChatEmplDept.is_leader_in_dept = weChatEmployee.is_leader_in_dept[i] == 1;
                                                //保存员工部门关系
                                                s.Insert(weChatEmplDept);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (var e in dbEmployeeResult.userlist)
                                    {
                                        //企业员工
                                        WeChatQYEmployee employee = employees.Where(empl => empl.userid.Equals(e.userid)).SingleOrDefault();
                                        if (employee != null)
                                        {
                                            User user = s.Get<User>("where login_name = @0", employee.userid);
                                            if (user != null)
                                            {
                                                user.mobile = e.mobile;
                                                user.email = e.email;
                                                s.Update(user);
                                            }

                                            e.user_id = user.id;
                                            e.id = employee.id;
                                            e.updated_date = DateTime.Now;
                                            e.updated_user_id = -1;
                                            s.Update(e);
                                        }
                                        else
                                        {
                                            //写入对应用户信息
                                            User user = new User();
                                            user.email = e.email;
                                            user.mobile = e.mobile;
                                            user.login_name = e.userid;
                                            user.created_date = DateTime.Now;
                                            s.Insert(user);

                                            e.user_id = user.id;
                                            e.created_date = DateTime.Now;
                                            e.created_user_id = -1;
                                            s.Insert(e);
                                        }

                                        //处理员工部门中间表
                                        WeChatEmployee weChatEmployee = employeeResult.userlist.Where(ee => ee.userid.Equals(e.userid)).SingleOrDefault();
                                        if (weChatEmployee != null)
                                        {
                                            for (int i = 0; i < weChatEmployee.department.Count(); i++)
                                            {
                                                //清空当前员工部门关系
                                                s.ExcuteUpdate("delete tb_wx_ent_empl_dept where user_id = @0", e.userid);

                                                WeChatEmplDept weChatEmplDept = new WeChatEmplDept();
                                                weChatEmplDept.dept_id = weChatEmployee.department[i];
                                                weChatEmplDept.user_id = e.userid;
                                                weChatEmplDept.order_index = weChatEmployee.order[i];
                                                weChatEmplDept.is_leader_in_dept = weChatEmployee.is_leader_in_dept[i] == 1;
                                                //保存员工部门关系
                                                s.Insert(weChatEmplDept);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //s.Commit();
                    return StateCode.State_200;
                }
                catch (Exception ex)
                {
                    //s.RollBack();
                    s.Insert(new APILogs() { content = ex.ToString(), created_date = DateTime.Now, type = 1 });
                    return StateCode.State_500;
                }
            }
        }


        #endregion


        #region 微信企业号授权同步用户信息


        /// <summary>
        /// 微信企业号授权同步用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public WeChatQYUserResult AuthSyncWXUser(string token, string code)
        {
            try
            {
                //根据code获取成员信息
                WeChatQYUserResult qy_user = WeChatEntHelper.GetUserInfo(token, code);
                if (qy_user.errcode == 0)
                {
                    //如果是企业员工
                    if (!string.IsNullOrEmpty(qy_user.UserId))
                    {
                        //获取企业微信员工信息
                        string employee_json = WeChatEntHelper.GetEmployeeByUserIdJson(token, qy_user.UserId);

                        //序列化原始对应，这里存在是企业用户，但是没有加入到该应用里面来
                        WeChatEmployee weChatEmployee = JsonConvert.DeserializeObject<WeChatEmployee>(employee_json);
                        if (weChatEmployee.errcode == 0)
                        {
                            //同步至数据库
                            SyncWXUser(weChatEmployee, employee_json);
                            qy_user.IsEmployee = true;
                        }
                    }
                    else
                    {
                        //获取微信用户信息
                        ServiceIoc.Get<APILogsService>().SaveError("[获取用户信息异常]AuthSyncWXUser==>" + JsonConvert.SerializeObject(qy_user));
                    }
                }
                else
                {
                    ServiceIoc.Get<APILogsService>().SaveError("AuthSyncWXUser==>" + JsonConvert.SerializeObject(qy_user));
                }

                return qy_user;
            }
            catch (Exception ex)
            {
                ServiceIoc.Get<APILogsService>().SaveError("AuthSyncWXUser==>" + ex.ToString());
                return null;
            }
        }



        /// <summary>
        /// 认证同步微信用户数据
        /// </summary>
        /// <param name="weChatEmployee"></param>
        /// <param name="user_json"></param>
        public void SyncWXUser(WeChatEmployee weChatEmployee, string user_json)
        {

            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    //序列化成可直接可以写入数据库
                    WeChatQYEmployee dbEmployeeResult = JsonConvert.DeserializeObject<WeChatQYEmployee>(user_json);

                    //是否存在该员工信息
                    WeChatQYEmployee QYEmployee = s.Get<WeChatQYEmployee>("where userid = @0", dbEmployeeResult.userid);
                    //存在用户数据
                    if (QYEmployee == null)
                    {
                        //写入对应用户信息
                        User user = new User();
                        user.email = dbEmployeeResult.email;
                        user.mobile = dbEmployeeResult.mobile;
                        user.login_name = dbEmployeeResult.mobile;
                        user.created_date = DateTime.Now;
                        s.Insert(user);

                        dbEmployeeResult.user_id = user.id;
                        dbEmployeeResult.created_date = DateTime.Now;
                        dbEmployeeResult.created_user_id = -2;
                        s.Insert(dbEmployeeResult);
                    }
                    else
                    {
                        dbEmployeeResult.user_id = QYEmployee.user_id;
                        dbEmployeeResult.id = QYEmployee.id;
                        s.Update(dbEmployeeResult);
                    }

                    //处理员工对应部门信息
                    for (int i = 0; i < weChatEmployee.department.Count(); i++)
                    {
                        //清空当前员工部门关系
                        s.ExcuteUpdate("delete tb_wx_ent_empl_dept where user_id = @0", weChatEmployee.userid);

                        WeChatEmplDept weChatEmplDept = new WeChatEmplDept();
                        weChatEmplDept.dept_id = weChatEmployee.department[i];
                        weChatEmplDept.user_id = weChatEmployee.userid;
                        weChatEmplDept.order_index = weChatEmployee.order[i];
                        weChatEmplDept.is_leader_in_dept = weChatEmployee.is_leader_in_dept[i] == 1;

                        //保存员工部门关系
                        s.Insert(weChatEmplDept);
                    }

                    s.Commit();
                }
                catch (Exception ex)
                {
                    s.RollBack();
                }
            }
        }


        #endregion

    }
}
