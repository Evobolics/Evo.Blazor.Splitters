using Evo.Controls.Blazor;
using Evo.Models.Blazor;

namespace Evo.Services.Blazor
{
    public class SplitterPaneService : SplitterPaneService_I
    {
        

        public bool IsLastPane(EvoSplitterPane virtualComponent)
        {
            var parent = virtualComponent.Parent;

            if (parent == null) throw new System.Exception("IsLastPane should not be called without a parent being set.");

            for (int i = 0; i < parent.RegisteredPanes.Count; i++)
            {
                var pane = parent.RegisteredPanes[i];

                if (object.ReferenceEquals(pane, virtualComponent))
                {
                    return (i + 1) >= parent.RegisteredPanes.Count;
                }
            }

            throw new System.Exception("IsLastPane - could not find pane.");
        }
    }
}
