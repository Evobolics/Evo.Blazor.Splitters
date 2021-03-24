using Evo.Controls.Blazor;

namespace Evo.Models.Blazor
{
    public class VirtualEvoSplitterPane
    {
        public VirtualEvoSplitter Parent { get; internal set; }

        public EvoSplitterPaneBase Component { get; internal set; }
        public decimal Percentage { get; internal set; } = 50M;
        public int Index { get; internal set; }
    }
}
