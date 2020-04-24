using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace PTCStore.RanderModels
{
    public static class ZExtensions
    {
        public static async Task<PagedResult<T>> GetPagedAsync<T>(this IQueryable<T> query,
                                     int pageSize, int page, Dictionary<string, string> orderlist = null, Dictionary<string, string> extMessage =null ) where T : class
        {
            var result = new PagedResult<T>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            //query = query.AsNoTracking();
 
            result.RowCount = await query.CountAsync();
            if (orderlist != null)
            {
                bool orderfirst = false;
                foreach (KeyValuePair<string, string> d in orderlist)
                {
                    PropertyInfo prop = typeof(T).GetProperty(d.Key);
                    if (prop != null)
                    {
                        var od = ToLambda<T>(prop.Name);
                        if (!orderfirst)
                        {
                            query = d.Value.ToLower() == "asc" ? query.OrderBy(od) : query.OrderByDescending(od);
                            orderfirst = true;
                        }
                        else
                        {
                            query = d.Value.ToLower() == "asc" ? ((IOrderedQueryable<T>)query).ThenBy(od) : ((IOrderedQueryable<T>)query).ThenByDescending(od);
                        }
                    }
                }
            }
            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);
            var skip = (page - 1) * pageSize;
            result.Results = await query.Skip(skip).Take(pageSize).ToListAsync();
            result.ExtMsg = extMessage ?? new Dictionary<string,string>();
            return result;
        }



        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }

    }
}
