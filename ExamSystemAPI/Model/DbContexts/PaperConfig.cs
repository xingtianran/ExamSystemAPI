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
            builder.HasMany(p => p.Topics).WithMany(t => t.Papers).UsingEntity("T_Papers_Topics", j => {
                j.Property<DateTime>("CreateTime")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}
