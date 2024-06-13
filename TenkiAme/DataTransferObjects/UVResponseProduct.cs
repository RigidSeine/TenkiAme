using Microsoft.Identity.Client;
using TenkiAme.UtilityObjects;

namespace TenkiAme.DataTransferObjects
{
    public class UVResponseProduct
    {
        List<UVResponseValue> Values;
        string Name;

        public UVResponseProduct(List<UVResponseValue> values, string name)
        {
            this.Values = values;
            this.Name = name;
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
