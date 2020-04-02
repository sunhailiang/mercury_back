using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public byte Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public string LastLoginIp { get; set; }
        public byte? UserLevel { get; set; }
        public string Nickname { get; set; }
        public string Mobile { get; set; }
        public string Avatar { get; set; }
        public string WeixinOpenid { get; set; }
        public string SessionKey { get; set; }
        public byte Status { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public byte? Deleted { get; set; }
    }
}
