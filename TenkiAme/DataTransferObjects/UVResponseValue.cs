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
    }
}
