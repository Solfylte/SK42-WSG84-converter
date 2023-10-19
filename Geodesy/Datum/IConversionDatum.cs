namespace Geodesy
{
    internal interface IConversionDatum
    {
        CoordSystem FirstCoordSystem { get; }
        CoordSystem SecondCoordSystem { get; }

        IElipsoid FirstElipsoid { get; }
        IElipsoid SecondElipsoid { get; }

        // linear transformation coeficionts
        double dx { get; }
        double dy { get; }
        double dz { get; }

        // angular transformation coeficients
        double wx { get; }
        double wy { get; }
        double wz { get; }

        // scale coeficient
        double ms { get; }
    }
}
