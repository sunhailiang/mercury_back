using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallFootprint
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GoodsId { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public byte? Deleted { get; set; }
    }
}
