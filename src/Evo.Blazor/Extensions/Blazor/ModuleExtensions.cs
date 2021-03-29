using System;
using System.Threading.Tasks;

namespace Microsoft.JSInterop
{
    public static class ModuleExtensions
    {
        public static Lazy<Task<IJSObjectReference>> ImportModule(this IJSRuntime jsRuntime, string modulePath)
        {
            return new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", modulePath).AsTask());
        }
    }
}
