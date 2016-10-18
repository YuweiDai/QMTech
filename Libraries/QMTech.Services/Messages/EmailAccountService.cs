using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMTech.Core.Domain.Messages;
using QMTech.Core.Data;
using QMTech.Services.Events;

namespace QMTech.Services.Messages
{
    public partial class EmailAccountService : IEmailAccountService
    {
        private readonly IRepository<EmailAccount> _emailAccountRepository;
        private readonly IEventPublisher _eventPublisher;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="emailAccountRepository">Email account repository</param>
        /// <param name="eventPublisher">Event published</param>
        public EmailAccountService(IRepository<EmailAccount> emailAccountRepository,
            IEventPublisher eventPublisher)
        {
            this._emailAccountRepository = emailAccountRepository;
            this._eventPublisher = eventPublisher;
        }

        #region 方法
        public void DeleteEmailAccount(EmailAccount emailAccount)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an email account by identifier
        /// </summary>
        /// <param name="emailAccountId">The email account identifier</param>
        /// <returns>Email account</returns>
        public virtual EmailAccount GetEmailAccountById(int emailAccountId)
        {
            if (emailAccountId == 0)
                return null;

            return _emailAccountRepository.GetById(emailAccountId);
        }

        /// <summary>
        /// Gets all email accounts
        /// </summary>
        /// <returns>Email accounts list</returns>
        public virtual IList<EmailAccount> GetAllEmailAccounts()
        {
            var query = from ea in _emailAccountRepository.Table
                        orderby ea.Id
                        select ea;
            var emailAccounts = query.ToList();
            return emailAccounts;
        }

        public void InsertEmailAccount(EmailAccount emailAccount)
        {
            throw new NotImplementedException();
        }

        public void UpdateEmailAccount(EmailAccount emailAccount)
        {
            throw new NotImplementedException();
        } 

        #endregion
    }
}
