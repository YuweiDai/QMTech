using System;
using System.Linq;
using System.Threading.Tasks;
using QMTech.Core.Domain.Authentication;
using QMTech.Core.Data;
using QMTech.Services.Events;

namespace QMTech.Services.Authentication
{
    public class RefreshTokenService : IRefreshTokenService
    {

        private readonly IRepository<RefreshToken> _refreshTokenRepository;
        private readonly IEventPublisher _eventPublisher;

        public RefreshTokenService(IRepository<RefreshToken> refreshTokenRepository, IEventPublisher eventPublisher)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _eventPublisher = eventPublisher;
        }

        public Task<bool> InsertRefreshTokenAsync(RefreshToken token)
        {
            if (token == null)
                throw new ArgumentNullException("token");

            _refreshTokenRepository.Insert(token);

            //event notification
            _eventPublisher.EntityInserted(token);

            return Task.FromResult(true);
        }

        public Task<bool> DeleteRefreshToken(RefreshToken refreshToken)
        {
            if (refreshToken == null)
                throw new ArgumentNullException("refreshToken");

            _refreshTokenRepository.Delete(refreshToken);

            return Task.FromResult(true);
        }

        public Task<bool> DeleteRefreshToken(string refreshTokenHashId)
        {
            if (refreshTokenHashId == null)
                throw new ArgumentNullException("refreshTokenHashId");

            var refreshToken = FindRefreshTokenAsync(refreshTokenHashId).Result;
            _refreshTokenRepository.Delete(refreshToken);
            return Task.FromResult(true);
        }

        public Task<RefreshToken> FindRefreshTokenAsync(string refreshTokenHashId)
        {
            if (refreshTokenHashId == null)
                throw new ArgumentNullException("refreshTokenHashId");

            var query = from rt in _refreshTokenRepository.Table
                        where rt.HashId == refreshTokenHashId
                        select rt;

            var refreshToken = query.ToList().FirstOrDefault();

            return Task.FromResult(refreshToken);
        }
    }
}
