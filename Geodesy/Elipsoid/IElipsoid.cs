namespace Geodesy
{
    public interface IElipsoid
    {
        double a { get; } // Elipsoid semi major axis
        double al { get; } // Elipsoid inverse flattening
        double ePow2 { get; } // e^2
    }
}