using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTCStore.RanderModels
{
    public abstract class PagedResultBase
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        public int FirstRowOnPage
        {

            get { return (CurrentPage - 1) * PageSize + 1; }
        }

        public int LastRowOnPage
        {
            get { return Math.Min(CurrentPage * PageSize, RowCount); }
        }
        public static async Task<PagedResult<T>> GetPagedAsync<T>(string query, DbContext dbContext, object qmodel,
                               int pageSize, int page, Dictionary<string, string> orderlist = null) where T : class
        {
            var result = new PagedResult<T>();
            var conn = dbContext.Database.GetDbConnection();
            conn.Open();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = await conn.ExecuteScalarAsync<int>($@"Select count(*) from ({query}) a",qmodel);
            if (orderlist != null && orderlist.Keys.Count>0)
            {
                var orderstr = $" Order by {string.Join(',', orderlist.Keys.Select(o => o + ' ' + orderlist[o]))}";
                query += orderstr;

            }
            else {
                query += " Order by 1";
            }

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);
            var skip = (page - 1) * pageSize;
            query += $" OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY ";
            result.Results = await conn.QueryAsync<T>(query, qmodel);
            return result;
        }
    }

    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IEnumerable<T> Results { get; set; }
        public Dictionary<string, string> ExtMsg { get; internal set; }

        public PagedResult()
        {
            Results = new List<T>();
        }
       
    }
}
