using QMTech.Core.Domain.Customers;

namespace QMTech.Services.Authentication.External
{
    public interface IWechatAppAuthorizer
    {
        Customer GetCusteomer(string jsCode);
    }
}
