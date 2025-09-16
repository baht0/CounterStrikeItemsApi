namespace CounterStrikeItemsApi.Application.DTOs
{
    public class PagedResult<T>
    {
        public List<T> Rows { get; set; } = [];
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalRows { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRows / PageSize);
        public int RowsInPage => Rows.Count;
    }
}
