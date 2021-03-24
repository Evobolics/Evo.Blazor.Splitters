using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Evo.Extensions
{
    public static class WebAssemblyHostBuilderExtensions
    {
        private static object _SyncRoot = new object();
        private static bool _Added = false;

        public async static Task AddBlazorSplitter(this WebAssemblyHostBuilder builder)
        {
            lock (_SyncRoot)
            {
                if (_Added)
                {
                    return;
                }

                _Added = true;
            }

            await builder.AddEvoBlazor();

            
        }
    }
}
