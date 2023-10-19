namespace Geodesy.Internal
{
    internal struct Coordinates : ICoordinates
    {
        public CoordSystem CoordSystem => coordSystem;
        public double X => x;
        public double Y => y;
        public double Longitude => longitude;
        public double Latitude => latitude;

        private CoordSystem coordSystem;
        private double x;
        private double y;
        private double longitude;
        private double latitude;

        public Coordinates(CoordSystem coordSystem, double x, double y, double longitude, double latitude)
        {
            this.coordSystem = coordSystem;
            this.x = x;
            this.y = y;
            this.longitude = longitude;
            this.latitude = latitude;
        }        
    }
}
