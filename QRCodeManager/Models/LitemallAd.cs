using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallAd
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Url { get; set; }
        public byte? Position { get; set; }
        public string Content { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public byte? Enabled { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public byte? Deleted { get; set; }
    }
}
