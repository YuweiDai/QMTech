using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using QMTech.Core.Configuration;
using QMTech.Core.Infrastructure.DependencyManagement;
using Autofac.Integration.WebApi;
using System.Web.Http;

namespace QMTech.Core.Infrastructure
{
    /// <summary>
    /// Engine
    /// </summary>
    public class QMTechEngine : IEngine
    {
        #region Fields

        private ContainerManager _containerManager;
        private HttpConfiguration _httpConfig;

        #endregion

        #region Utilities

        /// <summary>
        /// Run startup tasks
        /// </summary>
        protected virtual void RunStartupTasks()
        {
            //var typeFinder = _httpConfig.DependencyResolver.GetService(typeof(ITypeFinder)) as ITypeFinder;
            var typeFinder = _containerManager.Resolve<ITypeFinder>();
            var startUpTaskTypes = typeFinder.FindClassesOfType<IStartupTask>();
            var startUpTasks = new List<IStartupTask>();
            foreach (var startUpTaskType in startUpTaskTypes)
                startUpTasks.Add((IStartupTask)Activator.CreateInstance(startUpTaskType));
            //sort
            startUpTasks = startUpTasks.AsQueryable().OrderBy(st => st.Order).ToList();
            foreach (var startUpTask in startUpTasks)
                startUpTask.Execute();
        }

        /// <summary>
        /// Register dependencies
        /// </summary>
        /// <param name="config">Config</param>
        protected virtual void RegisterDependencies(QMTechConfig config, HttpConfiguration httpConfig)
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();

            //we create new instance of ContainerBuilder
            //because Build() or Update() method can only be called once on a ContainerBuilder.


            //dependencies
            var typeFinder = new WebAppTypeFinder(config);
            builder = new ContainerBuilder();
            builder.RegisterInstance(config).As<QMTechConfig>().SingleInstance();
            builder.RegisterInstance(this).As<IEngine>().SingleInstance();
            builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();
            builder.Update(container);

            //register dependencies provided by other assemblies
            builder = new ContainerBuilder();
            var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            var drInstances = new List<IDependencyRegistrar>();
            foreach (var drType in drTypes)
                drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
            //sort
            drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            foreach (var dependencyRegistrar in drInstances)
                dependencyRegistrar.Register(builder, typeFinder);
            builder.Update(container);


            this._containerManager = new ContainerManager(container);

            var webApiResolver = new AutofacWebApiDependencyResolver(container);
            httpConfig.DependencyResolver = webApiResolver; //OWIN                    
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize components and plugins in the QMTech environment.
        /// 基于 web api 修改，配置app
        /// </summary>
        /// <param name="config">Config</param>
        public void Initialize(QMTechConfig config,HttpConfiguration httpConfig)
        {
            _httpConfig = httpConfig;

            //register dependencies
            RegisterDependencies(config, httpConfig);

            //startup tasks
            if (!config.IgnoreStartupTasks)
            {
                RunStartupTasks();
            }

        }

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        public T Resolve<T>() where T : class
		{
            //return _httpConfig.DependencyResolver.GetService(typeof(T)) as T;
            return ContainerManager.Resolve<T>();
		}

        /// <summary>
        ///  Resolve dependency
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }
        
        /// <summary>
        /// Resolve dependencies
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

		#endregion

        #region Properties

        /// <summary>
        /// Container manager
        /// </summary>
        public ContainerManager ContainerManager
        {
            get { return _containerManager; }
        }

        #endregion
    }
}
