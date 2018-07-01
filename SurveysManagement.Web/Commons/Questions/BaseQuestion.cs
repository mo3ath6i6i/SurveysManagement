using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static SurveysManagement.Web.Enums.Enums;

namespace SurveysManagement.Web.Commons.Questions
{
    public abstract class BaseQuestion
    {
        protected string BaseTemplate { get; set; }
        public int QuestionUniqueID { get; set; }

        public string QuestionText { get; set; }

        public QuestionTypes QuestionType { get; set; }
        
        public List<MultipleChoiseQuestionAnswers> Answers { get; set; }

        public List<Tuple<int, string, bool?>> UserAnswers { get; set; }

        public string Html { get; protected set; }

        

        public BaseQuestion()
        {
            
            Answers = new List<MultipleChoiseQuestionAnswers>();
            UserAnswers = new List<Tuple<int, string, bool?>>();
            BaseTemplate = @"<div class='container'><div class='well'><div class='form-group' style='padding-left: 10px;padding-right: 10px;'>
                                      <label for='{0}'>
                                        {1}
                                    </label>
                                   {2}
                                </div></div></div>";
        }


        protected abstract string QuestionTemplate();

        public abstract void RenderQuestion();



    }
}