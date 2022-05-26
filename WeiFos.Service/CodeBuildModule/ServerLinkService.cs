using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WeiFos.CodeBuilder.Builder.CSharp;
using WeiFos.CodeBuilder.Entity;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Entity.CodeBuildModule;
using WeiFos.Entity.Enums;
using WeiFos.Entity.SystemModule;
using WeiFos.ORM.Data;
using WeiFos.ORM.Data.DBEntityModule;

namespace WeiFos.Service.CodeBuildModule
{
    /// <summary>
    /// 数据库服务器链接 Service
    /// @author yewei
    /// add by  @date 2018-10-28
    /// </summary>
    public class ServerLinkService : BaseService<ServerLink>
    {

        //查询表sql
        private string select_tables = @"DECLARE @TableInfo TABLE (name VARCHAR(50),sumrows VARCHAR(11), reserved VARCHAR(50) ,data VARCHAR(50),index_size VARCHAR(50) ,unused VARCHAR(50), pk VARCHAR(50) )
                            DECLARE @TableName TABLE ( name VARCHAR(50) )
                            DECLARE @name VARCHAR(50)
                            DECLARE @pk VARCHAR(50)
                            INSERT INTO @TableName ( name ) SELECT o.name FROM sysobjects o,sysindexes i 
							    WHERE o.id = i.id AND o.Xtype = 'U' AND i.indid < 2 {0} ORDER BY i.rows DESC , o.name
                            WHILE EXISTS (SELECT 1 FROM @TableName) 
							BEGIN 
								SELECT TOP 1 @name = name FROM @TableName DELETE @TableName WHERE name = @name DECLARE @objectid INT SET @objectid = OBJECT_ID(@name) SELECT @pk = COL_NAME(@objectid, colid) FROM sysobjects AS o INNER JOIN sysindexes AS i ON i.name = o.name INNER JOIN sysindexkeys AS k ON k.indid = i.indid WHERE o.xtype = 'PK' AND parent_obj = @objectid AND k.id = @objectid INSERT INTO @TableInfo ( name , sumrows , reserved , data , index_size , unused ) EXEC sys.sp_spaceused @name UPDATE @TableInfo SET pk = @pk WHERE name = @name END
								SELECT F.name as name, F.reserved  as reserved, F.data as data, F.index_size as index_size, RTRIM(F.sumrows) AS sumrows , F.unused as unused, ISNULL(p.remark, f.name) AS remark , F.pk as pk
								FROM @TableInfo F
								LEFT JOIN (
								SELECT name = CASE WHEN A.COLORDER = 1 THEN D.NAME ELSE '' END, 
								remark = CASE WHEN A.COLORDER = 1 THEN ISNULL(F.VALUE, '') ELSE '' 
							END FROM SYSCOLUMNS A LEFT JOIN SYSTYPES B ON A.XUSERTYPE = B.XUSERTYPE INNER JOIN SYSOBJECTS D ON A.ID = D.ID AND D.XTYPE = 'U' AND D.NAME <> 'DTPROPERTIES' LEFT JOIN sys.extended_properties F ON D.ID = F.major_id WHERE a.COLORDER = 1 AND F.minor_id = 0 ) P ON F.name = p.name
                             ORDER BY f.name";



        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public StateCode Save(long user_id, ServerLink entity)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    if (entity.db_type == DBType.SqlServer)
                    {
                        //Data Source=119.23.35.85;Initial Catalog=soutang;User ID=sa;password=myyd.net2017
                        //string r = Regex.Match(s, @"database=([^;]+)").Groups[1].Value;
                        string ConnStr = entity.connection_str.ToLower();
                        Dictionary<string, string> dictionary = ConnStr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToDictionary(x => x.Split('=')[0], x => x.Split('=')[1]);

                        foreach (KeyValuePair<string, string> pair in dictionary)
                        {
                            switch (pair.Key)
                            {
                                case "data source":
                                    entity.ip = pair.Value;
                                    break;
                                case "user id":
                                    entity.login_name = pair.Value;
                                    break;
                                case "password":
                                    entity.psw = pair.Value;
                                    break;
                                case "initial catalog":
                                    entity.db_name = pair.Value;
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                    else if (entity.db_type == DBType.Oracle)
                    {

                    }
 
                    if (entity.id == 0)
                    {
                        entity.created_date = DateTime.Now;
                        entity.created_user_id = user_id;
                        s.Insert<ServerLink>(entity);
                    }
                    else
                    {
                        entity.updated_date = DateTime.Now;
                        entity.updated_user_id = user_id;
                        s.Update<ServerLink>(entity);
                    }
                     
                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }



        /// <summary>
        /// 加载
        /// </summary>
        /// <returns></returns>
        public List<dynamic> LoadServer()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //当前链接集合
                List<ServerLink> links = s.List<ServerLink>("");

                //当前项目配置集合
                List<ProjectSetting> projects = s.List<ProjectSetting>("");

                //根据IP分组
                var group_links = links.GroupBy(a => a.ip).Select(g => (new { name = g.Key, count = g.Count() }));

                #region 具备ip项目配置集合

   
                #endregion

                List<dynamic> data = new List<dynamic>();

                foreach (var link in group_links)
                {
                    //返回项目集合
                    List<dynamic> _projects = new List<dynamic>();
                    foreach (var p in projects)
                    {
                        var tmp_link = links.Where(l => l.id == p.link_id).SingleOrDefault();
                        string ip = tmp_link == null ? "" : tmp_link.ip;
                        if (ip.Equals(link.name))
                        {
                            var project = new
                            {
                                p.id,
                                p.name,
                                p.link_id,
                                ip
                            };
                            _projects.Add(project);
                        }
                    }

                    var tmp = new
                    {
                        link.name,
                        projects = _projects
                    };

                    data.Add(tmp);
                }

                return data;
            }
        }



