namespace SurveysManagement.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TempSurveyQuestion
    {
        public int Id { get; set; }

        public int? TempSurveyId { get; set; }

        public int? TempQuestionId { get; set; }

        public virtual Question Question { get; set; }

        public virtual TempSurvey TempSurvey { get; set; }
    }
}
