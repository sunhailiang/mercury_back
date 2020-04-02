using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallRegion
    {
        public int Id { get; set; }
        public int Pid { get; set; }
        public string Name { get; set; }
        public byte Type { get; set; }
        public int Code { get; set; }
    }
}
