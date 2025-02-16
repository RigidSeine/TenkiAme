# MVC - Model View Controller
- Model manages the state of the database and contains the business logic.
- View contains the user interface and handles all the user behaviour by passing values to the Controller. The view receives data from the Model so that the UI can be updated to reflect the current state of the data. These implemented using View Templates that have the extension .cshtml.
- The Controller handles all the requests, passes data from the view to the model.
- Great pattern for maintaining separation of concerns which is great for testing and scalability.

# Passing Data From a Controller to a View
- We use a dictionary called ViewData to store values in the controller so that can be referenced in the View Template.
```C#
ViewData["Name"] = "Gerbil";
```
- This can then be referenced in a similar way in the View Template by preceding the reference with an @ (since @ precedes any C# behaviour).

# Using an API key to Make a Call
- Include it as a header in the request. APIs keep changing what they call the header but it usually is some form of "api-key" or "authentication". Just check the documentation.
```C#
httpClient.DefaultRequestHeaders.Add("x-api-key", "11111");
```

# Making POST Requests
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
- Deserialising from a JSON response into a C# tends to be more friendly with case-sensitivity so `[JsonProperty]` tends to not be required for classes created for deserialisation.

# Secrets
- Since using Azure Key Vault is a no-go due to how complex it was, we're going with `environment variables` for storing our API keys in production, and `User Secrets` in dev.
- Turns out C# now has a [`DotNetEnv`](https://www.nuget.org/packages/DotNetEnv) package which allows easy load of a `.env` file so going to be using that instead. 
## Steps Taken (Easy)
1. Install DotNetEnv package
2. Create `.env` file and populate it with the necessary keys e.g. `API_KEY=12345`
3. Use `Env.Load()` during setup (in `Program.cs`) to load the `.env` variables into the Environment.
4. Retrieve the keys from wherever using `System.Environment.GetEnvironmentVariable("API_KEY")`.


# Razor Pages vs. MVC 
- To use Razor Pages instead of MVC, include the following:
```C#
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
var app = builder.Build();
app.MapRazorPages();
```
- Otherwise to use MVC:
```C#
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.MapControllers(); //And or app.MapControllerRoute(name: "default",pattern: {controller=Home}/{action=Index}");
```
- You can technically use MVC without ```AddControllersWithViews()``` but you miss a lot of fundamental functionality.

# Routing - MVC
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

# Middleware
- Forms the HTTP request-processing pipeline and generation of an HTTP response.
- Routing is part of this (since you need to route an HTTP request to some form of behaviour of the API).
- A lot of the Middleware methods start with "app.Use(...)"

# Async
- You can use Task.WhenAll(GetVariables()); if you want to perform async calls in sequence.

# LINQ
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

# Using an external CSS sheet in Views [TODO]
- To e Linking
- Caching
- <link rel="stylesheet" href="~/css/site.css?v=@DateTime.Now.Ticks"/> 
- ?v=@DateTime.Now.Ticks is form of cache-busting to fetch CSS file from the server instead of the browser cache. This is useful when you update the CSS file and want to see the changes immediately. This is FOR DEV ONLY.


# Media Queries in CSS 
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

# Getting the Hourly Forecast to Scroll to The Current Hour
- Wrote IdentifyCurrentHour.js to solve this - this resolved half the problem and was made much easier upon learning that this needed to be done in JS rather than C#.
  - The key was to make use of the forecast box's scrollwidth and everything fell into place after that.
- The half of the problem is that current design of the forecast box doesn't really allow for marking the current hour.
- Therefore drew more inspiration from P4's persona selection and BBC weather for the new design.
- This meant scrapping the idea of having persisting headers as the user scrolls like in MetService.

# Images
- Similar to a snapshot, an image contains all the data on a disk at a specific moment in time in a serialised format. E.g. For backups.
- Images will also contain 
- Snapshots

# Dockerfiles
- VS can create a Dockerfile for you if you created the app in VS itself.
- The file contains a script on how to build a Docker image 

# Azure Key Vault
- In order to create a secret, the user must have the `Key Vault Administrator` usergroup for the vault's Resource Group. Even if they're the owner of the subscription.

# Unix Commands
- chmod changes the permissions of a file. 
- ```chmod 400  [foldername]``` set's the file's permissions so that only the file owner has read permission, and everyone else has no permissions.
- ```chmod 666 [foldername]``` means that all users can read and write but cannot execute the file/folder.
- ```chmod 777 [foldername]``` will give read, write, and execute permissions for everyone. 
- ```chown [new_owner] [foldername]``` sets the owner of the [foldername] to the [new_owner]
- ```systemctl enable [application].service``` enables a Linux service defined with a .service file. - See https://www.digitalocean.com/community/tutorials/how-to-use-systemctl-to-manage-systemd-services-and-units
- ```systemctl start [application].service``` starts the Linux service. The .service on the end is optional.
- ```ls -l``` lets you not only list the files in the current directory but also their owners and group-owners
```
~$ ls -l
drwxr-xr-x  2 owner group 4096 Aug 12 19:12 Desktop
...
```

# Nginx Commands
- https://docs.nginx.com/nginx/admin-guide/basic-functionality/runtime-control/

# Using an SSH private key
- In order to use an SSH private key, the permissions on the file must be set to read-only for the current user.
- This can be achieved on Linux using the chmod 400 command or by changing the security settings in file explorer on the key itself by setting the owner to be the user currently logged in. https://amitpatriwala.wordpress.com/2023/08/17/windows-ssh-permissions-for-private-key-are-too-open/. There is also a non-GUI version for this in Github.

# Set up Docker in a VM
- Create VM
- Get SSH keys for VM
- SSH log in to VM
  [To be continued?]
 
 # Deploy to web options - ranked easiest to hardest
 - Azure App Service
 - Azure Container Instances/Apps (if dockerising)
 - IIS

```
[RETROSPECTIVE]: I used none of these and just pushed my published binary files onto VM, set up a web server (nginx being the choice of software) and launched the app as a Linux service. 
```

 ## General web server (e.g. Kestrel + Nginx, Apache, etc.) on another server (VM, physical machine)
 - First part from https://www.youtube.com/watch?v=cpkX9mScZEU
 - Deploy VM with SSH port open
 - SSH log in to VM
 - Run ```sudo apt-get update``` to update apt-get. Apt-get is the Advanced Package Tool for Linux.
 - Run ```sudo apt-get install dotnet-sdk-8.0``` to install .NET 8.0's SDK. This version number changes depend on what .NET version your app is developed in.
   - If you run ```sudo apt-get install dotnet-runtime-8.0``` you should get a message saying that it's already installed.
   - The same should happen if you run ```sudo apt-get install aspnetcore-runtime-8.0```.
   - A good check is to run ```dotnet --info```
  - Now do something similar to run nginx: ```sudo apt-get install nginx```
    - You can check if nginx was installed correctly by visiting the IP address of your VM (just use your local machine's browser). 
    - You will need to open up port 80 (HTTP) or port 443 (HTTPS) on your VM in order for this to work.
  - Next run ```sudo nano /etc/nginx/sites-available/default``` to edit the default nginx configration file
  - Where it says location, replace the contents with the following and save the file.
  ```location / {
       proxy_pass http://localhost:5000;
       proxy_http_version 1.1;
       proxy_set_header Upgrade $http_upgrade;
       proxy_set_header Connection keep-alive;
       proxy_set_header Host $host;
       proxy_cache_bypass $http_upgrade;
       proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
       proxy_set_header X-Forwarded-Proto $scheme;
   }
   ``` 
```
[RETROSPECTIVE]: The server name should match the domain name, not just '_', and root directory should be the root directory of the app (this will be relevant after you run `sudo mkdir [APP_NAME]` later)
```
  - Now launch/create the web app and edit the launchSettings.json file and change the "applicationUrl" property value to listen on the port that was exposed in the nginx configuration file proxy settings.
  - Now switch to the /var/www folder in your VM using ```cd /var/www```
  - Make the new directory called "app" in it using ```sudo mkdir app```
```
[RETROSPECTIVE]: Don't call the directory "app". Give it a name appropriate to the actual app. You might create other directories here later for other apps.
```
  - Set permissions on the folder using ```sudo chmod 666 app```
  - Change the owner to the current user using ```sudo chown tenkiame app```
  - Use SCP to try and a transfer the app's files (the contents of the publish folder - check the destination in the solution's publish profile) into /var/www/app. WinSCP is great for this.
    - If you are stopped by a lack of permissions, you can use ```sudo chmod 777 app```. Be sure to reset this permission later.
  - You can check if the app is working by running ```sudo dotnet TenkiAme.dll```
  - Create a `.service` file in `/etc/systemd/system` folder of the VM to configure how Linux should run your app.

```nginx
[Unit]
##Unit Section describes the service. i.e. Metadata
Description=TenkiAme

[Service]
##Service Section contains the configuration options
WorkingDirectory=/var/www/app/
#Specifies the command to start the service. It runs the `dotnet` command with the specified .DLL file (TenkiAme.dll)
ExecStart=/usr/bin/dotnet /var/www/app/TenkiAme.dll
#Specifies that the service should restarted automatically if it exits unexpectedly
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
#Specifies the signal to use when stopping the service. `SIGINT` is the interrupt signal
KillSignal=SIGINT
#Identifies the service is syslog messages
SyslogIdentifier=dotnet-TenkiAme
#Specifies the user under which the service should be run. Set to root by default.
User=tenkiame
#Run in production mode
Environment=ASPNETCORE_ENVIRONMENT=Production
#Disable telemetry messages for .NET Core
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
##Install Section Specifies whether the service should be enabled or started automatically at boot time.
#Specifies the target this service should be started under. `multi-user.target` is standard for systems with
#multiple users.
WantedBy=multi-user.target
```

  - Now enable the service you just created with ```sudo systemctl enable /etc/systemd/system/TenkiAme.service```
    - You should get a message like ```Created symlink /etc/systemd/system/multi-user.target.wants/TenkiAme.service → /etc/systemd/system/TenkiAme.service.```
  - Now run the service with ```sudo systemctl start TenkiAme.service```
  - Now reload nginx using ```sudo nginx -s reload```
    - If you run into an error saying ```nginx: [error] open() "/run/nginx.pid" failed (2: No such file or directory)``` then it means that the `nginx.pid` is missing from the directory when nginx should have created it automatically.
    - To force this, you can run ```sudo service nginx restart``` which should create the file
    - Then you can run ```sudo nginx -s reload -t``` as a test.
    - And finally run the command again without the `-t` modifier. Then check the website out on your browser using either the IP address or domain name.
## Problems encountered:
 - Error 504 Gateway Time-out or Error 500 after final step
   -  Make sure `proxy_pass http://localhost:5000;` in the `sites-available/default` file align with the port opened in the `launchSettings.json` file.

# Making Updates to The App
- Test the changes locally (or in a docker container).
- Publish the build.
- Push the build files to the remote server.
- Run the following command to restart the service `sudo systemctl restart TenkiAme.service`.
  - If you get an error message like `Unit etc-systemd-system-TenkiAme.service.mount not found`, then try navigating to `/etc/systemd/system/multi-user.target.wants/` first before running the command again.
- Visit the website to check if the changes were made.

 # Reverse Proxy
 - A proxy server that appears to be an ordinary web server, but actually acts as an intermediary that forwards the client's request to to one or more ordinary web servers. 



 # Misc. Glossary
 - SCP - Secure Copy Protocol is a means of securely transferring computer files between a local host and a remote host or between two remote hosts.

# Getting the Domain Name
- Comes before installing certbot
- Bought from Cloudflare
- Bound to the IP address (of the VM) on Cloudflare by adding an A record (or two for www) (under DNS > Records)

# Installing Certbot
- Certbot automatically renews your certificates for you and can even change your nginx conf file, just go to https://certbot.eff.org/ and choose what system and web server software you're using. Instructions for installation will appear afterwards.

## ERRORS ENCOUNTERED + Change log
- **ERR_SSL_VERSION_OR_CIPHER_MISMATCH** - this later morphed into TOO_MANY_REDIRECTS upon revisiting the problem after a few hours.
- https://certbot.eff.org/pages/help contains links for debugging the connection issue.
- **TOO_MANY_REDIRECTS** was caused by a SSL/TLS setting on Cloudflare's side since the domain was bought from Cloudflare - this was suggested by https://letsdebug.net/ and https://community.cloudflare.com/t/website-in-redirect-loop-after-enabling-cloudflare-ssl/452212/2. The recommended (required) setting is that SSL option is set to 'Full SSL (strict)'.
  - Funnily enough, pausing/stopping Cloudflare allows for a visit to the site with HTTPS.
- https://world.siteground.com/kb/err-ssl-version-or-cipher-mismatch/ contains more info about ERR_SSL_VERSION_OR_CIPHER_MISMATCH.
- ```tenkiame.org is currently unable to handle this request. HTTP ERROR 500``` appeared the next day.
  - (Relevant) steps taken:
    - Checked that it's not a Cloudflare issue by temporarily disabling it.
    - SSH into the VM, 
    - Checked if there is a syntax issue with the config file using `sudo nginx -t`
    - Checked the Nginx error log by using `less /var/log/nginx/error.log`.
      - Got `2024/04/24 22:36:57 [crit] 18470#18470: *4370 SSL_do_handshake() failed (SSL: error:0A00006C:SSL routines::bad key share) while SSL handshaking, client: 175.30.48.153, server: 0.0.0.0:443`
      - The error resolved itself after half a day OR it was a result of turning off DNSSEC in the Cloudflare DNS settings.
  - [UPDATE 26-May-2024] - The problem returned. Therefore no correlation with DNSSEC.
    - Followed the above steps and checked the error log only to find it empty.
    - Function came back 20-30 minutes after checking.
  - [UPDATE 03/06/2024] - Current 500 problem was identified to be caused by favicon retrieval. `<link rel="icon" href="~/favicon.ico" type="image/x-icon" />`
    - The specific problem comes from the `type="image/x-icon"`. Removing this allowed for everything to work. 
    [UPDATE 09/06/2024] - A change was made to replace the API key the prior night and the app broke again. 
      - A restoration was made to the MVP version which got it working again after a restart to the VM.
      - However, it is broken again after no more changes. This is probably caused by the original problem which seems to be intermittent.
      - Time to implement logging.
- `curl: (35) schannel: next InitializeSecurityContext failed: Unknown error (0x80092012) - The revocation function was unable to check revocation for the certificate.` was encountered trying to run the curl command for the Niwa UV API.
  - Appending `--ssl-no-revoke` as an additional parameter to the command bypassed this issue. I trust Niwa enough. 
- `ssh: connect to host 20.55.34.183 port 22: Connection timed out` - Check the inbound port rules. Your IP address might have changed.
- **The target process exited without raising a CoreCLR started event. Ensure that the target process is configured to use .NET Core. This may be expected if the target process did not run on .NET Core.** - Encountered upon running the app locally after a few months of paused development.
  - Many guides say to run `dotnet --list-sdks` and `dotnet --list-runtimes` to make sure you're not missing the appropriate runtime and sdk for running and building.
  - Then to create a new project and see if it works. It didn't this time around.
  - The solution that did work this time around was to update Visual Studio Installer.
- **MSB3021 - Unable to copy file source to destination. Access to the path destination is denied.**
  - Check Avast antivirus settings. Avast might be quarantining/adding protection around files it doesn't need to.