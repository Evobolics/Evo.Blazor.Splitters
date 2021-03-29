using Evo.Blazor.Models;
using Evo.Events.Blazor;
using Evo.JsServices.Blazor;
using Evo.Models.Blazor;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Evo.Services.Blazor
{
    public class ElementService: ElementService_I
    {
        private ElementJsService _elementJsService;
        
        public ElementService(ElementJsService elementJsService)
        {
            _elementJsService = elementJsService;
        }

        public async Task<ResizeObserver> ObserveResizeAsync(Element element)
        {
            if (element.ResizeObserver != null)
            {
                return element.ResizeObserver;
            }

            var observer = new ResizeObserver()
            {
                Element = element,
            };

            element.ResizeObserver = observer;

            var observerReference = DotNetObjectReference.Create(element.ResizeObserver);

            observer.Reference = observerReference;

            await _elementJsService.ObserveResizeAsync(element, observerReference);

            return observer;
        }

        public async Task RefreshMeasurementsAsync(Element element)
        {
            var measurements = await _elementJsService.GetElementMeasurements(element);

            await UpdateMeasurementsAsync(element, measurements);
        }

        public async Task UpdateMeasurementsAsync(Element element, ElementMeasurements newValue)
        {
            var oldMeasurements = element.Measurements;

            if (!AreDifferent(oldMeasurements, newValue)) return;
            
            element.Measurements = newValue;

            // No lookup is required.
            await element.ElementMeasurementsChanged(this, new ElementMeasurementsChanged()
            {
                Element = element,
                Measurements = newValue
            });
            
        }

        public bool AreDifferent(ElementMeasurements oldValue, ElementMeasurements newValue)
        {
            if (oldValue == null && newValue == null) return false;
            if (oldValue != null && newValue == null) return true;
            if (oldValue == null && newValue != null) return true;

            if (oldValue.BoundingClientRect.Bottom != newValue.BoundingClientRect.Bottom) return true;
            if (oldValue.BoundingClientRect.Height != newValue.BoundingClientRect.Height) return true;
            if (oldValue.BoundingClientRect.Left != newValue.BoundingClientRect.Left) return true;
            if (oldValue.BoundingClientRect.Right != newValue.BoundingClientRect.Right) return true;
            if (oldValue.BoundingClientRect.Top != newValue.BoundingClientRect.Top) return true;
            if (oldValue.BoundingClientRect.Width != newValue.BoundingClientRect.Width) return true;
            if (oldValue.BoundingClientRect.X != newValue.BoundingClientRect.X) return true;
            if (oldValue.BoundingClientRect.Y != newValue.BoundingClientRect.Y) return true;

            if (oldValue.ClientHeight != newValue.ClientHeight) return true;
            if (oldValue.ClientLeft != newValue.ClientLeft) return true;
            if (oldValue.ClientTop != newValue.ClientTop) return true;
            if (oldValue.ClientWidth != newValue.ClientWidth) return true;
            if (oldValue.OffsetHeight != newValue.OffsetHeight) return true;
            if (oldValue.OffsetLeft != newValue.OffsetLeft) return true;
            if (oldValue.OffsetTop != newValue.OffsetTop) return true;
            if (oldValue.OffsetWidth != newValue.OffsetWidth) return true;
            if (oldValue.ScrollHeight != newValue.ScrollHeight) return true;
            if (oldValue.ScrollLeft != newValue.ScrollLeft) return true;
            if (oldValue.ScrollTop != newValue.ScrollTop) return true;
            if (oldValue.ScrollWidth != newValue.ScrollWidth) return true;

            return false;
        }
    }
}
