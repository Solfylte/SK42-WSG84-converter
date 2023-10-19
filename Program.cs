using Geodesy;
using Geodesy.Internal;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        static void Main(string[] args)
        {
            double B0 = 49.07021761693201;
            double L0 = 38.014637957020966;

            Console.WriteLine("Coordinate conversion test (From SK42 to WSG84):");
            Console.WriteLine("Input:\tB=" + B0 + " L=" + L0);

            IElipsoid elipsoidKrasovskii = new ElipsoidKrasovskii();

            ICoordinatesFabric coordinatesFabric = new SK42CoordinatesFabric(elipsoidKrasovskii);
            IConversionService conversionService = new ConversionService(coordinatesFabric);

            ICoordinates coords0 = coordinatesFabric.CreateCoordByBL(CoordSystem.SK42, B0, L0);
            ICoordinates coords1 = conversionService.Convert(coords0, CoordSystem.WSG84);

            Console.WriteLine("Output:\tB=" + coords1.Longitude + " L=" + coords1.Latitude);
            Console.WriteLine("\n----------------------------------------");
            Console.WriteLine("Geodesy to Gaus-Kruger coordinates test:");

            double B1 = 49D;
            double L1 = 38D;

            Console.WriteLine("Input:\tB=" + B1 + " L=" + L1);

            ICoordinates coords2 = coordinatesFabric.CreateCoordByBL(CoordSystem.SK42, B1, L1);

            Console.WriteLine("Output:\tX=" + (int)coords2.X + " Y=" + (int)coords2.Y);
            Console.WriteLine("\n----------------------------------------");
            Console.WriteLine("Gaus-Kruger to Geodesy coordinates test:");

            double X = 5438000;
            double Y = 7428000;

            Console.WriteLine("Input:\tx=" + X + " y=" + Y);

            ICoordinates coords3 = coordinatesFabric.CreateCoordByXY(CoordSystem.SK42, X, Y);
            Console.WriteLine("Output:\tB=" + coords3.Longitude + " L=" + coords3.Latitude);

            Console.WriteLine("----------------------------------------");
        }
    }
}
