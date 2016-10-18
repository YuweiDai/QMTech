using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Core.Domain.Identity
{
    public enum PasswordFormat
    {
        Clear = 0,
        Hashed = 1,
        Encrypted = 2
    }
}
