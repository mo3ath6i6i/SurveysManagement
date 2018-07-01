namespace SurveysManagement.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserSurvey
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserSurvey()
        {
            SurveyEntries = new HashSet<SurveyEntry>();
        }

        public int Id { get; set; }

        public int? UserId { get; set; }

        public int? SurveyId { get; set; }

        public bool? isDeleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SurveyEntry> SurveyEntries { get; set; }

        public virtual Survey Survey { get; set; }

        public virtual User User { get; set; }
    }
}
