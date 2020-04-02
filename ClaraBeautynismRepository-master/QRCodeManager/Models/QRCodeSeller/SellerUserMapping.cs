using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QRCodeManager.Models.QRCodeSeller
{
    public partial class SellerUserMapping
    {
        public int? CostumerId { get; set; }

        public int? SellerId { get; set; }

        public string Type { get; set; }

        public DateTime? CreateTime { get; set; }

        [Key]
        public int RecordId { get; set; }
    }
}
