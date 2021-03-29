using Evo.Blazor.Models;
using Evo.JsServices.Blazor;
using Evo.Models.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Evo.Services.Blazor
{
    public class ElementJsService : JsServiceBase
    {
        public ElementJsService(IJSRuntime runtime)
            : base(runtime, "elementJsInterop.js")
        {
            
        }

        /// <summary>
        /// Adds a class to particular DOM element.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="classname"></param>
        /// <returns></returns>
        public async ValueTask<bool> AddClass(Element element, string classname)
        {
            var elementRef = element.ElementReference;

            var module = await ModuleTask.Value;

            // The name is constructed as const to prevent 
            return await module.InvokeAsync<bool>("addClass", elementRef, classname);
        }

        /// <summary>
        /// Removes a class from a particular DOM element.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="classname"></param>
        /// <returns></returns>
        public async ValueTask<bool> RemoveClass(Element element, string classname)
        {
            var elementRef = element.ElementReference;

            var module = await ModuleTask.Value;

            return await module.InvokeAsync<bool>("removeClass", elementRef, classname);
        }

        /// <summary>
        /// Gets an element measurements
        /// </summary>
        /// <param name="element"></param>
        /// <returns>The elements measurements</returns>
        public async ValueTask<ElementMeasurements> GetElementMeasurements(Element element)
        {
            var elementRef = element.ElementReference;

            var module = await ModuleTask.Value;

            if (default(ElementReference).Equals(elementRef))
            {
                throw new Exception("ElementRef is null");
            }

            return await module.InvokeAsync<ElementMeasurements>("getElementMeasurements", elementRef);
        }

        public async Task ObserveResizeAsync(Element element, object observerReference)
        {
            var module = await ModuleTask.Value;

            await module.InvokeVoidAsync("create", element.Id,
                 observerReference, element.ElementReference);
        }
    }
}
