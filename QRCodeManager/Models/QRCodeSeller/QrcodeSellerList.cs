using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QRCodeManager.Models.QRCodeSeller
{
    public partial class QrcodeSellerList
    {
        public string IdentityCardNumber { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsEnable { get; set; }

        [Key]
        public int SellerId { get; set; }

        public string Type { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ModifyTime { get; set; }

        public Guid UserGuid { get; set; }

        public Guid CA { get; set; }
    }
}
