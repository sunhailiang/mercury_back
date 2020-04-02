using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallGrouponRules
    {
        public int Id { get; set; }
        public int GoodsId { get; set; }
        public string GoodsName { get; set; }
        public string PicUrl { get; set; }
        public decimal Discount { get; set; }
        public int DiscountMember { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? ExpireTime { get; set; }
        public byte? Deleted { get; set; }
    }
}
