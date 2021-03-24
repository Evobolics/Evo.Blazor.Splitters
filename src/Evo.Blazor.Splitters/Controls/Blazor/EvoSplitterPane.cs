using HtmlAgilityPack;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;


namespace Evo.Controls.Blazor
{
    public partial class EvoSplitterPane: EvoSplitterPaneBase
    {
        private bool _TryUseExistingDiv = true;
        private bool _UseExistingDiv = false;
        private bool _FirstRender = true;
        private string _InnerDivHtml = null;
        private Dictionary<string, object> _outerDivAttributes;
        private string _OrignalStyleValue;

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            await Javascript.UpdateElementMeasurements(RootDivElement);            
        }

        #region Rendering

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var percentage = this.VirtualComponent.Percentage.ToString("F3", CultureInfo.InvariantCulture);

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
                builder.AddContent(50, ChildContent);
            }

            builder.AddElementReferenceCapture(59, (elementReference) => RootDivElement.ElementReference = elementReference);

            builder.CloseElement();

            if (!this.IsLastPane())
            {
                TypeInference.CreateCascadingValue_0(builder, 60, 61, this, 62, (__builder2) =>
                {
                    __builder2.OpenComponent<EvoGutterBar>(63);
                    __builder2.CloseComponent();
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
                Console.WriteLine("Got mark up frame.");

                if (!TryGetCleanMarkupContent(markupFrame, out var markupContent))
                {
                    Console.WriteLine("Could not clean content");
                    _UseExistingDiv = false;
                    return;
                }

                var htmlDoc = new HtmlDocument();

                htmlDoc.LoadHtml(markupContent);

                var documentNode = htmlDoc.DocumentNode;

                if (documentNode.ChildNodes.Count > 1)
                {
                    Console.WriteLine("Not a single root node in child content");
                    _UseExistingDiv = false;
                    return;
                }

                var childNode = documentNode.ChildNodes[0];

                if (childNode.NodeType != HtmlNodeType.Element)
                {
                    Console.WriteLine("Single node is not an element.");
                    _UseExistingDiv = false;
                    return;
                }

                Console.WriteLine(childNode.Name);

                if (childNode.Name != "div")
                {
                    Console.WriteLine("Node is not a 'div' node.");
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

                

                Console.WriteLine("Could not get mark up frame.");
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
                Console.WriteLine(frame.FrameType);

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

        internal static class TypeInference
        {
            public static void CreateCascadingValue_0<TValue>(RenderTreeBuilder builder, int seq00, int seq01, TValue attributeValue, int seq02, RenderFragment childContent)
            {
                builder.OpenComponent<CascadingValue<TValue>>(seq00);
                builder.AddAttribute(seq01, "Value", attributeValue);
                builder.AddAttribute(seq02, "ChildContent", childContent);
                builder.CloseComponent();
            }
        }

        #endregion
    }   
}
