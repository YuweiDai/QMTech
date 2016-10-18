using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMTech.Core.Domain.Identity;
using Microsoft.AspNet.Identity;

namespace QMTech.Services.Identity
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }

        public ApplicationUser GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public IdentityResult InsertUser(ApplicationUser user)
        {
            var identityResult = _userManager.Create(user);

            return identityResult;
        }
    }
}
