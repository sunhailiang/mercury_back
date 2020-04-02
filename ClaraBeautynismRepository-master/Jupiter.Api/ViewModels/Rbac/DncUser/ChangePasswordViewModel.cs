/******************************************
 * AUTHOR:          Rector
 * CREATEDON:       2018-09-26
 * OFFICIAL_SITE:    码友网(https://codedefault.com)--专注.NET/.NET Core
 * 版权所有，请勿删除
 ******************************************/

namespace Jupiter.Api.ViewModels.Rbac.DncUser
{
    public class ChangePasswordViewModel
    {
        public string OldPasswordMD5 { get; set; }

        public string NewPasswordMD5 { get; set; }
    }
}
