using Evo.Delegates.Blazor;

namespace Evo.Models.Blazor
{
    public class EventSinkHub
    {
        //public Delegate Delegate { get; }

        //protected abstract Delegate Get_Deletate()
        //{

        //}
    }

    public class EventSinkHub<TEvent>: EventSinkHub
    {
        public EventSink<TEvent> EventSink { get; set; }

        //protected abstract Delegate Get_Deletate();
    }
}
