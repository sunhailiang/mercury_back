namespace QRCodeManager.Models
{
    public class CommissionViewModel
    {
        /// <summary>
        /// 前端传递过来的佣金比率，0.5~0.99之间
        /// </summary>
        public double Rate { get; set; }
        /// <summary>
        /// 前端传递过来的SellerId
        /// </summary>
        public int SellerID { get; set; }
    }
}
