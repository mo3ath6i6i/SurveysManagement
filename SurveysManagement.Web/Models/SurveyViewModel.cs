using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SurveysManagement.Web.Models
{
    public class SurveyViewModel
    {
        public int? Id { get; set; }
        
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreationDate { get; set; }

        public int? CategoryId { get; set; }

        public string Category { get; set; }

        public string Agent { get; set; }

        public int? AgentId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:mm/dd/yyyy}", ApplyFormatInEditMode = true)]
        private DateTime? _StartDate;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:mm/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate {
            get
            {
                return _StartDate;
            }
            set
            {
                if (value != null)
                {
                    _StartDate = Helpers.DateHelpers.ConvertStringToDate(value.ToString());
                    _StartDate = value;
                }
            }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:mm/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        public List<HelperViewModel> Agents { get; set; }

        public List<HelperViewModel> Categories { get; set; }

        public List<HelperViewModel> SurveyEntries { get; set; }

        public List<HelperViewModel> SurveyQuestions { get; set; }

        public List<HelperViewModel> UserSurveys { get; set; }

        public string hdnSurveyQuestions { get; set; }

        public string hdnUserSurveys { get; set; }
                
        public string hfIsPreview { get; set; }
        public int? hfPreviewId { get; set; }
    }
}