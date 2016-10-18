using QMTech.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Core.Domain
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SystemInformationSettings:ISettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether store is closed
        /// </summary>
        public bool StoreClosed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether administrators can visit a closed store
        /// </summary>
        public bool StoreClosedAllowForAdmins { get; set; }

        /// <summary>
        /// 是否开启SSL协议
        /// </summary>
        public bool SslEnabled { get; set; }



        public bool DisplayMiniProfilerInPublicStore { get; set; }
    }
}
