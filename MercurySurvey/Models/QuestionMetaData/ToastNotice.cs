﻿using System;

namespace MercurySurvey.Models.QuestionMetaData
{
    public class ToastNotice : IQuestion
    {
        public string Type => nameof(ToastNotice);

        public Guid Guid { get; set; }

        public string Content { get; set; }

        public string Progress { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public string Sources { get; set; }

        public string NewCardDescription { get; set; }
    }
}
