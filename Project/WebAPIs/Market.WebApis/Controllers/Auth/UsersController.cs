using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Http;
using Market.Authentication.Core.Entities;
using Market.Authentication.Core.Helpers;
using Market.Authentication.Core.Objects.Exceptions;
using Market.Authentication.Core.Objects.Requests;
using Market.Authentication.Core.Objects.Responses.Common;
using Market.Authentication.DataAccess.EF.Interfaces;
using Market.WebApis.Filters.Authorization;

namespace Market.WebApis.Controllers.Auth
{
    [RoutePrefix("api/auth/users")]
    [KeyAuthorization]
    public class UsersController : ApiController
    {
        private readonly IDataServiceAUTH dataServiceAuth;

        public UsersController(IDataServiceAUTH dataServiceAuth)
        {
            this.dataServiceAuth = dataServiceAuth;
        }

        [Route("sign-up")]
        [HttpPost]
        public IHttpActionResult SignUp(RegisterUserRequest request)
        {
            try
            {
                var root = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                var user = this.dataServiceAuth.RegisterUser(request);

                var thread = new Thread(delegate () { user.SendVerificationEmail(root); });
                thread.Start();

                return new HttpActionResult<GenericResponse<UserEntity>>(
                    HttpStatusCode.OK,
                    new GenericResponse<UserEntity>
                    {
                        Result = user
                    }
                );
            }
            catch (GeneralException exception)

            {
                return new HttpActionResult<GenericResponse<UserEntity>>(
                    HttpStatusCode.OK,
                    new GenericResponse<UserEntity>
                    {
                        Error = new Error
                        {
                            Code = exception.Code,
                            ResponseCode = HttpStatusCode.OK,
                            Message = exception.Message
                        }
                    }
                );
            }
            catch (Exception exception)
            {
                return new HttpActionResult<GenericResponse<UserEntity>>(
                    HttpStatusCode.InternalServerError,
                    new GenericResponse<UserEntity>
                    {
                        Error = new Error
                        {
                            Code = 10699,
                            ResponseCode = HttpStatusCode.InternalServerError,
                            Message = exception.Message
                        }
                    }
                );
            }
        }
    }
}