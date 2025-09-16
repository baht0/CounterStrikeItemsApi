using CounterStrikeItemsApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CounterStrikeItemsApi.Infrastructure.Configurations
{
    public class ItemTypesConfiguration : IEntityTypeConfiguration<ItemType>
    {
        public void Configure(EntityTypeBuilder<ItemType> builder)
        {
            builder.HasIndex(x => x.Slug)
                .IsUnique();
        }
    }

}
