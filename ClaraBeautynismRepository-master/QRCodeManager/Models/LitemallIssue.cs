using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallIssue
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public byte? Deleted { get; set; }
    }
}
