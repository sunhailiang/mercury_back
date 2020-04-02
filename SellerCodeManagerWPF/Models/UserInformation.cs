using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SellerCodeManagerWPF.Models
{
    class UserInformation
    {
        /// <summary>
        /// 这个字段暂时没有使用
        /// </summary>
        [JsonProperty(PropertyName = "access")]
        public ArrayList Access { get; set; }

        /// <summary>
        /// 用户头像地址
        /// </summary>
        [JsonProperty(PropertyName = "avator")]
        public string Avator { get; set; }

        /// <summary>
        /// 用户唯一ID
        /// </summary>
        [JsonProperty(PropertyName = "userGuid")]
        public Guid UserGuid { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }

        /// <summary>
        /// 用户类型代码
        /// </summary>
        [JsonProperty(PropertyName = "userType")]
        public int UserType { get; set; }

        /// <summary>
        /// 用户权限字典
        /// </summary>
        [JsonProperty(PropertyName = "permissions")]
        public Dictionary<string, string[]> Permissions { get; set; }

    }
}
