/******************************************
 * AUTHOR:          Rector
 * CREATEDON:       2018-09-26
 * OFFICIAL_SITE:    码友网(https://codedefault.com)--专注.NET/.NET Core
 * 版权所有，请勿删除
 ******************************************/

namespace Jupiter.Api.ViewModels.Rbac.DncUser
{
    public class UserSignUpViewModel
    {
        public string LoginName { get; set; }

        public string DisplayName { get; set; }

        public string PasswordMD5 { get; set; }
    }
}
