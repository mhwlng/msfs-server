# msfs-server

Web server, **using blazor and net8**, that connects via simconnect to MSFS 2020 and serves a web page that shows a moving map and instruments.

You need [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)  install at least : .NET Desktop Runtime \ x64

The port number of the web server is defined in appsettings.json (default = 5002)

The web server URL and any error messages can be found in log.txt

**This project requires an MQTT broker:**

All collected data from MSFS is also sent to an MQTT broker. 

The MQTT connection Settings are located in appsettings.json.

**Note that the MQTT data is not sent, until a connection is made with the web server first.**

![mqtt](https://i.imgur.com/UlSnpDn.png)

You need to create an account on https://www.openaip.net and then create your own api key on https://www.openaip.net/users/clients#tab-clients

You then need to edit the file \wwwroot\js\config.js and update the openaip api key and AIRAC number :

```
var config = {

    // https://www.openaip.net/users/clients#tab-clients

    OPENAIP_KEY: "xxxxxxyyyyyyyyyyzzzzzzzzzz",

    // AIRAC 2313 = europe, see https://www.openflightmaps.org/ed-germany/ top right. Changes monthly !!!

    AIRAC : "2313"

}
```

![touch screen](https://i.imgur.com/PDDLZTq.jpg)

The 8.8 inch touch screen, in above picture, is connected to a raspberry pi.
More information can be found here:

https://github.com/mhwlng/kiosk-server

The 3 Dials have an ESP32 processor and are made by M5Stack :

https://shop.m5stack.com/products/m5stack-dial-esp32-s3-smart-rotary-knob-w-1-28-round-touch-screen

The 3d-printed enclosure can be found here:

https://www.printables.com/@mhwlng_888536/collections/920676


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

# Arduino

This project includes various arduino applications, that show flight instruments on M5Stack dial displays (ESP32) :

https://shop.m5stack.com/products/m5stack-dial-esp32-s3-smart-rotary-knob-w-1-28-round-touch-screen


note that espMqttClient.h references this specific library:

https://github.com/bertmelis/espMqttClient

The heading indicator requires dial.bmp and plane.bmp to be uploaded to SPIFFS
These bitmaps have 8-bit colors


thanks to

https://hackaday.io/project/188839-gc9a01-flight-displays

https://www.hackster.io/SeeedStudio/wio-terminal-attitude-indicator-eae8d6

https://en.m.wikipedia.org/wiki/File:Heading_indicator.svg



