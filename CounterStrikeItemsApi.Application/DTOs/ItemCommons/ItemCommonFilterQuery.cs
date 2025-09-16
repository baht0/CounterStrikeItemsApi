using CounterStrikeItemsApi.Application.Helpers;
using System.ComponentModel.DataAnnotations;

namespace CounterStrikeItemsApi.Application.DTOs.ItemCommons
{
    /// <summary>
    /// List<string>? in Slug identifier only
    /// </summary>
    public class ItemCommonFilterQuery
    {
        public string? Search { get; set; }

        [SlugList]
        public List<string>? Collections { get; set; }

        [SlugList]
        public List<string>? Types { get; set; }

        [SlugList]
        public List<string>? Subtypes { get; set; }

        [SlugList]
        public List<string>? Qualities { get; set; }

        [Range(10, 100)]
        public int PageSize { get; set; } = 25;

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        public int Page { get; set; } = 1;
    }
}
