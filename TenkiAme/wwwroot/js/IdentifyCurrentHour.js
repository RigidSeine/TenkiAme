function GetCurrentNZTimeAsDate() {
    var currentDate = new Date();

    //Convert to NZ time as a string
    currentDate = currentDate.toLocaleString("en-NZ", { timeZone: "Pacific/Auckland" });

    //Split the string into date and time components as a list
    var splitDate = currentDate.split(", ");

    //Split the components into lists of the individual values
    var dateComponent = splitDate[0].split("/");
    var timeComponent = splitDate[1].replaceAll(":", ' ').split(' ');

    //Now convert the components into lists of integers; 
    for (var i = 0; i < 3; i++) {
        dateComponent[i] = parseInt(dateComponent[i]);
    }

    for (var i = 0; i < 3; i++) {
        timeComponent[i] = parseInt(timeComponent[i]);
    }

    //If it's 1pm onwards then add 12 hours to the time component
    timeComponent[0] = timeComponent[0] < 12 && timeComponent[3] === 'pm' ? timeComponent[0] + 12 : timeComponent[0];

    return new Date(
        dateComponent[2], //Year
        dateComponent[1] - 1, //Month (goes from 0 to 11)
        dateComponent[0], //Day
        timeComponent[0], //Hours
        timeComponent[1], //Minutes
        timeComponent[2]  //Seconds
    );
};


//Get the current time
var today = new Date();

//Since this is client-side, check if the current timezone is a NZ one
//If not, then convert to NZ time
if (Intl.DateTimeFormat().resolvedOptions().timeZone != "Pacific/Auckland") {
    today = GetCurrentNZTimeAsDate();
}

var currentHour = today.getHours();

//Don't bother scrolling if it's the beginning of the day
if (currentHour >= 2) {
    //Get reference to the hourly forecast box
    var hourlyForecast = document.getElementById("hourlyForecast");

    //Get reference to the current hour in the forecast box
    var currentHourElement = document.getElementById("day0hour" + currentHour);

    //Highlight the hour
    currentHourElement.className += ' dark-highlight';

    //Make the children wider to match the change by letting it flow free and hang loose
    var currentHourTimeElement = currentHourElement.children["day0hour" + currentHour + "Time"];

    if (currentHourTimeElement) {
        currentHourTimeElement.classList.remove("width-hour");
    }

    //Scroll to the current hour
    hourlyForecast.scroll((currentHour - 0.7) / 48 * hourlyForecast.scrollWidth, 0);
}

