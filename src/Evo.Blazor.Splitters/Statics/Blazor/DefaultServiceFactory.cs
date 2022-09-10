using Evo.Services.Blazor;

namespace Evo.Statics.Blazor
{
    public class DefaultServiceFactory
    {
        public static ISplitterService GetSplitterService() => new SplitterService();

        public static ISplitterPaneService GetSplitterPaneService() => new SplitterPaneService();
    }
}
