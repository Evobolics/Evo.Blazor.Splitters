using Evo.Blazor.Models;
using Evo.Models.Blazor;
using System.Threading.Tasks;

namespace Evo.Services.Blazor
{
    public interface ElementService_I
    {
        Task RefreshMeasurementsAsync(Element element);

        Task<ResizeObserver> ObserveResizeAsync(Element element);

        Task UpdateMeasurementsAsync(Element element, ElementMeasurements newValue);
    }
}
