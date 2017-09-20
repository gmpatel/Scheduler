using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Market.WebApis
{
    public class HttpActionResult<T> : IHttpActionResult
    {
        private readonly T content;
        private readonly HttpStatusCode statusCode;

        public HttpActionResult(HttpStatusCode statusCode, T content)
        {
            this.statusCode = statusCode;
            this.content = content;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(statusCode)
            {
                Content = new ObjectContent<T>(content, new JsonMediaTypeFormatter())
            };

            return Task.FromResult(response);
        }
    }
}