namespace TenkiAme.DataTransferObjects
{
    public class UVResponse
    {
        public List<UVResponseProduct> products { get; set; }

        public UVResponse() 
        {
            this.products = new List<UVResponseProduct>();
        }

        public void PrintToString()
        {
            foreach(var product in products)
            {
                product.PrintToString();
            }
        }
    }
}
