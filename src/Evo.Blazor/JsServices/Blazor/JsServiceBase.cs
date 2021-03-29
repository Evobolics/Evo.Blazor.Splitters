using Evo.Blazor.Models;
using Evo.Models.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Evo.JsServices.Blazor
{
    public abstract class JsServiceBase : IAsyncDisposable
    {
        public JsServiceBase(IJSRuntime runtime, string relativePath)
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

            ModuleTask = runtime.ImportModule($"./_content/Evo.Blazor/{relativePath}");
        }

        public Lazy<Task<IJSObjectReference>> ModuleTask { get; }

        public async ValueTask DisposeAsync()
        {
            if (ModuleTask.IsValueCreated)
            {
                var module = await ModuleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
