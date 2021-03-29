using Evo.Delegates.Blazor;
using Evo.Models.Blazor;
using System;
using System.Threading.Tasks;

namespace Evo.Services.Blazor
{
    public interface MessageBus_I
    {
        void Subscribe<T>(Func<object, T, Task> callback);

        Task Publish<T>(object sender, T message);
        

        EventSink<TEvent> GetSink<TEvent>(long id);

        EventSinkHub<TEvent> Subscribe<TEvent>(Object_I idObject, Func<object, TEvent, Task> callback);

        EventSinkHub<TEvent> Subscribe<TEvent>(long id, Func<object, TEvent, Task> callback);
    }
}
