namespace Geodesy
{
    internal interface IConversionService
    {
        ICoordinates Convert(ICoordinates coordinates, CoordSystem toSystem);
    }
}
