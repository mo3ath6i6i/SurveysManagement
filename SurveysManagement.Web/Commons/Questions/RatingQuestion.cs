using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveysManagement.Web.Commons.Questions
{
    public class RatingQuestion : BaseQuestion
    {
        public string PlaceHolder { get; set; }

        public double? From { get; set; }

        public double? To { get; set; }

        public int? Step { get; set; }

        public override void RenderQuestion()
        {
            Html = QuestionTemplate();
        }

        protected override string QuestionTemplate()
        {
            //string template = "<input type='number' id='{0}' name='{0}' placeholder='{1}'></input>";
            string template = @"<div class='input-group'>
                                <div class='hidden'>
                                     <input class='form-control' id='{0}' step='1' type='number' min='{2}' max='{3}' class='form-control hidden' name='{0}' placeholder='{1}' style='z-index:1;' value='{4}'>
                                </div></div>
<div class='rateit' data-rateit-backingfld='#{0}' data-rateit-value='{4}' data-rateit-step='1'></div>";
            string value = string.Empty;
            string listItems = string.Empty;
            int? step = Step.HasValue ? Step : 1;
            //check if it has a value selected
            var userHasSelection = UserAnswers.Where(x => x.Item1 == QuestionUniqueID).FirstOrDefault();

            if (userHasSelection != null)
                value = userHasSelection.Item2;

            From = From.HasValue ? From.Value : 0;
            To = To.HasValue ? To.Value : 100;

            //for (double? i = From; i < To; i += step)
            //{
            //    //listItems += @"<li class=""selectgroup-item"" onclick=""$('#" + base.QuestionUniqueID + "').val('"+ i + "');$(this).addClass('class', 'selectgroup-button').removeClass('selectgroup-item')\"> " + i + " </li>";
            //    listItems += @"<label class='selectgroup-item' onclick=""debugger;$('#" + base.QuestionUniqueID + "').val('" + i + "');$(this).siblings().removeClass('selectgroup-item-selected');$(this).addClass('selectgroup-item-selected').removeClass('selectgroup-item')\"><span class='selectgroup-button'>" + i + "</span></label>";
            //}

            var control = string.Format(template,
               base.QuestionUniqueID,
               !string.IsNullOrEmpty(PlaceHolder) ? PlaceHolder : string.Empty,
               From,
               To,
               value,
               listItems);

            string allChoices = string.Format(base.BaseTemplate, base.QuestionUniqueID, base.QuestionText, control);

            return allChoices;
        }
    }
}