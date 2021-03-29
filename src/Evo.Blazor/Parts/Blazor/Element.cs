using Evo.Attributes.Blazor;
using Evo.Delegates.Blazor;
using Evo.Events.Blazor;
using Evo.Models.Blazor;
using Evo.Services.Blazor;
using Microsoft.AspNetCore.Components;

namespace Evo.Blazor.Models
{

    public class Element: Object_I, EventSource_I
    {
        /// <summary>
        /// Creates a new element.
        /// </summary>
        /// <param name="service">The service associated with the type.</param>
        public Element(ElementService_I service)
        {
            Service = service;
        }

        /// <summary>
        /// Gets or sets the underlying element reference.
        /// </summary>
        public ElementReference ElementReference { get; set; }

        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the element's measurements.  This is kept seperate from the element itself so that 
        /// it can be populated witout having to map any additiona properties.
        /// </summary>
        public ElementMeasurements Measurements { get; set; }

        /// <summary>
        /// Gets or sets the service associated with the element. 
        /// </summary>
        public ElementService_I Service { get; set; }

        [EventSource]
        public EventSink<ElementMeasurementsChanged> ElementMeasurementsChanged { get; set; }
        public ResizeObserver ResizeObserver { get; internal set; }
    }
}
