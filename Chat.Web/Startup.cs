using Autofac;

using Chat.EntityFrameworkCore;
using Chat.EntityFrameworkCore.Core;
using Chat.EntityFrameworkCore.Repository;
using Chat.Uitl.Util;
using Chat.Web.Code.Gadget;
using Chat.Web.Code.Middleware;

using CSRedis;

using Cx.NetCoreUtils;
using Cx.NetCoreUtils.Filters;
using Cx.NetCoreUtils.Swagger;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Quartz;
using Quartz.Impl;

using Swashbuckle.AspNetCore.SwaggerUI;

using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Util;

using static Cx.NetCoreUtils.Swagger.SwaggerSetup;

namespace Chat.Web
{
    /// <summary>
    /// 配置文件
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 人口
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)  //增加环境配置文件，新建项目默认有
                .AddEnvironmentVariables()
                .Build();
        }
        /// <summary>
        /// appsettings文件
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        ///
        /// </summary>
        public IWebHostEnvironment Env { get; }
        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new AppSettings(Env.ContentRootPath));
            services.AddDbContext<MasterDbContext>(option => option.UseMySql(Configuration["connectionString:default"], new MySqlServerVersion(new Version(5, 7, 29))));
            services.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddTransient(typeof(IMasterDbRepositoryBase<,>), typeof(MasterDbRepositoryBase<,>));
            services.AddTransient(typeof(IRedisUtil), typeof(RedisUtil));
            services.AddTransient(typeof(IPrincipalAccessor), typeof(PrincipalAccessor));
            services.AddAuthenticationSetup();
            services.AddCorsSetup();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSignalR();
            services.AddMemoryCache();
            services.AddSwaggerSetup("1.0.0.1", "Chat API", "Chat Web API", new Contact { Email = "239573049@qq.com", Name = "xiaohu", Url = new System.Uri("https://github.com/239573049") });
            services.AddAutoMapperSetup("Chat.Application", "Chat.Web.Code");
            services.AddHttpContext();
            var csredis = new CSRedisClient(Configuration["connectionString:redis"]);
            RedisHelper.Initialization(csredis);
            services.AddSignalR()
                .AddRedis(Configuration["connectionString:redis"])
                .AddStackExchangeRedis(Configuration["connectionString:redis"]);
            services.AddControllers(o =>
            {
                o.Filters.Add(typeof(GlobalExceptionsFilter));
                o.Filters.Add(typeof(GlobalResponseFilter));
                o.Filters.Add<GlobalModelStateValidationFilter>();
                o.Filters.Add<CurrentLimiting>();
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateFormatString = Constants.DefaultFullDateFormat;
            });
        }
        /// <summary>
        /// 管道
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="log"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory log)
        {
            log.AddLog4Net();
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            app.UseGlobalBufferMiddleware();
            app.UseCors(Constants.CorsPolicy);
            app.UseStaticFiles();
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/1.0.0.1/swagger.json", "Chat API V1");
                c.DocExpansion(DocExpansion.None);
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("chatHub");
            });
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="containerBuilder"></param>
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            var basePath = ApplicationEnvironment.ApplicationBasePath;
            var servicesDllFile = Path.Combine(basePath, "Chat.Application.dll");
            var repositoryDllFile = Path.Combine(basePath, "Chat.EntityFrameworkCore.dll");
            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            containerBuilder.RegisterAssemblyTypes(assemblysServices)
                .Where(x => x.FullName.EndsWith("Service"))
                      .AsImplementedInterfaces()
                      .InstancePerDependency();//引用Autofac.Extras.DynamicProxy;
            // 获取 Repository.dll 程序集服务，并注册
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            containerBuilder.RegisterAssemblyTypes(assemblysRepository)
                .Where(x => x.FullName.EndsWith("Repository"))
                   .AsImplementedInterfaces()
                   .InstancePerDependency();
        }
    }
}
