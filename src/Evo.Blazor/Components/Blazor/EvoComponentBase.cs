using Evo.Services.Blazor;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Evo.Components.Blazor
{
    public class EvoComponentBase<TVirtualComponent, TService, TDefaultServices> : ComponentBase
        where TVirtualComponent : new()
    {
        #region Constructor(s)

        public EvoComponentBase()
        {
            VirtualComponent = new TVirtualComponent();
        }

        #endregion

        #region Properties and Components

        [Inject]
        public EvoJavascriptComponentService Javascript { get; set; }

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

        /// <summary>
        /// Gets or sets the virtual component that is associated with this component.  The virtual component contains the actual state.
        /// </summary>
        public TVirtualComponent VirtualComponent { get; set; }

        #endregion

        #region Methods

        protected override Task OnInitializedAsync()
        {
            var task = base.OnInitializedAsync();

            // Allows for the service to be overriden by users of the library.  If they want to change the algorithms used
            // they can.
            Service = ServiceProvider.GetServiceOrDefaultFrom<TService, TDefaultServices>();

            return task;
        }

        #endregion

    }
}
