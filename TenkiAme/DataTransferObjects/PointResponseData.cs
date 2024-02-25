namespace TenkiAme.DataTransferObjects
{
    public class PointResponseData
    {
        public Dictionary<string, VariableDetails> Variables { get; set; }

        public PointResponseData(//NoDataReasons noDataReasons, 
            Dictionary<string, VariableDetails> variables) 
        {
            this.Variables = variables;
        }
    }
}
