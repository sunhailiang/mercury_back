using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MercurySurvey.Models.QuestionMetaData
{
    public interface IQuestion
    {
        string Type { get; }

        Guid Guid { get; set; }

        string Title { get; set; }

        string Content { get; set; }

        string Description { get; set; }

        string Progress { get; set; }

        string Sources { get; set; }
    }
}
