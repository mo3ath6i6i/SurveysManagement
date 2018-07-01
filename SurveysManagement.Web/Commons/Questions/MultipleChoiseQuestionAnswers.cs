using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveysManagement.Web.Commons.Questions
{
    public class MultipleChoiseQuestionAnswers
    {
        public int QuestionID { get; set; }

        public int Value { get; set; }
        public string Text { get; set; }
        public bool? isOther { get; set; }

    }
}