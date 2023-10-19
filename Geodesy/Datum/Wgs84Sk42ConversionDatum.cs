namespace Geodesy.Internal
{
    internal class Wgs84Sk42ConversionDatum : IConversionDatum
    {
        public CoordSystem FirstCoordSystem => CoordSystem.SK42;
        public CoordSystem SecondCoordSystem => CoordSystem.WSG84;

        public IElipsoid FirstElipsoid => _firstElipsoid;
        public IElipsoid SecondElipsoid => _secondElipsoid;

        public double dx => 23.92;
        public double dy => -141.27;
        public double dz => -80.9;
        public double wx => 0;
        public double wy => 0;
        public double wz => 0;
        public double ms => 0;

        private IElipsoid _firstElipsoid = new ElipsoidKrasovskii();
        private IElipsoid _secondElipsoid = new ElipsoidWsg84();
    }
}