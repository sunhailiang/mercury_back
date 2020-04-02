using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallGroupon
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int? GrouponId { get; set; }
        public int RulesId { get; set; }
        public int UserId { get; set; }
        public int CreatorUserId { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string ShareUrl { get; set; }
        public byte Payed { get; set; }
        public byte? Deleted { get; set; }
    }
}
