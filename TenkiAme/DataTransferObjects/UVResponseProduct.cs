using Microsoft.Identity.Client;
using TenkiAme.UtilityObjects;

namespace TenkiAme.DataTransferObjects
{
    public class UVResponseProduct
    {
        public List<UVResponseValue> Values { get; set; }
        string Name { get; set; }

        public UVResponseProduct(List<UVResponseValue> values, string name)
        {
            this.Values = values;
            this.Name = name;
        }

        public double GetCurrentUVLevel()
        {
            //Get the current hour in NZST
            var currentNZHour = DateUtil.TruncateToHourStart(DateUtil.GetCurrentNZDateTime());

            try
            {
                //Find the current UV level
                return Values.Single(val => val.GetNZTime() == currentNZHour).Value;
            }
            catch (Exception ex)
            {
                DevUtil.PrintD("Error encountered in GetCurrentUVLevel " + ex.Message);
                return 0;
            }

        }

        public UVResponseValue? GetMaxUVLevelAndTime()
        {
            var currentNZDate = DateUtil.TruncateToDayStart(DateUtil.GetCurrentNZDateTime());

            try
            {
                //Filter out list of today's UV values
                var uVToday = Values.Where(val => DateUtil.TruncateToDayStart(val.GetNZTime()) == currentNZDate).ToList();
                //Return the max uv level and the corresponding hour
                return uVToday.MaxBy(val => val.Value);
            }
            catch (Exception ex)
            {
                DevUtil.PrintD("Error encountered in GetCurrentUVLevel " + ex.Message);
                return new UVResponseValue(currentNZDate, 0.0);
            }
        }

        //Assume reverse parabolic behaviour for UV levels during the day
        public List<UVResponseValue> CalculateUVSafetyThresholds()
        {
            var currentNZDate = DateUtil.TruncateToDayStart(DateUtil.GetCurrentNZDateTime());

            var thresholds = new List<UVResponseValue>();
            try
            {
                //Filter out list of today's UV values
                var uVToday = Values.Where(val => DateUtil.TruncateToDayStart(val.GetNZTime()) == currentNZDate).ToList();

                //Get the first UV level and hour where the UV level gets to 3
                //FirstOrDefault returns null on failing to return a valid value to avoid exceptions
                var risingThreshold = uVToday.FirstOrDefault(val => val.Value >= 3);

                //If null is returned, add dummy values
                //Otherwise add the rising threshold where UV levels start to get dangerous
                //Then find the descending threshold where UV levels go below 3 after the rising threshold.
                if (risingThreshold != null)
                {
                    thresholds.Add(risingThreshold);

                    var descendingThreshold = uVToday.FirstOrDefault(val => val.Value < 3 && val.Time > risingThreshold.Time);
                    thresholds.Add(descendingThreshold);
                }
                else
                {
                    thresholds.Add(new UVResponseValue(currentNZDate, 0.0));
                    thresholds.Add(new UVResponseValue(currentNZDate, 0.0));
                }

                return thresholds;
            }
            catch (Exception ex)
            {
                DevUtil.PrintD("Error encountered in GetCurrentUVLevel " + ex.Message);
                thresholds.Add(new UVResponseValue(currentNZDate, 0.0));
                thresholds.Add(new UVResponseValue(currentNZDate, 0.0));
                return thresholds;
            }
        }

        public string GetUVSafetyMessage()
        {
            var thresholds = this.CalculateUVSafetyThresholds();

            if (thresholds[0].Value == 0)
            {
                return "UV levels are safe today.\nEnjoy whatever sunlight is available.";
            }

            return "You should apply suncreen from " + thresholds[0].Time.ToString("t") + " today.\nUV levels will be safe from " + thresholds[1].Time.ToString("t") + ".";
        }

        public void PrintToString()
        {
            DevUtil.PrintD(Name);
            foreach (UVResponseValue val in Values) 
            {
                DevUtil.PrintD($"{val.Time.ToString()} {val.Value.ToString()}");
            };
        }
    }
}
