using Evo.Models.Blazor;
using Microsoft.AspNetCore.Components;

namespace Evo.Blazor.Models
{
    public class Element
    {
        public ElementReference ElementReference { get; set; }

        /// <summary>
        /// Gets or sets the element's measurements.  This is kept seperate from the element itself so that 
        /// it can be populated witout having to map any additiona properties.
        /// </summary>
        public ElementMeasurements Measurements { get; set; }
    }
}
