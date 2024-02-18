#MVC - Model View Controller
- Model manages the state of the database and contains the business logic.
- View contains the user interface and handles all the user behaviour by passing values to the Controller. The view receives data from the Model so that the UI can be updated to reflect the current state of the data. These implemented using View Templates that have the extension .cshtml.
- The Controller handles all the requests, passes data from the view to the model.
- Great pattern for maintaining separation of concerns which is great for testing and scalability.

#Passing Data From a Controller to a View
- We use a dictionary called ViewData to store values in the controller so that can be referenced in the View Template.
```C#
ViewData["Name"] = "Gerbil";
```
- This can then be referenced in a similar way in the View Template by preceding the reference with an @ (since @ precedes any C# behaviour).

#Using an API key to Make a Call
- Include it as a header in the request. APIs keep changing what they call the header but it usually is some form of "api-key" or "authentication". Just check the documentation.
```C#
httpClient.DefaultRequestHeaders.Add("x-api-key", "11111");
```

#Making POST Requests
- When putting together the classes that form the JSON input, you must be very deliberate to match the format EXACTLY as the documentation has it. I.e. Check for JSON arrays (C# Lists), check for case-sensitivity AND DEFINITELY CHECK FOR STUPID DATE FORMATS.
-If case-sensentivity is an issue, you can use a Newtonsoft attribute to modify it to what it should be when it gets converted to JSON. In the example below, the JSON key needs to be in camelCase.
```C#
[JsonProperty]("from")
public DateTime From {get; set;} 
```
- Alternatively, sometimes it's not enough to be using ISO 8601 date time format in JSON Sometimes, the API will include a string literal in the datetime for whatever reason. 2024-01-21T00:00:00 IS NOT THE SAME AS 2024-01-21T00:00:00.000Z. Therefore, just create some Newtonsoft.JsonSerializerSettings and change the dateformat to include the literal.
```C#
var jsonSettings = new JsonSerializerSettings { DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ" };
var jsonData = JsonConvert.SerializeObject(pointPostData, jsonSettings);
var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
```

- After making the call, you can return a response. If the call was successful, then you can read the response JSON as a string and then deserialise using a class that has the properties you want to store in the response JSON.
  - The "container" classes don't need to have all the attributes in the JSON. It can simply have the attributes you want to store.

#Razor Pages vs. MVC 
- To use Razor Pages instead of MVC, include the following:
```C#
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
var app = builder.Build();
app.MapRazorPages();
```
-Otherwise to use MVC:
```C#
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.MapControllers(); //And or app.MapControllerRoute(name: "default",pattern: {controller=Home}/{action=Index}");
```
- You can technically use MVC without ```AddControllersWithViews()``` but you miss a lot of fundamental functionality.

#Routing - MVC
- Defines how each part of the web app's API can be accessed.
E.g. Placing
```C#
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/");
```
In the Program.cs file allows the following code in the HomeController class to be called using localport([PORTCODE])/Home. 
```C#
        public IActionResult Index()
        {
            return View();
        }
``` 
Alternatively, (or even concurrently) you can use ```app.MapControllers()``` in Program.cs and set the route using Route attributes written above the class or method.
```C#
    public class HomeController : Controller
    {
        //GET: /Home/
        [Route("Home")]
        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
```

#Middleware
- Forms the HTTP request-processing pipeline and generation of an HTTP response.
- Routing is part of this (since you need to route an HTTP request to some form of behaviour of the API).
- A lot of the Middleware methods start with "app.Use(...)"

#Async
- You can use Task.WhenAll(GetVariables()); if you want to perform async calls in sequence.

#LINQ
##Where()
- Similar to SQL in that foreach element (instead of record) the condition must return true to be selected.
- Can also use the ```index``` and ```value``` keywords to reference the index and value of the enumerable collection that Where() is being used on.
```C#
            foreach (var day in hoursByDay)
            {
                //Group the rain data by day - aka foreach index of rainData check if the
                //corresponding value in weatherTimeSeries matches the current group's date
                //and return the value (precipitation rate) if so
                var rainByDay = rainData.Where((value, index) => weatherTimeSeries[index].Date == day.Key).ToList();

                //Do the same for temperature
                var tempByDay = temperatureData.Where((value, index) => weatherTimeSeries[index].Date == day.Key).ToList();
            }
```

#Using an external CSS sheet in Views [TODO]
- To e Linking
- Caching
- <link rel="stylesheet" href="~/css/site.css?v=@DateTime.Now.Ticks"/> 

#Media Queries in CSS 
- Allow for the application of different styles depending on the characteristic of a user's device e.g. screen size, resolution, orientation, etc.
- E.g. If the current media is at least 768px in width, then set the font size for the html tag to be 16px, set main tag to have property of flex-wrap: nowrap and div tags to have property of flex-basis: 33%. If the width is less than 768px then the font size in the html tag will be 14px.
```CSS
html {
  font-size: 14px;
}

@media (min-width: 768px) {
  html {
    font-size: 16px;
  }
    main {
        flex-wrap: nowrap;
    }

    div {
        flex-basis: 33%;
    }
}
```

