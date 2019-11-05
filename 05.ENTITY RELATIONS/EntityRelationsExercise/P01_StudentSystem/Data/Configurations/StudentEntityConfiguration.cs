
namespace P01_StudentSystem.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Data.Models;
    using static DataValidator.Student;

    public class StudentEntityConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> student)
        {
            student.HasKey(s => s.StudentId);

            student.Property(s => s.Name)
                .HasMaxLength(NameMaxLength)
                .IsUnicode()
                .IsRequired();

            student.Property(s => s.PhoneNumber)
                .HasColumnType($"char({PhoneNumberLength})")
                .IsRequired(false)
                .IsUnicode(false);

            student.Property(s => s.RegisteredOn)
                .HasColumnType("DATETIME2")
                .IsRequired();

            student.Property(s => s.Birthday)
                .HasColumnType("DATETIME2")
                .IsRequired(false);

            student.HasMany(s => s.HomeworkSubmissions)
                .WithOne(h => h.Student)
                .HasForeignKey(h => h.StudentId);

            student.HasMany(s => s.CourseEnrollments)
                .WithOne(c => c.Student)
                .HasForeignKey(c => c.StudentId);
        }
    }
}
