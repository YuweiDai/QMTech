using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using QMTech.Core.Infrastructure;
using QMTech.Web.Framework.Security.Authorization;
using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using FluentValidation.WebApi;
using QMTech.Web.Framework;
using QMTech.Web.Framework.Filters;
using System.Web.Http.Validation;
using Autofac.Integration.WebApi;
using System.Web;

[assembly: OwinStartup(typeof(QMTech.Api.Startup))]
namespace QMTech.Api
{
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //增加过滤
            config.Filters.Add(new ValidateModelAttribute());
            config.Filters.Add(new APIExceptionFilterAttribute());

            //solved from http://stackoverflow.com/questions/12905174/how-to-add-custom-modelvalidatorproviders-to-web-api-project
            //自定义配置
            config.Services.Add(typeof(ModelValidatorProvider), new FluentValidationModelValidatorProvider(new QMValidatorFactory()));

            ConfigureOAuth(app);

            //initialize engine context
            EngineContext.Initialize(false, config);

            // Register the Autofac middleware FIRST, then the Autofac Web API middleware,
            // and finally the standard Web API middleware.
            app.UseAutofacMiddleware(EngineContext.Current.ContainerManager.Container);
            app.UseAutofacWebApi(config);

            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(10),
                Provider = new QMAuthorizationServerProvider(),

                //refresh token provider
                RefreshTokenProvider = new QMRefreshTokenProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}