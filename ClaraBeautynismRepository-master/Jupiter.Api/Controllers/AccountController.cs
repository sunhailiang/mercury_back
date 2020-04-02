/******************************************
 * AUTHOR:          Rector
 * CREATEDON:       2018-09-26
 * OFFICIAL_SITE:    码友网(https://codedefault.com)--专注.NET/.NET Core
 * 版权所有，请勿删除
 ******************************************/

using Jupiter.Api.Entities;
using Jupiter.Api.Extensions;
using Jupiter.Api.Extensions.AuthContext;
using Jupiter.Api.ViewModels.Rbac.DncMenu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static Jupiter.Api.Entities.Enums.CommonEnum;

namespace Jupiter.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly DncZeusDbContext _dbContext;

        private Dictionary<string, IGrouping<string, string>> GetPermissionsByGuid(Guid guid)
        {
            DncUser user = _dbContext.DncUser.FirstOrDefaultAsync(x => x.Guid == guid).Result;
            IQueryable<IGrouping<string, string>> pagePermissions;

            if (user.UserType == UserType.SuperAdministrator)
            {
                //如果是超级管理员
                pagePermissions = from urm in _dbContext.DncRole
                                  join rpm in _dbContext.DncRolePermissionMapping on urm.Code equals rpm.RoleCode into rpms
                                  from permission in rpms.DefaultIfEmpty()
                                  group permission.PermissionCode by urm.Code;
            }
            else
            {
                pagePermissions = from urm in _dbContext.DncUserRoleMapping
                                  where urm.UserGuid == guid
                                  join rpm in _dbContext.DncRolePermissionMapping on urm.DncRole equals rpm.DncRole into rpms
                                  from permission in rpms.DefaultIfEmpty()
                                  group permission.PermissionCode by urm.RoleCode;
            }
            return pagePermissions.ToDictionary(x => x.Key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public AccountController(DncZeusDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 读取当前用户基本信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/expose/Account/Profile")]
        public IActionResult Profile()
        {
            Models.Response.ResponseModel response = ResponseModelFactory.CreateInstance;
            Guid guid = AuthContextService.CurrentUser.Guid;
            DncUser user = _dbContext.DncUser.FirstOrDefaultAsync(x => x.Guid == guid).Result;
            response.SetData(new
            {
                access = new string[] { },
                avator = user.Avatar,
                userGuid = user.Guid,
                userName = user.DisplayName,
                userType = user.UserType,
                permissions = GetPermissionsByGuid(guid)
            });
            return Ok(response);
        }

        /// <summary>
        /// 获取指定Guid用户的权限码字典，仅限后端模块调用，严禁使用expose暴露
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("{guid}")]
        public IActionResult Permission(Guid guid)
        {
            Models.Response.ResponseModel response = ResponseModelFactory.CreateInstance;
            response.SetData(GetPermissionsByGuid(guid));
            return Ok(response);
        }


        //private List<string> FindParentMenuAlias(List<DncMenu> menus, Guid? parentGuid)
        //{
        //    List<string> pages = new List<string>();
        //    DncMenu parent = menus.FirstOrDefault(x => x.Guid == parentGuid);
        //    if (parent != null)
        //    {
        //        if (!pages.Contains(parent.Alias))
        //        {
        //            pages.Add(parent.Alias);
        //        }
        //        else
        //        {
        //            return pages;
        //        }
        //        if (parent.ParentGuid != Guid.Empty)
        //        {
        //            pages.AddRange(FindParentMenuAlias(menus, parent.ParentGuid));
        //        }
        //    }

        //    return pages.Distinct().ToList();
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Menu()
        {
            string strSql = @"SELECT M.* FROM DncRolePermissionMapping AS RPM 
LEFT JOIN DncPermission AS P ON P.Code = RPM.PermissionCode
INNER JOIN DncMenu AS M ON M.Guid = P.MenuGuid
WHERE P.IsDeleted=0 AND P.Status=1 AND P.Type=0 AND M.IsDeleted=0 AND M.Status=1 AND EXISTS (SELECT 1 FROM DncUserRoleMapping AS URM WHERE URM.UserGuid={0} AND URM.RoleCode=RPM.RoleCode)";

            if (AuthContextService.CurrentUser.UserType == UserType.SuperAdministrator)
            {
                //如果是超级管理员
                strSql = @"SELECT * FROM DncMenu WHERE IsDeleted=0 AND Status=1";
            }
            List<DncMenu> menus = _dbContext.DncMenu.FromSql(strSql, AuthContextService.CurrentUser.Guid).ToList();
            List<DncMenu> rootMenus = _dbContext.DncMenu.Where(x => x.IsDeleted == IsDeleted.No && x.Status == Status.Normal && x.ParentGuid == Guid.Empty).ToList();
            foreach (DncMenu root in rootMenus)
            {
                if (menus.Exists(x => x.Guid == root.Guid))
                {
                    continue;
                }
                menus.Add(root);
            }
            menus = menus.OrderBy(x => x.Sort).ThenBy(x => x.CreatedOn).ToList();
            List<MenuItem> menu = MenuItemHelper.LoadMenuTree(menus, "0");
            return Ok(menu);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class MenuItemHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="menus"></param>
        /// <param name="selectedGuid"></param>
        /// <returns></returns>
        public static List<MenuItem> BuildTree(this List<MenuItem> menus, string selectedGuid = null)
        {
            ILookup<string, MenuItem> lookup = menus.ToLookup(x => x.ParentId);

            List<MenuItem> Build(string pid)
            {
                return lookup[pid]
                    .Select(x => new MenuItem()
                    {
                        Guid = x.Guid,
                        ParentId = x.ParentId,
                        Children = Build(x.Guid),
                        Component = x.Component ?? "Main",
                        Name = x.Name,
                        Path = x.Path,
                        Meta = new MenuMeta
                        {
                            BeforeCloseFun = x.Meta.BeforeCloseFun,
                            HideInMenu = x.Meta.HideInMenu,
                            Icon = x.Meta.Icon,
                            NotCache = x.Meta.NotCache,
                            Title = x.Meta.Title,
                            Permission = x.Meta.Permission
                        }
                    }).ToList();
            }

            List<MenuItem> result = Build(selectedGuid);
            return result;
        }

        public static List<MenuItem> LoadMenuTree(List<DncMenu> menus, string selectedGuid = null)
        {
            List<MenuItem> temp = menus.Select(x => new MenuItem
            {
                Guid = x.Guid.ToString(),
                ParentId = x.ParentGuid != null && ((Guid)x.ParentGuid) == Guid.Empty ? "0" : x.ParentGuid?.ToString(),
                Name = x.Alias,
                Path = $"/{x.Url}",
                Component = x.Component,
                Meta = new MenuMeta
                {
                    BeforeCloseFun = x.BeforeCloseFun ?? "",
                    HideInMenu = x.HideInMenu == YesOrNo.Yes,
                    Icon = x.Icon,
                    NotCache = x.NotCache == YesOrNo.Yes,
                    Title = x.Name
                }
            }).ToList();
            List<MenuItem> tree = temp.BuildTree(selectedGuid);
            return tree;
        }
    }
}