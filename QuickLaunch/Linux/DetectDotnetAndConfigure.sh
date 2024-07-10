#!/bin/sh

# Copyright (c) 2014+ by Michael Penner.  All rights reserved.

dotnet_found()
{
	# Menu loop
	while true; do
		echo
		echo "Please select the type of system you're running on:"
		echo
		echo "1. linux-x64 (Intel/AMD)"
		echo "2. linux-arm64 (Arm)"
		echo
		read -p "Enter the number corresponding to your system: " CHOICE

		case "$CHOICE" in
			1)
				TARGET="linux-x64"
				break
				;;
			2)
				TARGET="linux-arm64"
				break
				;;
			*)
				echo
				echo "Invalid selection. Please try again."
				;;
		esac
	done

	PUBLISH_FILES="./publish/$TARGET/native/*"
	RUNTIME_FILES="./runtimes/$TARGET/native/*"

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
	DTF_PATH="$BASE_PATH/QuickLaunch/Linux/EamonPM.Desktop.desktop"
	LNK_PATH="$BASE_PATH/QuickLaunch/Linux/EamonPM.Desktop"

	# Create a .desktop file
	echo "[Desktop Entry]" > $DTF_PATH
	echo "Version=3.0" >> $DTF_PATH
	echo "Name=EamonPM.Desktop" >> $DTF_PATH
	echo "Exec=$EXE_PATH" >> $DTF_PATH
	echo "Comment=EamonPM.Desktop Application" >> $DTF_PATH
	echo "Icon=$BASE_PATH/System/EamonPM.Desktop/Resources/ten_sided_die.png" >> $DTF_PATH
	echo "Terminal=false" >> $DTF_PATH
	echo "Type=Application" >> $DTF_PATH
	echo "Categories=Utility;" >> $DTF_PATH

	echo

	# Check if .desktop file creation was successful
	if [ -f $DTF_PATH ]; then
		echo "Desktop file creation successful."
	else
		echo "Desktop file creation failed."
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

LINUX_PREREQUISITES_TXT="./LINUX_PREREQUISITES.TXT"
DOTNET_SDK_REGEX_TXT="./DOTNET_SDK_REGEX.TXT"
DOTNET_RUNTIME_REGEX_TXT="./DOTNET_RUNTIME_REGEX.TXT"

UN=$$

DOTNET_SDKS_TXT="./DOTNET_SDKS_$UN.TXT"
DOTNET_RUNTIMES_TXT="./DOTNET_RUNTIMES_$UN.TXT"

if [ ! -f $LINUX_PREREQUISITES_TXT ]
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

cat $LINUX_PREREQUISITES_TXT

read -p "Press Enter to continue . . . " enterKey

exit 1
