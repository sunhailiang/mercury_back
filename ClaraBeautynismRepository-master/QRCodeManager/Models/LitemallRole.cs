using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public byte? Enabled { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public byte? Deleted { get; set; }
    }
}
