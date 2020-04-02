using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MercurySurvey.Models
{
    public class QuestionQueue
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 表中拥有相同Guid的行属于同一个Queue
        /// </summary>
        public Guid QueueGuid { get; set; }

        /// <summary>
        /// 指向DbContext.Question.Version字段
        /// </summary>
        public Guid QuestionVersion { get; set; }

        /// <summary>
        /// 标识当前队列的用户归属
        /// </summary>
        public Guid QueueUser { get; set; }

        /// <summary>
        /// 标识当前队列的构建时间
        /// </summary>
        public DateTime Ctime { get; set; }

        /// <summary>
        /// 指向DbContext.Questionnaire.Version
        /// </summary>
        public Guid QuestionnaireGuid { get; set; }

        /// <summary>
        /// QueueBuilder调用时动态生成的Content，优先级高于Question表中的Content
        /// </summary>
        public string DynamicContent { get; set; }
    }
}
