using DrugRegistry.API.Domain;

namespace DrugRegistry.API.Utils;

public static class GeoUtils
{
    /// <summary>
    /// Calculates the distance in meters between two locations on Earth.
    /// </summary>
    /// <param name="l1">The first location.</param>
    /// <param name="l2">The second location.</param>
    /// <returns>The distance in meters between the two locations.</returns>
    /// <remarks>
    /// This method uses the Haversine formula to calculate the distance between two points
    /// on a sphere (in this case, the Earth) given their longitudes and latitudes.
    /// </remarks>
    public static double GetDistanceBetweenLocations(Location l1, Location l2)
    {
        var (longitude, otherLongitude, latitude, otherLatitude) =
            (l1.Longitude, l2.Longitude, l1.Latitude, l2.Latitude);
        var d1 = latitude * (Math.PI / 180.0);
        var num1 = longitude * (Math.PI / 180.0);
        var d2 = otherLatitude * (Math.PI / 180.0);
        var num2 = otherLongitude * (Math.PI / 180.0) - num1;
        var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                 Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

        return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
    }
}