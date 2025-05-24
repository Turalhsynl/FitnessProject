using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.EmailVerification.Configuration;

public class EmailVerificationConfiguration : IEntityTypeConfiguration<Domain.Entities.EmailVerification>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.EmailVerification> builder)
    {
        builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Code).IsRequired().HasMaxLength(6);
        builder.Property(x => x.IsUsed).HasDefaultValue(false);
    }
}

