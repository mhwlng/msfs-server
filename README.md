# msfs-server

**EARLY PROTOTYPE, UNDER DEVELOPMENT**

Web server, **using blazor and net7**, that connects via simconnect to MSFS 2020 and serves a web page that shows a moving map and instruments.

You need dotnet 7 [.NET 7.0](https://dotnet.microsoft.com/download/dotnet/7.0)  install at least : .NET Desktop Runtime \ x64

The port number of the web server is defined in appsettings.json (default = 5002)

The web server URL and any error messages can be found in log.txt

You need to create an account on https://www.openaip.net and then create your own api key on https://www.openaip.net/users/clients#tab-clients

You then need to edit the file \wwwroot\js\config.js and update the openaip api key and AIRAC number :

```
var config = {

    // https://www.openaip.net/users/clients#tab-clients

    OPENAIP_KEY: "xxxxxxyyyyyyyyyyzzzzzzzzzz",

    // AIRAC 2301 = europe, see https://www.openflightmaps.org/ed-germany/ top right. Changes monthly !!!

    AIRAC : "2301"

}
```

![touch screen](https://i.imgur.com/4YI13mJ.jpg)


![touch screen](https://i.imgur.com/erLvZY7.jpg)


you need the deja vu font for the garmin G5. download ttf here

https://www.fontsquirrel.com/fonts/dejavu-sans

or on raspberry pi

sudo apt-get install fonts-dejavu


thanks to

https://github.com/kurt1288/msfs-flight-following

https://github.com/mracko/MSFS-Mobile-Companion-App

https://github.com/joeherwig/portable-sim-panels
