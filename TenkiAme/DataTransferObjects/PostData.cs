namespace TenkiAme.DataTransferObjects
{

    public class PostData
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
    }

    public class PostResponse
    {
        public int Id { get; set; }
    }
}
