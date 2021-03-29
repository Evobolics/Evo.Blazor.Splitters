using Evo.Blazor.Models;
using Evo.Components.Blazor;
using Evo.JsServices.Blazor;
using Evo.Services.Blazor;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;


namespace Evo.Controls.Blazor
{
    public partial class EvoSplitterPane: DivComponentBase<SplitterPaneService_I>, IAsyncDisposable
    {
        private bool _TryUseExistingDiv = true;
        private bool _UseExistingDiv = false;
        private bool _FirstRender = true;
        private string _InnerDivHtml = null;
        private Dictionary<string, object> _outerDivAttributes;
        private string _OrignalStyleValue;

     

        [Parameter]
        public string Style { get; set; }

        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public int MinimumSizeInPixels { get; set; }

        [Parameter]
        public bool IsDraggable { get; set; } = false;



        [CascadingParameter]
        public EvoSplitter Splitter { get; set; }           /* This property is identified by type.
                                                             * 
                                                             * Per Microsoft, "Cascading values are bound to cascading parameters by type"
                                                             * 
                                                             * See: https://docs.microsoft.com/en-us/aspnet/core/blazor/components/cascading-values-and-parameters?view=aspnetcore-5.0
                                                             */

        /// <summary>
        /// Gets or sets the child content of the individual pane.    
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }    /*  NOTE: The property receiving the RenderFragment content 
                                                             *        must be named ChildContent by convention. 
                                                             *       
                                                             *       https://docs.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-5.0
                                                             */


        

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            await Element.RefreshMeasurementsAsync();

            if (firstRender)
            {
                await Element.ObserveResizeAsync();
            }

            //Element.GetParent();
        }

        #region Rendering

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var percentage = this.Percentage.ToString("F3", CultureInfo.InvariantCulture);

            var calcFunction = "calc";

            int gutterSize = Splitter.GutterSize;

            var paneMarginInPixels = gutterSize / 2;

            string dynamicStyle = string.Empty;

            if (this.Splitter.Orientation == SplitOrientation.Horizontal)
            {
                dynamicStyle = $"height:{calcFunction}({percentage}% - {paneMarginInPixels}px);";
            }
            else
            {
                dynamicStyle = $"width:{calcFunction}({percentage}% - {paneMarginInPixels}px);float:left;height:100%;";
            }

            if (Splitter.IsSliding)
            {
                dynamicStyle += "user-select:none;";
            }

            if (string.IsNullOrWhiteSpace(this.Style))
            {
                dynamicStyle += this.Style;
            }

            if (_FirstRender && _TryUseExistingDiv)
            {
                _FirstRender = false;

                AttemptToUseExistingDiv();
            }

            builder.OpenElement(0, "div");
            builder.AddAttribute(2, "onmouseup", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseUp));
            builder.AddAttribute(3, "onmousemove", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseMove));
            builder.AddAttribute(4, "onmouseout", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseOut));

            if (!IsDraggable)
            {
                builder.AddAttribute(5, "draggable", "false");
                builder.AddEventPreventDefaultAttribute(6, "onmousedown", true);
            }

            builder.AddAttribute(8, "b-9gda3aicu3");

            if (_UseExistingDiv)
            {
                if (string.IsNullOrWhiteSpace(_OrignalStyleValue))
                {
                    builder.AddAttribute(10, "style", dynamicStyle);
                }
                else
                {
                    //
                    builder.AddAttribute(10, "style", $"{_OrignalStyleValue};{dynamicStyle}");
                }

                builder.AddMultipleAttributes(20, _outerDivAttributes);

                builder.AddMarkupContent(50, _InnerDivHtml);
            }
            else
            {
                builder.AddAttribute(10, "style", dynamicStyle);

                if (!string.IsNullOrWhiteSpace(this.Class))
                {
                    builder.AddAttribute(20, "class", this.Class);
                }

                CascadingValueTypeInference.CreateCascadingValue(builder, 47, 48, this, 49, (innerBuilder) =>
                {
                    innerBuilder.AddContent(50, ChildContent);
                });
            }

            builder.AddElementReferenceCapture(59, (elementReference) =>
            {
                Element.ElementReference = elementReference;

                //Console.WriteLine("Element captured");
            });

            builder.CloseElement();

            // TODO: Consider moving to the splitter control.
            if (!this.IsLastPane())
            {
                CascadingValueTypeInference.CreateCascadingValue(builder, 60, 61, this, 62, (innerBuilder) =>
                {
                    innerBuilder.OpenComponent<EvoGutterBar>(63);
                    innerBuilder.CloseComponent();
                });
            }
        }

        public void AttemptToUseExistingDiv()
        {
            var builder = new RenderTreeBuilder();

            ChildContent.Invoke(builder);

#pragma warning disable BL0006
            var frames = builder.GetFrames().Array;
#pragma warning restore BL0006

            if (TryGetMarkupFrame(frames, out var markupFrame))
            {
                //Console.WriteLine("Got mark up frame.");

                if (!TryGetCleanMarkupContent(markupFrame, out var markupContent))
                {
                    //Console.WriteLine("Could not clean content");
                    _UseExistingDiv = false;
                    return;
                }

                var htmlDoc = new HtmlDocument();

                htmlDoc.LoadHtml(markupContent);

                var documentNode = htmlDoc.DocumentNode;

                if (documentNode.ChildNodes.Count > 1)
                {
                    //Console.WriteLine("Not a single root node in child content");
                    _UseExistingDiv = false;
                    return;
                }

                var childNode = documentNode.ChildNodes[0];

                if (childNode.NodeType != HtmlNodeType.Element)
                {
                    //Console.WriteLine("Single node is not an element.");
                    _UseExistingDiv = false;
                    return;
                }

                //Console.WriteLine(childNode.Name);

                if (childNode.Name != "div")
                {
                    //Console.WriteLine("Node is not a 'div' node.");
                    _UseExistingDiv = false;
                    return;
                }

                _InnerDivHtml = childNode.InnerHtml;

                _outerDivAttributes = new Dictionary<string, object>();

                foreach (var attribute in childNode.Attributes)
                {
                    if (attribute?.OriginalName?.ToLower() == "style")
                    {
                        _OrignalStyleValue = attribute.Value;
                    }
                    else
                    {
                        _outerDivAttributes.Add(attribute.OriginalName, attribute.Value);
                    }
                }

                _UseExistingDiv = true;
            }
            else
            {
                //Console.WriteLine("Could not get mark up frame.");

                _UseExistingDiv = false;

                return;
            }
        }
