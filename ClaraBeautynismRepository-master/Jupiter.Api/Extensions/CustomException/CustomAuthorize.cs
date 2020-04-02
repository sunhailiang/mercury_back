/******************************************
 * AUTHOR:          Rector
 * CREATEDON:       2018-09-26
 * OFFICIAL_SITE:    码友网(https://codedefault.com)--专注.NET/.NET Core
 * 版权所有，请勿删除
 ******************************************/

using Jupiter.Api.Entities;
using Jupiter.Api.Extensions.AuthContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace Jupiter.Api.Extensions.CustomException
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public bool RequireSuperAdministratorRole { get; set; } = false;

        public bool RequireAdministratorRole { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public CustomAuthorizeAttribute()
        {

        }

        public CustomAuthorizeAttribute(string policy) : base(policy)
        {

        }

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

        }
    }
}
