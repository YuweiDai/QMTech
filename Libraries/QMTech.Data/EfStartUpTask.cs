﻿using QMTech.Core;
using QMTech.Core.Data;
using QMTech.Core.Infrastructure;

namespace QMTech.Data
{
    public class EfStartUpTask : IStartupTask
    {
        public void Execute()
        {
            var provider = EngineContext.Current.Resolve<IDataProvider>();
            if (provider == null)
                throw new QMTechException("No IDataProvider found");
            provider.SetDatabaseInitializer();
        }

        public int Order
        {
            //ensure that this task is run first 
            get { return -1000; }
        }
    }
}
