# Raspkate
Raspkate is a small, lightweight web server that can both hosts static files and provides RESTful services. Raspkate is also open for extension, which means you can extend it to meet your business needs on top of the HTTP request and response contexts without having to know much detail about the underlying implementation.

User Scenarios
--
You can use Raspkate as:

- A small HTTP server embedded in your application to provide powerful HTML and Javascript hosting
- A standalone system service to serve your HTTP requests
- A RESTful service to receive HTTP requests and provide RESTful APIs
- A lightweight web server running on a small device (like Raspberry Pi) to provide professional HTML user interfaces for manipulating the device

Source Code
--
Raspkate was developed with Microsoft Visual Studio 2013 in C# language, with .NET Framework 4.5.2. It can also be compiled and run under Linux system on top of the Mono framework. This project is licensed under [GPL v2.0](http://www.gnu.org/licenses/old-licenses/gpl-2.0.en.html), as one of its dependencies used for Raspberry Pi compatibility, was released under this license.

### Prerequisites
For development in Windows:

- Windows 7, 8, 8.1, 10
- Visual Studio 2013/2015 with .NET Framework 4.5.2

For run in Linux:

- Ubuntu server 14.04.4 LTS was qualified
- [Raspbian JESSIE](https://www.raspberrypi.org/downloads/raspbian/) was qualified
- Mono 4.2.2 or above was qualified

### How to Use the Code
Before compile and run, you should firstly either download the source code as a zip archive or clone the source repository to your local simply by the following git command:

`git clone https://github.com/daxnet/raspkate.git`

- For development in Windows, you can open the `raspkate.sln` file directly with Microsoft Visual Studio 2013 or Microsoft Visual Studio 2015 and build the entire solution. 

- For compilation in Windows, you can run the `build.bat` with `Debug` or `Release` as its parameter to compile the source code. `msbuild` in both 12.0 and 14.0 should be supported.

- For compilation in Linux, you need firstly install the Mono framework. For more information about how to install Mono, please click [here](http://www.mono-project.com/docs/compiling-mono/linux/) for the steps. After you have successfully installed Mono framework, run the `build.sh` with either `Debug` or `Release` as its parameter to compile the source code.

- For running in both Windows and Linux, after the successful compilation in either platform, go into the `bin` folder which is a sub folder of `src`, and find the executable file named `RaspkateService.exe` under the output folder of corresponding configuration (e.g. `Debug` or `Release`), execute it directly, you will see a console output like following:

![Service Started](https://raw.githubusercontent.com/wiki/daxnet/raspkate/img/ServiceStarted.png)

- After successfully running the server, you can access `http://127.0.0.1:9023` in your web browser to check your server information, like below:
 
![Server Information](https://raw.githubusercontent.com/wiki/daxnet/raspkate/img/Congrats.png)

> Important Note: If you are going to load `Raspkate.RaspberryPi` module during the server startup, in order that to be able to access the functionality specific to Raspberry Pi device, you should run Raspkate service under Raspberry Pi by using the command `sudo ./RaspkateService.exe` with the root priviledge granted.

Documentation
--
(T.B.D)