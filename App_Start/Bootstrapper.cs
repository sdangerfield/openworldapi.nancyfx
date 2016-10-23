//directive if defined will force Create DB
//#define EXPLODEDB
//directive if defined will seed Data (only if createDB defined as well)
//#define SEED_DATA



namespace OpenworldAPI.nancyfx.AppStart
{
    using Nancy;
    using Nancy.Bootstrapper;
    using Ninject;
    using Nancy.Bootstrappers.Ninject;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using NHibernate;
    using NHibernate.Context;
    using NHibernate.Tool.hbm2ddl;
    using Ninject.Activation;
    using Ninject.Web.Common;
    using DataModel.libData;
    using DataModel.libDB;
    using DataModel.libHosting;
    using Nancy.Serialization.JsonNet;
    using Newtonsoft.Json;
    using OpenworldAPI.nancyfx.siteUtils;
    using Nancy.SimpleAuthentication;
    using SimpleAuthentication.Core;
    using SimpleAuthentication.Core.Providers;
    using OpenworldAPI.nancyfx.lib;
   
 

    public class Bootstrapper : NinjectNancyBootstrapper
    {
        

        protected override void ConfigureApplicationContainer(IKernel existingContainer)
        {
            //application singleton
            //existingContainer.Bind<IApplicationSingleton>().To<ApplicationSingleton>().InSingletonScope();
            
            //bindings // General // Singletons
            //container.Bind<IDateTime>().To<DateTimeAdapter>().InSingletonScope();
            //container.Bind<IUpdateablePropertyDetector>().To<JObjectUpdateablePropertyDetector>().InSingletonScope();
            //container.Bind<IBasicSecurityService>().To<BasicSecurityService>().InSingletonScope();
            ConfigureNHibernate(existingContainer);
            existingContainer.Bind<JsonSerializer>().To<CustomJsonSerializer>().InRequestScope();
            

            //ConfigureLog4Net(container);
            
            //ConfigureAutoMapper(container);


            //transient binding
            //existingContainer.Bind<ICommandHandler>().To<CommandHandler>();
        }

        
         protected override NancyInternalConfiguration InternalConfiguration
         {
                get { return NancyInternalConfiguration.WithOverrides(c => c.Serializers.Insert(0, typeof(JsonNetSerializer))); }
         }
        
   

        protected override void ConfigureRequestContainer(IKernel container, NancyContext context)
        {
            //container here is a child container. I.e. singletons here are in request scope.
            //IDisposables will get disposed at the end of the request when the child container does.
            //container.Bind<IPerRequest>().To<PerRequest>().InSingletonScope();
            container.Bind<ISession>().ToMethod(CreateSession).InRequestScope();
            ConfigureUserSession(container);
        }

        //configurators
        public void ConfigureLog4Net(IKernel container)
        {
            //XmlConfigurator.Configure();

            //var logManager = new LogManagerAdapter();
            //container.Bind<ILogManager>().ToConstant(logManager);
        }

        private void ConfigureNHibernate(IKernel container)
        {

            #if EXPLODEDB
                //recreate Schema
                var sessionFactory = Fluently.Configure()
                   .Database(
                       MsSqlConfiguration.MsSql2008.ConnectionString(
                           c => c.FromConnectionStringWithKey("owDbConn")))
                   .CurrentSessionContext("web")
                   .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                   .Mappings(m => m.FluentMappings.AddFromAssemblyOf<sysBaseRuleset>())
                   .BuildSessionFactory();
                //seed data
                #if SEED_DATA
                    var session = sessionFactory.OpenSession();
                    nwdbRepository nwRepo = new nwdbRepository(session);
                    nwRepo.SeedMockData();
                #endif
            #else
                var sessionFactory = Fluently.Configure()
                    .Database(
                        MsSqlConfiguration.MsSql2008.ConnectionString(
                            c => c.FromConnectionStringWithKey("owDbConn")))
                    .CurrentSessionContext("web")
                    .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<sysBaseRuleset>())
                    .BuildSessionFactory();
            #endif

            //Bind Created or Updated to Ninject
            container.Bind<ISessionFactory>().ToConstant(sessionFactory);


            }

        private ISession CreateSession(IContext context)
        {
            var sessionFactory = context.Kernel.Get<ISessionFactory>();
            if (!CurrentSessionContext.HasBind(sessionFactory))
            {
                var session = sessionFactory.OpenSession();
                CurrentSessionContext.Bind(session);
            }
            return sessionFactory.GetCurrentSession();
        }
    
        protected void ConfigureUserSession(IKernel container) {

            // hostConstants.appId; hostConstants.appSecret;
            var facebookProvider = new FacebookProvider(new ProviderParams { PublicApiKey = hostConstants.appId, SecretApiKey = hostConstants.appSecret});
            var authenticationProviderFactory = new AuthenticationProviderFactory();
            authenticationProviderFactory.AddProvider(facebookProvider);
            container.Bind<IAuthenticationCallbackProvider>().To<AuthenticationCallbackProvider>().InRequestScope();
        }
    }
   

}