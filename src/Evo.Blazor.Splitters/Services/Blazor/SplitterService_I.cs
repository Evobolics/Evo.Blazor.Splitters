using Evo.Controls.Blazor;
using Evo.Models.Blazor;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace Evo.Services.Blazor
{
    public interface SplitterService_I
    {
        Task ChangeSlidingStateAsync(EvoSplitterBase splitter, bool newState, double screenX = 0, double screenY = 0);

        Task<bool> ResizePanes(EvoSplitterBase splitter, MouseEventArgs args);

        /// <summary>
        /// Registers the panes existance with the parent splitter. 
        /// </summary>
        /// <param name="pane">The pane to be registered.</param>
        Task RegisterPaneAsync(EvoSplitterBase splitter, EvoSplitterPane pane);

        /// <summary>
        /// Unregisters the panes existance with the parent splitter. 
        /// </summary>
        /// <param name="pane">The pane to be registered.</param>
        Task UnregisterPaneAsync(EvoSplitterBase splitter, EvoSplitterPane pane);

        void UpdateBoxHeight(EvoSplitterBase splitter, EvoSplitterPane pane0, EvoSplitterPane pane1, ElementRectangle boundingBox);

        ElementRectangle CalculateHorizontalBoundingBox(ElementRectangle pane0Rect, ElementRectangle pane1Rect);

        void UpdateBoxWidth(EvoSplitterBase splitter, EvoSplitterPane pane0, EvoSplitterPane pane1, ElementRectangle boundingBox);

        ElementRectangle CalculateVerticalBoundingBox(ElementRectangle pane0Rect, ElementRectangle pane1Rect);
    }
}
