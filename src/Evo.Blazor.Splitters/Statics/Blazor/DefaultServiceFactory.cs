using Evo.Services.Blazor;

namespace Evo.Statics.Blazor
{
    public class DefaultServiceFactory
    {
        public static SplitterService_I GetSplitterService() => new SplitterService();

        public static SplitterPaneService_I GetSplitterPaneService() => new SplitterPaneService();
    }
}
