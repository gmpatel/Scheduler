using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Market.Authentication.Core.Entities;
using Market.Authentication.Core.Helpers;
using Market.Authentication.Core.Objects.Exceptions;
using Market.Authentication.Core.Objects.Requests;
using Market.Authentication.DataAccess.EF.Interfaces;

namespace Market.Authentication.DataAccess.EF.Defaults
{
    public class DataServiceAUTH : IDataServiceAUTH
    {
        private readonly IDataContext dataContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<UserEntity> usersRepository;
        private readonly IRepository<RoleEntity> rolesRepository;
        private readonly IRepository<FamilyTypeEntity> familyTypesRepository;
        private readonly IRepository<StateEntity> statesRepository;
        private readonly IRepository<IncomeRangeEntity> incomeRangesRepository;
        private readonly IRepository<TokenEntity> tokensRepository;

        internal static class Constants
        {
        }

        private static long objectsCounter;

        static DataServiceAUTH()
        {
        }

        public DataServiceAUTH(IDataContext dataContext, IUnitOfWork unitOfWork,
            IRepository<UserEntity> usersRepository,
            IRepository<RoleEntity> rolesRepository,
            IRepository<FamilyTypeEntity> familyTypesRepository,
            IRepository<StateEntity> statesRepository,
            IRepository<IncomeRangeEntity> incomeRangesRepository,
            IRepository<TokenEntity> tokensRepository
        )
        {
            Id = ++objectsCounter;

            this.dataContext = dataContext;
            this.unitOfWork = unitOfWork;

            this.usersRepository = usersRepository;
            this.rolesRepository = rolesRepository;
            this.familyTypesRepository = familyTypesRepository;
            this.statesRepository = statesRepository;
            this.incomeRangesRepository = incomeRangesRepository;
            this.tokensRepository = tokensRepository;
        }

        public long Id { get; private set; }
        public long Instances => objectsCounter;

        public void Dispose()
        {
        }

