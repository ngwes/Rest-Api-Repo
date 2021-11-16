using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApiRepo.Cache
{
    public class CacheKeyBuilder : IAddRequestPath, IAddQueryParameters, ICacheKeyFromHttpRequestBuilder, IAddMethod
    {
        private StringBuilder _queryBuilder;

        private CacheKeyBuilder(){}
        public IAddQueryParameters AddPath(PathString path)
        {
            this._queryBuilder.Append($"{path}");
            return this;
        }

        public string AddQueryParameteres(IQueryCollection keyValues/*, string cacheKeyFormat = null*/)
        {
            //if (string.IsNullOrEmpty(cacheKeyFormat))
            //    cacheKeyFormat = "|{key}-{value}";

            foreach(var (key, value) in keyValues.OrderBy(x => x.Key)){
                _queryBuilder.Append($"|{key}-{value}");
            }
            var cacheKey = _queryBuilder.ToString();
            _queryBuilder.Clear();
            return cacheKey;
        }
        public static ICacheKeyFromHttpRequestBuilder CreateCacheKeyBuilder()
        {
            return new CacheKeyBuilder();
        }
        public IAddMethod BuildKeyFromHttpRequest()
        {
            this._queryBuilder = new StringBuilder();
            return this;
        }

        public IAddRequestPath AddMethod(string method)
        {
            _queryBuilder.Append(method);
            return this;
        }
        
    }
    public interface ICacheKeyFromHttpRequestBuilder
    {
        IAddMethod BuildKeyFromHttpRequest();
    }

    public interface IAddMethod {
        IAddRequestPath AddMethod(string method);
    }
    public interface IAddRequestPath
    {
        IAddQueryParameters AddPath(PathString path);
    }

    public interface IAddQueryParameters
    {
        string AddQueryParameteres(IQueryCollection keyValues);
    }
}
