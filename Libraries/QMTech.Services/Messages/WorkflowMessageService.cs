using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMTech.Core.Domain.Customers;
using QMTech.Core.Domain.Messages;
using QMTech.Services.Events;

namespace QMTech.Services.Messages
{
    public class WorkflowMessageService : IWorkflowMessageService
    {
        #region Fields

        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly ITokenizer _tokenizer;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IMessageTokenProvider _messageTokenProvider;
        //private readonly IStoreService _storeService;
        //private readonly IStoreContext _storeContext;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        public WorkflowMessageService(IMessageTemplateService messageTemplateService,
            IQueuedEmailService queuedEmailService,
            ITokenizer tokenizer,
            IEmailAccountService emailAccountService,
            IMessageTokenProvider messageTokenProvider,
            //IStoreService storeService,
            //IStoreContext storeContext,
            EmailAccountSettings emailAccountSettings,
            IEventPublisher eventPublisher)
        {
            this._messageTemplateService = messageTemplateService;
            this._queuedEmailService = queuedEmailService;
            this._tokenizer = tokenizer;
            this._emailAccountService = emailAccountService;
            this._messageTokenProvider = messageTokenProvider;
            //this._storeService = storeService;
            //this._storeContext = storeContext;
            this._emailAccountSettings = emailAccountSettings;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Utilites

        protected virtual int SendNotification(MessageTemplate messageTemplate,
    EmailAccount emailAccount,   IEnumerable<Token> tokens,
    string toEmailAddress, string toName,
    string attachmentFilePath = null, string attachmentFileName = null,
    string replyToEmailAddress = null, string replyToName = null)
        {
            //retrieve localized message template data
            var bcc = messageTemplate.BccEmailAddresses;
            var subject = messageTemplate.Subject;
            var body = messageTemplate.Body;

            //Replace subject and body tokens 
            var subjectReplaced = _tokenizer.Replace(subject, tokens, false);
            var bodyReplaced = _tokenizer.Replace(body, tokens, true);

            var email = new QueuedEmail
            {
                Priority = QueuedEmailPriority.High,
                From = emailAccount.Email,
                FromName = emailAccount.DisplayName,
                To = toEmailAddress,
                ToName = toName,
                ReplyTo = replyToEmailAddress,
                ReplyToName = replyToName,
                CC = string.Empty,
                Bcc = bcc,
                Subject = subjectReplaced,
                Body = bodyReplaced,
                AttachmentFilePath = attachmentFilePath,
                AttachmentFileName = attachmentFileName,
                AttachedDownloadId = messageTemplate.AttachedDownloadId,
                CreatedOnUtc = DateTime.UtcNow,
                EmailAccountId = emailAccount.Id
            };

            _queuedEmailService.InsertQueuedEmail(email);
            return email.Id;
        }

        protected virtual MessageTemplate GetActiveMessageTemplate(string messageTemplateName)
        {
            var messageTemplate = _messageTemplateService.GetMessageTemplateByName(messageTemplateName);

            //no template found
            if (messageTemplate == null)
                return null;

            //ensure it's active
            var isActive = messageTemplate.IsActive;
            if (!isActive)
                return null;

            return messageTemplate;
        }

        protected virtual EmailAccount GetEmailAccountOfMessageTemplate(MessageTemplate messageTemplate)
        {
            var emailAccount = _emailAccountService.GetEmailAccountById(messageTemplate.EmailAccountId);
            if (emailAccount == null)
                emailAccount = _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
            if (emailAccount == null)
                emailAccount = _emailAccountService.GetAllEmailAccounts().FirstOrDefault();
            return emailAccount;
        }
        #endregion

        public int SendCustomerEmailValidationMessage(Customer custome)
        {
            throw new NotImplementedException();
        }

        public int SendCustomerPasswordRecoveryMessage(Customer custome)
        {
            throw new NotImplementedException();
        }

        public int SendCustomerRegisteredNotificationMessage(Customer custome)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends a welcome message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <returns>Queued email identifier</returns>
        public int SendCustomerWelcomeMessage(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var messageTemplate = GetActiveMessageTemplate("Customer.WelcomeMessage");
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate);

            //tokens
            var tokens = new List<Token>();
            _messageTokenProvider.AddStoreTokens(tokens, emailAccount);
            _messageTokenProvider.AddCustomerTokens(tokens, customer);

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

            var toEmail = customer.Email;
            var toName = customer.UserName; //原来是获取全称
            return SendNotification(messageTemplate, emailAccount,
                 tokens,
                toEmail, toName);
        }

    }
}
