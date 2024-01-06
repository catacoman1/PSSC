using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    public abstract class CancellationEvent
    {

        public abstract TResult Match<TResult>(
        Func<CancellationSucceedEvent, TResult> whenCancellationSucceedEvent,
        Func<CancellationFailedEvent, TResult> whenCancellationFailedEvent);



        public class CancellationSucceedEvent : CancellationEvent
        {
            public string OrderId { get; }

            public CancellationSucceedEvent(string orderId)
            {
                OrderId = orderId;
            }

            public override TResult Match<TResult>(
                Func<CancellationSucceedEvent, TResult> whenCancellationSucceedEvent,
                Func<CancellationFailedEvent, TResult> whenCancellationFailedEvent)
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

            public override TResult Match<TResult>(
                Func<CancellationSucceedEvent, TResult> whenCancellationSucceedEvent,
                Func<CancellationFailedEvent, TResult> whenCancellationFailedEvent)
            {
                return whenCancellationFailedEvent(this);
            }
        }
    }
}
