namespace TenkiAme.DataTransferObjects
{
    public class VariableDetails
    {
        public string StandardName { get; set; }
        public string Units { get; set; }
        public List<string?>? Dimensions { get; set; }
        public List<double?>? Data {  get; set; }
        public List<int> NoData { get; set; }

        public VariableDetails(
            string standardName, 
            string units, 
            List<string>? dimensions, 
            List<double?>? data,
            List<int> NoData)
        {
            this.StandardName = standardName;
            this.Units = units;
            this.Dimensions = dimensions;
            this.Data = data;
            this.NoData = NoData;
        }
    }
}
