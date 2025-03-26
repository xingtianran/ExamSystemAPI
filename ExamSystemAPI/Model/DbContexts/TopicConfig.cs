using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamSystemAPI.Model.DbContexts
{
    public class TopicConfig : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.ToTable("T_Topics").Ignore(t => t.TempTime);
            builder.Property(t => t.Title).HasMaxLength(32);
            builder.Property(t => t.Content).HasMaxLength(512);
            builder.Property(t => t.Answer).HasMaxLength(512);
            builder.Property(t => t.Type).HasMaxLength(1);
            builder.Property(t => t.State).HasMaxLength(1);
            builder.Property(t => t.Column1).HasMaxLength(512);
            builder.Property(t => t.Column2).HasMaxLength(512);
            builder.Property(t => t.Column3).HasMaxLength(512);
            builder.Property(t => t.Column4).HasMaxLength(512);
            builder.Property(t => t.Column5).HasMaxLength(512);
            builder.Property(t => t.Column6).HasMaxLength(512);
            builder.HasOne(t => t.Category).WithMany().HasForeignKey(t => t.CategoryId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(t => t.User).WithMany().OnDelete(DeleteBehavior.NoAction);
        }
    }
}
