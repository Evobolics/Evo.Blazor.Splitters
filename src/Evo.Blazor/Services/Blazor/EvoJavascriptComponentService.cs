using Evo.Blazor.Models;
using Evo.Models.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evo.Services.Blazor
{
    public class EvoJavascriptComponentService
    {
        private GeneralJsService _General;

        private ElementJsService _Element;


        public EvoJavascriptComponentService(GeneralJsService generalJsService, ElementJsService elementJsService)
        {
            _General = generalJsService;
            _Element = elementJsService;
        }

        public async Task ConsoleLog(string message)
        {
            await _General.ConsoleLog(message);
        }

        public async ValueTask<ElementMeasurements> GetElementMeasurements(Element element)
        {
            return await _Element.GetElementMeasurements(element);
        }

        public async ValueTask<ElementMeasurements> UpdateElementMeasurements(Element element)
        {
            element.Measurements = await _Element.GetElementMeasurements(element);

            return element.Measurements;
        }

        

        public async Task RemoveAllSelections()
        {
            await _General.RemoveAllSelections();
        }
    }
}
