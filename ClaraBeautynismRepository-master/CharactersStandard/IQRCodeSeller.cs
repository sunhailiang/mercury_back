using System;

namespace Characters
{
    public interface IQRCodeSeller
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateTime { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        string IdentityCardNumber { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        bool IsEnable { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        DateTime ModifyTime { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        string PhoneNumber { get; set; }

        /// <summary>
        /// 太阳码ID
        /// </summary>
        int SellerID { get; set; }

        /// <summary>
        /// 太阳码类型
        /// </summary>
        string Type { get; set; }

        /// <summary>
        /// Jupiter User ID
        /// </summary>
        Guid UserGuid { get; set; }
    }
}