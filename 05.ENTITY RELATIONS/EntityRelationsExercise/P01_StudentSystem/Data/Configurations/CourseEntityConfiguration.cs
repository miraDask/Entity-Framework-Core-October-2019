namespace P01_StudentSystem.Data.Configurations
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using static DataValidator.Course;

    public class CourseEntityConfiguration : IEntityTypeConfiguration<Course>
    {

        public void Configure(EntityTypeBuilder<Course> course)
        {
            course.HasKey(c => c.CourseId);

            course.Property(c => c.Name)
                .HasMaxLength(NameMaxLength)
                .IsUnicode()
                .IsRequired();

            course.Property(c => c.Description)
                .IsUnicode()
                .IsRequired(false);

            course.Property(c => c.StartDate)
                .HasColumnType("DATETIME2")
                .IsRequired();

            course.Property(c => c.EndDate)
                .HasColumnType("DATETIME2")
                .IsRequired();

            course.HasMany(c => c.StudentsEnrolled)
                .WithOne(s => s.Course)
                .HasForeignKey(s => s.CourseId);

            course.HasMany(c => c.Resources)
                .WithOne(r => r.Course)
                .HasForeignKey(r => r.CourseId);

            course.HasMany(c => c.HomeworkSubmissions)
                .WithOne(h => h.Course)
                .HasForeignKey(h => h.CourseId);

            //TODO Price???
        }
    }
}
