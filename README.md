
# Tamagotchi

A microscopic Tamagotchi implementation that features the following.

1. Thread synchronisation, with some tricks to avoid thread issues, such as implementing ICloneable, etc.
2. State machines
3. Design patterns (mostly Singleton, which isn't the best design pattern arguably, but relevant for this little thing)
4. Etc ...

The actual Tamagotchi library (important parts), are implemented as a .Net Standard library, while the _"GUI"_ is a simple
.Net Core 3.0 Console Application.

<p align="center">
<img alt="A microscopic Tamagotchi implementation for .Net Core" title="A microscopic Tamagotchi implementation for .Net Core" src="https://servergardens.files.wordpress.com/2020/04/tamagotchi-screenshot.png" />
</p>


## Building

Should build easily with .Net Core CLI (Command Line Interface) by using e.g. `dotnet run` or something equivalent, even
though I personally use Visual Studio for Mac OS X myself.

## Testing the program out

Compile, run, and follow the instructions on the screen. Pressing 1-6 to interact with the Tamagotchi. There is a
`System.Threading.Timer` that is invoked every 5 seconds, slowly increamenting the parameters of the internally kept
Tamagotchi instance, which implies the Tamagotchi will die by itself if not interacted with for a couple of minutes.

## Purpose

Keep all the parameters of your Tamagotchi _below 100_. Once any of these reaches 100, the Tamagotchi dies!
