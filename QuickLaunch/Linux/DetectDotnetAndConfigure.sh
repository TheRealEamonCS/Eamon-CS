#!/bin/sh

# Copyright (c) 2014+ by Michael Penner.  All rights reserved.

dotnet_found()
{
	SYSTEM_CONFIGURED=0

	if [ ! -f "./_avalonia.zip" ]; then
		SYSTEM_CONFIGURED=1
	fi

	if [ ! -f "./_linux-arm64.zip" ]; then
		SYSTEM_CONFIGURED=1
	fi

	if [ ! -f "./_linux-x64.zip" ]; then
		SYSTEM_CONFIGURED=1
	fi

	if [ "$SYSTEM_CONFIGURED" -eq "1" ]; then
		echo
		echo "This Eamon CS repository has already been configured."
		echo
		read -p "Press any key to continue . . ." enterKey
		exit 3
	fi

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

	echo

	# Unzip Avalonia package(s)
	if unzip -o -q "./_avalonia.zip" -d "." >/dev/null 2>&1; then
		echo "Avalonia package(s) unzip successful."
	else
		echo "Avalonia package(s) unzip failed."
	fi

	echo

	# Unzip platform-specific package(s)
	if unzip -o -q "./_$TARGET.zip" -d "." >/dev/null 2>&1; then
		echo "Platform-specific package(s) unzip successful."
	else
		echo "Platform-specific package(s) unzip failed."
	fi

	# Extract script directory
	EXE_PATH="$BASE_PATH/System/Bin/EamonPM.Desktop"
	LNK_PATH="$BASE_PATH/QuickLaunch/Linux/EamonPM.Desktop"
	DTF_PATH="$BASE_PATH/QuickLaunch/Linux/EamonPM.Desktop.desktop"

	echo

	# Chmod executable(s)
	if chmod 770 "$EXE_PATH" >/dev/null 2>&1; then
		echo "Executable(s) chmod successful."
	else
		echo "Executable(s) chmod failed."
	fi

	# Create a .desktop file
	echo "[Desktop Entry]" > "$DTF_PATH"
	echo "Version=3.0" >> "$DTF_PATH"
	echo "Name=EamonPM.Desktop" >> "$DTF_PATH"
	echo "Exec=$EXE_PATH" >> "$DTF_PATH"
	echo "Comment=EamonPM.Desktop Application" >> "$DTF_PATH"
	echo "Icon=$BASE_PATH/System/EamonPM.Desktop/Resources/ten_sided_die.png" >> "$DTF_PATH"
	echo "Terminal=false" >> "$DTF_PATH"
	echo "Type=Application" >> "$DTF_PATH"
	echo "Categories=Utility;" >> "$DTF_PATH"

	echo

	# Check if .desktop file creation was successful
	if [ -f "$DTF_PATH" ]; then
		echo "Desktop file creation successful."
	else
		echo "Desktop file creation failed."
	fi

	echo

	# Check if shortcut creation was successful
	if ln -sf "$EXE_PATH" "$LNK_PATH" >/dev/null 2>&1; then
	
		echo "Shortcut creation successful."
		
		rm -f "./_avalonia.zip" >/dev/null 2>&1
		rm -f "./_linux-arm64.zip" >/dev/null 2>&1
		rm -f "./_linux-x64.zip" >/dev/null 2>&1
		rm -f "./_osx.zip" >/dev/null 2>&1
		rm -f "./_osx-arm64.zip" >/dev/null 2>&1
		rm -f "./_osx-x64.zip" >/dev/null 2>&1
		rm -f "./_win-arm64.zip" >/dev/null 2>&1
		rm -f "./_win-x64.zip" >/dev/null 2>&1

		echo

		echo "Repository configuration successful."
		
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

cd "$(dirname "$0")"

SCRIPT_DIR=$(pwd)

if ! echo "$SCRIPT_DIR" | grep -E ".*/Eamon-CS[^/]*/QuickLaunch/Linux$" >/dev/null 2>&1; then
	echo
	echo "This script must be located in a valid Eamon CS repository."
	echo
	read -p "Press any key to continue . . ." enterKey
	exit 1
fi

cd ../..

BASE_PATH=$(pwd)

cd System/Bin

LOCK_PATH="./DETECT_DOTNET_AND_CONFIGURE.LOCK"

if [ -e "$LOCK_PATH" ]; then
    echo
    echo "Found DETECT_DOTNET_AND_CONFIGURE.LOCK file in System/Bin, waiting . . ."
fi

while ! ln -s "$0" "$LOCK_PATH" >/dev/null 2>&1; do
    sleep 1 >/dev/null 2>&1
done

trap 'rm -f "$LOCK_PATH"' EXIT

trap 'rm -f "$LOCK_PATH"; exit $?' HUP INT QUIT ABRT SEGV TERM

LINUX_PREREQUISITES_TXT="./LINUX_PREREQUISITES.TXT"
DOTNET_SDK_REGEX_TXT="./DOTNET_SDK_REGEX.TXT"
DOTNET_RUNTIME_REGEX_TXT="./DOTNET_RUNTIME_REGEX.TXT"

UN=$$

DOTNET_SDKS_TXT="./DOTNET_SDKS_$UN.TXT"
DOTNET_RUNTIMES_TXT="./DOTNET_RUNTIMES_$UN.TXT"

if [ ! -f "$LINUX_PREREQUISITES_TXT" ]; then
	dotnet_found "$@"
fi

DOTNET_SDK_REGEX=$(cat "$DOTNET_SDK_REGEX_TXT")
dotnet --list-sdks > "$DOTNET_SDKS_TXT" 2>&1
grep -E "$DOTNET_SDK_REGEX" "$DOTNET_SDKS_TXT" >/dev/null 2>&1
DOTNET_SDK_FOUND=$?
rm -f "$DOTNET_SDKS_TXT" >/dev/null 2>&1

if [ "$DOTNET_SDK_FOUND" -eq "0" ]; then
	dotnet_found "$@"
fi

DOTNET_RUNTIME_REGEX=$(cat "$DOTNET_RUNTIME_REGEX_TXT")
dotnet --list-runtimes > "$DOTNET_RUNTIMES_TXT" 2>&1
grep -E "$DOTNET_RUNTIME_REGEX" "$DOTNET_RUNTIMES_TXT" >/dev/null 2>&1
DOTNET_RUNTIME_FOUND=$?
rm -f "$DOTNET_RUNTIMES_TXT" >/dev/null 2>&1

if [ "$DOTNET_RUNTIME_FOUND" -eq "0" ]; then
	dotnet_found "$@"
fi

if command -v resize >/dev/null 2>&1; then
	resize -s 45 110 >/dev/null 2>&1
else
	printf '\033[8;45;110t'
fi

cat "$LINUX_PREREQUISITES_TXT"

read -p "Press Enter to continue . . . " enterKey

exit 2
