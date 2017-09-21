using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Market.Authentication.Core.Helpers;
using Market.Authentication.Core.Objects.Exceptions;
using Market.Authentication.Core.Objects.Responses.Common;
using Market.Authentication.DataAccess.EF.Interfaces;
using Market.Authentication.Properties;
using Market.WebApis.Filters.Authorization;

namespace Market.WebApis.Controllers.Auth
{
    [RoutePrefix("api/auth/tokens")]
    [KeyAuthorization]
    public class TokensController : ApiController
    {
        private readonly IDataServiceAUTH dataServiceAuth;

        public TokensController(IDataServiceAUTH dataServiceAuth)
        {
            this.dataServiceAuth = dataServiceAuth;
        }

        [Route("renew")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            var email = default(string);

            try
            {
                var token = string.Empty;

                if (Request.Headers.Contains(Resources.TokenHeaderKey1))
                {
                    token = Request.Headers.GetValues(Resources.TokenHeaderKey1).FirstOrDefault();
                }
                else if (Request.Headers.Contains(Resources.TokenHeaderKey2))
                {
                    token = Request.Headers.GetValues(Resources.TokenHeaderKey2).FirstOrDefault();
                }

                if (string.IsNullOrEmpty(token))
                {
                    throw new GeneralException(10101, "Token is missing!");
                }

                var tokenString = token.Decrypt();
                var tokenStringParts = tokenString.Split(new char[] { '|' });

                if (tokenStringParts.Length == 9)
                {
                    var sysKey = tokenStringParts[0];
                    email = tokenStringParts[3].Trim().ToLower();

                    if (!sysKey.Equals(WebSystem.BackEndKey, StringComparison.CurrentCultureIgnoreCase))
                    {
                        throw new GeneralException(10102, "Token is invalid! Token belongs to another system.");
                    }
                }
            }
            catch (GeneralException exception)
            {
                return new HttpActionResult<GenericResponse<string>>(
                    HttpStatusCode.Unauthorized,
                    new GenericResponse<string>
                    {
                        Error = new Error
                        {
                            ResponseCode = HttpStatusCode.Unauthorized,
                            Message = exception.Message
                        }
                    }
                );
            }
            catch (Exception)
            {
                return new HttpActionResult<GenericResponse<string>>(
                    HttpStatusCode.Unauthorized,
                    new GenericResponse<string>
                    {
                        Error = new Error
                        {
                            Code = 10105,
                            ResponseCode = HttpStatusCode.Unauthorized,
                            Message = "Token is invalid!"
                        }
                    }
                );
            }

            try
            {
                var newToken = dataServiceAuth.GetToken(email);

                return new HttpActionResult<GenericResponse<string>>(
                    HttpStatusCode.OK,
                    new GenericResponse<string>
                    {
                        Result = newToken.String
                    }
                );
            }
            catch (GeneralException exception)
            {
                return new HttpActionResult<GenericResponse<string>>(
                    HttpStatusCode.Unauthorized,
                    new GenericResponse<string>
                    {
                        Error = new Error
                        {
                            Code = exception.Code,
                            ResponseCode = HttpStatusCode.Unauthorized,
                            Message = exception.AllMessages()
                        }
                    }
                );
            }
            catch (Exception exception)
            {
                return new HttpActionResult<GenericResponse<string>>(
                    HttpStatusCode.InternalServerError,
                    new GenericResponse<string>
                    {
                        Error = new Error
                        {
                            Code = 10199,
                            ResponseCode = HttpStatusCode.InternalServerError,
                            Message = exception.AllMessages()
                        }
                    }
                );
            }
        }
    }
}