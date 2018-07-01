using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveysManagement.Web.Models
{
    public class QuestionViewModel
    {
        public int? Id { get; set; }

        public string Question { get; set; }

        public string Text { get; set; }

        public string Type { get; set; }

        public string TypeAr { get; set; }

        public double? From { get; set; }

        public double? To { get; set; }

        public double? Step { get; set; }

        public bool? hasOtherOption { get; set; }

        public string hasOtherOptionval { get; set; }

        public virtual List<HelperViewModel> Answers { get; set; }

        public virtual List<HelperViewModel> QuestionTypes { get; set; }
    }    
}