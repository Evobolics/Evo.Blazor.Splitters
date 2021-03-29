namespace Evo.Models.Blazor
{
    public static class ElementMeasurementsExtensions
    {
        public static decimal BorderHeightTop(this ElementMeasurements measurements)
        {
            return measurements.ClientTop;
        }

        public static decimal BorderHeightBottom(this ElementMeasurements measurements)
        {
            return measurements.OffsetHeight - measurements.ClientHeight - measurements.ClientTop;
        }
    }
}
