namespace SV22T1020761.Models
{
    /// <summary>
    /// Kết quả tìm kiếm có phân trang
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginationSearchResult<T>
    {
        public IList<T> Data { get; set; } = new List<T>();
        public string SearchValue { get; set; } = "";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int RowCount { get; set; } = 0;
        public int PageCount
        {
            get
            {
                if (PageSize == 0) return 1;
                int c = RowCount / PageSize;
                if (RowCount % PageSize > 0) c++;
                return c;
            }
        }
    }
}