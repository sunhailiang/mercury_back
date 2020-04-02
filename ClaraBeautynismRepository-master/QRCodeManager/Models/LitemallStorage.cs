using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallStorage
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }
        public string Url { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public byte? Deleted { get; set; }
    }
}
