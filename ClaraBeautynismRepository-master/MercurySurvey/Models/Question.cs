using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MercurySurvey.Models
{
    public class Question
    {
        /// <summary>
        /// 用户的全球唯一标识
        /// </summary>
        [Key]
        public Guid QuestionGuid { get; set; }

        /// <summary>
        /// 问题的内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 核分脚本
        /// </summary>
        public string Calculator { get; set; }

        /// <summary>
        /// 修改的时间
        /// </summary>
        public DateTime Ctime { get; set; }

#warning PK应该是Version 而不是QuestionGuid
        /// <summary>
        /// 版本号guid  pk
        /// </summary>
        public Guid Version { get; set; }

        /// <summary>
        /// 题目的类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Question的自增主键
        /// </summary>
        public int QuestionPK { get; set; }

        /// <summary>
        /// 删除字段默认为true为显示删除则改为false
        /// </summary>
        public bool IsEnable { get; set; }
    }
}
