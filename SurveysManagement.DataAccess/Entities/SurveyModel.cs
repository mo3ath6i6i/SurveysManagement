namespace SurveysManagement.DataAccess.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SurveyModel : DbContext
    {
        public SurveyModel()
            : base("name=SurveyModel")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionType> QuestionTypes { get; set; }
        public virtual DbSet<SurveyAnswer> SurveyAnswers { get; set; }
        public virtual DbSet<SurveyEntry> SurveyEntries { get; set; }
        public virtual DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TempSurveyQuestion> TempSurveyQuestions { get; set; }
        public virtual DbSet<TempSurvey> TempSurveys { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSurvey> UserSurveys { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.TempSurveys)
                .WithOptional(e => e.Category)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Question>()
                .HasMany(e => e.TempSurveyQuestions)
                .WithOptional(e => e.Question)
                .HasForeignKey(e => e.TempQuestionId);

            modelBuilder.Entity<QuestionType>()
                .HasMany(e => e.Questions)
                .WithOptional(e => e.QuestionType)
                .HasForeignKey(e => e.TypeId);

            modelBuilder.Entity<User>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Surveys)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.AgentId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.TempSurveys)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.AgentId)
                .WillCascadeOnDelete();
        }
    }
}
