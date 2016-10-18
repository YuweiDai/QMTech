using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMTech.Core.Domain.Messages;
using QMTech.Core.Caching;
using QMTech.Services.Events;
using QMTech.Core.Data;
using QMTech.Core.Domain.Catalog;

namespace QMTech.Services.Messages
{
    public class MessageTemplateService : IMessageTemplateService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// </remarks>
        private const string MESSAGETEMPLATES_ALL_KEY = "Qm.messagetemplate.all";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : template name
        /// </remarks>
        private const string MESSAGETEMPLATES_BY_NAME_KEY = "Qm.messagetemplate.name-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string MESSAGETEMPLATES_PATTERN_KEY = "Qm.messagetemplate.";

        #endregion

        #region Fields

        private readonly IRepository<MessageTemplate> _messageTemplateRepository;
        //private readonly IRepository<StoreMapping> _storeMappingRepository;
        //private readonly IStoreMappingService _storeMappingService;
        private readonly CatalogSettings _catalogSettings;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
     
        /// <param name="messageTemplateRepository">Message template repository</param>
        /// <param name="catalogSettings">Catalog settings</param>
        /// <param name="eventPublisher">Event published</param>
        public MessageTemplateService(ICacheManager cacheManager,
          
                        IRepository<MessageTemplate> messageTemplateRepository,
            CatalogSettings catalogSettings,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._messageTemplateRepository = messageTemplateRepository;
            this._catalogSettings = catalogSettings;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region 方法
        public MessageTemplate CopyMessageTemplate(MessageTemplate messageTemplate)
        {
            throw new NotImplementedException();
        }

        public void DeleteMessageTemplate(MessageTemplate messageTemplate)
        {
            throw new NotImplementedException();
        }

        public IList<MessageTemplate> GetAllMessageTemplates()
        {
            throw new NotImplementedException();
        }

        public MessageTemplate GetMessageTemplateById(int messageTemplateId)
        {
            throw new NotImplementedException();
        }

        public MessageTemplate GetMessageTemplateByName(string messageTemplateName )
        {
            if (string.IsNullOrWhiteSpace(messageTemplateName))
                throw new ArgumentException("messageTemplateName");

            string key = string.Format(MESSAGETEMPLATES_BY_NAME_KEY, messageTemplateName);
            return _cacheManager.Get(key, () => {
                var query = _messageTemplateRepository.Table;
                query = query.Where(t => t.Name == messageTemplateName);
                query = query.OrderBy(t => t.Id);
                query = query.OrderBy(t => t.Id);
                var templates = query.ToList();

                //TODO:Store mapping

                return templates.FirstOrDefault();
            });
        }

        public void InsertMessageTemplate(MessageTemplate messageTemplate)
        {
            throw new NotImplementedException();
        }

        public void UpdateMessageTemplate(MessageTemplate messageTemplate)
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
