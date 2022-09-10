using Evo.Controls.Blazor;
using Evo.Models.Blazor;

namespace Evo.Services.Blazor
{
    public interface ISplitterPaneService
    {
        
        bool IsLastPane(EvoSplitterPane virtualComponent);
    }
}
