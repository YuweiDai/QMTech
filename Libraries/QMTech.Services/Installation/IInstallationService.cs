using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Services.Installation
{
    public partial interface IInstallationService
    {
        void InstallData(string defaultUserEmail, string defaultUserPassword, bool installSampleData = true);
    }
}
