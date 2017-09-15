using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http.Headers;
using Market.Authentication.Core.Entities;
using Market.Authentication.Core.Objects.Requests;
using Market.Authentication.Core.Objects.Responses.Common;
using Market.Authentication.Properties;

namespace Market.Authentication.Core.Helpers
{
    public static class Extensions
    {
        public static DbTransaction BeginTransaction(this DbContext context, IsolationLevel? isolationLevel = null)
        {
            if (context.Database.Connection.State != ConnectionState.Open)
                context.Database.Connection.Open();

            var transaction = isolationLevel == null 
                ? context.Database.Connection.BeginTransaction() 
                : context.Database.Connection.BeginTransaction(isolationLevel.Value);

            context.Database.UseTransaction(transaction);

            return transaction;
        }

        private static async Task<string> GetMyExternalIp()
        {
            try
            {
                using (var client = new HttpClient { BaseAddress = new Uri("https://api.ipify.org") })
                {
                    var response = await client.GetStringAsync("/"); //?format=json
                    return response;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem, Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem) where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }

        public static string AllMessages(this Exception exception)
        {
            var messages = exception.FromHierarchy(ex => ex.InnerException)
                .Select(ex => ex.Message
                    .Replace("\n", string.Empty)
                    .Replace("\r", string.Empty)
                    .Replace("\t", ", ").Trim()
                );

            return string.Join(" ", messages)
                .Replace("See the inner exception for details.", string.Empty)
                .Replace(".", ". ")
                .Replace("    ", " ")
                .Replace("   ", " ")
                .Replace("  ", " ")
                .Trim();
        }

        public static string GetClientIp(this HttpRequestMessage request)
        {
            const string httpContext = "MS_HttpContext";
            const string remoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
        
            if (request.Properties.ContainsKey(httpContext))
            {
                dynamic ctx = request.Properties[httpContext];

                if (ctx != null)
                {
                    var ip = ctx.Request.UserHostAddress;

                    if (ip == "::1" || ip == "127.0.0.1")
                    {
                        var json = default(string);

                        var worker = new Thread(() => {
                            json = GetMyExternalIp().Result;
                        });

                        worker.Start(); worker.Join(); while (worker.IsAlive) { }
                        worker = null;

                        ip = json ?? ip;
                    }

                    return ip;
                }
            }

            if (request.Properties.ContainsKey(remoteEndpointMessage))
            {
                dynamic remoteEndpoint = request.Properties[remoteEndpointMessage];

                if (remoteEndpoint != null)
                {
                    var ip = remoteEndpoint.Address;

                    if (ip == "::1" || ip == "127.0.0.1")
                    {
                        var json = default(string);

                        var worker = new Thread(() => {
                            json = GetMyExternalIp().Result;
                        });

                        worker.Start(); worker.Join(); while (worker.IsAlive) { }
                        worker = null;

                        ip = json ?? ip;
                    }

                    return ip;
                }
            }

            return null;
        }

        public static DateTime GetPreviousWeekDay(this DateTime currentDate, DayOfWeek dow, int weeks = 2)
        {
            int currentDay = (int)currentDate.DayOfWeek, gotoDay = (int)dow;
            return currentDate.AddDays((-7 * weeks)).AddDays(gotoDay - currentDay);
        }

        public static IList<string> GetSplittedString(this string value, char[] delimeter)
        {
            return value.Split(delimeter).Select(part => part.Trim()).Where(val => !string.IsNullOrEmpty(val)).ToList();
        }

        public static IList<T> GetSplittedEnums<T>(this string value, char[] delimeter) where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type.");
            }

            return value.GetSplittedString(delimeter).Select(d => (T)Enum.Parse(typeof(T), d, true)).ToList();
        }

        public static bool SendEmail(this EmailRequest request)
        {
            try
            {
                var mail = new MailMessage
                {
                    From = new MailAddress(Resources.FromEmail, Resources.FromName)
                };

                foreach (var toEmail in request.ToEmails)
                {
                    mail.To.Add(toEmail);
                }

                mail.Subject = request.Subject;
                mail.IsBodyHtml = true;
                mail.Body = request.Body;

                var smtpServer = new SmtpClient(Resources.SMTPServer)
                {
                    Port = 587,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(Resources.FromEmail, Resources.FromEmailPassword),
                    EnableSsl = true
                };

                smtpServer.Send(mail);

                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public static int GetAge(this DateTime birthDate)
        {
            if (birthDate.Equals(default(DateTime)))
            {
                return 0;
            }

            var reference = DateTime.Now;

            var age = reference.Year - birthDate.Year;

            if (reference < birthDate.AddYears(age))
            {
                age--;
            }

            return age;
        }

        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static string GetUniqueString(this string nothing)
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty).ToUpper();
        }

