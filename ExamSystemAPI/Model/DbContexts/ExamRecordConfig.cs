using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamSystemAPI.Model.DbContexts
{
    public class ExamRecordConfig : IEntityTypeConfiguration<ExamRecord>
    {
        public void Configure(EntityTypeBuilder<ExamRecord> builder)
        {
            builder.ToTable("T_ExamRecords");
            builder.Property(e => e.Name).HasMaxLength(32);
            builder.Property(e => e.State).HasMaxLength(1);
            builder.HasOne(c => c.User).WithMany();
        }
    }
}
