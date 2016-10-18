using System;

namespace QMTech.Core.Data
{
    /// <summary>
    /// Base data provider manager
    /// </summary>
    public abstract class BaseDataProviderManager
    {
 
 
        /// <summary>
        /// Load data provider
        /// </summary>
        /// <returns>Data provider</returns>
        public abstract IDataProvider LoadDataProvider();
    }
}