        public static string GenerateToken(this UserEntity user, string systemKey, DateTime create, DateTime expire)
        {
            var tokenString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}",
                (string.IsNullOrEmpty(systemKey) ? new string('0', 32) : systemKey.Trim()),
                GetUniqueString(string.Empty),
                user.Id, 
                user.Email, 
                user.Password,
                user.Role.Id,
                user.Role.Name,
                create.ToString("yyyy-MM-dd HH:mm:ss.fff"), 
                expire.ToString("yyyy-MM-dd HH:mm:ss.fff")
            );

            return tokenString.Encrypt();
        }

        public static string GetRole(this HttpRequestMessage request)
        {
            try
            {
                var token = string.Empty;

                if (request.Headers.Contains(Resources.TokenHeaderKey1))
                {
                    token = request.Headers.GetValues(Resources.TokenHeaderKey1).FirstOrDefault();
                }
                else if (request.Headers.Contains(Resources.TokenHeaderKey2))
                {
                    token = request.Headers.GetValues(Resources.TokenHeaderKey2).FirstOrDefault();
                }

                if (!string.IsNullOrEmpty(token))
                {
                    var tokenString = token.Decrypt();
                    var tokenStringParts = tokenString.Split(new char[] { '|' });

                    if (tokenStringParts.Length == 9)
                    {
                        return tokenStringParts[6].Trim();
                    }
                }
            }
            catch
            {
                // throw;
            }

            return default(string);
        }

        public static string GetEmail(this HttpRequestMessage request)
        {
            try
            {
                var token = string.Empty;

                if (request.Headers.Contains(Resources.TokenHeaderKey1))
                {
                    token = request.Headers.GetValues(Resources.TokenHeaderKey1).FirstOrDefault();
                }
                else if (request.Headers.Contains(Resources.TokenHeaderKey2))
                {
                    token = request.Headers.GetValues(Resources.TokenHeaderKey1).FirstOrDefault();
                }

                if (!string.IsNullOrEmpty(token))
                {
                    var tokenString = token.Decrypt();
                    var tokenStringParts = tokenString.Split(new char[] {'|'});

                    if (tokenStringParts.Length == 9)
                    {
                        return tokenStringParts[3].Trim().ToLower();
                    }
                }
            }
            catch
            {
                // throw;
            }

            return default(string);
        }

        public static GenericResponse<bool> IsValidToken(this string token, string backEndKey, IList<string> roles)
        {
            try
            {
                var tokenString = token.Decrypt();
                var tokenStringParts = tokenString.Split(new char[] {'|'});

                if (tokenStringParts.Length == 9)
                {
                    var beKey = tokenStringParts[0];
                    var uniqueString = tokenStringParts[1];
                    var userId = long.Parse(tokenStringParts[2]);
                    var email = tokenStringParts[3];
                    var password = tokenStringParts[4];
                    var roleId = long.Parse(tokenStringParts[5]);
                    var role = tokenStringParts[6];
                    var create = DateTime.Parse(tokenStringParts[7]);
                    var expire = DateTime.Parse(tokenStringParts[8]);

                    if (!beKey.Equals(backEndKey, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return new GenericResponse<bool>
                        {
                            Error = new Error
                            {
                                Code = 10102,
                                ResponseCode = HttpStatusCode.Unauthorized,
                                Message = "Token is invalid! Token belongs to another system."
                            }
                        };
                    }

                    if (DateTime.Now >= expire)
                    {
                        return new GenericResponse<bool>
                        {
                            Error = new Error
                            {
                                Code = 10103,
                                ResponseCode = HttpStatusCode.Unauthorized,
                                Message = "Token has expired!"
                            }
                        };
                    }

                    if (!roles.Contains("*") && !roles.Any(x => x.Equals(role, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        return new GenericResponse<bool>
                        {
                            Error = new Error
                            {
                                Code = 10104,
                                ResponseCode = HttpStatusCode.Unauthorized,
                                Message = string.Format("Token is valid, but, the user '{0}' with the role '{1}' is unauthorized to access the api!", email, role)
                            }
                        };
                    }

                    return new GenericResponse<bool>
                    {
                        Result = true
                    };
                }
            }
            catch (Exception)
            {

            }

            return new GenericResponse<bool>
            {
                Error = new Error
                {
                    Code = 10105,
                    ResponseCode = HttpStatusCode.Unauthorized,
                    Message = "Token is invalid!"
                }
            };
        }

        public static bool SendVerificationEmail(this UserEntity user, string root)
        {
            var url = string.Format("{0}/api/v1/user/verify/get?key={1}&code={2}", root, user.Key, user.Code);
            var email = new EmailRequest
            {
                Body = Templates.AccountVerificationEmail.Replace("{{{{AccountVerificationLink}}}}", url).Replace("{{{{EmailAddress}}}}", user.Email),
                Subject = string.Format(string.Format("InsureMe | Account verification - {0}", user.Email)),
                ToEmails = new List<string>
                {
                    "kanchi7880@gmail.com",
                    "gmpat4u@gmail.com"
                }
            };

            return email.SendEmail();
        }

        public static string GetAccountVerifiedPage(this UserEntity user)
        {
            var html = Templates.AccountVerifiedPage
                .Replace("{{{{FirstAndLastName}}}}", string.Format("{0} {1}", user.FirstName, user.LastName))
                .Replace("{{{{EmailAddress}}}}", user.Email);

            return html;
        }

        public static string GetAccountAlreadyVerifiedPage(this UserEntity user)
        {
            var html = Templates.AccountAlreadyVerifiedPage
                .Replace("{{{{FirstAndLastName}}}}", string.Format("{0} {1}", user.FirstName, user.LastName))
                .Replace("{{{{EmailAddress}}}}", user.Email);

            return html;
        }

        public static string GetAccountVerificationErrorPage(this Exception exception)
        {
            var html = Templates.AccountVerificationErrorPage
                .Replace("{{{{Error}}}}", exception.Message);

            return html;
        }
    }
}