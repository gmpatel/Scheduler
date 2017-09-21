using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Market.Authentication.Core.Helpers;
using Market.Authentication.Core.Objects.Responses.Common;
using Market.WebApis.Properties;

namespace Market.WebApis.Filters.Authorization
{
    public class TokenAuthorization : AuthorizationFilterAttribute
    {
        internal static class Constants
        {
            public static readonly bool MasterTokensEnabled = true;

            public static readonly IList<string> MasterTokens = new List<string>
            {
                Resources.MasterToken1,
                Resources.MasterToken2
            };
        }

        private readonly IList<string> roles;

        public TokenAuthorization(string roles = "*")
        {
            this.roles = roles.Split(new char[] { ',' }).Select(part => part.Trim()).Where(value => !string.IsNullOrEmpty(value)).ToList();
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var response = IsAuthorized(actionContext);

            if (response.Error != null)
            {
                HandleUnauthorizedRequest(actionContext, response);
            }
        }

        GenericResponse<bool> IsAuthorized(HttpActionContext actionContext)
        {
            var token = string.Empty;

            if (actionContext.Request.Headers.Contains(Resources.TokenHeaderKey1))
            {
                token = actionContext.Request.Headers.GetValues(Resources.TokenHeaderKey1).FirstOrDefault();
            }
            else if (actionContext.Request.Headers.Contains(Resources.TokenHeaderKey2))
            {
                token = actionContext.Request.Headers.GetValues(Resources.TokenHeaderKey2).FirstOrDefault();
            }

            if (Constants.MasterTokensEnabled && Constants.MasterTokens.Contains(token))
            {
                return new GenericResponse<bool>
                {
                    Result = true
                };
            }

            if (!string.IsNullOrEmpty(token))
            {
                return token.IsValidToken(WebSystem.BackEndKey, this.roles);
            }

            return new GenericResponse<bool>
            {
                Error = new Error
                {
                    ResponseCode = HttpStatusCode.Unauthorized,
                    Code = 10101,
                    Message = "Token is missing!"
                }
            };
        }

        static void HandleUnauthorizedRequest(HttpActionContext actionContext, GenericResponse<bool> tokenResponse)
        {
            var content = new GenericResponse<bool>
            {
                Error = new Error
                {
                    Code = tokenResponse.Error.Code,
                    ResponseCode = tokenResponse.Error.ResponseCode,
                    Message = tokenResponse.Error.Message,
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