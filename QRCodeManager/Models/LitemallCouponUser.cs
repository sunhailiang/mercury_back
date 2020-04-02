using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallCouponUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CouponId { get; set; }
        public short? Status { get; set; }
        public DateTime? UsedTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? OrderId { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public byte? Deleted { get; set; }
    }
}
