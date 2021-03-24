using Evo.Models.Blazor;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Evo.Services.Blazor
{
    public class GeneralJsService : IAsyncDisposable
    {
        private IJSRuntime _JSRuntime { get; set; }

        
        

        private readonly Lazy<Task<IJSObjectReference>> _moduleTask;


        public GeneralJsService(IJSRuntime runtime)
        {
            _JSRuntime = runtime;

            _moduleTask = runtime.ImportModule("./_content/Evo.Blazor/generalJsInterop.js");
        }

        public async Task ConsoleLog(string message)
        {
            await _JSRuntime.InvokeVoidAsync("console.log", message);
        }

       

        public async Task RemoveAllSelections()
        {
            var module = await _moduleTask.Value;

            await module.InvokeVoidAsync("removeAllSelections");
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
