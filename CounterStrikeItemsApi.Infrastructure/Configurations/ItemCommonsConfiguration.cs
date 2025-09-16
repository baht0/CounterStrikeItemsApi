using CounterStrikeItemsApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CounterStrikeItemsApi.Infrastructure.Configurations
{
    public class ItemCommonsConfiguration : IEntityTypeConfiguration<ItemCommon>
    {
        public void Configure(EntityTypeBuilder<ItemCommon> builder)
        {
            builder.HasIndex(x => x.Slug)
                .IsUnique();
        }
    }

}
