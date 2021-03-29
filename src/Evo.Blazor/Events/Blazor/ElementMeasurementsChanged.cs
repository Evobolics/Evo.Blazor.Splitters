using Evo.Blazor.Models;
using Evo.Models.Blazor;

namespace Evo.Events.Blazor
{
    public class ElementMeasurementsChanged
    {
        public Element Element { get; set; }

        public ElementMeasurements Measurements { get; set; }
    }
}
