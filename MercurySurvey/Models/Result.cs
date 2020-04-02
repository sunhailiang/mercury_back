using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MercurySurvey.Models
{
    public class Result
    {
        /// <summary>
        /// 结果的guid
        /// </summary>
        [Key]
        public Guid AnswerGuid { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public Guid Userid { get; set; }

        /// <summary>
        /// 详细信息  暂时为空未来增加一个Record表
        /// </summary>
        public int RecordId { get; set; }

        /// <summary>
        /// 答案提交时间
        /// </summary>
        public DateTime Ctime { get; set; }

        /// <summary>
        /// 答案
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 问题的id
        /// </summary>
        public Guid Question { get; set; }

        /// <summary>
        /// 问卷guid
        /// </summary>
        public Guid QuestionnaireGuid { get; set; }
    }
}
