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
        public virtual DbSet<SummarySubject> SummarySubject { get; set; }
        public virtual DbSet<SummarySubjectSemeter> SummarySubjectSemeter { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Class>(entity =>
            {
            });

            modelBuilder.Entity<ClassStudent>(entity =>
            {
                entity.HasKey(e => new { e.ClassId, e.StudentId })
                    .HasName("PK__Class_St__48357579DF6EC238");

                entity.ToTable("Class_Student");

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

                entity.HasIndex(e => new { e.SubjectName, e.Semeter, e.ClassId, e.StudentId })
                    .HasName("UQ__Record_S__CA821CD02A967351")
                    .IsUnique();

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.RecordSubject)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK__Record_Su__Class__2D27B809");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.RecordSubject)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK__Record_Su__Stude__2E1BDC42");
            });

            modelBuilder.Entity<ScoreRecordSubject>(entity =>
            {
                entity.ToTable("Score_Record_Subject");

                entity.HasOne(d => d.RecordSubject)
                    .WithMany(p => p.ScoreRecordSubject)
                    .HasForeignKey(d => d.RecordSubjectId)
                    .HasConstraintName("FK__Score_Re__Recor__30F848ED");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.Birth).HasColumnType("datetime");
            });

            modelBuilder.Entity<SummarySubject>(entity =>
            {
                entity.ToTable("Summary_Subject");

                entity.HasIndex(e => new { e.SubjectName, e.ClassId })
                    .HasName("UQ__Summary___F5B4DD73D003A2C0")
                    .IsUnique();

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.SummarySubject)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK__Summary_S__Class__38996AB5");
            });

            modelBuilder.Entity<SummarySubjectSemeter>(entity =>
            {
                entity.ToTable("Summary_Subject_Semeter");

                entity.HasIndex(e => new { e.SubjectName, e.Semeter, e.ClassId })
                    .HasName("UQ__Summary___73013082B281F30A")
                    .IsUnique();

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.SummarySubjectSemeter)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK__Summary_S__Class__34C8D9D1");
            });
        }
    }
}
