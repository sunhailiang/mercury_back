using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeManager.Models
{
    public class SellerUserRelationship
    {
        /// <summary>
        ///主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户的id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 分销码
        /// </summary>
        public int Rid { get; set; }
      

    }
}
