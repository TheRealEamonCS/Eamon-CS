# Eamon CS
### The Wonderful World of Eamon (C# Branch)

#### Note: the Wiki now serves as a development log, to be updated periodically with the current project status.

#### Last Wiki Update 20241013

#### Thanks to all who've starred this repository!  If you find the content enjoyable, please consider giving it a star to help others discover it.

This project is Eamon CS (ECS), a C# port of the classic Eamon roleplaying game that debuted on the Apple II.  Initially created by Donald Brown, many variants have existed over the years on various computer systems.  ECS is the production version of Eamon AC (EAC), a prototype intended to convert the game from BASIC.  Hopefully, this Eamon will be the definitive version for the C family of languages, as EAC is obsolete.

#### Prerequisites

Eamon CS includes precompiled binaries for Linux, macOS, and Windows that require the .NET 8 Runtime or SDK (for developers) installed on your system.  Unfortunately, having the .NET installer in the Eamon CS distribution is not practical, but the QuickLaunch files auto-detect missing packages and tell you where to find them.  There are instructions for obtaining and installing these [PREREQUISITES](https://TheRealEamonCS.github.io/pages/TechnicalPaper/TechnicalPaper.html#ECSTP2) in Technical Paper on the [Eamon CS website](https://TheRealEamonCS.github.io).

Eamon CS Mobile currently runs on devices using Android 8.0 and newer.

#### Installing

There is no formal installer for Eamon CS.  To obtain a copy of this repository, you can either do a Git Clone using Visual Studio 2022+ or, more simply, download a .zip file using the green Code button above.  You may need to eliminate security warning message boxes if you download a .zip file on macOS or Windows.  To do this, please see PREREQUISITES as mentioned above.

To obtain Eamon CS Mobile, download the [EamonPM.Android-Signed.apk](https://github.com/TheRealEamonCS/Eamon-CS-Misc/tree/master/System/Bin) file directly onto your mobile device and install it.

#### Playing

Inside the QuickLaunch directory, you'll find a platform-specific script called DetectDotnetAndConfigure.  Successfully running this script configures Eamon CS, making it playable. 

The Eamon CS game runner, built with Avalonia UI, offers a consistent user experience across all supported platforms and includes a Settings tab for customization.  Please see [SUGGESTED GAMEPLAY SETTINGS](https://TheRealEamonCS.github.io/pages/TechnicalPaper/TechnicalPaper.html#ECSTP6) in the Technical Paper for more details.

If helpful, you can move the generated EamonPM program shortcut to your desktop for easy access.

#### Contributing

Like all Eamons, ECS allows you to create adventures with no programming involved via the EamonDD data file editor.  But for the intrepid game designer, the system is infinitely extensible, using typical C# inheritance mechanisms.  The documentation has improved, and many adventures can be recompiled in Debug mode and stepped through to gain a better understanding of the system.  Please see [BUILDING NEW ADVENTURES](https://TheRealEamonCS.github.io/pages/TechnicalPaper/TechnicalPaper.html#ECSTP10) and associated sections in the Technical Paper for more details.

Feel free to contact me if you are interested in contributing to the Eamon CS project or wish to port your own game or build a new one.  I can provide insight if areas of the code need clarification.  Eamon has always been an ideal programmer's learning tool.  If you build a game, you aren't just contributing to the system; you're honing your skills as a C# developer while having fun!

#### Roadmap

The current plan is to produce fully polished games as time allows.  You'll get priority if you have an old BASIC game that you'd like to see ported and are willing to assist in that task (just through your insight).  Otherwise, the emphasis is quality over quantity.

There are currently plans to port Eamon CS Mobile to iOS.

Many third-party technologies can seamlessly integrate with ECS, some of which may push the game in new directions.  Stay tuned and see what comes of it.

#### License

Eamon CS is free software released under the MIT License.  Please see [LICENSE](https://TheRealEamonCS.github.io/pages/LICENSE.html) on the Eamon CS website for more details.

