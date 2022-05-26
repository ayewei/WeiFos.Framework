using WeiFos.ORM.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using WeiFos.Entity.Enums;

namespace WeiFos.Service
{
    /// <summary>
    /// Service 基类
    /// @author yewei 
    /// @date 2013-04-28
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseService<T> where T : class, new()
    {

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="t"></param>
        public void Insert(T t)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                s.Insert<T>(t);
            }
        }

        /// <summary>
        /// 根据自定义表名 新增
        /// </summary>
        /// <param name="t"></param>
        /// <param name="tablename"></param>
        public void Insert(T t, string tablename)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                s.Insert<T>(t, tablename);
            }
        }

        /// <summary>
        /// 根据自定义表名 批量写入新增
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tablename"></param>
        public void BulkInsert(List<T> list, string tablename)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                s.BulkInsert<T>(list, tablename);
            }
        }


        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="list"></param>
        /// <param name="columns"></param>
        public void BatchUpdate(List<T> list, IEnumerable<string> columns)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    s.BatchUpdate<T>(list, columns);
                    s.Commit();
                }
                catch
                {
                    s.RollBack();
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ID"></param>
        public StateCode Delete(long Id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.Delete<T>(Id);
                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }

        /// <summary>
        /// 根据自定义表名 删除 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="tablename"></param>
        public StateCode Delete(long Id, string tablename)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.Delete<T>(Id, tablename);
                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="Id"></param>
        public StateCode Delete(long[] Ids)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.Delete<T>(Ids);
                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }

        /// <summary>
        /// 根据自定义表名 批量删除
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="tablename"></param>
        public StateCode Delete(long[] Ids, string tablename)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.Delete<T>(Ids, tablename);
                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="t"></param>
        public void Update(T t, string tablename)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                s.Update<T>(t, tablename);
            }
        }

        /// <summary>
        /// 根据自定义表名 修改
        /// </summary>
        /// <param name="t"></param>
        public void Update(T t)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                s.Update<T>(t);
            }
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        public T GetById(long Id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<T>(Id);
            }
        }

        /// <summary>
        /// 自定义表名 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public T GetById(long Id, string tablename)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<T>(Id, tablename);
            }
        }


        /// <summary>
        /// Lambda表达式条件查询
        /// 还没采用sqlparameter 处理参数
        /// 建议少用
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public List<T> Where(Expression<Func<T, bool>> expression)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Where(expression);
            }
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public List<T> GetList(Criteria c)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<T>(c);
            }
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="wheresql"></param>
        /// <param name="paraments"></param>
        /// <returns>T</returns>
        public List<T> GetList(string wheresql, params object[] paraments)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<T>(wheresql, paraments);
            }
        }

        /// <summary>
        /// 获取Top数据
        /// </summary>
        /// <param name="count"></param>
        /// <param name="wheresql"></param>
        /// <param name="paraments"></param>
        /// <returns></returns>
        public List<T> GetTop(int count, string wheresql, params object[] paraments)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.GetTop<T>(count, wheresql, paraments);
            }
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T</returns>
        public List<T> GetAll()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<T>("", "");
            }
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public DataTable Fill(Criteria criteria)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Fill(criteria);
            }
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paraments"></param>
        /// <returns></returns>
        public DataTable Fill(string sql, params object[] paraments)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Fill(sql, paraments);
            }
        }



    }

}

