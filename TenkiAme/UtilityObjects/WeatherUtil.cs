namespace TenkiAme.UtilityObjects
{
    public static class WeatherUtil
    {
        public static double KelvinToCelsius(double? kelvin)
        {
            if (kelvin < 0) { return KelvinToCelsius(0); }
            if (kelvin == null ) { return 0; }
            return (double) Math.Round((decimal) (kelvin - 273.15), 1);
        }

        public static double RoundPrecipitationRate(double? precipitationRate) 
        { 
            if(precipitationRate == null) { return 0.0; }
            return (double) Math.Abs(Math.Round((decimal)precipitationRate, 1));
        }

        public static string RoundTwilightTime(string twilightTime)
        {
            DateTime twilightTimeDate = DateTime.Parse(twilightTime);

            return twilightTimeDate.ToString("h:mmtt");
        }
    }
}
