#!/bin/sh

# Copyright (c) 2014+ by Michael Penner.  All rights reserved.

dotnet_found()
{
	SYSTEM_CONFIGURED=0

	if [ ! -d "./publish" ]; then
		SYSTEM_CONFIGURED=1
	fi

	if [ ! -d "./runtimes" ]; then
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

	echo

	# Copy publish files and list them
	COUNT=0
	
	if [ -d "./publish/$TARGET/native" ]; then
		for file in ./publish/$TARGET/native/*; do
			cp "$file" .
			echo "$file"
			COUNT=$((COUNT + 1))
		done
	else
		echo "File not found - *"
	fi
	
	echo "$COUNT File(s) copied"

	echo

	# Copy runtime files and list them
	COUNT=0
	
	if [ -d "./runtimes/osx/native" ]; then
		for file in ./runtimes/osx/native/*; do
			cp "$file" .
			echo "$file"
			COUNT=$((COUNT + 1))
		done
	else
		echo "File not found - *"
	fi
		
	echo "$COUNT File(s) copied"

	# Extract script directory
	EXE_PATH="$BASE_PATH/System/Bin/EamonPM.Desktop"
	LNK_PATH="$BASE_PATH/QuickLaunch/MacOS/EamonPM.Desktop"
	ICON_PATH="$BASE_PATH/System/EamonPM.Desktop/Resources/ten_sided_die.icns"

	# Apply custom icon using fileicon utility
	if [ -f "$EXE_PATH" ]; then

		./fileicon -q set "$EXE_PATH" "$ICON_PATH"
	fi
	
	echo

	# Check if shortcut creation was successful
	if ln -sf "$EXE_PATH" "$LNK_PATH" >/dev/null 2>&1; then
		
		echo "Shortcut creation successful."
		
		rm -f "./publish/linux-arm64/native/EamonPM.Desktop" >/dev/null 2>&1
		rm -f "./publish/linux-x64/native/EamonPM.Desktop" >/dev/null 2>&1
		rm -f "./publish/osx-arm64/native/EamonPM.Desktop" >/dev/null 2>&1
		rm -f "./publish/osx-x64/native/EamonPM.Desktop" >/dev/null 2>&1
		rm -f "./publish/win-arm64/native/EamonPM.Desktop.exe" >/dev/null 2>&1
		rm -f "./publish/win-x64/native/EamonPM.Desktop.exe" >/dev/null 2>&1

		rmdir "./publish/linux-arm64/native" >/dev/null 2>&1
		rmdir "./publish/linux-arm64" >/dev/null 2>&1
		rmdir "./publish/linux-x64/native" >/dev/null 2>&1
		rmdir "./publish/linux-x64" >/dev/null 2>&1

		rmdir "./publish/osx-arm64/native" >/dev/null 2>&1
		rmdir "./publish/osx-arm64" >/dev/null 2>&1
		rmdir "./publish/osx-x64/native" >/dev/null 2>&1
		rmdir "./publish/osx-x64" >/dev/null 2>&1

		rmdir "./publish/win-arm64/native" >/dev/null 2>&1
		rmdir "./publish/win-arm64" >/dev/null 2>&1
		rmdir "./publish/win-x64/native" >/dev/null 2>&1
		rmdir "./publish/win-x64" >/dev/null 2>&1

		rmdir "./publish" >/dev/null 2>&1

		rm -f "./runtimes/linux-arm64/native/libHarfBuzzSharp.so" >/dev/null 2>&1
		rm -f "./runtimes/linux-arm64/native/libSkiaSharp.so" >/dev/null 2>&1
		rm -f "./runtimes/linux-x64/native/libHarfBuzzSharp.so" >/dev/null 2>&1
		rm -f "./runtimes/linux-x64/native/libSkiaSharp.so" >/dev/null 2>&1
		rm -f "./runtimes/osx/native/libAvaloniaNative.dylib" >/dev/null 2>&1
		rm -f "./runtimes/osx/native/libHarfBuzzSharp.dylib" >/dev/null 2>&1
		rm -f "./runtimes/osx/native/libSkiaSharp.dylib" >/dev/null 2>&1

		rm -f "./runtimes/win-arm64/native/av_libglesv2.dll" >/dev/null 2>&1
		rm -f "./runtimes/win-arm64/native/libHarfBuzzSharp.dll" >/dev/null 2>&1
		rm -f "./runtimes/win-arm64/native/libSkiaSharp.dll" >/dev/null 2>&1
		rm -f "./runtimes/win-x64/native/av_libglesv2.dll" >/dev/null 2>&1
		rm -f "./runtimes/win-x64/native/libHarfBuzzSharp.dll" >/dev/null 2>&1
		rm -f "./runtimes/win-x64/native/libSkiaSharp.dll" >/dev/null 2>&1

		rmdir "./runtimes/linux-arm64/native" >/dev/null 2>&1
		rmdir "./runtimes/linux-arm64" >/dev/null 2>&1
		rmdir "./runtimes/linux-x64/native" >/dev/null 2>&1
		rmdir "./runtimes/linux-x64" >/dev/null 2>&1

		rmdir "./runtimes/osx/native" >/dev/null 2>&1
		rmdir "./runtimes/osx" >/dev/null 2>&1

		rmdir "./runtimes/win-arm64/native" >/dev/null 2>&1
		rmdir "./runtimes/win-arm64" >/dev/null 2>&1
		rmdir "./runtimes/win-x64/native" >/dev/null 2>&1
		rmdir "./runtimes/win-x64" >/dev/null 2>&1

		rmdir "./runtimes" >/dev/null 2>&1

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

if ! echo "$SCRIPT_DIR" | grep -E ".*/Eamon-CS[^/]*/QuickLaunch/MacOS$" >/dev/null 2>&1; then
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

if [ -L "$LOCK_PATH" ]; then
    echo
    echo "Found DETECT_DOTNET_AND_CONFIGURE.LOCK file in System/Bin, waiting . . ."
fi

while ! ln -s "$0" "$LOCK_PATH" >/dev/null 2>&1; do
    sleep 1 >/dev/null 2>&1
done

trap 'rm -f "$LOCK_PATH"' EXIT

trap 'rm -f "$LOCK_PATH"; exit $?' HUP INT QUIT ABRT SEGV TERM

MACOS_PREREQUISITES_TXT="./MACOS_PREREQUISITES.TXT"
DOTNET_SDK_REGEX_TXT="./DOTNET_SDK_REGEX.TXT"
DOTNET_RUNTIME_REGEX_TXT="./DOTNET_RUNTIME_REGEX.TXT"

UN=$$

DOTNET_SDKS_TXT="./DOTNET_SDKS_$UN.TXT"
DOTNET_RUNTIMES_TXT="./DOTNET_RUNTIMES_$UN.TXT"

if [ ! -f "$MACOS_PREREQUISITES_TXT" ]; then
	dotnet_found "$@"
fi

DOTNET_SDK_REGEX=$(cat "$DOTNET_SDK_REGEX_TXT")
dotnet --list-sdks >"$DOTNET_SDKS_TXT" 2>&1
grep -E "$DOTNET_SDK_REGEX" "$DOTNET_SDKS_TXT" >/dev/null 2>&1
DOTNET_SDK_FOUND=$?
rm -f "$DOTNET_SDKS_TXT" >/dev/null 2>&1

if [ "$DOTNET_SDK_FOUND" -eq "0" ]; then
	dotnet_found "$@"
fi

DOTNET_RUNTIME_REGEX=$(cat "$DOTNET_RUNTIME_REGEX_TXT")
dotnet --list-runtimes >"$DOTNET_RUNTIMES_TXT" 2>&1
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

cat "$MACOS_PREREQUISITES_TXT"

read -p "Press Enter to continue . . . " enterKey

exit 2
