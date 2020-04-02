using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallOrderGoods
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int GoodsId { get; set; }
        public string GoodsName { get; set; }
        public string GoodsSn { get; set; }
        public int ProductId { get; set; }
        public short Number { get; set; }
        public decimal Price { get; set; }
        public string Specifications { get; set; }
        public string PicUrl { get; set; }
        public int? Comment { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public byte? Deleted { get; set; }
    }
}
