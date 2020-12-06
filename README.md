![alt text](https://github.com/artiomle/lonhand-new/blob/master/lonhand-logo.jpg)
# This project is created to monitor temerature in water boilers (or any other application)
    It based on wifi based relay (to control the power circuit of the boiler)
    (USR-WM1h 1 Output Wifi Remote)
    A raspberry pi running linux with mono as C# runtime framework 
   ``` 
   pi@raspberrypi ~ $ sudo apt-get update
   pi@raspberrypi ~ $ sudo apt-get install mono-complete
   ```
   
 # Connecting Temp Sensor (ds18b20) to raspberry pi
   ```
   https://tutorials-raspberrypi.com/raspberry-pi-temperature-sensor-1wire-ds18b20/
   ```
   
Important note 
```
In this version does not support port scan is implemented, therefore a config file needed to be added on same dir of the executable 

File name should be "db.xml"

The required fieled name is : LonHandIP

The value should include the actual ip address of the USR-WM1h device (of couse it should be on same network with the 
raspberrypi which should run the compiled code from this sln (TempGetGUI_Raspberry)
```
