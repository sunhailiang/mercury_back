using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeManager.Models
{
    public class Commission
    {
        /// <summary>
        /// 主键identity
        /// </summary>
        [Key]
        public int CommissionPK { get; set; }

        /// <summary>
        /// 分销码
        /// </summary>
        /// post info
        public int SellerId { get; set; }

        /// <summary>
        /// 分销创建时间
        /// </summary>
        public DateTime Mtime { get; set; }

        /// <summary>
        ///用户Userguid
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// 佣金比率
        /// </summary>
        /// post info
        public double Rate { get; set; }

        /// <summary>
        /// 用户的UserGuid
        /// </summary>
        public Guid UserGuid { get; set; }
    }
}
