#!/bin/sh

# Copyright (c) 2014+ by Michael Penner.  All rights reserved.

dotnet_found()
{
	# Menu loop
	while true; do
		echo
		echo "Please select the type of system you're running on:"
		echo
		echo "1. osx-x64 (Intel/AMD)"
		echo "2. osx-arm64 (Arm)"
		echo
		read -p "Enter the number corresponding to your system: " CHOICE

		case "$CHOICE" in
			1)
				TARGET="osx-x64"
				break
				;;
			2)
				TARGET="osx-arm64"
				break
				;;
			*)
				echo
				echo "Invalid selection. Please try again."
				;;
		esac
	done

	PUBLISH_FILES="./publish/$TARGET/native/*"
	RUNTIME_FILES="./runtimes/osx/native/*"

	echo

	# Copy publish files and list them
	COUNT=0
	for file in $PUBLISH_FILES; do
		cp "$file" .
		echo "$file"
		COUNT=$((COUNT + 1))
	done
	echo "$COUNT File(s) copied"

	echo

	# Copy runtime files and list them
	COUNT=0
	for file in $RUNTIME_FILES; do
		cp "$file" .
		echo "$file"
		COUNT=$((COUNT + 1))
	done
	echo "$COUNT File(s) copied"

	# Extract script directory
	EXE_PATH="$BASE_PATH/System/Bin/EamonPM.Desktop"
	LNK_PATH="$BASE_PATH/QuickLaunch/MacOS/EamonPM.Desktop"
	ICON_PATH="$BASE_PATH/System/EamonPM.Desktop/Resources/ten_sided_die.icns"

	# Apply custom icon using fileicon utility
	if [ -f $EXE_PATH ]; then

		./fileicon -q set $EXE_PATH $ICON_PATH
	fi
	
	echo

	# Check if shortcut creation was successful
	if ln -sf $EXE_PATH $LNK_PATH >/dev/null 2>&1; then
		echo "Shortcut creation successful."
	else
		echo "Shortcut creation failed."
	fi

	echo

	# Prompt the user to press any key to continue
	read -p "Press Enter to continue . . . " enterKey

	echo
	
	exit 0
}

sleep 1 >/dev/null 2>&1

cd -- "$(dirname "$0")"

cd ../..

BASE_PATH=`pwd`

cd System/Bin

MACOS_PREREQUISITES_TXT="./MACOS_PREREQUISITES.TXT"
DOTNET_SDK_REGEX_TXT="./DOTNET_SDK_REGEX.TXT"
DOTNET_RUNTIME_REGEX_TXT="./DOTNET_RUNTIME_REGEX.TXT"

UN=$$

DOTNET_SDKS_TXT="./DOTNET_SDKS_$UN.TXT"
DOTNET_RUNTIMES_TXT="./DOTNET_RUNTIMES_$UN.TXT"

if [ ! -f $MACOS_PREREQUISITES_TXT ]
then
	dotnet_found $@
fi

DOTNET_SDK_REGEX=`cat $DOTNET_SDK_REGEX_TXT`
dotnet --list-sdks >$DOTNET_SDKS_TXT 2>&1
grep -E "$DOTNET_SDK_REGEX" $DOTNET_SDKS_TXT >/dev/null 2>&1
DOTNET_SDK_FOUND=$?
rm -f $DOTNET_SDKS_TXT >/dev/null 2>&1

if [ "$DOTNET_SDK_FOUND" -eq "0" ]
then
	dotnet_found $@
fi

DOTNET_RUNTIME_REGEX=`cat $DOTNET_RUNTIME_REGEX_TXT`
dotnet --list-runtimes >$DOTNET_RUNTIMES_TXT 2>&1
grep -E "$DOTNET_RUNTIME_REGEX" $DOTNET_RUNTIMES_TXT >/dev/null 2>&1
DOTNET_RUNTIME_FOUND=$?
rm -f $DOTNET_RUNTIMES_TXT >/dev/null 2>&1

if [ "$DOTNET_RUNTIME_FOUND" -eq "0" ]
then
	dotnet_found $@
fi

if command -v resize >/dev/null 2>&1; then
	resize -s 45 110 >/dev/null 2>&1
else
	printf '\033[8;45;110t'
fi

cat $MACOS_PREREQUISITES_TXT

read -p "Press Enter to continue . . . " enterKey

exit 1
