/******************************************
 * AUTHOR:          Rector
 * CREATEDON:       2018-09-26
 * OFFICIAL_SITE:    码友网(https://codedefault.com)--专注.NET/.NET Core
 * 版权所有，请勿删除
 ******************************************/

using Jupiter.Api.Auth;
using Jupiter.Api.Entities;
using Jupiter.Api.Extensions;
using Jupiter.Api.Utils;
using Jupiter.Api.ViewModels.Rbac.DncUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using static Jupiter.Api.Entities.Enums.CommonEnum;

namespace Jupiter.Api.Controllers
{
    /// <summary>
    /// 该控制器只处理Token相关请求
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OauthController : ControllerBase
    {
        private readonly AppAuthenticationSettings _appSettings;
        private readonly DncZeusDbContext _dbContext;

        /// <summary>
        /// 
        /// </summary>
        public OauthController(IOptions<AppAuthenticationSettings> appSettings, DncZeusDbContext dbContext)
        {
            _appSettings = appSettings.Value;
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("/expose/Account/SignIn")]
        public IActionResult Auth([FromBody]UserSignInViewModel userSignInViewModel)
        {
            Models.Response.ResponseModel response = ResponseModelFactory.CreateInstance;
            DncUser user;
            user = _dbContext.DncUser.FirstOrDefault(x => x.LoginName == userSignInViewModel.UserName.Trim());

            string[] passwordSplit = user.Password.Split("$");

            if (user == null)
            {
                response.SetFailed("用户不存在");
                return Ok(response);
            }

            if (passwordSplit[1] != PasswordCalculator.SaltPassword(passwordSplit[0], userSignInViewModel.PasswordMD5))
            {
                response.SetFailed("密码不正确");
                return Ok(response);
            }

            try
            {
                response.SetData(TokenBulider(user));
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                response.SetFailed(ex.Message);
                return Ok(response);
            }
        }

        /// <summary>
        /// 向当前请求用户发送新Token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult RefreshToken()
        {
            Guid currentUserGuid = Guid.Parse((from claim in HttpContext.User.Claims
                                               where claim.Type == "guid"
                                               select claim.Value).First());
            Models.Response.ResponseModel response = ResponseModelFactory.CreateInstance;
            DncUser user = _dbContext.DncUser.FirstOrDefault(x => x.Guid == currentUserGuid);

            try
            {
                response.SetData(TokenBulider(user));
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                response.SetFailed(ex.Message);
                return Ok(response);
            }


        }

        /// <summary>
        /// 对指定实体的用户签署Bearer Token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string TokenBulider(DncUser user)
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