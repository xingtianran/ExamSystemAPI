using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamSystemAPI.Model.DbContexts
{
    public class ErrorRecordConfig : IEntityTypeConfiguration<ErrorRecord>
    {
        public void Configure(EntityTypeBuilder<ErrorRecord> builder)
        {
            builder.ToTable("T_ErrorRecords");
            builder.Property(e => e.Answer).HasMaxLength(512);
            builder.HasOne(c => c.User).WithMany();
            builder.HasOne(c => c.Topic).WithMany();
        }
    }
}
