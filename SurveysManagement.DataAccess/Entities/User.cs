namespace SurveysManagement.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Surveys = new HashSet<Survey>();
            TempSurveys = new HashSet<TempSurvey>();
            UserSurveys = new HashSet<UserSurvey>();
        }

        public int Id { get; set; }

        public int? RegistrationId { get; set; }

        public string Name { get; set; }

        [Required]
        [StringLength(128)]
        public string ASPUserId { get; set; }

        public bool? isDeleted { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpiryDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreationDate { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string Area { get; set; }

        [StringLength(50)]
        public string Street { get; set; }

        [StringLength(3)]
        public string Gender { get; set; }

        [StringLength(50)]
        public string Education { get; set; }

        [StringLength(50)]
        public string Major { get; set; }

        [StringLength(50)]
        public string ShiftStart { get; set; }

        [StringLength(50)]
        public string ShiftEnd { get; set; }

        [StringLength(50)]
        public string ShiftDescription { get; set; }

        public DateTime? BirthDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Survey> Surveys { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TempSurvey> TempSurveys { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserSurvey> UserSurveys { get; set; }
    }
}
