using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallKeyword
    {
        public int Id { get; set; }
        public string Keyword { get; set; }
        public string Url { get; set; }
        public byte IsHot { get; set; }
        public byte IsDefault { get; set; }
        public int SortOrder { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public byte? Deleted { get; set; }
    }
}
