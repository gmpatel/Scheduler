using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Market.Authentication.Core.Objects.Responses.Common;
using Market.WebApis.Properties;

namespace Market.WebApis.Filters.Authorization
{
    public class KeyAuthorization : AuthorizationFilterAttribute
    {
        internal static class Constants
        {
            public static readonly bool OpenApisEnabled = true;

            public static readonly IList<string> OpenApis = new List<string>
            {
                // "/api/v1/user/verify/get",
                // "/api/v1/value/test/get"
            };
        }

        public KeyAuthorization()
        {
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var response = IsAuthorized(actionContext);

            if (response.Error != null)
            {
                HandleUnauthorizedRequest(actionContext, response);
            }
        }

        static GenericResponse<bool> IsAuthorized(HttpActionContext actionContext)
        {
            var api = actionContext.Request.RequestUri.AbsolutePath.ToLower();
            var key = string.Empty;

            if (actionContext.Request.Headers.Contains(Resources.KeyHeaderKey1))
            {
                key = actionContext.Request.Headers.GetValues(Resources.KeyHeaderKey1).FirstOrDefault();
            }
            else if (actionContext.Request.Headers.Contains(Resources.KeyHeaderKey2))
            {
                key = actionContext.Request.Headers.GetValues(Resources.KeyHeaderKey2).FirstOrDefault();
            }

            if (Constants.OpenApisEnabled)
            {
                if (Constants.OpenApis.Any(openApi => api.StartsWith(openApi)))
                {
                    return new GenericResponse<bool>
                    {
                        Result = true
                    };
                }
            }

            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(WebSystem.ApiKey) && WebSystem.ApiKey.Equals(key, StringComparison.CurrentCultureIgnoreCase) && WebSystem.ApiKeyEnabled)
            {
                return new GenericResponse<bool>
                {
                    Result = true
                };
            }

            if (string.IsNullOrEmpty(key))
            {
                return new GenericResponse<bool>
                {
                    Error = new Error
                    {
                        ResponseCode = HttpStatusCode.Unauthorized,
                        Code = 10001,
                        Message = "API key is missing!"
                    }
                };
            }

            if (string.IsNullOrEmpty(WebSystem.ApiKey))
            {
                return new GenericResponse<bool>
                {
                    Error = new Error
                    {
                        ResponseCode = HttpStatusCode.Unauthorized,
                        Code = 10002,
                        Message = "API key is not generated on serve side for this app!"
                    }
                };
            }

            if (!WebSystem.ApiKey.Equals(key, StringComparison.CurrentCultureIgnoreCase))
            {
                return new GenericResponse<bool>
                {
                    Error = new Error
                    {
                        ResponseCode = HttpStatusCode.Unauthorized,
                        Code = 10003,
                        Message = "API key is not valid!"
                    }
                };
            }

            if (!WebSystem.ApiKeyEnabled)
            {
                return new GenericResponse<bool>
                {
                    Error = new Error
                    {
                        ResponseCode = HttpStatusCode.Unauthorized,
                        Code = 10004,
                        Message = "API key is not ative or has been disabled!"
                    }
                };
            }

            return new GenericResponse<bool>
            {
                Error = new Error
                {
                    ResponseCode = HttpStatusCode.Unauthorized,
                    Code = 10005,
                    Message = "Unknown issue with API key!"
                }
            };
        }

        static void HandleUnauthorizedRequest(HttpActionContext actionContext, GenericResponse<bool> keyResponse)
        {
            var content = new GenericResponse<bool>
            {
                Error = new Error
                {
                    Code = keyResponse.Error.Code,
                    ResponseCode = keyResponse.Error.ResponseCode,
                    Message = keyResponse.Error.Message,
                }
            };

            var response = new HttpResponseMessage
            {
                StatusCode = content.Error.ResponseCode,
                Content = new ObjectContent<GenericResponse<bool>>(
                    content,
                    new JsonMediaTypeFormatter()
                )
            };

            actionContext.Response = response;
        }
    }
}