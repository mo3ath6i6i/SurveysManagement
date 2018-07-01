namespace SurveysManagement.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Question
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Question()
        {
            Answers = new HashSet<Answer>();
            SurveyAnswers = new HashSet<SurveyAnswer>();
            SurveyQuestions = new HashSet<SurveyQuestion>();
            TempSurveyQuestions = new HashSet<TempSurveyQuestion>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public int? TypeId { get; set; }

        public double? From { get; set; }

        public double? To { get; set; }

        public bool? isDeleted { get; set; }

        public bool? hasOtherOption { get; set; }

        public double? Step { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Answer> Answers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SurveyAnswer> SurveyAnswers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SurveyQuestion> SurveyQuestions { get; set; }

        public virtual QuestionType QuestionType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TempSurveyQuestion> TempSurveyQuestions { get; set; }
    }
}
