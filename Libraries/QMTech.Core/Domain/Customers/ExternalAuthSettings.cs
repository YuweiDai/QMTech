using QMTech.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Core.Domain.Customers
{
    /// <summary>
    /// 第三方登录key
    /// </summary>
    public class ExternalAuthSettings : ISettings
    {
        public string ClientKeyIdentifier { get; set; }
        public string ClientSecret { get; set; }

        public string SystemName { get; set; }
    }
}
