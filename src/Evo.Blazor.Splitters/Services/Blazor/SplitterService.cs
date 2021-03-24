using Evo.Controls.Blazor;
using Evo.Models.Blazor;
using System;
using System.Threading.Tasks;

namespace Evo.Services.Blazor
{
    public class SplitterService: SplitterService_I
    {
        

        /// <summary>
        /// Registers the panes existance with the parent splitter. 
        /// </summary>
        /// <param name="pane">The pane to be registered.</param>
        public Task RegisterPaneAsync(VirtualEvoSplitter splitter, EvoSplitterPaneBase pane)
        {
            if (splitter == null)
            {
                throw new ArgumentNullException("splitter", "Splitter is null");
            }

            if (pane == null)
            {
                throw new ArgumentNullException("pane", "Pane is null");
            }

            lock (splitter.RegisteredPanes)
            {
                Console.WriteLine("Registering Pane...");

                pane.VirtualComponent.Parent = splitter;

                pane.VirtualComponent.Index = splitter.RegisteredPanes.Count;

                splitter.RegisteredPanes.Add(pane.VirtualComponent);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Unregisters the panes existance with the parent splitter. 
        /// </summary>
        /// <param name="pane">The pane to be registered.</param>
        public Task UnregisterPaneAsync(VirtualEvoSplitter splitter, EvoSplitterPaneBase pane)
        {
            lock (splitter.RegisteredPanes)
            {
                splitter.RegisteredPanes.Remove(pane.VirtualComponent);

                pane.VirtualComponent.Parent = null;
            }

            return Task.CompletedTask;
        }
    }
}
