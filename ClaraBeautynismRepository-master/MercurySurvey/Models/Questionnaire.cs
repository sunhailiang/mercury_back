using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MercurySurvey.Models
{
    public class Questionnaire
    {
        /// <summary>
        /// 同标题记录行TID相同
        /// </summary>
        [Key]
        public Guid QuestionnaireGuid { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 问题QuestionGuid的集合
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// 核分脚本
        /// </summary>
        public string Calculator { get; set; }

        /// <summary>
        /// 创建的时间
        /// </summary>
        public DateTime Ctime { get; set; }

#warning PK应该是Version而不是Questionnaire Guid
        /// <summary>
        /// 版本号Guid  pk
        /// </summary>
        public Guid Version { get; set; }

        /// <summary>
        /// Questionnaire的自增主键
        /// </summary>
        public int QuestionnairePK { get; set; }
        
        /// <summary>
        /// 指示当前记录是否已经被移除
        /// </summary>
        public bool IsDeleted { get; set; }


    }
}
