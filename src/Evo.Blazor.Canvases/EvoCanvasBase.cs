using Evo.Components.Blazor;
using Evo.Events.Blazor;
using Evo.Services.Blazor;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Evo.Blazor.Canvases
{
    public class EvoCanvasBase:ComponentBase
    {
        [CascadingParameter]
        public DivComponentBase_I Parent { get; set; }

        [Inject]
        public MessageBus_I MessageBus { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            MessageBus.Subscribe<ElementMeasurementsChanged>(Parent.Element, Element_MeasurementsUpdatedAsync);

            
        }

        private Task Element_MeasurementsUpdatedAsync(object sender, ElementMeasurementsChanged data)
        {
            

            var e = data.Measurements;

            Width = e.ClientWidth.ToString() + "px";
            Height = e.ClientHeight.ToString() + "px";

            this.StateHasChanged();

            return Task.CompletedTask;
        }

        [Parameter]
        public string Width { get; set; } = "0px";

        [Parameter]
        public string Height { get; set; } = "0px";
    }
}
