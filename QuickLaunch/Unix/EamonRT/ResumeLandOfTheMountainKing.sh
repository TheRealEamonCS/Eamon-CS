#!/bin/sh

# Copyright (c) 2014+ by Michael Penner.  All rights reserved.

dotnet_found()
{
	xterm -e dotnet ./EamonPM.WindowsUnix.dll -pfn LandOfTheMountainKing.dll -wd ../../Adventures/LandOfTheMountainKing
	exit 0
}

cd ../../../System/Bin

UNIX_PREREQUISITES_SH="./UnixPrerequisites.sh"
UNIX_PREREQUISITES_TXT="./UNIX_PREREQUISITES.TXT"
DOTNET_SDK_REGEX_TXT="./DOTNET_SDK_REGEX.TXT"
DOTNET_RUNTIME_REGEX_TXT="./DOTNET_RUNTIME_REGEX.TXT"

UN=$$

DOTNET_SDKS_TXT="./DOTNET_SDKS_$UN.TXT"
DOTNET_RUNTIMES_TXT="./DOTNET_RUNTIMES_$UN.TXT"

if [ ! -f $UNIX_PREREQUISITES_TXT ]
then
	dotnet_found
fi

DOTNET_SDK_REGEX=`cat $DOTNET_SDK_REGEX_TXT`
dotnet --list-sdks >$DOTNET_SDKS_TXT 2>&1
grep "$DOTNET_SDK_REGEX" $DOTNET_SDKS_TXT >/dev/null 2>&1
DOTNET_SDK_FOUND=$?
rm -f $DOTNET_SDKS_TXT >/dev/null 2>&1

if [ "$DOTNET_SDK_FOUND" -eq "0" ]
then
	dotnet_found
fi

DOTNET_RUNTIME_REGEX=`cat $DOTNET_RUNTIME_REGEX_TXT`
dotnet --list-runtimes >$DOTNET_RUNTIMES_TXT 2>&1
grep "$DOTNET_RUNTIME_REGEX" $DOTNET_RUNTIMES_TXT >/dev/null 2>&1
DOTNET_RUNTIME_FOUND=$?
rm -f $DOTNET_RUNTIMES_TXT >/dev/null 2>&1

if [ "$DOTNET_RUNTIME_FOUND" -eq "0" ]
then
	dotnet_found
fi

xterm -geometry 100x45 -e $UNIX_PREREQUISITES_SH
exit 1
