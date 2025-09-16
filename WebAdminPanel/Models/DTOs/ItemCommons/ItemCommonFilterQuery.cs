using Refit;

namespace WebAdminPanel.Models.DTOs.ItemCommons
{
    public class ItemCommonFilterQuery
    {
        public string? Search { get; set; }
        [Query(CollectionFormat = CollectionFormat.Multi)]
        public List<string>? Collections { get; set; }
        [Query(CollectionFormat = CollectionFormat.Multi)]
        public List<string>? Types { get; set; }
        [Query(CollectionFormat = CollectionFormat.Multi)]
        public List<string>? Subtypes { get; set; }
        [Query(CollectionFormat = CollectionFormat.Multi)]
        public List<string>? Qualities { get; set; }
        public int PageSize { get; set; } = 25;
        public int Page { get; set; } = 1;
    }
}
