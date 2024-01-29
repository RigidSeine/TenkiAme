namespace TenkiAme.DataTransferObjects
{
    public class PointResponseData
    {
        public NoDataReasons NoDataReasons { get; set; }
        public Dictionary<string, VariableDetails> Variables { get; set; }

        public PointResponseData(NoDataReasons noDataReasons, Dictionary<string, VariableDetails> variables) 
        {
            this.NoDataReasons = noDataReasons;
            this.Variables = variables;
        }
    }
}
