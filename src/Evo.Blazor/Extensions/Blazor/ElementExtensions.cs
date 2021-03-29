using Evo.Models.Blazor;
using System.Threading.Tasks;

namespace Evo.Blazor.Models
{
    public static class ElementExtensions
    {
        public static Task RefreshMeasurementsAsync(this Element element)
        {
            return element.Service.RefreshMeasurementsAsync(element);
        }

        public static Task<ResizeObserver> ObserveResizeAsync(this Element element)
        {
            return element.Service.ObserveResizeAsync(element);
        }

        public static Task UpdateMeasurementsAsync(this Element element, ElementMeasurements newValue)
        {
            return element.Service.UpdateMeasurementsAsync(element, newValue);
        }
    }
}
