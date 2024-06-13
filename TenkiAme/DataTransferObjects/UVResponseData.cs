namespace TenkiAme.DataTransferObjects
{
    public class UVResponseData
    {
        List<UVResponseProduct> products;

        public UVResponseData() 
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
