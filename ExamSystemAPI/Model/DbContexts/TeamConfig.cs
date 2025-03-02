using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamSystemAPI.Model.DbContexts
{
    public class TeamConfig : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.ToTable("T_Teams");
            builder.Property(t => t.Name).HasMaxLength(32);
            builder.Property(t => t.Password).HasMaxLength(64);
            builder.Property(t => t.State).HasMaxLength(1);
            builder.HasMany(t => t.Users).WithMany(u => u.Teams).UsingEntity("T_Teams_Users");
        }
    }
}
