using System.Data.Entity.ModelConfiguration;

namespace QMTech.Data.Mapping
{
    public abstract class EntityTypeConfiguration<T> : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<T> where T : class
    {
        protected EntityTypeConfiguration()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {
            
        }
    }
}