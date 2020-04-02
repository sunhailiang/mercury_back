using System;

namespace MercurySurvey.Models.QuestionMetaData
{
    //拍照题

    public class PhotoGraph : IQuestion
    {
        public string Type => nameof(PhotoGraph);

        public Guid Guid { get; set; }

        public string Content { get; set; }

        public string Progress { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public string Sources { get; set; }
    }

}
