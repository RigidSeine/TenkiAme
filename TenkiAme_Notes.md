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

#Routing
- Defines how each part of the web app's API can be accessed.
E.g. Placing
```C#
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
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



