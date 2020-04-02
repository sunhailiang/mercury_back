using System;
using System.Collections.Generic;

namespace MercurySurvey.Models.QuestionMetaData
{
    /// <summary>
    /// 单选题-二选一
    /// </summary>
    public class BooleanOption : IQuestion
    {
        public virtual string Type => nameof(BooleanOption);

        public Guid Guid { get; set; }

        public string Content { get; set; }

        public string Progress { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public string Sources { get; set; }

        public Uri CardImage { get; set; }

        public Dictionary<string, Option> Options { get; set; }

        public class Option
        {
            /// <summary>
            /// 卡面图片资源地址
            /// </summary>
            public Uri CardImage { get; set; }

            /// <summary>
            /// 选项
            /// </summary>
            public string Text { get; set; }
        }
    }
}
