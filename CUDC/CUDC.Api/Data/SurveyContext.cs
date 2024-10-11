using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CUDC.Api.Data
{
    public class SurveyContext : DbContext
    {
        public SurveyContext(DbContextOptions<SurveyContext> options) : base(options)
        {
        }

        public DbSet<Survey> Surveys { get; set; }

        public DbSet<SurveyType> SurveyTypes { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<QuestionType> QuestionTypes { get; set; }

        public DbSet<QuestionRevision> QuestionRevisions { get; set; }

        public DbSet<QuestionOption> QuestionOptions { get; set; }

        public DbSet<Response> Responses { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Employee> Employees { get; set; }

        //public DbSet<EmployeeExtension> EmployeeExtensions { get; set; }
              

        public DbSet<QuestionReference> QuestionReferences { get; set; }
        public virtual DbSet<NLog> NLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           
            modelBuilder.Entity<NLog>(entity =>
            {
                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Logged).HasColumnType("datetime");

                entity.Property(e => e.Logger).HasMaxLength(250);

                entity.Property(e => e.MachineName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Message).IsRequired();
            });
        }
    }
}
