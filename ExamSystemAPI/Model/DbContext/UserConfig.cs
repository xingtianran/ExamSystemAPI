using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamSystemAPI.Model.DbContext
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("T_Users");
            builder.Property(u => u.Name).HasMaxLength(32);
            builder.Property(u => u.Password).HasMaxLength(64);
            builder.Property(u => u.Email).HasMaxLength(16).IsUnicode();
            builder.Property(u => u.Phone).HasMaxLength(11).IsUnicode();
            builder.Property(u => u.Role).HasMaxLength(12);
            builder.Property(u => u.State).HasMaxLength(1);
        }
    }
}
