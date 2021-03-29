using Evo.Services.Blazor;
using Microsoft.AspNetCore.Components;
using System;

namespace Evo.Components.Blazor
{
    public class EvoComponentBase<TService> : ComponentBase
        
    {
        #region Constructor(s)

        public EvoComponentBase()
        {
            
        }

        #endregion

        #region Properties and Components

        [Inject]
        public EvoJavascriptComponentService Javascript { get; set; }

        [Inject]
        public FactoryService_I Factory { get; set; }

        [Inject]
        /// <summary>
        /// Gets or sets the instance of the splitter service that contains the logic for this component.
        /// </summary>
        public TService Service { get; set; }

        /// <summary>
        /// Gets or sets the service provider.  This is injected by the blazor framework and used to 
        /// get overriding service implementations for this component.  The component is only a shell
        /// and the service is what contains all the logic.
        /// </summary>
        [Inject]
        public IServiceProvider ServiceProvider { get; set; }

        ///// <summary>
        ///// Gets or sets the virtual component that is associated with this component.  The virtual component contains the actual state.
        ///// </summary>
        //public TVirtualComponent VirtualComponent { get; set; }

        #endregion

        


    }
}
