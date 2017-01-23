﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Core.Data
{
    public partial class DataSettings
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public DataSettings()
        {
            RawDataSettings = new Dictionary<string, string>();
        }

        /// <summary>
        /// Data provider
        /// </summary>
        public string DataProvider { get; set; }

        /// <summary>
        /// Connection string
        /// </summary>
        public string DataConnectionString { get; set; }

        /// <summary>
        /// Raw settings file
        /// </summary>
        public IDictionary<string, string> RawDataSettings { get; private set; }

        /// <summary>
        /// A value indicating whether entered information is valid
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return !String.IsNullOrEmpty(this.DataProvider) && !String.IsNullOrEmpty(this.DataConnectionString);
        }
    }
}
