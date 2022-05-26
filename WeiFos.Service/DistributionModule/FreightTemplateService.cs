using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data;
using WeiFos.Entity.DistributionModule;
using WeiFos.Service;
using WeiFos.Entity.Enums;

namespace WeiFos.Service.DistributionModule
{
    /// <summary>
    /// 运费模板 Service
    /// @author yewei
    /// add by  @date 2015-10-28
    /// </summary>
    public class FreightTemplateService : BaseService<FreightTemplate>
    {




        /// <summary>
        /// 保存运费模板
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="entity"></param>
        /// <param name="FRegions"></param>
        /// <returns></returns>
        public StateCode Save(long userID, FreightTemplate entity, List<FreightRegion> FRegions)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                s.StartTransaction();
                try
                {
                    if (entity.id == 0)
                    {
                        entity.created_date = DateTime.Now;
                        entity.created_user_id = userID;
                        s.Insert<FreightTemplate>(entity);
                    }
                    else
                    {
                        entity.updated_date = DateTime.Now;
                        entity.updated_user_id = userID;
                        s.Update<FreightTemplate>(entity);
                    }

                    //临时模板集合
                    List<FreightRegion> FRegionTmps = new List<FreightRegion>();

                    //当前存在的模板
                    List<FreightRegion> FRegionOlds = s.List<FreightRegion>("where freight_template_id = @0", entity.id);

                    if (FRegions != null)
                    {
                        foreach (FreightRegion fr in FRegions)
                        {
                            fr.freight_template_id = entity.id;
                            int exist = s.Exist<FreightRegion>("where id = @0", fr.id);
                            if (exist > 0)
                            {
                                s.Update<FreightRegion>(fr);
                                FRegionTmps.Add(fr);
                            }
                            else
                            {
                                int exist_fr = s.Exist<FreightRegion>("where region_name like '%'+@0+'%'", fr.region_name);
                                if (exist_fr == 0)
                                {
                                    s.Insert<FreightRegion>(fr);
                                }
                            }
                        }

                        //筛选出删除的数据
                        foreach (FreightRegion FTRegionOld in FRegionOlds)
                        {
                            bool exists = true;
                            foreach (FreightRegion FTRegionTmp in FRegionTmps)
                            {
                                if (FTRegionOld.id == FTRegionTmp.id)
                                {
                                    exists = false;
                                    break;
                                }
                            }
                            if (exists)
                            {
                                s.ExcuteUpdate("delete tb_dist_freight_template where id = @0", FTRegionOld.id);
                            }
                        }
                    }
                    else
                    {
                        s.ExcuteUpdate("delete tb_dist_freight_region where freight_template_id = @0", entity.id); 
                    }
                    s.Commit();
                    return StateCode.State_200;
                }
                catch
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
            }
        }




        /// <summary>
        /// 删除运费模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StateCode DeleteFreightTemplate(long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    int exist = s.Exist<DeliveryMode>("where freight_template_id = @0", id);
                    if (exist > 0) return StateCode.State_351;
                    s.Delete<FreightTemplate>(id);
                }
                catch
                {
                    return StateCode.State_500;
                }
                return StateCode.State_200;
            }
        }





    }
}
