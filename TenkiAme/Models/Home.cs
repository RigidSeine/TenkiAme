using System.ComponentModel.DataAnnotations;

namespace TenkiAme.Models
{
    public class Home
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        [DataType(DataType.Date)] //Removes the requirement for time info and hides the time info as well
        public DateTime ReleaseDate { get; set; }
        public string? Genre { get; set; }
        public decimal Price { get; set; }
        public string Url { get; set; }

    }
}
