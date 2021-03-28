
*** RELEASE NOTE 20210331 ***

My apologies for any hassles this may cause, but a difficult decision has been made to drop and recreate the Eamon CS repositories from scratch.  There may be other less disruptive ways to do this, but it is the quickest and easiest.  The reason for this is a lot of cruft has built up in the repositories over the years during development, mainly related to binary files; this was before I switched to the monthly check-in schedule.  Please clone the repositories again if you did so previously and wish to continue to monitor them.  Hopefully, this will be the one and only time this drastic action is taken.

Sadly, the repositories will lose their past check-in history, stars, forks, watchers, etc.  If you starred, forked, or watched any repo, PLEASE do so again; this will ensure Github's algorithms stay aware of it!

Thank you!

----------------------------------------------------------------------------------

Here are some additional comments detailing an important change in Eamon CS 1.8:

Converted the Eamon CS .XML file format to compressed .DAT files in the gzip format.  Huge space savings of 90% or more on average.  This upgrade is fully backward compatible with all existing .XML files, which will be upgraded in place to .DAT files when encountered.  For example, if you wish to use an existing CHARACTERS.XML file, drop a copy of it in System\Bin and enter the Main Hall.

Using the 7-Zip utility, a .DAT file's content can still be easily accessed and modified without extracting it.  Right-click on the .DAT file and open it using 7-Zip, then right-click on the content and choose the Edit menu option.  When the content is saved, it will automatically update the .DAT file.

Please note: if you upgrade to Eamon CS 1.8 and migrate any in-progress games, when you resume them, you should always promptly Restore each saved game slot.  It ensures that lingering .XML textfiles are properly upgraded to .DAT format.

Thank you!