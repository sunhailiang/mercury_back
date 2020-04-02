using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallBrand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string PicUrl { get; set; }
        public byte? SortOrder { get; set; }
        public decimal? FloorPrice { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public byte? Deleted { get; set; }
    }
}
