using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SurveysManagement.Web.Commons.Questions
{
    public class CheckboxQuestion : BaseQuestion
    {


        

        protected override string QuestionTemplate()
        {
            
            //string template = @"<input type='checkbox' id='{1}' value='{1}' name='{0}'>{2}</input>";
            string template = @"<div class='checkbox' style='padding-left:35px; padding-right:35px;'><label>
                                <input type='hidden' name='{0}' value='__Multi__' />
                                <input type='checkbox' id='{1}-{0}' value='{1}' name='{0}' {3} {7} />{5}
                                <input type='text' id='{1}-{0}other' name='{1}-{0}other' value='{4}' {6}/></label></div>";
            //string template = @"<div class='form-check'>
            //                  <input class='form-check-input' type='checkbox' name='{0}' id='{1}' value='{1}' checked=''>
            //                  <label class='form-check-label' for='{1}'>
            //                    {2}
            //                  </label>
            //                </div>";
            StringBuilder builder = new StringBuilder();
            foreach (var checkbox in base.Answers)
            {
                string isChecked = string.Empty;
                string isOther = "class='hidden'";
                string strScript = string.Empty;
                string strIsOther = string.Empty;
                //check if it has a value selected
                var userHasSelection = UserAnswers.Where(x => x.Item1 == QuestionUniqueID && x.Item2 == checkbox.Value.ToString()).FirstOrDefault();
                if (checkbox.isOther.HasValue && checkbox.isOther.Value)
                {
                    strIsOther = "Other";                    
                    strScript = @"onclick='showCustomText(this)'";
                }
                else
                {
                    strIsOther = checkbox.Text;                    
                    strScript = string.Empty;
                }

                if (userHasSelection != null)
                {
                    isChecked = "checked=''";
                    isOther = string.Empty;
                }
                    
                builder.AppendFormat(template, QuestionUniqueID, checkbox.Value, checkbox.Text, isChecked, checkbox.Text, strIsOther, isOther, strScript);
            }
            string allChoices = string.Format(base.BaseTemplate, base.QuestionUniqueID, base.QuestionText, builder.ToString());

            return allChoices;
        }

        public override void RenderQuestion()
        {
            Html = QuestionTemplate();
        }
    }
}