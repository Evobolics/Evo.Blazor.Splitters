using Evo.Components.Blazor;
using Evo.Controls.Blazor;
using Evo.Models.Blazor;
using Evo.Services.Blazor;
using Evo.Statics.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Evo.Controls.Blazor
{
    public class EvoSplitterBase: EvoComponentBase<SplitterService_I>
    {
        public event EventHandler OnSlidingStateChanged;

        /// <summary>
        /// Gets whether the control is actively sliding the divider.
        /// </summary>
        public bool IsSliding { get; set; }
        public decimal InitialScreenX { get; set; }
        public decimal InitialScreenY { get; set; }
        public decimal OffsetY { get; set; }
        public decimal OffsetX { get; set; }

        /// <summary>
        /// Creates a new EvoSplitterBase
        /// </summary>
        public EvoSplitterBase()
        {
           
            
        }

        #region Parameters

        /*
         *  Parameters - The following section contains a list of all the parameters that can be specified by the user when
         *               using the application.  Taking a page out of the split.js library, the goal is to make the framework
         *               as friendly as possible with existing applications.  This means we want to minimize what we impose on
         *               user when implementing this api.
         *               
         *               All the paramters listed here are in alphabetical order.  
         */

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
         *  has been changed to EvoSplitterPanes
         */

        /// <summary>
        /// Gets or sets the child content.    
        /// </summary>
        [Parameter]
        public RenderFragment EvoSplitterPanes { get; set; }

        public void OnDragEnter(DragEventArgs args)
        {
            //Console.WriteLine("Drag Entered");
        }

        [Parameter]
        public string Cursor { get; set; } = null;

        /// <summary>
        /// Gets or sets the Css class that is used by the gutter bar.  
        /// </summary>
        [Parameter]
        public string GutterBarClass { get; set; }  // Thought here is that if one is not supplied, a default style can be applied so that the control works correctly.

        

        /// <summary>
        /// Gets or set the orientation of the split.  By default the value is set to horizontal.
        /// </summary>
        [Parameter]
        public SplitOrientation Orientation { get; set; } = SplitOrientation.Horizontal;



        [Parameter]
        public int GutterSize { get; set; } = 8;

        

        public List<EvoSplitterPane> RegisteredPanes { get; set; } = new List<EvoSplitterPane>();

        #endregion
      
    

        

        public void UpdateSlidingState(bool isSliding, decimal screenX, decimal screenY)
        {
            IsSliding = isSliding;

            InitialScreenX = screenX;
            InitialScreenY = screenY;

            OnSlidingStateChanged?.Invoke(this, new EventArgs());

            this.StateHasChanged();
        }


        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        public async Task ResizePanesAsync(MouseEventArgs args)
        {
            if (!await Service.ResizePanes(this, args)) return;

            this.StateHasChanged();
        }

        

        
    }
}
