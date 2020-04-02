using System;
using System.Collections.Generic;

namespace QRCodeManager.Models
{
    public partial class LitemallFeedback
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Mobile { get; set; }
        public string FeedType { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
        public byte? HasPicture { get; set; }
        public string PicUrls { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public byte? Deleted { get; set; }
    }
}
