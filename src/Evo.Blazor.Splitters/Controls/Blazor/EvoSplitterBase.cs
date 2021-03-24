using Evo.Components.Blazor;
using Evo.Controls.Blazor;
using Evo.Models.Blazor;
using Evo.Services.Blazor;
using Evo.Statics.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;


namespace Evo.Controls.Blazor
{
    public class EvoSplitterBase: EvoComponentBase<VirtualEvoSplitter, SplitterService_I, DefaultServiceFactory>
    {
        public event EventHandler OnSlidingStateChanged;
        private bool _IsSliding;
        private decimal _InitialScreenX;
        private decimal _InitialScreenY;
        private decimal _OffsetY;
        private decimal _OffsetX;

        /// <summary>
        /// Creates a new EvoSplitterBase
        /// </summary>
        public EvoSplitterBase()
        {
            // Contains the state but is not the real component.
            VirtualComponent = new VirtualEvoSplitter()
            {
                Component = this
            };
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

        

        /// <summary>
        /// Gets or sets the child content.    
        /// </summary>
        [Parameter]
        public RenderFragment EvoSpitterPanes
        {
            get
            {
                return VirtualComponent.EvoSpitterPanes;
            }
            set
            {
                VirtualComponent.EvoSpitterPanes = value;
            }
        }

        public void OnDragEnter(DragEventArgs args)
        {
            Console.WriteLine("Drag Entered");
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

        //[Parameter]
        //public int DragInterval { get; set; } = 1;

        //[Parameter]
        //public bool ExpanedToMin { get; set; } = false;

        //[Parameter]
        //public SplitGutterAlign GutterAlign { get; set; } = SplitGutterAlign.Center;

        [Parameter]
        public int GutterSize { get; set; } = 8;

        

        #endregion
        /// <summary>
        /// Gets whether the control is actively sliding the divider.
        /// </summary>
        public bool IsSliding => _IsSliding;

        public Task ChangeSlidingStateAsync(bool newState, double screenX = 0, double screenY = 0)
        {
            if (newState == _IsSliding)
            {
                return Task.CompletedTask;
            }

            var existingIsSliding = _IsSliding;

            _IsSliding = newState;

            _InitialScreenX = (decimal)screenX;
            _InitialScreenY = (decimal)screenY;

            OnSlidingStateChanged?.Invoke(this, new EventArgs());

            Console.WriteLine("Is Sliding: " + newState.ToString());

            this.StateHasChanged();

            return Task.CompletedTask;
        }


        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        public async Task ResizePanesAsync(MouseEventArgs args)
        {
            if (!await ShouldResize(args))
            {
                return;
            }

            var pane0 = this.VirtualComponent.RegisteredPanes[0];
            var pane1 = this.VirtualComponent.RegisteredPanes[1];

            var pane0Rect = pane0.Component.RootDivElement.Measurements.BoundingClientRect;
            var pane1Rect = pane1.Component.RootDivElement.Measurements.BoundingClientRect;

            var difference = CalculateDiff(args);

            if (Orientation == SplitOrientation.Horizontal)
            {
                if (difference.Y == 0) return;

                var boundingBox = CalculateHorizontalBoundingBox(pane0Rect, pane1Rect);

                if (!IsMouseWithinBoundingBox(boundingBox, args))
                {
                    await ChangeSlidingStateAsync(false);

                    return;
                }

                _OffsetY += difference.Y;

                UpdateBoxHeight(pane0, pane1, boundingBox);

            }
            else if (Orientation == SplitOrientation.Vertical)
            {
                if (difference.X == 0) return;

                var boundingBox = CalculateVerticalBoundingBox(pane0Rect, pane1Rect);

                if (!IsMouseWithinBoundingBox(boundingBox, args))
                {
                    await ChangeSlidingStateAsync(false);

                    return;
                }

                _OffsetX += difference.X;

                UpdateBoxWidth(pane0, pane1, boundingBox);
            }
            else
            {
                throw new NotSupportedException($"Orientation {Orientation} is not supported.");
            }

            this.StateHasChanged();
        }

        private (decimal X, decimal Y) CalculateDiff(MouseEventArgs args)
        {
            var screenX = (decimal)args.ScreenX;
            var screenY = (decimal)args.ScreenY;

            var differenceX = screenX - _InitialScreenX;
            var differenceY = screenY - _InitialScreenY;

            (decimal X, decimal Y) diff = (X: differenceX, differenceY);

            _InitialScreenX = screenX;
            _InitialScreenY = screenY;

            return diff;
        }

        private async Task<bool> ShouldResize(MouseEventArgs args)
        {
            // If sliding is not currently enabled, then there is no need to resize.
            if (!IsSliding)
            {
                return false;
            }

            var leftButtonDepressed = (args.Buttons & 1) > 0;

            // If the left mouse button is down, then 
            if (!leftButtonDepressed)
            {
                await ChangeSlidingStateAsync(false);
                return false;
            }

            return true;
        }

        private bool IsMouseWithinBoundingBox(ElementRectangle boundingBox, MouseEventArgs args)
        {
            var leftLocation = (decimal)args.ClientX - boundingBox.Left;
            var topLocation = (decimal)args.ClientY - boundingBox.Top;
            var rightLocation = boundingBox.Right - (decimal)args.ClientX;
            var bottomLocation = boundingBox.Bottom - (decimal)args.ClientY;

            return (leftLocation >= 1 && rightLocation >= 1 && topLocation >= 1 && bottomLocation >= 1);
        }

        #region Vertical Implementation

        private ElementRectangle CalculateVerticalBoundingBox(ElementRectangle pane0Rect, ElementRectangle pane1Rect)
        {
            var boundingBox = new ElementRectangle();

            boundingBox.Top = pane0Rect.Top;
            boundingBox.Bottom = pane0Rect.Bottom;
            boundingBox.Left = pane0Rect.Left;
            boundingBox.Right = pane1Rect.Right;
            boundingBox.Width = boundingBox.Right - boundingBox.Left;
            boundingBox.Height = boundingBox.Bottom - boundingBox.Top;

            return boundingBox;
        }

        private void UpdateBoxWidth(VirtualEvoSplitterPane pane0, VirtualEvoSplitterPane pane1, ElementRectangle boundingBox)
        {
            var boxwidth = boundingBox.Width / 2M;
            var boxwidth0 = boxwidth + _OffsetX;

            if (boxwidth0 < pane0.Component.MinimumSizeInPixels)
            {
                boxwidth0 = pane0.Component.MinimumSizeInPixels;
            }

            var boxwidth1 = boundingBox.Width - boxwidth0;

            if (boxwidth1 < pane1.Component.MinimumSizeInPixels)
            {
                boxwidth1 = pane1.Component.MinimumSizeInPixels;
                boxwidth0 = boundingBox.Width - boxwidth1;
            }

            pane0.Percentage = boxwidth0 / boundingBox.Width * 100M;
            pane1.Percentage = boxwidth1 / boundingBox.Width * 100M;
        }

        #endregion

        #region Horizontal Implementation

        private void UpdateBoxHeight(VirtualEvoSplitterPane pane0, VirtualEvoSplitterPane pane1, ElementRectangle boundingBox)
        {
            var boxheight = boundingBox.Height / 2M;
            var boxheight0 = boxheight + _OffsetY;

            if (boxheight0 < pane0.Component.MinimumSizeInPixels)
            {
                boxheight0 = pane0.Component.MinimumSizeInPixels;
            }

            var boxheight1 = boundingBox.Height - boxheight0;

            if (boxheight1 < pane1.Component.MinimumSizeInPixels)
            {
                boxheight1 = pane1.Component.MinimumSizeInPixels;
                boxheight0 = boundingBox.Height - boxheight1;
            }

            pane0.Percentage = boxheight0 / boundingBox.Height * 100M;
            pane1.Percentage = boxheight1 / boundingBox.Height * 100M;
        }

        private ElementRectangle CalculateHorizontalBoundingBox(ElementRectangle pane0Rect, ElementRectangle pane1Rect)
        {
            var boundingBox = new ElementRectangle();

            boundingBox.Top = pane0Rect.Top;
            boundingBox.Bottom = pane1Rect.Bottom;
            boundingBox.Left = pane0Rect.Left;
            boundingBox.Right = pane0Rect.Right;
            boundingBox.Width = boundingBox.Right - boundingBox.Left;
            boundingBox.Height = boundingBox.Bottom - boundingBox.Top;

            return boundingBox;
        }

        #endregion
    }
}
