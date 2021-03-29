using Evo.Blazor.Models;
using Evo.JsServices.Blazor;
using Evo.Services.Blazor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Evo.Extensions
{
    public static class WebAssemblyHostBuilderExtensions
    {
        private static object _SyncRoot = new object();
        private static bool _Added = false;

        public static Task AddEvoBlazor(this WebAssemblyHostBuilder builder)
        {
            lock (_SyncRoot)
            {
                if (_Added)
                {
                    return Task.CompletedTask;
                }

                _Added = true;
            }

            builder.Services.AddScoped<GeneralJsService>();
            builder.Services.AddScoped<ElementJsService>();
            
            builder.Services.AddScoped<ElementService_I, ElementService>();
            builder.Services.AddScoped<EvoJavascriptComponentService>();
            builder.Services.AddTransient<Element>();
            builder.Services.AddScoped<MessageBus_I, MessageBus>();
            builder.Services.AddScoped<FactoryService_I, FactoryService>();



            return Task.CompletedTask;
        }
    }
}
