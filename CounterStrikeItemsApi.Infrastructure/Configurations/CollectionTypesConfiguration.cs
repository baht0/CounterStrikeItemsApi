using CounterStrikeItemsApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CounterStrikeItemsApi.Infrastructure.Configurations
{
    public class CollectionTypesConfiguration : IEntityTypeConfiguration<CollectionType>
    {
        public void Configure(EntityTypeBuilder<CollectionType> builder)
        {
            builder.HasIndex(x => x.Slug)
                .IsUnique();
        }
    }

}
