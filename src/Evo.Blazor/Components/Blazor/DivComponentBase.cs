using Evo.Blazor.Models;
using System.Threading.Tasks;

namespace Evo.Components.Blazor
{
    public class DivComponentBase<TService>: EvoComponentBase<TService>, DivComponentBase_I
    {

        protected override Task OnInitializedAsync()
        {
            Element = Factory.Create<Element>();

            return base.OnInitializedAsync();
        } 

        public Element Element { get; set; }
    }
}
