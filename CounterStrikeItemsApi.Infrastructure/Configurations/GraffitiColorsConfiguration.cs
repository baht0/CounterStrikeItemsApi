using CounterStrikeItemsApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CounterStrikeItemsApi.Infrastructure.Configurations
{
    public class GraffitiColorsConfiguration : IEntityTypeConfiguration<GraffitiColor>
    {
        public void Configure(EntityTypeBuilder<GraffitiColor> builder)
        {
            builder.HasIndex(x => x.Slug)
                .IsUnique();
        }
    }

}