        /// <summary>
        /// 获取Table名称
        /// </summary>
        /// <param name="table_id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public TableInfo GetTableByName(int table_id, string name)
        {
            ServerLink link = null;
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //服务器链接
                link = s.Get<ServerLink>(table_id);
            }

            //查询表数据集合
            TableInfo entity = new TableInfo();

            //SqlServer数据库
            if (link.db_type == DBType.SqlServer)
            {
                using (SqlConnection conn = new SqlConnection(link.connection_str))
                {
                    conn.Open(); 

                    SqlCommand command = new SqlCommand(string.Format(select_tables, "and o.name = @0"), conn);
                    command.Parameters.Add(new SqlParameter("@0", SqlDbType.VarChar)).Value = name;

                    SqlDataReader dr = command.ExecuteReader();
                    if (dr.Read())
                    {  
                        entity.name = dr["name"].ToString();
                        entity.remark = dr["remark"].ToString();
                    }
                    conn.Close();
                }

                //表字段详情
                entity.fields = GetDataTableInfo(table_id, name);

                return entity;
            }

            return null;
        }



        /// <summary>
        /// 查询服务器数据库
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<TableInfo> GetTables(long id)
        {
            ServerLink link = null;
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                link = s.Get<ServerLink>(id);
            }

            //查询表数据集合
            List<TableInfo> data = new List<TableInfo>();

            //SqlServer数据库
            if (link.db_type == DBType.SqlServer)
            {
                using (SqlConnection conn = new SqlConnection(link.connection_str))
                {
                    conn.Open();  

                    SqlCommand command = new SqlCommand(string.Format(select_tables, ""), conn);
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        data.Add(new TableInfo
                        { 
                            name = dr["name"].ToString(),
                            remark = dr["remark"].ToString()
                        });
                    }

                    conn.Close();
                }
            }

            return data;
        }




        /// <summary>
        /// 加载表详情
        /// </summary>
        /// <param name="link_id"></param>
        /// <param name="tb_name"></param>
        /// <returns></returns>
        public List<FieldDetail> GetDataTableInfo(int link_id, string tb_name)
        {
            ServerLink link = null;
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                link = s.Get<ServerLink>(link_id);
            }

            if (link == null) return null;

            //查询表数据集合
            List<FieldDetail> data = new List<FieldDetail>();

            //SqlServer数据库
            if (link.db_type == DBType.SqlServer)
            {
                using (SqlConnection conn = new SqlConnection(link.connection_str))
                {
                    conn.Open();

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(@"SELECT [order_index] = a.colorder , [column] = a.name , [datatype] = b.name , [length] = COLUMNPROPERTY(a.id, a.name, 'PRECISION') , [identity] = CASE WHEN COLUMNPROPERTY(a.id, a.name, 'IsIdentity') = 1 THEN '1' ELSE '' END , [key] = CASE WHEN EXISTS ( SELECT 1 FROM sysobjects WHERE xtype = 'PK' AND parent_obj = a.id AND name IN ( SELECT name FROM sysindexes WHERE indid IN ( SELECT indid FROM sysindexkeys WHERE id = a.id AND colid = a.colid ) ) ) THEN '1' ELSE '' END , [isnullable] = CASE WHEN a.isnullable = 1 THEN '1' ELSE '' END , [defaults] = ISNULL(e.text, '') , [remark] = ISNULL(g.[value], a.name)
                                FROM syscolumns a LEFT JOIN systypes b ON a.xusertype = b.xusertype INNER JOIN sysobjects d ON a.id = d.id AND d.xtype = 'U' AND d.name <> 'dtproperties' LEFT JOIN syscomments e ON a.cdefault = e.id LEFT JOIN sys.extended_properties g ON a.id = g.major_id AND a.colid = g.minor_id LEFT JOIN sys.extended_properties f ON d.id = f.major_id AND f.minor_id = 0
                                WHERE d.name = @0
                                ORDER BY a.id , a.colorder");

                    SqlCommand command = new SqlCommand(strSql.ToString(), conn);
                    command.Parameters.Add(new SqlParameter("@0", SqlDbType.VarChar)).Value = tb_name;
                    SqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        data.Add(new FieldDetail
                        {
                            order_index = int.Parse(dr["order_index"].ToString()),
                            name = dr["column"].ToString(),
                            typename = dr["datatype"].ToString(),
                            length = int.Parse(dr["length"].ToString()),
                            is_identity = "1".Equals(dr["identity"].ToString()),
                            is_primary = "1".Equals(dr["key"].ToString()),
                            remark = dr["remark"].ToString(),
                            is_nullable = "1".Equals(dr["isnullable"].ToString())
                        });
                    }

                    conn.Close();
                }
            }

            return data;
        }




        /// <summary>
        /// 加载代码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="link_id"></param>
        /// <param name="pro_module"></param>
        /// <param name="tb_name"></param>
        /// <returns></returns>
        public CSharpMVCCode LoadCode(SysUser user, int link_id, string[] pro_module, string tb_name)
        {
            //获取当前表详情
            TableInfo tableInfo = GetTableByName(link_id, tb_name);

            //MVC项目
            return BuildCSharpMVC.CreateCode(tableInfo, pro_module, user.login_name);
        }





    }
}
