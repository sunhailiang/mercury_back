using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallUserFormid
    {
        public int Id { get; set; }
        public string FormId { get; set; }
        public byte Isprepay { get; set; }
        public int UseAmount { get; set; }
        public DateTime ExpireTime { get; set; }
        public string OpenId { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public byte? Deleted { get; set; }
    }
}
