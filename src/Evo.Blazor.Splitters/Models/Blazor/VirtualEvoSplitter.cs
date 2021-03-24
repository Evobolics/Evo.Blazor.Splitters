using Evo.Controls.Blazor;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Evo.Models.Blazor
{
    public class VirtualEvoSplitter
    {
        public VirtualEvoSplitter()
        {

        }

        /*  NOTE: The property receiving the RenderFragment content 
         *        must be named ChildContent by convention. 
         *
         *        https://docs.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-5.0
         *
         */

        /*  NOTE: If multiple render fragments are going to be used, then the 
         *        render fragment being filled needs to be named within the 
         *        child component.  
         *        
         *        https://blazor-university.com/templating-components-with-renderfragements/
         * 
         */

        /*  DESIGN NOTE
         *
         *  This WAS named ChildContent, which was the default name for child content
         *  within blazor, by convention.  But it implies that the content can be anything.
         *  To help specify that the content should just be EvoSplitterPane(s), the name
         *  has been changed to EvoSpitterPanes
         */

        public EvoSplitterBase Component { get; set; }

        /// <summary>
        /// Gets or sets the child content.    
        /// </summary>
        public RenderFragment EvoSpitterPanes { get; set; }

        public List<VirtualEvoSplitterPane> RegisteredPanes { get; set; } = new List<VirtualEvoSplitterPane>();
    }
}
