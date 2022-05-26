using System; 
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.ProductModule
{
    /// <summary>
    /// 商品自定义规格值
    /// @author yewei
    /// add by @date 2015-04-15
    /// </summary>
    [Serializable]
    [Table(Name = "tb_pdt_spec_custom")]
    public class SpecCustom
    {
        #region Model
		/// <summary>
		/// 
		/// </summary>
		[ID]
		public long id
		{
			set;
			get;
		}
		/// <summary>
		/// 供应商商品ID
		/// </summary>
        public long product_id
		{
			set;
			get;
		}
		/// <summary>
		/// 规格名称ID
		/// </summary>
        public long specname_id
		{
			set;
			get;
		}
		/// <summary>
		/// 货号
		/// </summary>
        public long specvalue_id
		{
			set;
			get;
		}
		/// <summary>
		/// 供应商供货价
		/// </summary>
		public string custom_value
		{
			set;
			get;
		}
		/// <summary>
		/// 图片路径
		/// </summary>
		public string imgurl
		{
			set;
			get;
		}
		#endregion Model
    }
}
