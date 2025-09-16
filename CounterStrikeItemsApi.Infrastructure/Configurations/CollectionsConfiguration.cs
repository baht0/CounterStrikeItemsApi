using CounterStrikeItemsApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CounterStrikeItemsApi.Infrastructure.Configurations
{
    public class CollectionsConfiguration : IEntityTypeConfiguration<Collection>
    {
        public void Configure(EntityTypeBuilder<Collection> builder)
        {
            builder.HasIndex(x => x.Slug)
                .IsUnique();
        }
    }

}
