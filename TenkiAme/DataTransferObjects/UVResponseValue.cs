namespace TenkiAme.DataTransferObjects
{
    public class UVResponseValue
    {
        public DateTime Time { get; set; }
        public double Value {  get; set; }

        public UVResponseValue(DateTime time, double value)
        {
            this.Time = time;
            this.Value = value;
        }

        public DateTime GetNZTime()
        {
            var nzst = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(Time, nzst);
        }
    }
}
