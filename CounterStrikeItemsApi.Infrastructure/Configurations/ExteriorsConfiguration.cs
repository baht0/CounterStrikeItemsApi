using CounterStrikeItemsApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CounterStrikeItemsApi.Infrastructure.Configurations
{
    public class ExteriorsConfiguration : IEntityTypeConfiguration<Exterior>
    {
        public void Configure(EntityTypeBuilder<Exterior> builder)
        {
            builder.HasIndex(x => x.Slug)
                .IsUnique();
        }
    }

}
