using Geodesy.Internal;

namespace Geodesy
{
    internal abstract class CoordinatesFabric : ICoordinatesFabric
    {
        protected const double ro = 57.29577951D;

        protected IElipsoid _elipsoid;

        public CoordinatesFabric(IElipsoid elipsoid) 
        {
            _elipsoid = elipsoid;
        }

        public ICoordinates CreateCoordByBL(CoordSystem coordSystem, double longitude, double latitude)
        {
            (double X, double Y) gausKrugerCoords = ConvertToGausKrugerCoords(longitude, latitude);
            return new Coordinates(coordSystem, gausKrugerCoords.X, gausKrugerCoords.Y, longitude, latitude);
        }

        public ICoordinates CreateCoordByXY(CoordSystem coordSystem, double x, double y)
        {
            (double Longitude, double Latitude) geodeticСoords = ConvertToGeodeticCoords(x, y);
            return new Coordinates(coordSystem, x, y, geodeticСoords.Longitude, geodeticСoords.Latitude);
        }

        protected abstract (double X, double Y) ConvertToGausKrugerCoords(double longitude, double latitude);

        protected abstract (double Longitude, double Latitude) ConvertToGeodeticCoords(double x, double y);
    }
}
