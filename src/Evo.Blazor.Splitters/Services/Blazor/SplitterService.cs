using Evo.Controls.Blazor;
using Evo.Models.Blazor;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;

namespace Evo.Services.Blazor
{
    public class SplitterService: SplitterService_I
    {
        public Task ChangeSlidingStateAsync(EvoSplitterBase splitter, bool newState, double screenX = 0, double screenY = 0)
        {
            if (newState == splitter.IsSliding)
            {
                return Task.CompletedTask;
            }

            splitter.UpdateSlidingState(newState, (decimal)screenX, (decimal)screenY);

            return Task.CompletedTask;
        }

        private async Task<bool> ShouldResize(EvoSplitterBase splitter, MouseEventArgs args)
        {
            // If sliding is not currently enabled, then there is no need to resize.
            if (!splitter.IsSliding)
            {
                return false;
            }

            var leftButtonDepressed = (args.Buttons & 1) > 0;

            // If the left mouse button is down, then 
            if (!leftButtonDepressed)
            {
                await ChangeSlidingStateAsync(splitter, false);

                return false;
            }

            return true;
        }


        public async Task<bool> ResizePanes(EvoSplitterBase splitter, MouseEventArgs args)
        {
            if (!await ShouldResize(splitter, args))
            {
                return false;
            }

            var pane0 = splitter.RegisteredPanes[0];
            var pane1 = splitter.RegisteredPanes[1];

            var pane0Rect = pane0.Element.Measurements.BoundingClientRect;
            var pane1Rect = pane1.Element.Measurements.BoundingClientRect;

            var difference = CalculateDiff(splitter, args);

            if (splitter.Orientation == SplitOrientation.Horizontal)
            {
                if (difference.Y == 0) return false;

                var boundingBox = CalculateHorizontalBoundingBox(pane0Rect, pane1Rect);

                if (!IsMouseWithinBoundingBox(boundingBox, args))
                {
                    await ChangeSlidingStateAsync(splitter, false);

                    return false;
                }

                splitter.OffsetY += difference.Y;

                UpdateBoxHeight(splitter, pane0, pane1, boundingBox);

            }
            else if (splitter.Orientation == SplitOrientation.Vertical)
            {
                if (difference.X == 0) return false;

                var boundingBox = CalculateVerticalBoundingBox(pane0Rect, pane1Rect);

                if (!IsMouseWithinBoundingBox(boundingBox, args))
                {
                    await ChangeSlidingStateAsync(splitter, false);

                    return false;
                }

                splitter.OffsetX += difference.X;

                UpdateBoxWidth(splitter, pane0, pane1, boundingBox);
            }
            else
            {
                throw new NotSupportedException($"Orientation {splitter.Orientation} is not supported.");
            }

            return true;
        }

        public (decimal X, decimal Y) CalculateDiff(EvoSplitterBase splitter, MouseEventArgs args)
        {
            var screenX = (decimal)args.ScreenX;
            var screenY = (decimal)args.ScreenY;

            var differenceX = screenX - splitter.InitialScreenX;
            var differenceY = screenY - splitter.InitialScreenY;

            (decimal X, decimal Y) diff = (X: differenceX, differenceY);

            splitter.InitialScreenX = screenX;
            splitter.InitialScreenY = screenY;

            return diff;
        }

        public bool IsMouseWithinBoundingBox(ElementRectangle boundingBox, MouseEventArgs args)
        {
            var leftLocation = (decimal)args.ClientX - boundingBox.Left;
            var topLocation = (decimal)args.ClientY - boundingBox.Top;
            var rightLocation = boundingBox.Right - (decimal)args.ClientX;
            var bottomLocation = boundingBox.Bottom - (decimal)args.ClientY;

            return (leftLocation >= 1 && rightLocation >= 1 && topLocation >= 1 && bottomLocation >= 1);
        }

        /// <summary>
        /// Registers the panes existance with the parent splitter. 
        /// </summary>
        /// <param name="pane">The pane to be registered.</param>
        public Task RegisterPaneAsync(EvoSplitterBase splitter, EvoSplitterPane pane)
        {
            if (splitter == null)
            {
                throw new ArgumentNullException("splitter", "Splitter is null");
            }

            if (pane == null)
            {
                throw new ArgumentNullException("pane", "Pane is null");
            }

            lock (splitter.RegisteredPanes)
            {
                //Console.WriteLine("Registering Pane...");

                pane.Parent = splitter;

                pane.Index = splitter.RegisteredPanes.Count;

                splitter.RegisteredPanes.Add(pane);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Unregisters the panes existance with the parent splitter. 
        /// </summary>
        /// <param name="pane">The pane to be registered.</param>
        public Task UnregisterPaneAsync(EvoSplitterBase splitter, EvoSplitterPane pane)
        {
            lock (splitter.RegisteredPanes)
            {
                splitter.RegisteredPanes.Remove(pane);

                pane.Parent = null;
            }

            return Task.CompletedTask;
        }

        #region Horizontal Implementation

        public void UpdateBoxHeight(EvoSplitterBase splitter, EvoSplitterPane pane0, EvoSplitterPane pane1, ElementRectangle boundingBox)
        {
            var boxheight = boundingBox.Height / 2M;
            var boxheight0 = boxheight + splitter.OffsetY;

            if (boxheight0 < pane0.MinimumSizeInPixels)
            {
                boxheight0 = pane0.MinimumSizeInPixels;
            }

            var boxheight1 = boundingBox.Height - boxheight0;

            if (boxheight1 < pane1.MinimumSizeInPixels)
            {
                boxheight1 = pane1.MinimumSizeInPixels;
                boxheight0 = boundingBox.Height - boxheight1;
            }

            pane0.Percentage = boxheight0 / boundingBox.Height * 100M;
            pane1.Percentage = boxheight1 / boundingBox.Height * 100M;
        }

        public ElementRectangle CalculateHorizontalBoundingBox(ElementRectangle pane0Rect, ElementRectangle pane1Rect)
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

        #region Vertical Implementation

        public ElementRectangle CalculateVerticalBoundingBox(ElementRectangle pane0Rect, ElementRectangle pane1Rect)
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

        public void UpdateBoxWidth(EvoSplitterBase splitter, EvoSplitterPane pane0, EvoSplitterPane pane1, ElementRectangle boundingBox)
        {
            var boxwidth = boundingBox.Width / 2M;
            var boxwidth0 = boxwidth + splitter.OffsetX;

            if (boxwidth0 < pane0.MinimumSizeInPixels)
            {
                boxwidth0 = pane0.MinimumSizeInPixels;
            }

            var boxwidth1 = boundingBox.Width - boxwidth0;

            if (boxwidth1 < pane1.MinimumSizeInPixels)
            {
                boxwidth1 = pane1.MinimumSizeInPixels;
                boxwidth0 = boundingBox.Width - boxwidth1;
            }

            pane0.Percentage = boxwidth0 / boundingBox.Width * 100M;
            pane1.Percentage = boxwidth1 / boundingBox.Width * 100M;
        }

        #endregion
    }
}
