using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QMTech.Api.Models.Catalog
{
    public class StoresStatisticsModel
    {
        /// <summary>
        /// 商店总数
        /// </summary>
        public int StoresCount { get; set; }

        /// <summary>
        /// 商品总数
        /// </summary>
        public int ProductCount { get; set; }

        /// <summary>
        /// 合作商家
        /// </summary>
        public int CoorperateCount { get; set; }

        /// <summary>
        /// 营业中的商店
        /// </summary>
        public int InSalesCount { get; set; }
    }
}