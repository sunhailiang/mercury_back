using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallOrder
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string OrderSn { get; set; }
        public short OrderStatus { get; set; }
        public string Consignee { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Message { get; set; }
        public decimal GoodsPrice { get; set; }
        public decimal FreightPrice { get; set; }
        public decimal CouponPrice { get; set; }
        public decimal IntegralPrice { get; set; }
        public decimal GrouponPrice { get; set; }
        public decimal OrderPrice { get; set; }
        public decimal ActualPrice { get; set; }
        public string PayId { get; set; }
        public DateTime? PayTime { get; set; }
        public string ShipSn { get; set; }
        public string ShipChannel { get; set; }
        public DateTime? ShipTime { get; set; }
        public DateTime? ConfirmTime { get; set; }
        public short? Comments { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public byte? Deleted { get; set; }
    }
}
