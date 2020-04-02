using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallGoodsSpecification
    {
        public int Id { get; set; }
        public int GoodsId { get; set; }
        public string Specification { get; set; }
        public string Value { get; set; }
        public string PicUrl { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public byte? Deleted { get; set; }
    }
}
