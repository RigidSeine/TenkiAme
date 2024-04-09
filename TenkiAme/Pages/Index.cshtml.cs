using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TenkiAme.Models;

namespace TenkiAme.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async void OnGet()
        {
            var weatherApiService = new WeatherAPIService();
            await weatherApiService.GetPointTimeSeriesAsync();
        }
    }
}
