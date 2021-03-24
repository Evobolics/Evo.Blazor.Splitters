using Evo.Blazor.Models;
using Evo.Models.Blazor;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Evo.Services.Blazor
{
    public class ElementJsService : IAsyncDisposable
    {
        

        
        private const string AddClassCommand = "addClass";
        private const string RemoveClassCommand = "removeClass";
        private const string GetElementStateCommand = "getElementMeasurements";

        private readonly Lazy<Task<IJSObjectReference>> _moduleTask;


        public ElementJsService(IJSRuntime runtime)
        {
            // Explanation of the _content/{PackageId}/{Filename} path.

            // a)  _content -   this path part is the root location where all consumed component libraries’ resources are placed

            // b) {PackageId} - the Package Id of the binary that contains the resources.
            //                  This is the name you see entered in the Package id input
            //                  when you right-click your class library, select Properties,
            //                  and select the Package tab. If you installed the library
            //                  via NuGet, it is the name of the package you installed.
            // 
            // c) {FileName} -  the  the name of any resource within the component library’s
            //                  wwwroot folder. The resource can be directly within that folder,
            //                  or the path can identify a resource within any level of sub-folders,
            //                  such as /_content/BlazorUniversity.ConsumedLibrary/scripts/HelloWorld.js

            _moduleTask = runtime.ImportModule("./_content/Evo.Blazor/elementJsInterop.js");
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

            var module = await _moduleTask.Value;

            // The name is constructed as const to prevent 
            return await module.InvokeAsync<bool>(AddClassCommand, elementRef, classname);
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

            var module = await _moduleTask.Value;

            return await module.InvokeAsync<bool>(RemoveClassCommand, elementRef, classname);
        }

        /// <summary>
        /// Gets an element measurements
        /// </summary>
        /// <param name="element"></param>
        /// <returns>The elements measurements</returns>
        public async ValueTask<ElementMeasurements> GetElementMeasurements(Element element)
        {
            var elementRef = element.ElementReference;

            var module = await _moduleTask.Value;

            return await module.InvokeAsync<ElementMeasurements>(GetElementStateCommand, elementRef);
        }

        public async ValueTask DisposeAsync()
        {
            if (_moduleTask.IsValueCreated)
            {
                var module = await _moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
