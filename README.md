# **GTMS-Telemetry-Software**

Telemetry and data logging software developed for and used by Georgia Tech Motorsports. Records and logs data such as ground speed, engine rpm, and gear position. Attempting to close the UI prompts the user as to whether or not data should be logged. 

![](https://i.imgsafe.org/0d8ca9fe7d.png)

Also generates live graphs such as kRPV vs Time and kRPM vs. Speed:

![enter image description here](https://i.imgsafe.org/0d9c939b35.png)

## Installation

The program reads incoming data received by an Arduino via a USB serial port connection. To start, download all files and upload testTelemetry.ino to an Arduino. This sketch will simply have the Arduino output "dummy" data. Next, open the program (either in Visual Studio or somewhere else) and add the Aquagauge.dll file (developed by Ambalavanar Thirugnanam) as a reference. In addition, install LCD-N font. Finally, connect the Arduino to the computer, select the corresponding COM port, and run the program.

## License

Copyright (c) Richard Chen.  Distributed under the [MIT license](https://opensource.org/licenses/MIT).


