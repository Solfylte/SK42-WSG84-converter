using Geodesy.Internal;

namespace Geodesy
{
    internal class DatumFabric : IDatumFabric
    {
        private const string THE_SAME_COORDINATES_ERROR = "The coordinate system cannot be translated into itself ";
        private const string DATUM_NOT_FOUND_ERROR = "Datum not found.";

        private ICoordinates _coordinatesSource;
        private CoordSystem _toCoordSystem;

        public IConversionDatum GetConversionDatum(ICoordinates coordinatesSource, CoordSystem toCoordSystem)
        {
            _coordinatesSource = coordinatesSource;
            _toCoordSystem = toCoordSystem;

            if(IsTheSameCoordinatesSystems())
                throw new Exception($"{THE_SAME_COORDINATES_ERROR}: '{coordinatesSource.CoordSystem}' and '{toCoordSystem}'");

            if (IsWsg84Sk42())
                return new Wgs84Sk42ConversionDatum();
            // TO DO: other types of coordinates conversions

            throw new Exception(DATUM_NOT_FOUND_ERROR);
        }

        private bool IsWsg84Sk42() => (_coordinatesSource.CoordSystem == CoordSystem.SK42 && _toCoordSystem == CoordSystem.WSG84)
                                    || (_coordinatesSource.CoordSystem == CoordSystem.WSG84 && _toCoordSystem == CoordSystem.SK42);

        private bool IsTheSameCoordinatesSystems() => _coordinatesSource.CoordSystem == _toCoordSystem;
    }
}