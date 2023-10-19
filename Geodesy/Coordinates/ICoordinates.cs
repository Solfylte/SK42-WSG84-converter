namespace Geodesy
{
    public interface ICoordinates
    {
        CoordSystem CoordSystem { get; }
        double X { get; }
        double Y { get; }
        double Longitude { get; }
        double Latitude { get; }
    }
}