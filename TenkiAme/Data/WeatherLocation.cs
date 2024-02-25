using TenkiAme.DataTransferObjects;

namespace TenkiAme.Data
{
    public static class WeatherLocation
    {
        public static Dictionary<string, Point> Locations;

        static WeatherLocation()
        {
            Locations = new Dictionary<string, Point>
                {
                    {"Wellington", new Point(-41.2865, 174.7762) },
                    {"Auckland", new Point(-36.8507, 174.7645) }
                    // Add more locations here
                };
        }

        public static Point GetLocationCoordinates(string location)
        {
            return Locations[location];
        }
    }
}
