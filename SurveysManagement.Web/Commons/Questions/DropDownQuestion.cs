using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SurveysManagement.Web.Commons.Questions
{
    public class DropDownQuestion : BaseQuestion
    {
        protected override string QuestionTemplate()
        {
            string selectTemplate = "<input type='hidden' name='{0}' value='__Multi__' /><select class='form-control' id='{0}' name='{0}'>{1}</select>";
            string template = "<option value='{0}' {2}>{1}</option>";

            StringBuilder builder = new StringBuilder();
            foreach (var option in base.Answers)
            {
                string isChecked = string.Empty;
                //check if it has a value selected
                var userHasSelection = UserAnswers.Where(x => x.Item1 == QuestionUniqueID && x.Item2 == option.Value.ToString()).FirstOrDefault();

                if (userHasSelection != null)
                    isChecked = "selected";
                builder.AppendFormat(template, option.Value, option.Text, isChecked);
            }

            var boundSelect = string.Format(selectTemplate, QuestionUniqueID, builder.ToString());
            string allChoices = string.Format(base.BaseTemplate, base.QuestionUniqueID, base.QuestionText, boundSelect);

            return allChoices;
        }

        public override void RenderQuestion()
        {
            Html = QuestionTemplate();
        }
    }
}