namespace Geodesy
{
    public interface ICoordinatesFabric
    {
        ICoordinates CreateCoordByXY(CoordSystem coordType, double x, double y);
        ICoordinates CreateCoordByBL(CoordSystem coordType, double longitude, double latitude);
    }
}
