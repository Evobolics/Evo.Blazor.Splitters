namespace Evo.Models.Blazor
{
    /// <summary>
    /// Contains an elements measurements.
    /// </summary>
    public class ElementMeasurements
    {
        public ElementRectangle BoundingClientRect { get; set; }

        /// <summary>
        /// Gets the client's last reported client height.  The Client height is equal to the CSS height
        /// + CSS padding - height of the horizontal scrollbar, if it is present. 
        /// </summary>
        public int ClientHeight { get; set; }

        /// <summary>
        /// Gets the client's last report client left, which is the thickness
        /// of the border around the element.  If a border is not specified, 
        /// the value is zero.
        /// </summary>
        public int ClientLeft { get; set; }

        /// <summary>
        /// Gets the client's last report client top, which is the thickness
        /// of the border around the element.  If a border is not specified, 
        /// the value is zero.
        /// </summary>
        public int ClientTop { get; set; }

        /// <summary>
        /// Gets the client's last reported client width.  It includes
        /// padding, but borders, margins and vertical scrollbars are 
        /// excluded from the measurement.
        /// </summary>
        public int ClientWidth { get; set; }

        public decimal OffsetHeight { get; set; }

        public decimal OffsetLeft { get; set; }

        public decimal OffsetTop { get; set; }

        public decimal OffsetWidth { get; set; }

        public decimal ScrollHeight { get; set; }

        public decimal ScrollLeft { get; set; }

        public decimal ScrollTop { get; set; }

        public decimal ScrollWidth { get; set; }

     
    }
}
