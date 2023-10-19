namespace Geodesy
{
    internal class ConversionService : IConversionService
    {
        private const double RO = 57.29577951D;

        private IDatumFabric _datumFabric;
        private ICoordinatesFabric _coordinatesFabric;
        private IConversionDatum _datum;
        private IElipsoid _firstElipsoid;
        private IElipsoid _secondElipsoid;

        // elipsoid transformation coeficients
        private double _a;
        private double _e2;
        private double _da;
        private double _de2;

        public ConversionService(ICoordinatesFabric coordinatesFabric)
        {
            _datumFabric = new DatumFabric();
            _coordinatesFabric = coordinatesFabric;
        }

        public ICoordinates Convert(ICoordinates coordinates, CoordSystem toSystem)
        {
            _datum = _datumFabric.GetConversionDatum(coordinates, toSystem);
            InitializeElipsoidData();

            (double B, double L) bl = GetConvertedBL(coordinates);
            return _coordinatesFabric.CreateCoordByBL(toSystem, bl.B, bl.L);
        }

        private void InitializeElipsoidData()
        {
            _firstElipsoid = _datum.FirstElipsoid;
            _secondElipsoid = _datum.SecondElipsoid;

            _a = (_firstElipsoid.a + _secondElipsoid.a) / 2;
            _e2 = (_firstElipsoid.ePow2 + _secondElipsoid.ePow2) / 2;
            _da = _secondElipsoid.a - _firstElipsoid.a;
            _de2 = _secondElipsoid.ePow2 - _firstElipsoid.ePow2;
        }

        private (double, double) GetConvertedBL(ICoordinates coordinates)
        {
            double Bd = coordinates.Longitude;
            double Ld = coordinates.Latitude;
            double H = 100;

            bool isForwardTransformation = _datum.FirstCoordSystem == coordinates.CoordSystem;
            int transformationDirection = isForwardTransformation ? 1 : -1;

            double B = Bd + GetdB(Bd, Ld, H) * transformationDirection;
            double L = Ld + GetdL(Bd, Ld, H) * transformationDirection;

            return (B, L);
        }

        private double GetdB(double B, double L, double H)
        {
            double dB = RO / (M(B) + H) * (N(B) / _a * _e2 * SinGrad(B) * CosGrad(B)
                * _da + (Math.Pow(N(B), 2) / Math.Pow(_a, 2) + 1) * N(B) * SinGrad(B) * CosGrad(B)
                * _de2 / 2 - (_datum.dx * CosGrad(L) + _datum.dy * SinGrad(L)) * SinGrad(B) 
                + _datum.dz * CosGrad(B)) - _datum.wx * SinGrad(L) * (1 + _e2 * CosGrad(2 * B))
                + _datum.wy * CosGrad(L) * (1 + _e2 * CosGrad(2 * B)) - RO * _datum.ms * _e2 * SinGrad(B) * CosGrad(B);
            return dB;
        }

        private double GetdL(double B, double L, double H)
        {
            double dL = RO / ((N(B) + H) * CosGrad(B)) * (-_datum.dx * SinGrad(L) + _datum.dy * CosGrad(L))
                        + TanGrad(B) * (1 - _e2) * (_datum.wx * CosGrad(L) + _datum.wy * SinGrad(L)) - _datum.wz;
            return dL;
        }

        private double N(double B) => _a * Math.Pow((1 - _e2 * Math.Pow(SinGrad(B), 2)), -0.5);
        private double M(double B) => _a * (1 - _e2) / Math.Pow((1 - _e2 * Math.Pow(SinGrad(B), 2)), 1.5);
        private double CosGrad(double value) => Math.Cos(value * (Math.PI / 180.0));
        private double SinGrad(double value) => Math.Sin(value * (Math.PI / 180.0));
        private double TanGrad(double value) => Math.Tan(value * (Math.PI / 180.0));
    }
}
