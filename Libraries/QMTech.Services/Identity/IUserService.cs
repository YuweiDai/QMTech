using Microsoft.AspNet.Identity;
using QMTech.Core.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Services.Identity
{
    public interface IUserService
    {
        ApplicationUser GetUserByEmail(string email);

        IdentityResult InsertUser(ApplicationUser user);
    }
}
