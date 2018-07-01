using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveysManagement.Web.Commons.Questions
{
    public class TextBoxQuestion : BaseQuestion
    {

        public string PlaceHolder { get; set; }

        public override void RenderQuestion()
        {
            Html = QuestionTemplate();
        }

        protected override string QuestionTemplate()
        {
            string template = "<input class='form-control' type='text' id='{0}' name='{0}' placeholder='{1}' value='{2}'></input>";
            string value = string.Empty;
            //check if it has a value selected
            var userHasSelection = UserAnswers.Where(x => x.Item1 == QuestionUniqueID).FirstOrDefault();

            if (userHasSelection != null)
                value = userHasSelection.Item2;
            var control = string.Format(template, base.QuestionUniqueID, !string.IsNullOrEmpty(PlaceHolder) ? PlaceHolder : string.Empty, value);
            
            string allChoices = string.Format(base.BaseTemplate, base.QuestionUniqueID, base.QuestionText, control);

            return allChoices;
        }
    }
}