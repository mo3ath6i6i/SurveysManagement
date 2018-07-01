namespace SurveysManagement.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TempSurvey
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TempSurvey()
        {
            TempSurveyQuestions = new HashSet<TempSurveyQuestion>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? CategoryId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? isDeleted { get; set; }

        public int? AgentId { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TempSurveyQuestion> TempSurveyQuestions { get; set; }

        public virtual User User { get; set; }
    }
}
