using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<Class> Class { get; set; }
        public virtual DbSet<ClassStudent> ClassStudent { get; set; }
        public virtual DbSet<RecordSubject> RecordSubject { get; set; }
        public virtual DbSet<ScoreRecordSubject> ScoreRecordSubject { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<Summary> Summary { get; set; }
        public virtual DbSet<SummarySubject> SummarySubject { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ClassStudent>(entity =>
            {
                entity.HasKey(e => new { e.ClassId, e.StudentId })
                    .HasName("PK__Class_St__48357579DF6EC238");

                entity.ToTable("Class_Student");

                entity.HasIndex(e => e.StudentId);

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassStudent)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Class_Stu__Class__286302EC");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.ClassStudent)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Class_Stu__Stude__29572725");
            });

            modelBuilder.Entity<RecordSubject>(entity =>
            {
                entity.ToTable("Record_Subject");

                entity.HasIndex(e => e.ClassId);

                entity.HasIndex(e => e.StudentId);

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.RecordSubject)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK__Record_Su__Class__2D27B809");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.RecordSubject)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK__Record_Su__Stude__2E1BDC42");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.RecordSubject)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK_Record_Subject_Subject");
            });

            modelBuilder.Entity<ScoreRecordSubject>(entity =>
            {
                entity.ToTable("Score_Record_Subject");

                entity.HasIndex(e => e.RecordSubjectId);

                entity.HasOne(d => d.RecordSubject)
                    .WithMany(p => p.ScoreRecordSubject)
                    .HasForeignKey(d => d.RecordSubjectId)
                    .HasConstraintName("FK__Score_Re__Recor__30F848ED");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.Birth).HasColumnType("datetime");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Summary>(entity =>
            {
                entity.HasIndex(e => e.ClassId);

                entity.HasIndex(e => new { e.Semeter, e.ClassId })
                    .HasName("UQ__Summary___F5B4DD73D003A2C0")
                    .IsUnique()
                    .HasFilter("([Semeter] IS NOT NULL AND [ClassId] IS NOT NULL)");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Summary)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK__Summary_S__Class__38996AB5");
            });

            modelBuilder.Entity<SummarySubject>(entity =>
            {
                entity.ToTable("Summary_Subject");

                entity.HasIndex(e => e.ClassId);

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.SummarySubject)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK__Summary_S__Class__34C8D9D1");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.SummarySubject)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK_Summary_Subject_Subject");
            });
        }
    }
}
