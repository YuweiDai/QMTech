using Microsoft.AspNet.Identity;
using QMTech.Core.Domain.Customers;
using QMTech.Core.Domain.Customers.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Services.Customers
{
    /// <summary>
    /// 重新继承IUserStore
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public interface IUserStore<TUser> : IUserStore<TUser, int>, IDisposable where TUser : class, IUser<int>
    {
    }
}
