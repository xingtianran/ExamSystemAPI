using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamSystemAPI.Model.DbContexts
{
    public class PaperConfig : IEntityTypeConfiguration<Paper>
    {
        public void Configure(EntityTypeBuilder<Paper> builder)
        {
            builder.ToTable("T_Papers").Ignore(p => p.TopicIds);
            builder.Property(p => p.Title).HasMaxLength(32);
            builder.Property(p => p.State).HasMaxLength(1);
            builder.HasOne(p => p.User).WithMany();
            builder.HasOne(p => p.Category).WithMany().HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(p => p.Topics).WithMany(t => t.Papers).UsingEntity<PaperTopic>("T_Papers_Topics",
                j => j.HasOne(pt => pt.Topic).WithMany().HasForeignKey(pt => pt.TopicId),
                j => j.HasOne(pt => pt.Paper).WithMany().HasForeignKey(pt => pt.PaperId)
                );

            builder.HasMany(p => p.Teams).WithMany(t => t.Papers).UsingEntity<PaperTeam>("T_Papers_Teams",
                j => j.HasOne(pt => pt.Team).WithMany().HasForeignKey(pt => pt.TeamId),
                j => j.HasOne(pt => pt.Paper).WithMany().HasForeignKey(pt => pt.PaperId)
                );
        }
    }
}
