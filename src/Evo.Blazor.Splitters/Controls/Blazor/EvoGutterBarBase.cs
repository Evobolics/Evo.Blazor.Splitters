using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace Evo.Controls.Blazor
{
    public class EvoGutterBarBase: ComponentBase
    {
        [CascadingParameter]
        public EvoSplitter Splitter { get; set; }

        [CascadingParameter]
        public EvoSplitterPane SplitterPane { get; set; }

        [Parameter]
        public string Class { get; set; }

        public string Style
        {
            get
            {
                if (Splitter.Orientation == SplitOrientation.Horizontal)
                {
                    return $"height:{Splitter.GutterSize}px;cursor:row-resize;";
                }
                else
                {
                    return $"width:{Splitter.GutterSize}px;cursor:col-resize;height:100%;float:left;";
                }
            }
        }


        protected override Task OnInitializedAsync()
        {
            var task = base.OnInitializedAsync();

            Class = Splitter.GutterBarClass;

            return task;
        }

        
        

        public async Task OnMouseDown(MouseEventArgs args)
        {
            await Splitter.ChangeSlidingStateAsync(true, args.ScreenX, args.ScreenY);
        }

        public async Task OnMouseUp(MouseEventArgs args)
        {
            await Splitter.ResizePanesAsync(args);

            await Splitter.ChangeSlidingStateAsync(false);
        }


        public async Task OnMouseOut(MouseEventArgs args)
        {
            await Splitter.ResizePanesAsync(args);
        }

        public async Task OnMouseMove(MouseEventArgs args)
        {
            await Splitter.ResizePanesAsync(args);
        }

        

        
    }
}
