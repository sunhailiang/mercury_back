using JupiterClient;
using MercurySurvey.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace MercurySurvey
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
            var x = Configuration.GetConnectionString("MercurySqlServer");
            //限制传入参数的大小
            //services.Configure<FormOptions>(options =>
            //{
            //    options.MultipartBodyLengthLimit = 1000000;
            //});

            //代码配置数据库链接串
            services
                .AddEntityFrameworkSqlServer()
                .AddDbContextPool<NoteContext>((serviceProvider, options) => options.UseSqlServer(Configuration.GetConnectionString("MercurySqlServer")).UseInternalServiceProvider(serviceProvider))
                .AddJwtBearerAuthentication(Configuration)//代码配置Jupiter验票
                .Configure<CookiePolicyOptions>(options =>
            {

                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            })
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IHttpClientFactory http, IOptionsMonitor<JupiterKeys> options)
        {
            //一wwwroot中的index.html页面作为默认页面
            FileServerOptions fileServerOptions = new FileServerOptions();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseFileServer(fileServerOptions);

            app.UseExceptionHandler((x) =>
            {
                x.Run(async (context) =>
                {
                    IExceptionHandlerPathFeature exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    if (exceptionHandlerPathFeature?.Error is UnauthorizeException ex)
                    {
                        await context.Response.WriteAsync($"{{\"success\":False,\"message\":\"{ex.Message}\"}}");
                    }
                });
            });

            app
                .UseStatusCodePages()
                .UseStaticFiles()
                .UseCookiePolicy()
                .UseAuthentication()
                .UseCorsMiddleware()
                //.UseAutoRefreshToken(options.CurrentValue, http)
                .UseMvc();
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CorsMiddlewareExtensions
    {
        public static IApplicationBuilder UseCorsMiddleware(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Access-Control-Allow-Headers", context.Request.Headers["Access-Control-Request-Headers"]);
                context.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,OPTIONS");
                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                context.Response.Headers.Add("Access-Control-Expose-Headers", "authorization");
                context.Response.Headers.Add("Access-Control-Max-Age", "86400");//缓存一天

                if (context.Request.Method == "OPTIONS")
                {
                    context.Response.StatusCode = 204;
                    await context.Response.WriteAsync("OK");
                }
                else
                {
                    await next();

                }
            });

            return app;
        }
    }
}
