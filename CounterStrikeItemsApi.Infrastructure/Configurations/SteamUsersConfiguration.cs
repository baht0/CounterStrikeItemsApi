using CounterStrikeItemsApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CounterStrikeItemsApi.Infrastructure.Configurations
{
    public class SteamUsersConfiguration : IEntityTypeConfiguration<SteamUser>
    {
        public void Configure(EntityTypeBuilder<SteamUser> builder)
        {
            builder.HasIndex(x => x.SteamId)
                .IsUnique();
        }
    }
}
