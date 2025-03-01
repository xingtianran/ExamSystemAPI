using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamSystemAPI.Model.Config
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("T_Categories");
            builder.Property(c => c.Name).HasMaxLength(32);
            builder.Property(c => c.State).HasMaxLength(1);
            builder.HasOne(c => c.User).WithMany();
            builder.HasOne(c => c.Parent).WithMany(c => c.Children).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
