namespace Geodesy
{
    internal struct ElipsoidWsg84: IElipsoid
    {
        public double a => 6378137D; // Elipsoid semi major axis
        public double al => 1 / 298.257223563; // Elipsoid inverse flattening
        public double ePow2 => 2 * al - Math.Pow(al, 2);
    }
}
