using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QRCodeManager.Authorizer;
using QRCodeManager.ConfigModels;
using QRCodeManager.Models;
using QRCodeManager.Models.QRCodeSeller;

namespace QRCodeManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // 代码配置数据库链接串
            services
                .AddEntityFrameworkSqlServer()
                .AddDbContextPool<QRCodeSellerContext>((serviceProvider, options) => options.UseSqlServer(Configuration.GetConnectionString("QRCodeSellerConnection")).UseInternalServiceProvider(serviceProvider))
                .AddDbContext<LitemallContext>(options => options.UseMySql(Configuration.GetConnectionString("LitemallDBConnection")));

            // 代码配置Jupiter验票
            var appSettingSection = Configuration.GetSection("AppSetting");
            services
                .Configure<JupiterKeys>(appSettingSection)
                .AddJwtBearerAuthentication(appSettingSection.Get<JupiterKeys>());

            services
                .AddHttpClient()
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(option =>
                {
                    //配置大小写问题，默认是首字母小写
                    //option.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                    //配置序列化时时间格式为yyyy-MM-dd HH:mm:ss
                    option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStatusCodePages();
            app.UseStaticFiles("/static");
            app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseMvc();
        }
    }
}