        public UserEntity RegisterUser(RegisterUserRequest request)
        {
            try
            {
                if (!usersRepository.FindBy(x => x.Email.Equals(request.Email, StringComparison.CurrentCultureIgnoreCase)).Any())
                {
                    //var role = RolesRepository.FindBy(x => x.Name.Equals(request.Role.Trim(), StringComparison.CurrentCultureIgnoreCase) || x.Code.Equals(request.Role.Trim(), StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    //var familyType = FamilyTypesRepository.FindBy(x => x.Name.Equals(request.FamilyType.Trim(), StringComparison.CurrentCultureIgnoreCase) || x.Code.Equals(request.FamilyType.Trim(), StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    //var state = StatesRepository.FindBy(x => x.Name.Equals(request.State.Trim(), StringComparison.CurrentCultureIgnoreCase) || x.Code.Equals(request.State.Trim(), StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    //var incomeRange = IncomeRangesRepository.FindBy(x => x.Id.Equals(1)).First();

                    var role = rolesRepository.FindBy(x => x.Id.Equals(1)).First();
                    var familyType = familyTypesRepository.FindBy(x => x.Id.Equals(0)).First();
                    var state = statesRepository.FindBy(x => x.Id.Equals(0)).First();
                    var incomeRange = incomeRangesRepository.FindBy(x => x.Id.Equals(0)).First();

                    var dateTime = DateTime.UtcNow;

                    if (role != null)
                    {
                        if (familyType != null)
                        {
                            if (state != null)
                            {
                                if (request.Email.IsValidEmail())
                                {
                                    var user = usersRepository.Add(new UserEntity
                                    {
                                        FirstName = request.FirstName.Trim(),
                                        LastName = request.LastName.Trim(),
                                        BirthDate = request.BirthDate,
                                        FamilyTypeId = familyType.Id,
                                        FamilyType = familyType,
                                        StateId = state.Id,
                                        State = state,
                                        PostCode = request.PostCode?.Trim().ToUpper(),
                                        Mobile = request.Mobile?.Trim().ToUpper(),
                                        Email = request.Email.Trim().ToLower(),
                                        Password = request.Password.Trim().HashMD5(),
                                        Key = request.Email.Trim().HashMD5(),
                                        Code = string.Empty.GetUniqueString(),
                                        RoleId = role.Id,
                                        Enabled = true,
                                        Verified = false,
                                        IncomeRangeId = incomeRange.Id,
                                        IncomeRange = incomeRange,
                                        DateTimeCreated = dateTime,
                                        Role = role
                                    });

                                    unitOfWork.Save();

                                    return user;
                                }

                                throw new GeneralException(10607, string.Format("The email address '{0}' is invalid!", request.Email.Trim().ToLower()));
                            }

                            throw new GeneralException(10202, string.Format("State with the name or code '{0}' not found!", request.State.Trim()));
                        }

                        throw new GeneralException(10302, string.Format("Family type with the name or code '{0}' not found!", request.FamilyType));
                    }

                    throw new GeneralException(10502, string.Format("Role with the name or code '{0}' not found!", request.Role));
                }

                throw new GeneralException(10606, string.Format("User with the email address '{0}' is already exists!", request.Email.Trim().ToLower()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UserEntity SigninUser(LoginUserRequest request)
        {
            var user = usersRepository.FindBy(x => x.Email.Equals(request.Email.Trim().ToLower(), StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            
            if (user != null)
            {
                if (user.Enabled)
                {
                    if (user.Verified)
                    {
                        if (user.Password.Equals(request.Password.Trim().HashMD5(), StringComparison.CurrentCulture))
                        {
                            return user;
                        }

                        throw new GeneralException(10605, string.Format("The password you have provided for the user '{0}' is incorrect!", request.Email.Trim().ToLower()));
                    }

                    throw new GeneralException(10603, string.Format("User '{0}' is exists, but, the email address on the account has not been verified! Please check your email and click on the link provided in email to verify the account!", request.Email.Trim().ToLower()));
                }

                throw new GeneralException(10602, string.Format("User '{0}' is exists, but, the account may be terminated or blocked! Please contact us!", request.Email.Trim().ToLower()));
            }

            throw new GeneralException(10601, string.Format("User with the email address '{0}' does not exists!", request.Email.Trim().ToLower()));
        }

        public TokenEntity GetToken(string userEmail)
        {
            return GetToken(usersRepository.FindBy(x => x.Email.Equals(userEmail.Trim().ToLower(), StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
        }

        public TokenEntity GetToken(UserEntity user)
        {
            try
            {
                if (user != null && user.Id >= 0)
                {
                    if (user.Enabled)
                    {
                        if (user.Verified)
                        {
                            var dateTime = DateTime.UtcNow;
                            var token = tokensRepository.FindBy(x => x.UserId.Equals(user.Id) && x.DateTimeExpire > dateTime).OrderByDescending(x => x.DateTimeCreated).FirstOrDefault();

                            if (token == null)
                            {
                                var lifeSpan = double.Parse(Properties.Resources.TokenLifeSpanMinutes);
                                var expire = dateTime.AddMinutes(lifeSpan);
                                var tokenString = user.GenerateToken(Properties.Resources.BackEndKey, dateTime, expire);

                                var newToken = tokensRepository.Add(new TokenEntity
                                {
                                    String = tokenString,
                                    UserId = user.Id,
                                    DateTimeCreated = dateTime,
                                    DateTimeExpire = expire,
                                    User = user
                                });

                                this.unitOfWork.Save();

                                return newToken;
                            }

                            return token;
                        }

                        throw new GeneralException(10603, string.Format("User with the email address '{0}' has been marked as not verified!", user.Email.Trim().ToLower()));
                    }

                    throw new GeneralException(10602, string.Format("User with the email address '{0}' has been marked as disabled!", user.Email.Trim().ToLower()));
                }

                throw new GeneralException(10601, string.Format("User with the email address '{0}' does not exists!", user?.Email?.Trim().ToLower()));
            }
            catch (Exception exception)
            {
                throw;
            }
        }
    }
}