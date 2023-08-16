namespace FTWRK.Application.Common.Models
{
    public class PagedList<T>
    {
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public IEnumerable<T> Items { get; set; }
        public PagedList()
        {

        }
        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalRecords = count;
            var totalPages = ((double)count / (double)pageSize);
            TotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            Items = items;
        }

        public PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
