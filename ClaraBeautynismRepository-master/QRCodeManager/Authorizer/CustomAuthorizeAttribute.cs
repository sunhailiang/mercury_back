using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using QRCodeManager.ConfigModels;

namespace QRCodeManager.Authorizer
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public CustomAuthorizeAttribute()
        {

        }

        public CustomAuthorizeAttribute(string policy) : base(policy)
        {

        }

        public string Permission { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            List<IFilterMetadata> filters = new List<IFilterMetadata>(filterContext.Filters);
            filters.Reverse();
            foreach (var filter in filters)
            {
                if (filter.Equals(this))
                {
                    break;
                }
                if (filter is AllowAnonymousFilter)
                {
                    return;
                }
            }
            // 以下权限拦截器未现实，所以直接return
            var user = filterContext.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                throw new UnauthorizeException();
            }

            if (!string.IsNullOrWhiteSpace(Permission))
            {
                //获取HttpClient服务
                IHttpClientFactory clientFactory = filterContext.HttpContext.RequestServices.GetService(typeof(IHttpClientFactory)) as IHttpClientFactory;
                //获取Jupiter路径
                string jupiterPath = (filterContext.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration).GetSection("AppSetting").Get<JupiterKeys>().JupiterPath;
                //获取当前用户Guid
                var guid = Guid.Parse(user.Claims.FirstOrDefault(x => x.Type == "guid").Value);
                //获取Permissions
                using (var client = clientFactory.CreateClient())
                {
                    var x = client.GetAsync($"{jupiterPath}/api/v1/Account/Permission/{guid}").Result.Content.ReadAsStringAsync().Result;

                    //Dictionary<string, string[]> permissions = JsonConvert.DeserializeObject<dynamic>(x).data;
                    var permission = from JProperty role in JObject.Parse(x)["data"]
                                     from JValue perm in role.Value
                                     where perm.Value<string>()==Permission
                                     select perm;
                    if (permission.FirstOrDefault() == default)
                    {
                        throw new UnauthorizeException($"用户缺少{Permission}权限");
                    }
                }

            }
        }
    }


}
