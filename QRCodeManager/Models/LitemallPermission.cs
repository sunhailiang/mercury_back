using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallPermission
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public string Permission { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public byte? Deleted { get; set; }
    }
}
