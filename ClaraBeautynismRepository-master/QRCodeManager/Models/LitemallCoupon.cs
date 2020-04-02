using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallCoupon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Tag { get; set; }
        public int Total { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Min { get; set; }
        public short? Limit { get; set; }
        public short? Type { get; set; }
        public short? Status { get; set; }
        public short? GoodsType { get; set; }
        public string GoodsValue { get; set; }
        public string Code { get; set; }
        public short? TimeType { get; set; }
        public short? Days { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public byte? Deleted { get; set; }
    }
}
