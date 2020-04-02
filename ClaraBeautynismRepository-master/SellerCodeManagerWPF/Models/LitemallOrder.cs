using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellerCodeManagerWPF.Models
{
    class LitemallOrder
    {
        public int Id { get; set; }

        /// <summary>
        /// 用户表的用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderSn { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public short OrderStatus { get; set; }

        /// <summary>
        /// 收货人名称
        /// </summary>
        public string Consignee { get; set; }

        /// <summary>
        /// 收货人手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 收货人具体地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 用户订单留言
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 商品总费用
        /// </summary>
        public decimal GoodsPrice { get; set; }

        /// <summary>
        /// 配送费
        /// </summary>
        public decimal FreightPrice { get; set; }

        /// <summary>
        /// 优惠券减免
        /// </summary>
        public decimal CouponPrice { get; set; }

        /// <summary>
        /// 用户积分减免
        /// </summary>
        public decimal IntegralPrice { get; set; }

        /// <summary>
        /// 团购优惠价减免
        /// </summary>
        public decimal GrouponPrice { get; set; }

        /// <summary>
        /// 订单费用
        /// </summary>
        public decimal OrderPrice { get; set; }

        /// <summary>
        /// 实付费用
        /// </summary>
        public decimal ActualPrice { get; set; }

        /// <summary>
        /// 微信付款id
        /// </summary>
        public string PayId { get; set; }

        /// <summary>
        /// 付款时间
        /// </summary>
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 发货编号
        /// </summary>
        public string ShipSn { get; set; }

        /// <summary>
        /// 发货快递公司
        /// </summary>
        public string ShipChannel { get; set; }

        /// <summary>
        /// 发货开始时间
        /// </summary>
        public DateTime? ShipTime { get; set; }

        /// <summary>
        /// 确认收货时间
        /// </summary>
        public DateTime? ConfirmTime { get; set; }

        /// <summary>
        /// 待评价订单商品数量
        /// </summary>
        public short? Comments { get; set; }

        /// <summary>
        /// 订单关闭时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 订单创建时间
        /// </summary>
        public DateTime? AddTime { get; set; }

        /// <summary>
        /// 订单更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        public byte? Deleted { get; set; }
    }
}
