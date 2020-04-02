using System;

namespace MercurySurvey.Models.QuestionMetaData
{
    //结论
    public class Conclusion : BooleanOption
    {

        public string Link { get; set; }

        public override string Type => nameof(Conclusion);
    }

    public class BreakConclusion : Conclusion
    {
        public override string Type => nameof(BreakConclusion);
    }
}
