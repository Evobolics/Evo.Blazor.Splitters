using Evo.Controls.Blazor;
using Evo.Models.Blazor;
using System.Threading.Tasks;

namespace Evo.Services.Blazor
{
    public interface SplitterService_I
    {
        /// <summary>
        /// Registers the panes existance with the parent splitter. 
        /// </summary>
        /// <param name="pane">The pane to be registered.</param>
        Task RegisterPaneAsync(VirtualEvoSplitter splitter, EvoSplitterPaneBase pane);

        /// <summary>
        /// Unregisters the panes existance with the parent splitter. 
        /// </summary>
        /// <param name="pane">The pane to be registered.</param>
        Task UnregisterPaneAsync(VirtualEvoSplitter splitter, EvoSplitterPaneBase pane);
    }
}
