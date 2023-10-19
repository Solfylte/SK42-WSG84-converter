namespace Geodesy
{
    internal class SK42CoordinatesFabric : CoordinatesFabric
    {
        public SK42CoordinatesFabric(IElipsoid elipsoid) : base(elipsoid) { }

        protected override (double Longitude, double Latitude) ConvertToGeodeticCoords(double x, double y)
        {
            double Longitude = GetLongitude(x, y);
            double Latitude = GetLatitude(x, y);

            return (Longitude, Latitude);
        }

        protected override (double X, double Y) ConvertToGausKrugerCoords(double longitude, double latitude)
        {
            double X = GetX(longitude, latitude);
            double Y = GetY(longitude, latitude);
            return (X, Y);
        }

        private double GetLongitude(double x, double y)
        {
            double B0 = GetB0(x);
            double z = GetZ(B0, y);
            double zPow2 = Math.Pow(z, 2);

            double b2 = (0.5 + 0.003369 * CosPowRad(B0, 2)) * Math.Sin(B0) * Math.Cos(B0);
            double b4 = 0.25 + (0.16161 + 0.00562 * CosPowRad(B0, 2)) * CosPowRad(B0, 2);

            double Longitude = (B0 - (1 - (b4 - 0.12 * zPow2) * zPow2) * zPow2 * b2) * ro;
            return Longitude;
        }

        private double GetLatitude(double x, double y)
        {
            double B0 = GetB0(x);
            double z = GetZ(B0, y);
            double zPow2 = Math.Pow(z, 2);

            double b3 = 0.333333D - (0.166667D - 0.001123D * CosPowRad(B0, 2)) * CosPowRad(B0, 2);
            double b5 = 0.2 - (0.1667 - 0.0088 * CosPowRad(B0, 2)) * CosPowRad(B0, 2);

            double l = (1 - (b3 - b5 * zPow2) * zPow2) * z * ro;
            double L0 = 6 * GetZoneNumber(y) - 3;
            double Latitude = L0 + l;

            return Latitude;
        }

        private double GetX(double longitude, double latitude)
        {
            double l = (latitude - L0(latitude)) / ro;
            double cosPow2B = CosPow2(longitude);

            double a0 = 32140.404 - (135.3302 - (0.7092 - 0.004 * cosPow2B) * cosPow2B) * cosPow2B;
            double a4 = (0.25 + 0.00252 * cosPow2B) * cosPow2B - 0.04166;
            double a6 = (0.166 * cosPow2B - 0.0084) * cosPow2B;

            double X = 6367558.4969D * (longitude / ro) -
                (a0 - (0.5 + (a4 + a6 * Math.Pow(l, 2)) * Math.Pow(l, 2)) * Math.Pow(l, 2) * GetN0ByB(longitude)) * SinGrad(longitude) * CosGrad(longitude);
            
            return X;
        }

        private double GetY(double longitude, double latitude)
        {
            double l = (latitude - L0(latitude)) / ro;
            double cosPow2B = CosPow2(longitude);

            double a3 = ((0.3333333D + 0.001123D * cosPow2B) * cosPow2B) - 0.1666667D;

            double a51 = 0.1968D + 0.0040D * cosPow2B;
            double a52 = 0.1667D - a51 * cosPow2B;
            double a5 = 0.0083D - a52 * cosPow2B;

            double lPow2 = Math.Pow(l, 2);
            double y1 = a5 * lPow2;
            double y2 = (a3 + y1) * lPow2;
            double y3 = (1 + y2);
            double y = y3 * l * GetN0ByB(longitude) * CosGrad(longitude);
            double Y = ZoneNumber(latitude) * 1000000 + 500000 + y;

            return Y;
        }

        private double GetB0(double x)
        {
            double beta = (x / 6367558.4969D);
            double cosPow2Beta = CosPow2(beta);

            double B0 = beta + (50221746 + (293622 + (2350 + 22 * cosPow2Beta) * cosPow2Beta) * cosPow2Beta) *
                Math.Pow(10, -10) * Math.Sin(beta) * Math.Cos(beta);
            return B0;
        }

        private double GetZ(double B0, double y)
        {
            double N0 = GetN0ByB0(B0);
            int n = GetZoneNumber(y);
            double yOfsetted = y - (n * 1000000 + 500000);

            double z = yOfsetted / (N0 * Math.Cos(B0));
            return z;
        }

        private int GetZoneNumber(double y) => (int)y / 1000000;

        double GetN0ByB0(double B0) => 6399698.902 - 21562.267 * CosPowRad(B0, 2) + 108.973 * CosPowRad(B0, 4)
                    - 0.612 * CosPowRad(B0, 6) + 0.004 * CosPowRad(B0, 8);

        private int ZoneNumber(double latitude) => (int)(latitude / 6 + 1);
        private double L0(double latitude) => 6 * ZoneNumber(latitude) - 3;
        private double GetN0ByB(double B) => _elipsoid.a / Math.Sqrt(1 - (_elipsoid.ePow2 * SinPow2(B)));
        private double CosGrad(double value) => Math.Cos(value * (Math.PI / 180.0));
        private double SinGrad(double value) => Math.Sin(value * (Math.PI / 180.0));
        private double SinPow2(double value) => (1D - CosGrad(2D * value)) / 2D;
        private double CosPow2(double value) => (1D + CosGrad(2D * value)) / 2D;
        private double CosPowRad(double value, int power) => Math.Pow(Math.Cos(value), power);
    }
}
