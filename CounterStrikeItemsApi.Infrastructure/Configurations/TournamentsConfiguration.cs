using CounterStrikeItemsApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CounterStrikeItemsApi.Infrastructure.Configurations
{
    public class TournamentsConfiguration : IEntityTypeConfiguration<Tournament>
    {
        public void Configure(EntityTypeBuilder<Tournament> builder)
        {
            builder.HasIndex(x => x.Slug)
                .IsUnique();
        }
    }

}
