using SurveysManagement.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveysManagement.Web.Models
{
    public class AnsweredSurveyViewModel
    {
        public List<SurveyAnswer> Answers { get; set; }

        public List<SurveyQuestion> Questions { get; set; }

    }
}