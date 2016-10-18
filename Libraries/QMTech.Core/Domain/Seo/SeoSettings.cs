using QMTech.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Core.Domain.Seo
{
    /// <summary>
    /// SEO设置
    /// </summary>
    public class SeoSettings : ISettings
    {
        /// <summary>
        /// WWW requires (with or without WWW)
        /// </summary>
        public WwwRequirement WwwRequirement { get; set; }
    }
}
