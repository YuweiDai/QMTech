using Microsoft.AspNet.Identity;
using QMTech.Core.Domain.Customers;
using System;

namespace QMTech.Services.Customers
{
    //
    // 摘要:
    //     Stores a user's password hash
    //
    // 类型参数:
    //   TUser:
    public interface IUserPasswordStore<TUser> : IUserPasswordStore<TUser, int>, IUserStore<TUser, int>, IDisposable where TUser : class, ICustomer
    {
    }
}
