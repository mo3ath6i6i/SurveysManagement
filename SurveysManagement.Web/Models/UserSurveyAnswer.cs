using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveysManagement.Web.Models
{
    public class UserSurveyAnswer
    {
        public int QuestionID { get; set; }

        public int? AnswerID { get; set; }

        public string AnswerText { get; set; }


    }
}