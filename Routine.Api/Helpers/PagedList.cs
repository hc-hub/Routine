using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Helpers
{
    public class PagedList<T>:List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => TotalCount > CurrentPage;
        public PagedList(List<T> item,int count,int pageNumber,int pageSize)
        {
            CurrentPage = pageNumber;
            TotalCount = count;
            PageSize = pageSize;
            TotalPages = (int)(TotalCount/(double)pageSize);
            AddRange(item);
        }
        
    }
}
