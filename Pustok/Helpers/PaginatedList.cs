namespace Pustok.Helpers
{
    public class PaginatedList<T> : List<T>
    {
        public PaginatedList(List<T> datas,int count,int page,int pageSize)
        {
            this.AddRange(datas);
            ActivePage= page;
            TotalPageCount = (int)Math.Ceiling(count / (double)pageSize);
        }
        public int ActivePage { get; set; }
        public int TotalPageCount { get; set; }
        public bool HasNext { get => ActivePage < TotalPageCount; }
        public bool HasPrev { get => ActivePage > 1; }
    }
}
