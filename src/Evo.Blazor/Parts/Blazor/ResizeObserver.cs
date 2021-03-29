using Evo.Blazor.Models;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Evo.Models.Blazor
{
    public class ResizeObserver
    {
        public Element Element { get; set; }

        public DotNetObjectReference<ResizeObserver> Reference { get; internal set; }

        [JSInvokable("OnResizeObserved")]
        public async Task OnObservedChange(ElementMeasurements measurements)
        {
            await Element.UpdateMeasurementsAsync(measurements);

            Console.WriteLine($"OnObservedChange fired: {measurements != null}");
        }
    }
}
