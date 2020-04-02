using System;
using System.ComponentModel.DataAnnotations;

namespace MercurySurvey.Models
{
    public class Answer
    {
      
        /// <summary>
        /// Answer记录的GUID
        /// </summary>
        [Key]
        public Guid AnswerGuid { get; set; }
        
        /// <summary>
        /// 与该记录关联的用户GUID
        /// </summary>
        public Guid Userid { get; set; }
        
        /// <summary>
        /// 保留字段
        /// </summary>
        public int RecordId { get; set; }

        /// <summary>
        /// 问题发送给用户的时间
        /// </summary>
        public DateTime Ctime { get; set; }

        /// <summary>
        /// 用户返回答案的时间
        /// </summary>
        public DateTime Mtime { get; set; }
        
        /// <summary>
        /// 用户提交的答案的元数据json
        /// </summary>
        public string Content { get; set; }
        
        /// <summary>
        /// 与该记录关联的问题的Version Guid
        /// </summary>
        public Guid QuestionVersion { get; set; }

        /// <summary>
        /// 与该记录关联的问题的Guid
        /// </summary>
        public Guid QuestionGuid { get; set; }

        /// <summary>
        /// 与该记录关联的问卷的Version Guid
        /// </summary>
        public Guid QuestionnaireGuid { get; set; }
    }
}
