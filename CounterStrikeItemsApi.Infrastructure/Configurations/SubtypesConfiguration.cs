using CounterStrikeItemsApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CounterStrikeItemsApi.Infrastructure.Configurations
{
    public class SubtypesConfiguration : IEntityTypeConfiguration<Subtype>
    {
        public void Configure(EntityTypeBuilder<Subtype> builder)
        {
            builder.HasIndex(x => x.Slug)
                .IsUnique();
        }
    }

}
