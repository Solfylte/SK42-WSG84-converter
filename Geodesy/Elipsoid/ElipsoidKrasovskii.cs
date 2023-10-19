using Geodesy;

namespace Geodesy
{
    internal struct ElipsoidKrasovskii : IElipsoid
    {
        public double a => 6378245D; // Elipsoid semi major axis
        public double al => 1 / 298.3; // Elipsoid inverse flattening
        public double ePow2 => 2 * al - Math.Pow(al, 2);
    }
}