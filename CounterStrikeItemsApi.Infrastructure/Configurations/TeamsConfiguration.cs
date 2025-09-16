using CounterStrikeItemsApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CounterStrikeItemsApi.Infrastructure.Configurations
{
    public class TeamsConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasIndex(x => x.Slug)
                .IsUnique();
        }
    }

}
