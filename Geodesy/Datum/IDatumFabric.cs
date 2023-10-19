namespace Geodesy
{
    internal interface IDatumFabric
    {
        IConversionDatum GetConversionDatum(ICoordinates coordinatesSource, CoordSystem toCoordSystem);
    }
}