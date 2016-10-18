using QMTech.Core.Domain.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Services.Authentication
{
    public interface IRefreshTokenService
    {
        Task<bool> DeleteRefreshToken(RefreshToken refreshToken);

        Task<bool> DeleteRefreshToken(string refreshTokenHashId);

        Task<RefreshToken> FindRefreshTokenAsync(string refreshTokenHashId);            

        Task<bool> InsertRefreshTokenAsync(RefreshToken token);
    }
}