#pragma warning disable BL0006
        private bool TryGetCleanMarkupContent(RenderTreeFrame markupFrame, out string cleanContent)
        {
            if (markupFrame.FrameType != RenderTreeFrameType.Markup)
            {
                cleanContent = null;
                return false;
            }

            var content = markupFrame.MarkupContent;

            if (content == null)
            {
                cleanContent = null;
                return false;
            }

            cleanContent = content?.Trim();

            return true;
        }

        private bool TryGetMarkupFrame(RenderTreeFrame[] frames, out RenderTreeFrame markupFrame)
        {
            foreach (var frame in frames)
            {
                //Console.WriteLine(frame.FrameType);

                if (frame.FrameType == RenderTreeFrameType.Markup)
                {
                    markupFrame = frame;
                    return true;
                }
            }

            markupFrame = default;
            return false;
        }
#pragma warning restore BL0006

        

        public EvoSplitterBase Parent { get; internal set; }


        public decimal Percentage { get; internal set; } = 50M;
        public int Index { get; internal set; }


        

       


        /*  NOTE: If multiple render fragments are going to be used, then the 
         *        render fragment being filled needs to be named within the 
         *        child component.  
         * 
         * https://blazor-university.com/templating-components-with-renderfragements/
         */

        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            // Register this pane with the splitter so that it knows it exists.
            await Splitter.Service.RegisterPaneAsync(Splitter, this);

            Splitter.OnSlidingStateChanged += Splitter_OnSlidingStateChanged;
        }

        private void Splitter_OnSlidingStateChanged(object sender, EventArgs e)
        {
            this.StateHasChanged();
        }

        public async ValueTask DisposeAsync()
        {
            await Splitter.Service.UnregisterPaneAsync(Splitter, this);
        }

        public bool IsLastPane()
        {
            return Service.IsLastPane(this);
        }



        public async Task OnMouseUp(MouseEventArgs args)
        {
            await Splitter.Service.ChangeSlidingStateAsync(Splitter, false);
        }

        public async Task OnMouseMove(MouseEventArgs args)
        {
            await Splitter.ResizePanesAsync(args);
        }

        public async Task OnMouseOut(MouseEventArgs args)
        {
            await Splitter.ResizePanesAsync(args);
        }


        internal static class CascadingValueTypeInference
        {
            public static void CreateCascadingValue<TValue>(RenderTreeBuilder builder, int seq00, int seq01, TValue attributeValue, int seq02, RenderFragment childContent)
            {
                builder.OpenComponent<CascadingValue<TValue>>(seq00);
                builder.AddAttribute(seq01, "Value", attributeValue);
                builder.AddAttribute(seq02, "ChildContent", childContent);
                builder.CloseComponent();
            }
        }
    }

    #endregion


}
