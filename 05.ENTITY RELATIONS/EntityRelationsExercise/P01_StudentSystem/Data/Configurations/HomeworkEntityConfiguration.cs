namespace P01_StudentSystem.Data.Configurations
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class HomeworkEntityConfiguration : IEntityTypeConfiguration<Homework>
    {
        public void Configure(EntityTypeBuilder<Homework> homework)
        {
            homework.HasKey(h => h.HomeworkId);

            homework.Property(h => h.Content)
                .IsUnicode(false)
                .IsRequired();

            homework.Property(h => h.ContentType)
                .IsRequired();

            homework.Property(h => h.SubmissionTime)
                .HasColumnType("DATETIME2")
                .IsRequired();
        }
    }
}
