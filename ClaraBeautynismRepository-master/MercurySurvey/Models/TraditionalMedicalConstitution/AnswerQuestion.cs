using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MercurySurvey.Models.TraditionalMedicalConstitution
{
    public class AnswerQuestion
    {
        /// <summary>
        /// Answer表中的Content
        /// </summary>
        public string AnswerContent { get; set; }
        /// <summary>
        /// Question表中的Content
        /// </summary>
        public object QuestionContent { get; set; }
    }
}
