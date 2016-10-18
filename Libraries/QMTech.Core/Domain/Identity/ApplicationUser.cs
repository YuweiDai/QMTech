using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Core.Domain.Identity
{
    public class ApplicationUser : IdentityUser
    {

        public string LastIpAddress { get; set; }

        public DateTime LastActivityDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public string AdminComment { get; set; }

        public bool IsSystemAccount { get; set; }

        public string SystemName { get; set; }
    }
}
