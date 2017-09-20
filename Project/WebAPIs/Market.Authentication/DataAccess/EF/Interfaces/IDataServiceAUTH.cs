using System;
using System.Collections.Generic;
using Market.Authentication.Core.Entities;
using Market.Authentication.Core.Objects.Requests;

namespace Market.Authentication.DataAccess.EF.Interfaces
{
    public interface IDataServiceAUTH : IDisposable
    {
        long Id { get; }
        long Instances { get; }

        UserEntity RegisterUser(RegisterUserRequest request);
    }
}