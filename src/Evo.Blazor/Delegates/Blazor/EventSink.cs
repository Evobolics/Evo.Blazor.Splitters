using System.Threading.Tasks;

namespace Evo.Delegates.Blazor
{
    public delegate Task EventSink<TEvent>(object sender, TEvent data);
}
