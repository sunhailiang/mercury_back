using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeManager.Models.QRCodeSeller
{
    public class Calculator
    {
        /// <summary>
        /// 虚拟实体类litemallOrder表中的ConfirmTime时间
        /// </summary>
        public DateTime? ConfirmTime { get; set; }
        /// <summary>
        /// 虚拟实体类litemallOrder表中的ActualPrice价格
        /// </summary>
        public decimal ActualPrice { get; set; }
        /// <summary>
        /// 虚拟实体类Commission表中的折扣Rate
        /// </summary>
        public double Rate { get; set; }

    }
}
