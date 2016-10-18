using QMTech.Core;
using QMTech.Core.Domain.Messages;
using QMTech.Core.Events;

namespace QMTech.Services.Events
{
    public static class EventPublisherExtensions
    {
        public static void EntityInserted<T>(this IEventPublisher eventPublisher, T entity) where T : BaseEntity
        {
            eventPublisher.Publish(new EntityInserted<T>(entity));
        }

        public static void EntityUpdated<T>(this IEventPublisher eventPublisher, T entity) where T : BaseEntity
        {
            eventPublisher.Publish(new EntityUpdated<T>(entity));
        }

        public static void EntityDeleted<T>(this IEventPublisher eventPublisher, T entity) where T : BaseEntity
        {
            eventPublisher.Publish(new EntityDeleted<T>(entity));
        }

        public static void EntityTokensAdded<T, U>(this IEventPublisher eventPublisher, T entity, System.Collections.Generic.IList<U> tokens) where T : BaseEntity
        {
            eventPublisher.Publish(new EntityTokensAddedEvent<T, U>(entity, tokens));
        }

        public static void MessageTokensAdded<U>(this IEventPublisher eventPublisher, MessageTemplate message, System.Collections.Generic.IList<U> tokens)
        {
            eventPublisher.Publish(new MessageTokensAddedEvent<U>(message, tokens));
        }
    }
}