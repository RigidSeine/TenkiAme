namespace TenkiAme.DataTransferObjects
{
    public class NoDataReasons
    {
        public int Good { get; set; }
        public int MaskLand { get; set; }

        public NoDataReasons(int Good, int MaskLand)
        {
            this.Good = Good;
            this.MaskLand = MaskLand;
        }
    }
}
