using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallCollect
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ValueId { get; set; }
        public byte Type { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public byte? Deleted { get; set; }
    }
}
