using AutoMapper;
using Jupiter.Api.Auth;
using Jupiter.Api.Entities;
using Jupiter.Api.Entities.Enums;
using Jupiter.Api.Extensions;
using Jupiter.Api.Extensions.AuthContext;
using Jupiter.Api.Extensions.CustomException;
using Jupiter.Api.Extensions.DataAccess;
using Jupiter.Api.Models.Response;
using Jupiter.Api.RequestPayload.Rbac.User;
using Jupiter.Api.Utils;
using Jupiter.Api.ViewModels.Rbac.DncUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using static Jupiter.Api.Entities.Enums.CommonEnum;
/******************************************
* AUTHOR:          Rector
* CREATEDON:       2018-09-26
* OFFICIAL_SITE:    码友网(https://codedefault.com)--专注.NET/.NET Core
* 版权所有，请勿删除
******************************************/

namespace Jupiter.Api.Controllers.Api.V1.Rbac
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class UserController : ControllerBase
    {
        private readonly DncZeusDbContext _dbContext;
        private readonly AppAuthenticationSettings _appSettings;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public UserController(DncZeusDbContext dbContext, IMapper mapper, IOptions<AppAuthenticationSettings> appSettings)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(UserRequestPayload payload)
        {
            using (_dbContext)
            {
                IQueryable<DncUser> query = _dbContext.DncUser.AsQueryable();
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.LoginName.Contains(payload.Kw.Trim()) || x.DisplayName.Contains(payload.Kw.Trim()));
                }
                if (payload.IsDeleted > CommonEnum.IsDeleted.All)
                {
                    query = query.Where(x => x.IsDeleted == payload.IsDeleted);
                }
                if (payload.Status > UserStatus.All)
                {
                    query = query.Where(x => x.Status == payload.Status);
                }

                if (payload.FirstSort != null)
                {
                    query = query.OrderBy(payload.FirstSort.Field, payload.FirstSort.Direct == "DESC");
                }
                System.Collections.Generic.List<DncUser> list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                int totalCount = query.Count();
                System.Collections.Generic.IEnumerable<UserJsonModel> data = list.Select(_mapper.Map<DncUser, UserJsonModel>);
                ResponseResultModel response = ResponseModelFactory.CreateResultInstance;
                response.SetData(data, totalCount);
                return Ok(response);
            }
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="model">用户视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(UserCreateViewModel model)
        {
            ResponseModel response = ResponseModelFactory.CreateInstance;
            if (model.LoginName.Trim().Length <= 0)
            {
                response.SetFailed("请输入登录名称");
                return Ok(response);
            }
            if (_dbContext.DncUser.Count(x => x.LoginName == model.LoginName) > 0)
            {
                response.SetFailed("登录名已存在");
                return Ok(response);
            }
            DncUser entity = _mapper.Map<UserCreateViewModel, DncUser>(model);
            entity.CreatedOn = DateTime.Now;
            entity.Guid = Guid.NewGuid();
            entity.Status = model.Status;
            _dbContext.DncUser.Add(entity);
            _dbContext.SaveChanges();
            response.SetSuccess();
            response.SetData(entity.Guid);
            return Ok(response);
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="guid">用户GUID</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                DncUser entity = _dbContext.DncUser.FirstOrDefault(x => x.Guid == guid);
                ResponseModel response = ResponseModelFactory.CreateInstance;
                response.SetData(_mapper.Map<DncUser, UserEditViewModel>(entity));
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑后的用户信息
        /// </summary>
        /// <param name="model">用户视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(UserEditViewModel model)
        {
            ResponseModel response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                DncUser entity = _dbContext.DncUser.FirstOrDefault(x => x.Guid == model.Guid);
                if (entity == null)
                {
                    response.SetFailed("用户不存在");
                    return Ok(response);
                }
                entity.DisplayName = model.DisplayName;
                entity.IsDeleted = model.IsDeleted;
                entity.IsLocked = model.IsLocked;
                entity.ModifiedByUserGuid = AuthContextService.CurrentUser.Guid;
                entity.ModifiedByUserName = AuthContextService.CurrentUser.DisplayName;
                entity.ModifiedOn = DateTime.Now;
                entity.Password = model.Password;
                entity.Status = model.Status;
                entity.UserType = model.UserType;
                entity.Description = model.Description;
                _dbContext.SaveChanges();
                response = ResponseModelFactory.CreateInstance;
                return Ok(response);
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ids">用户GUID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        public IActionResult Delete(string ids)
        {
            ResponseModel response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            response = UpdateIsDelete(CommonEnum.IsDeleted.Yes, ids);
            return Ok(response);
        }

        /// <summary>
        /// 恢复用户
        /// </summary>
        /// <param name="ids">用户GUID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        public IActionResult Recover(string ids)
        {
            ResponseModel response = UpdateIsDelete(CommonEnum.IsDeleted.No, ids);
            return Ok(response);
        }

        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ids">用户ID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Batch(string command, string ids)
        {
            ResponseModel response = ResponseModelFactory.CreateInstance;
            switch (command)
            {
                case "delete":
                    if (ConfigurationManager.AppSettings.IsTrialVersion)
                    {
                        response.SetIsTrial();
                        return Ok(response);
                    }
                    response = UpdateIsDelete(CommonEnum.IsDeleted.Yes, ids);
                    break;
                case "recover":
                    response = UpdateIsDelete(CommonEnum.IsDeleted.No, ids);
                    break;
                case "forbidden":
                    if (ConfigurationManager.AppSettings.IsTrialVersion)
                    {
                        response.SetIsTrial();
                        return Ok(response);
                    }
                    response = UpdateStatus(UserStatus.Forbidden, ids);
                    break;
                case "normal":
                    response = UpdateStatus(UserStatus.Normal, ids);
                    break;
                default:
                    break;
            }
            return Ok(response);
        }

        #region 用户-角色
        /// <summary>
        /// 保存用户-角色的关系映射数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("/api/v1/rbac/user/save_roles")]
        public IActionResult SaveRoles(SaveUserRolesViewModel model)
        {
            ResponseModel response = ResponseModelFactory.CreateInstance;
            System.Collections.Generic.List<DncUserRoleMapping> roles = model.AssignedRoles.Select(x => new DncUserRoleMapping
            {
                UserGuid = model.UserGuid,
                CreatedOn = DateTime.Now,
                RoleCode = x.Trim()
            }).ToList();
            _dbContext.Database.ExecuteSqlCommand("DELETE FROM DncUserRoleMapping WHERE UserGuid={0}", model.UserGuid);
            bool success = true;
            if (roles.Count > 0)
            {
                _dbContext.DncUserRoleMapping.AddRange(roles);
                success = _dbContext.SaveChanges() > 0;
            }

            if (success)
            {
                response.SetSuccess();
            }
            else
            {
                response.SetFailed("保存用户角色数据失败");
            }
            return Ok(response);
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids">用户ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(CommonEnum.IsDeleted isDeleted, string ids)
        {
            using (_dbContext)
            {
                //var idList = ids.Split(",").ToList();
                ////idList.ForEach(x => {
                ////  _dbContext.Database.ExecuteSqlCommand($"UPDATE DncUser SET IsDeleted=1 WHERE Id = {x}");
                ////});
                //_dbContext.Database.ExecuteSqlCommand($"UPDATE DncUser SET IsDeleted={(int)isDeleted} WHERE Id IN ({ids})");
                System.Collections.Generic.List<SqlParameter> parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                string parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                string sql = string.Format("UPDATE DncUser SET IsDeleted=@IsDeleted WHERE Guid IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDeleted", (int)isDeleted));
                _dbContext.Database.ExecuteSqlCommand(sql, parameters);
                ResponseModel response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="status">用户状态</param>
        /// <param name="ids">用户ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateStatus(UserStatus status, string ids)
        {
            using (_dbContext)
            {
                System.Collections.Generic.List<SqlParameter> parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                string parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                string sql = string.Format("UPDATE DncUser SET Status=@Status WHERE Guid IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@Status", (int)status));
                _dbContext.Database.ExecuteSqlCommand(sql, parameters);
                ResponseModel response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }
        #endregion

        /// <summary>
        /// 对外暴露的注册接口
        /// </summary>
        /// <param name="createBody"></param>
        /// <returns></returns>
        [Route("/expose/Account/SignUp")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult SignUp([FromBody]UserSignUpViewModel createBody)
        {
            UserCreateViewModel user = new UserCreateViewModel
            {
                LoginName = createBody.LoginName,
                DisplayName = createBody.DisplayName ?? createBody.LoginName,
                IsLocked = CommonEnum.IsLocked.UnLocked,
                Status = UserStatus.Normal,
                UserType = UserType.GeneralUser,
                IsDeleted = CommonEnum.IsDeleted.No,
            };
            string passwordMD5 = createBody.PasswordMD5;
            user.Password = $"{user.LoginName}${PasswordCalculator.SaltPassword(user.LoginName, passwordMD5)}";

            return Create(user);
        }

        /// <summary>
        /// 已登录用户修改密码
        /// </summary>
        /// <param name="changePassword"></param>
        /// <returns></returns>
        [Route("/expose/Account/ChangePassword")]
        [HttpPost]
        public IActionResult ExposeChangePassword([FromBody]ChangePasswordViewModel changePassword)
        {
            ResponseModel response = ResponseModelFactory.CreateInstance;
            DncUser user = _dbContext.DncUser.First(x => x.Guid == AuthContextService.CurrentUser.Guid);
            string[] passwordSplit = user.Password.Split("$");
            if (passwordSplit[1] != PasswordCalculator.SaltPassword(passwordSplit[0], changePassword.OldPasswordMD5))
            {
                response.SetFailed("旧密码错误");
                return Ok(response);
            }
            else
            {
                _dbContext.DncUser.First(x => x.Guid == AuthContextService.CurrentUser.Guid).Password = user.Password = $"{user.LoginName}${PasswordCalculator.SaltPassword(user.LoginName, changePassword.NewPasswordMD5)}";
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 该方法接受手机号或者微信UnionID，返回第一个匹配的用户GUID，如无将自动创建
        /// </summary>
        /// <param name="model"></param>
        /// <returns>TargetUserGuid</returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult AutoCreate([FromBody]AutoCreateUserViewModel model, bool autoPassword = true, bool returnToken = false)
        {
            ResponseModel response = ResponseModelFactory.CreateInstance;

            //查找用户是否已经存在
            DncUser target = _dbContext.DncUser.FirstOrDefault(x => (!string.IsNullOrWhiteSpace(model.WeChatUnionID) && x.WeChatUnionID == model.WeChatUnionID) || x.PhoneNumber == model.PhoneNumber);

            //不存在时自动创建
            if (target == default)
            {
                UserCreateViewModel user = new UserCreateViewModel
                {
                    LoginName = model.PhoneNumber ?? model.WeChatUnionID,
                    DisplayName = model.PhoneNumber,
                    PhoneNumber = model.PhoneNumber,
                    IsLocked = IsLocked.UnLocked,
                    Status = UserStatus.Normal,
                    UserType = UserType.GeneralUser,
                    IsDeleted = IsDeleted.No,
                };
                if (autoPassword)
                {
                    string password = model.PhoneNumber ?? DateTime.Now.ToString();
                    user.Password = model.PhoneNumber != null ? $"{user.LoginName}${PasswordCalculator.SaltPassword(user.LoginName, PasswordCalculator.Md5(password))}" : null;
                }
                Create(user);
                target = _dbContext.DncUser.FirstOrDefault(x => (!string.IsNullOrWhiteSpace(model.WeChatUnionID) && x.WeChatUnionID.Trim() == model.WeChatUnionID) || x.PhoneNumber.Trim() == model.PhoneNumber);
            }

            //依然为default则创建失败
            if (target == default)
            {
                response.SetFailed("用户创建失败");
            }
            else
            {
                //如果Permission不为空，则检查是否具有对应的权限位
                if (!string.IsNullOrWhiteSpace(model.Permission))
                {
                    IQueryable<DncUserRoleMapping> targetPermission = from role in _dbContext.DncUserRoleMapping
                                                                      where role.UserGuid == target.Guid
                                                                      join permission in _dbContext.DncRolePermissionMapping on role.RoleCode equals permission.RoleCode
                                                                      where permission.PermissionCode == model.Permission
                                                                      select role;
                    if (targetPermission.FirstOrDefault() == default)
                    {
                        _dbContext.DncUserRoleMapping.Add(new DncUserRoleMapping
                        {
                            CreatedOn = DateTime.Now,
                            UserGuid = target.Guid,
                            RoleCode = model.Permission
                        });
                        _dbContext.SaveChanges();
                    }

                }

                //创建返回体
                dynamic result = new ExpandoObject();
                result.Guid = target.Guid;
                if(returnToken)
                {
                    result.Authorization = TokenBulider(target);
                }
                response.SetData(result);
            }
            return Ok(response);


            string TokenBulider(DncUser user)
            {
                if (user == null || user.IsDeleted == IsDeleted.Yes)
                {
                    throw new InvalidOperationException("用户不存在");
                }

                if (user.IsLocked == IsLocked.Locked)
                {
                    throw new InvalidOperationException("账号已被锁定");
                }

                if (user.Status == UserStatus.Forbidden)
                {
                    throw new InvalidOperationException("账号已被禁用");
                }

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.LoginName),
                    new Claim("guid",user.Guid.ToString()),
                    new Claim("avatar",""),
                    new Claim("displayName",user.DisplayName),
                    new Claim("loginName",user.LoginName),
                    new Claim("emailAddress",""),
                    new Claim("guid",user.Guid.ToString()),
                    new Claim("userType",((int)user.UserType).ToString()),
                    });

                return JwtBearerAuthenticationExtension.GetJwtAccessToken(_appSettings, claimsIdentity);
            }
        }
    }
}