namespace SurveysManagement.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SurveyEntry
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SurveyEntry()
        {
            SurveyAnswers = new HashSet<SurveyAnswer>();
        }

        public int Id { get; set; }

        public bool? isDeleted { get; set; }

        public int? UserSurveyId { get; set; }

        public DateTime? Date { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SurveyAnswer> SurveyAnswers { get; set; }

        public virtual UserSurvey UserSurvey { get; set; }
    }
}
