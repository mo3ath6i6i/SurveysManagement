using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveysManagement.Web.Commons.Questions
{
    public class PercentageQuestion : BaseQuestion
    {
        public string PlaceHolder { get; set; }

        public override void RenderQuestion()
        {
            Html = QuestionTemplate();
        }

        protected override string QuestionTemplate()
        {
            //string template = "<input type='number' id='{0}' name='{0}' placeholder='{1}'></input>";
            string template = @"<div class='input-group'>
                                     <input id='{0}' type='number' min='0' max='100' class='form-control' name='{0}' placeholder='{1}' style='z-index:1;' value='{2}'>
                                     <span class='input-group-addon' style='display: inline;padding: 8px 12px;'><i class='glyphicon glyphicon-percentage'>%</i></span>
                                </div>";
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