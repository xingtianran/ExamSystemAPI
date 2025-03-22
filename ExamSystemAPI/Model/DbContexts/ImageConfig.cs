using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamSystemAPI.Model.DbContexts
{
    public class ImageConfig : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("T_Images");
            builder.Property(i => i.Name).HasMaxLength(256);
            builder.Property(i => i.OriginalName).HasMaxLength(128);
            builder.Property(i => i.Origin).HasMaxLength(1);
            builder.Property(i => i.State).HasMaxLength(1);
            builder.HasOne(i => i.User).WithMany();
        }
    }
}
