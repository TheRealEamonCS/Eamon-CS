# Eamon CS
### The Wonderful World of Eamon (C# Branch)

#### Note: the Wiki is now serving as a development log; it will be updated periodically with the current project status.

#### Last Wiki Update 20210101

This is Eamon CS (ECS), a C# port of the classic Eamon roleplaying game that debuted on the Apple II.  Initially created by Donald Brown, there have been many variants over the years on various computer systems.  ECS is the production version of Eamon AC (EAC), a prototype intended to convert the game from BASIC.  Hopefully, this Eamon will be the definitive version for the C family of languages, as EAC is obsolete.

#### Prerequisites

Eamon CS requires a .NET Standard 2.0 compliant runtime; for example, .NET 4.7.2+ on Windows and Mono 5.18.0+ on Unix.  Also needed is the .NET Core 2.X runtime or SDK (for developers).  There are instructions for obtaining and installing these prerequisites, if necessary, in [Technical Paper](https://TheRealEamonCS.github.io/pages/TechnicalPaper/TechnicalPaper.html) under DOCUMENT LINKS on the [Eamon CS website](https://TheRealEamonCS.github.io).

Eamon CS Mobile currently runs on devices using Android 4.0 through 10.0.

#### Installing

There is no formal installer for Eamon CS.  To obtain a copy of this repository (and a full set of binaries), you can either do a Git Clone using Visual Studio 2019+ or, more simply, download a .zip file using the green Code button above.  If you download a .zip file on Windows, before unzipping it, you should right-click on it and select Properties.  Then click the Unblock checkbox (or button) in the lower right corner of the form, and finally click Apply and OK.  The gameplay experience will improve by eliminating security warning message boxes.

To obtain Eamon CS Mobile, download the [EamonPM.Android-Signed.apk](https://github.com/TheRealEamonCS/Eamon-CS-Misc/tree/master/System/Bin) file directly onto your mobile device and install it.

#### Playing

ECS programs are launched using a collection of batch files (or shell scripts in Unix) located under the QuickLaunch directory.  You can create a shortcut to this folder on your desktop for easy access to the system.

ECS Mobile mirrors the hierarchical directory structure of ECS Desktop, making the experience very similar.

#### Contributing

Like all Eamons, ECS allows you to create adventures with no programming involved, via the EamonDD data file editor.  But for the intrepid game designer, the engine is infinitely extensible, using typical C# subclassing mechanisms.  The documentation has improved, and many adventures can be recompiled in Debug mode and stepped through to gain a better understanding of the system.  See Technical Paper under DOCUMENT LINKS on the Eamon CS website for more details.

If you are interested in contributing to the Eamon CS project or wish to port your own game or build a new one, please contact me.  I can provide insight if there are areas of the code that need clarification.  Eamon has always been an ideal programmer's learning tool.  If you build a game, you aren't just contributing to the system; you're honing your skills as a C# developer while having fun doing it!

#### Roadmap

The current plan is to produce fully polished games as time allows.  If you have an old BASIC game that you'd like to see ported and are willing to assist in that task (just through your insight), you'll get priority.  Otherwise, the emphasis is quality over quantity.

There are currently plans to port Eamon CS Mobile to iOS.

Many 3rd party technologies can seamlessly integrate with ECS, some of which may push the game in new directions.  Stay tuned and see what comes of it.

#### License

Eamon CS is free software released under the MIT License.  See [LICENSE](https://TheRealEamonCS.github.io/pages/LICENSE.html) under DOCUMENT LINKS on the Eamon CS website for more details.

