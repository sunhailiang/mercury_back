using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallTopic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Content { get; set; }
        public decimal? Price { get; set; }
        public string ReadCount { get; set; }
        public string PicUrl { get; set; }
        public int? SortOrder { get; set; }
        public string Goods { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public byte? Deleted { get; set; }
    }
}
