using System;

namespace MercurySurvey.Models.QuestionMetaData
{
    //填空题
    public class TextBox : IQuestion
    {
        public string Type => nameof(TextBox);

        public Guid Guid { get; set; }

        public string Content { get; set; }

        public string Progress { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public string Sources { get; set; }
    }


}
