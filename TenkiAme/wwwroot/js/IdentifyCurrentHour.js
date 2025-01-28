// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function GetCurrentNZTimeAsDate() {
    var currentDate = new Date();

    //Convert to NZ time as a string
    currentDate = currentDate.toLocaleString("en-NZ", { timeZone: "Pacific/Auckland" });

    //Split the string into date and time components as a list
    var splitDate = currentDate.split(", ");

    //Split the components into lists of the individual values
    var dateComponentStr = splitDate[0].split("/");
    var timeComponentStr = splitDate[1].replaceAll(":", ' ').split(' ');

    //Now convert the components into lists of integers
    var dateComponentInt = [];
    var timeComponentInt = []; 

    for (var i = 0; i < 3; i++) {
        dateComponentInt.push(parseInt(dateComponentStr[i]));
    }

    for (var i = 0; i < 3; i++) {
        timeComponentInt.push(parseInt(timeComponentStr[i]));
    }

    //If it's 1pm onwards then add 12 hours to the time component
    timeComponentInt[0] = timeComponentInt[0] < 12 && timeComponentStr[3] === 'pm' ? timeComponentInt[0] + 12 : timeComponentInt[0];

    return new Date(
        dateComponentInt[2], //Year
        dateComponentInt[1] - 1, //Month (goes from 0 to 11)
        dateComponentInt[0], //Day
        timeComponentInt[0], //Hours
        timeComponentInt[1], //Minutes
        timeComponentInt[2]  //Seconds
    );
}


//Get the current time
var currentDate = new Date();

//Since this is client-side, check if the current timezone is a NZ one
//If not, then convert to NZ time
if (Intl.DateTimeFormat().resolvedOptions().timeZone != "Pacific/Auckland") {
    currentDate = currentDate.toLocaleString("en-NZ", { timeZone: "Pacific/Auckland" });
}

var currentHour = currentDate.getHours();

//Don't bother scrolling 
if (currentHour < 2) {
    return;
}

var hourlyForecast = document.getElementById("hourlyForecast");

var currentDate = new Date();
currentDate.

var day0hour9 = document.getElementById("day0hour9");
hourlyForecast.scroll(24 / 48 * hourlyForecast.scrollWidth, 0)