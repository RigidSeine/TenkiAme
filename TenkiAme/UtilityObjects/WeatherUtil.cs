namespace TenkiAme.UtilityObjects
{
    public static class WeatherUtil
    {
        public static double? KelvinToCelsius(double? kelvin)
        {
            if (kelvin < 0) { return KelvinToCelsius(0); }
            if (kelvin == null ) { return 0; }
            return kelvin - 273.15;
        }

        //Count days, order list, loop through and compare DateTime.day
        //Use days to create list of lists of hours
        //Use list of lists to create list of tuples or object (Datetime, tempHigh, tempLow)

    }
}
