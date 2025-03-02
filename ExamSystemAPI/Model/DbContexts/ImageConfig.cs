using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamSystemAPI.Model.DbContexts
{
    public class ImageConfig : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("T_Images");
            builder.Property(i => i.Url).HasMaxLength(64);
            builder.Property(i => i.Name).HasMaxLength(32);
            builder.Property(i => i.Path).HasMaxLength(64);
            builder.Property(i => i.ContentType).HasMaxLength(12);
            builder.Property(i => i.State).HasMaxLength(1);
            builder.HasOne(i => i.User).WithMany();
        }
    }
}
