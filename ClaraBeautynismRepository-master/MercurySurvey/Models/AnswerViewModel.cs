using System;

namespace MercurySurvey.Models
{
    /// <summary>
    /// Questionnaire/BeginTheAnswer方法参数类
    /// </summary>
    public class AnswerViewModel
    {
        /// <summary>
        /// 接收前端发送的Questionnaireguid
        /// </summary>
        public Guid Guid { get; set; }
        /// <summary>
        /// 接受前端发送的问题的答案
        /// </summary>
        public object Answer { get; set; }
    }
}
