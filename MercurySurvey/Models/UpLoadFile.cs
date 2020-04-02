using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MercurySurvey.Models
{
    public class UploadFile
    {
        /// <summary>
        /// 生成的新的guid
        /// </summary>
        [Key]
        public Guid FileGuid { get; set; }

        /// <summary>
        /// 用户的guid
        /// </summary>
        public Guid UserGuid { get; set; }

        public string LocalFilePath { get; set; }

        ///// <summary>
        ///// 二进制文件
        ///// </summary>
        //public byte[] Files { get; set; }

        /// <summary>
        /// 文件添加到数据库的时间
        /// </summary>
        public DateTime Ctime { get; set; }
    }
}
