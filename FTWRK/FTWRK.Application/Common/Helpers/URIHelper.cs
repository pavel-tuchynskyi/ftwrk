using Microsoft.AspNetCore.Http.Extensions;

namespace FTWRK.Application.Common.Helpers
{
    public static class URIHelper
    {
        public static string CreateLink(string scheme, string host, int port, string path,
            Dictionary<string, string>? queryParams = null)
        {
            var uriBuilder = CreateLink(scheme, host, port, path);

            if(queryParams != null)
            {
                var queryBuilder = new QueryBuilder();
                foreach (var param in queryParams)
                {
                    queryBuilder.Add(param.Key, param.Value);
                }

                uriBuilder.Query = queryBuilder.ToQueryString().Value;
            }

            return uriBuilder.Uri.AbsoluteUri;
        }

        private static UriBuilder CreateLink(string scheme, string host, int port, string path)
        {
            var builder = new UriBuilder();
            builder.Scheme = scheme;
            builder.Host = host;
            builder.Port = port;
            builder.Path = path;

            return builder;
        }

        public static string AddQueryParameters(string link, Dictionary<string, string> queryParameters)
        {
            var ub = new UriBuilder(link);
            var qb = new QueryBuilder();

            foreach (var param in queryParameters)
            {
                qb.Add(param.Key, param.Value);
            }

            ub.Query = qb.ToQueryString().Value;

            return ub.Uri.AbsoluteUri;
        }
    }
}
