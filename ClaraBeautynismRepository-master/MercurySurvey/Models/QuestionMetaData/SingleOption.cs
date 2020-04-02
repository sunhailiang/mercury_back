using System;

namespace MercurySurvey.Models.QuestionMetaData
{
    //单选题
    public class SingleOption : IQuestion
    {
        public string Type => nameof(SingleOption);

        public Guid Guid { get; set; }

        public string Content { get; set; }

        public string Progress { get; set; }

        public string Description { get; set; }

        public string[] Options { get; set; }

        public string Title { get; set; }

        public string Sources { get; set; }
    }
}
