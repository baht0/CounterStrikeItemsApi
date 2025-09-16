using CounterStrikeItemsApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CounterStrikeItemsApi.Infrastructure.Configurations
{
    public class ProfessionalPlayersConfiguration : IEntityTypeConfiguration<ProfessionalPlayer>
    {
        public void Configure(EntityTypeBuilder<ProfessionalPlayer> builder)
        {
            builder.HasIndex(x => x.Slug)
                .IsUnique();
        }
    }

}
