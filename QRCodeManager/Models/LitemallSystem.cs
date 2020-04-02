using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallSystem
    {
        public int Id { get; set; }
        public string KeyName { get; set; }
        public string KeyValue { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public byte? Deleted { get; set; }
    }
}
