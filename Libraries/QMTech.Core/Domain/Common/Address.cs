using System;

namespace QMTech.Core.Domain.Common
{
    public partial class Address : BaseEntity, ICloneable
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public bool Male { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string MobileNumber { get; set; }
       
        /// <summary>
        /// 详细地址
        /// </summary>
        public string FullAddress { get; set; }


        /// <summary>
        /// 门牌号，单元号
        /// </summary>
        public string Door { get; set; }

        /// <summary>
        /// 默认都为衢州
        /// </summary>
        public string City { get; set; }       

        /// <summary>
        /// 经度
        /// </summary>
        public double Lng { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// 自定义属性
        /// </summary>
        public string CustomAttributes { get; set; }

        public object Clone()
        {
            var addr = new Address
            {
                Name = this.Name,
                MobileNumber = this.MobileNumber,
                FullAddress = this.FullAddress,
                City = this.City,
                CustomAttributes = this.CustomAttributes,
                Lng = this.Lng,
                Lat = this.Lat
            };
            return addr;
        }
    }
}
