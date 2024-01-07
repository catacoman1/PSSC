using System;

namespace Exemple.Domain.Models
{
    public abstract class CancellationEvent
    {
        // The Match method now returns CancellationEvent instead of a generic TResult
        public abstract CancellationEvent Match(
            Func<CancellationSucceedEvent, CancellationEvent> whenCancellationSucceedEvent,
            Func<CancellationFailedEvent, CancellationEvent> whenCancellationFailedEvent);

        public class CancellationSucceedEvent : CancellationEvent
        {
            public string OrderId { get; }

            public CancellationSucceedEvent(string orderId)
            {
                OrderId = orderId;
            }

            public override CancellationEvent Match(
                Func<CancellationSucceedEvent, CancellationEvent> whenCancellationSucceedEvent,
                Func<CancellationFailedEvent, CancellationEvent> whenCancellationFailedEvent)
            {
                return whenCancellationSucceedEvent(this);
            }
        }

        public class CancellationFailedEvent : CancellationEvent
        {
            public string Error { get; }

            public CancellationFailedEvent(string error)
            {
                Error = error;
            }

            public override CancellationEvent Match(
                Func<CancellationSucceedEvent, CancellationEvent> whenCancellationSucceedEvent,
                Func<CancellationFailedEvent, CancellationEvent> whenCancellationFailedEvent)
            {
                return whenCancellationFailedEvent(this);
            }
        }
    }
}
