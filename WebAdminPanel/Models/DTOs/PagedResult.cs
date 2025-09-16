namespace WebAdminPanel.Models.DTOs
{
    public class PagedResult<T>
    {
        public List<T> Rows { get; set; } = [];
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRows { get; set; }
        public int RowsInPage { get; set; }
    }
}
