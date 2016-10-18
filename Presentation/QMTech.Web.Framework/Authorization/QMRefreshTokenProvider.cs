using Microsoft.Owin.Security.Infrastructure;
using QMTech.Core;
using QMTech.Core.Domain.Authentication;
using QMTech.Core.Infrastructure;
using QMTech.Services.Authentication;
using System;
using System.Threading.Tasks;

namespace QMTech.Web.Framework.Security.Authorization
{
    public class QMRefreshTokenProvider : IAuthenticationTokenProvider
    {
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var refreshTokenHashId = Guid.NewGuid().ToString("n");

            var token = new RefreshToken()
            {
                HashId=refreshTokenHashId.GetHash(),
                Subject = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
            };

            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

            token.ProtectedTicket = context.SerializeTicket();

            var refreshTokenService = EngineContext.Current.Resolve<IRefreshTokenService>();

            var result = await refreshTokenService.InsertRefreshTokenAsync(token);
          
            if (result)
            {
                context.SetToken(refreshTokenHashId);
            }
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            string refreshTokenHashId = context.Token.GetHash();

            var refreshTokenService = EngineContext.Current.Resolve<IRefreshTokenService>();

            var refreshToken = await refreshTokenService.FindRefreshTokenAsync(refreshTokenHashId);

            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                var result = refreshTokenService.DeleteRefreshToken(refreshTokenHashId);
            }
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

    }
}