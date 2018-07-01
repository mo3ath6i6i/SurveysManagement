namespace SurveysManagement.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SurveyAnswer
    {
        public int Id { get; set; }

        public int? QuestionId { get; set; }

        public int? AnswerId { get; set; }

        public string AnswerText { get; set; }

        public int? SurveyEntryId { get; set; }

        public virtual Answer Answer { get; set; }

        public virtual Question Question { get; set; }

        public virtual SurveyEntry SurveyEntry { get; set; }
    }
}
