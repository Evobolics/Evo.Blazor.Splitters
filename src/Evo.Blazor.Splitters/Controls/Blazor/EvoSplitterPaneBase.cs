using Evo.Blazor.Models;
using Evo.Components.Blazor;
using Evo.Models.Blazor;
using Evo.Services.Blazor;
using Evo.Statics.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;

namespace Evo.Controls.Blazor
{
    public class EvoSplitterPaneBase: EvoComponentBase<VirtualEvoSplitterPane, SplitterPaneService_I, DefaultServiceFactory>, IAsyncDisposable
    {
        

        public EvoSplitterPaneBase()
        {
            this.VirtualComponent.Component = this;
        }

        [Parameter]
        public ElementReference ElementReference { get; set; }

        [Parameter]
        public string Style { get; set; }

        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public int MinimumSizeInPixels { get; set; }

        [Parameter]
        public bool IsDraggable { get; set; } = false;


        public Element RootDivElement { get; set; } = new Element();

        [CascadingParameter]
        public EvoSplitter Splitter { get; set; }           /* This property is identified by type.
                                                             * 
                                                             * Per Microsoft, "Cascading values are bound to cascading parameters by type"
                                                             * 
                                                             * See: https://docs.microsoft.com/en-us/aspnet/core/blazor/components/cascading-values-and-parameters?view=aspnetcore-5.0
                                                             */

        /// <summary>
        /// Gets or sets the child content of the individual pane.    
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }    /*  NOTE: The property receiving the RenderFragment content 
                                                             *        must be named ChildContent by convention. 
                                                             *       
                                                             *       https://docs.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-5.0
                                                             */


                                                             /*  NOTE: If multiple render fragments are going to be used, then the 
                                                              *        render fragment being filled needs to be named within the 
                                                              *        child component.  
                                                              * 
                                                              * https://blazor-university.com/templating-components-with-renderfragements/
                                                              */

        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            // Register this pane with the splitter so that it knows it exists.
            await Splitter.Service.RegisterPaneAsync(Splitter.VirtualComponent, this);

            Splitter.OnSlidingStateChanged += Splitter_OnSlidingStateChanged;

            


        }

        private void Splitter_OnSlidingStateChanged(object sender, EventArgs e)
        {
            this.StateHasChanged();
        }

        public async ValueTask DisposeAsync()
        {
            await Splitter.Service.UnregisterPaneAsync(Splitter.VirtualComponent, this);
        }

        public bool IsLastPane()
        {
            return Service.IsLastPane(this.VirtualComponent);
        }

        

        public async Task OnMouseUp(MouseEventArgs args)
        {
            await Splitter.ChangeSlidingStateAsync(false);
        }

        public async Task OnMouseMove(MouseEventArgs args)
        {
            await Splitter.ResizePanesAsync(args);
        }

        public async Task OnMouseOut(MouseEventArgs args)
        {
            await Splitter.ResizePanesAsync(args);
        }

    }
}
