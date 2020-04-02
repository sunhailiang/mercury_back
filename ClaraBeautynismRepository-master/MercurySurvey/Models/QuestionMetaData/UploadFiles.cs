using System;

namespace MercurySurvey.Models.QuestionMetaData
{
    //文件上传题
    public class UploadFiles : IQuestion
    {
        public string Type => nameof(UploadFiles);

        public Guid Guid { get; set; }

        public string Content { get; set; }

        public string Progress { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public string Sources { get; set; }
    }


}
