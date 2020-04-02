using System;
using System.Collections;
using Characters;

namespace SellerCodeManagerWPF.Models
{
    class QRCodeSeller : IQRCodeSeller
    {
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdentityCardNumber { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 分销码
        /// </summary>
        public int SellerID { get; set; }
        /// <summary>
        /// 类型默认Basic
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 创建的时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改的时间
        /// </summary>
        public DateTime ModifyTime { get; set; }
        /// <summary>
        /// 用户的唯一标识Guid
        /// </summary>
        public Guid UserGuid { get; set; }

        public double Rate { get; set; } 
    }
}
