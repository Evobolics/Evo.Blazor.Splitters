using Evo.Delegates.Blazor;
using Evo.Models.Blazor;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evo.Services.Blazor
{
    public class MessageBus:MessageBus_I
    {
        // Really not needed yet as Blazor is not yet multi-threaded, but it can help later on down the line.
        private ConcurrentDictionary<RuntimeTypeHandle, List<Func<object, object, Task>>> _Subscriptions 
            = new ConcurrentDictionary<RuntimeTypeHandle, List<Func<object, object, Task>>>();

        private ConcurrentDictionary<long, ConcurrentDictionary<RuntimeTypeHandle, EventSinkHub>> _SubscriptionsById
            = new ConcurrentDictionary<long, ConcurrentDictionary<RuntimeTypeHandle, EventSinkHub>>();

        private System.Type _ElementSinkType;

        public MessageBus()
        {
            _ElementSinkType = typeof(EventSink<object>).GetGenericTypeDefinition();
        }

        /// <summary>
        /// Subcribe to an event
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        public void Subscribe<T>(Func<object, T, Task> callback)
        {
            var subscriptionType = typeof(T);

            if (!_Subscriptions.TryGetValue(subscriptionType.TypeHandle, out var subscriptions))
            {
                subscriptions = _Subscriptions.AddOrUpdate(
                    subscriptionType.TypeHandle, 
                    new List<Func<object, object, Task>>(),
                    (a, existing) => existing
                );
            }

            subscriptions.Add(async (sender, tEvent) =>
            {
                await callback(sender, (T)tEvent);
            });
        }

        public EventSinkHub<TEvent> Subscribe<TEvent>(Object_I idObject, Func<object, TEvent, Task> callback)
        {
            return Subscribe(idObject.Id, callback);
        }

        public EventSinkHub<TEvent> Subscribe<TEvent>(long id, Func<object, TEvent, Task> callback)
        {
            if (!_SubscriptionsById.TryGetValue(id, out var byEventType))
            {
                byEventType = _SubscriptionsById.AddOrUpdate(
                    id,
                    new ConcurrentDictionary<RuntimeTypeHandle, EventSinkHub>(),
                    (a, existing) => existing
                );
            }

            var eventType = typeof(TEvent);

            var eventSinkHub = byEventType.AddOrUpdate(
                eventType.TypeHandle,
                new EventSinkHub<TEvent>()
                {
                    EventSink = new EventSink<TEvent>(async (sender, data) =>
                    {
                        await callback(sender, data);
                    })
                },
                (a, existingHub) =>
                {
                    EventSinkHub<TEvent> hub = (EventSinkHub<TEvent>)existingHub;

                    hub.EventSink += new EventSink<TEvent>(async (sender, data) =>
                    {
                        await callback(sender, data);
                    });

                    return existingHub;
                });

            return (EventSinkHub<TEvent>)eventSinkHub;
            
        }

        /// <summary>
        /// Publish an event
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task Publish<T>(object sender, T message)
        {
            var messageType = typeof(T).TypeHandle;

            if (!_Subscriptions.TryGetValue(messageType, out var subscriptions))
            {
                return;
            }

            foreach (var subscription in subscriptions)
            {
                await Task.Run(async () => await subscription(sender, message));
            }
        }

        public EventSink<TEvent> GetSink<TEvent>(long id)
        {
            var hub = Subscribe<TEvent>(id, (a, b) =>
            {
                return Task.CompletedTask;
            });

            var outer = new EventSink<TEvent>(async (a, b) =>
            {
                await hub.EventSink(a, b);
            });

            return outer;
        }

        
    }
}
