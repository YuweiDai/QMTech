using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Services.Authentication.External
{
    /// <summary>
    /// Open authentication status
    /// </summary>
    public enum OpenAuthenticationStatus
    {
        Unknown,
        Error,
        Authenticated,
        RequiresRedirect,
        AssociateOnLogon,
        AutoRegisteredEmailValidation,
        AutoRegisteredAdminApproval,
        AutoRegisteredStandard,
    }
}
