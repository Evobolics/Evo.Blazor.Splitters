namespace Evo.Models.Blazor
{
    public class EventSinkArgs<TObject, TData>
    {
        public EventSinkArgs()
        {

        }
        public TData Data { get; set; }

        public TObject Object { get; set; }
    }
}
