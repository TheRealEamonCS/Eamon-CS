@echo off

REM Copyright (c) 2014+ by Michael Penner.  All rights reserved.

timeout /t 1 /nobreak >nul 2>&1

cd ..\..

set "BASE_PATH=%CD%"

cd System\Bin

set LIB_SRC_PATH="."
set LIB_DEST_PATH=".\runtimes\win-x64\native"

set AV_LIBGLESV2_DLL="av_libglesv2.dll"
set LIBHARFBUZZSHARP_DLL="libHarfBuzzSharp.dll"
set LIBSKIASHARP_DLL="libSkiaSharp.dll"

if not exist "%LIB_DEST_PATH%" (
	mkdir "%LIB_DEST_PATH%" >nul 2>&1
)

if not exist "%LIB_DEST_PATH%\%AV_LIBGLESV2_DLL%" (
	copy "%LIB_SRC_PATH%\%AV_LIBGLESV2_DLL%" "%LIB_DEST_PATH%\%AV_LIBGLESV2_DLL%" >nul 2>&1
)

if not exist "%LIB_DEST_PATH%\%LIBHARFBUZZSHARP_DLL%" (
	copy "%LIB_SRC_PATH%\%LIBHARFBUZZSHARP_DLL%" "%LIB_DEST_PATH%\%LIBHARFBUZZSHARP_DLL%" >nul 2>&1
)

if not exist "%LIB_DEST_PATH%\%LIBSKIASHARP_DLL%" (
	copy "%LIB_SRC_PATH%\%LIBSKIASHARP_DLL%" "%LIB_DEST_PATH%\%LIBSKIASHARP_DLL%" >nul 2>&1
)

set WINDOWS_PREREQUISITES_TXT=".\WINDOWS_PREREQUISITES.TXT"
set DOTNET_SDK_REGEX_TXT=".\DOTNET_SDK_REGEX.TXT"
set DOTNET_RUNTIME_REGEX_TXT=".\DOTNET_RUNTIME_REGEX.TXT"

set UN=%RANDOM%

set DOTNET_SDKS_TXT=".\DOTNET_SDKS_%UN%.TXT"
set DOTNET_RUNTIMES_TXT=".\DOTNET_RUNTIMES_%UN%.TXT"

if not exist %WINDOWS_PREREQUISITES_TXT% (
	goto dotnet_found
)

set /p DOTNET_SDK_REGEX=<%DOTNET_SDK_REGEX_TXT%
dotnet --list-sdks >%DOTNET_SDKS_TXT% 2>&1
powershell -Command "$result = (Get-Content '%DOTNET_SDKS_TXT%') -match '%DOTNET_SDK_REGEX%'; if ($result) { exit 0 } else { exit 1 }" >nul 2>&1
set DOTNET_SDK_FOUND=%ERRORLEVEL%
del /f /q %DOTNET_SDKS_TXT% >nul 2>&1

if "%DOTNET_SDK_FOUND%"=="0" (
	goto dotnet_found
)

set /p DOTNET_RUNTIME_REGEX=<%DOTNET_RUNTIME_REGEX_TXT%
dotnet --list-runtimes >%DOTNET_RUNTIMES_TXT% 2>&1
powershell -Command "$result = (Get-Content '%DOTNET_RUNTIMES_TXT%') -match '%DOTNET_RUNTIME_REGEX%'; if ($result) { exit 0 } else { exit 1 }" >nul 2>&1
set DOTNET_RUNTIME_FOUND=%ERRORLEVEL%
del /f /q %DOTNET_RUNTIMES_TXT% >nul 2>&1

if "%DOTNET_RUNTIME_FOUND%"=="0" (
	goto dotnet_found
)

mode con: cols=110 lines=50
type %WINDOWS_PREREQUISITES_TXT%
pause
exit 1

:dotnet_found

setlocal EnableDelayedExpansion

:menu

echo:
echo Please select the type of system you're running on:
echo:
echo 1. win-x64 (Intel/AMD)
echo 2. win-arm64 (Arm)
echo:
set /p CHOICE=Enter the number corresponding to your system: 

if "%CHOICE%"=="1" set TARGET=win-x64
if "%CHOICE%"=="2" set TARGET=win-arm64

if not defined TARGET (
    echo:
    echo Invalid selection. Please try again.
    goto menu
)

set PUBLISH_FILES=".\publish\%TARGET%\native\*"
set RUNTIME_FILES=".\runtimes\%TARGET%\native\*"

echo:

xcopy %PUBLISH_FILES% "." /I /S /Y

echo:

xcopy %RUNTIME_FILES% "." /I /S /Y

set EXE_PATH='%BASE_PATH%\System\Bin\EamonPM.Desktop.exe'
set LNK_PATH='%BASE_PATH%\QuickLaunch\Windows\EamonPM.Desktop.exe.lnk'

powershell -Command "$WS = New-Object -ComObject WScript.Shell; $SC = $WS.CreateShortcut(%LNK_PATH%); $SC.TargetPath = %EXE_PATH%; $result = $SC.Save(); exit $result" >nul 2>&1
set SHORTCUT_CREATED=%ERRORLEVEL%

echo:

if "%SHORTCUT_CREATED%"=="0" (
    echo Shortcut creation successful.
) else (
    echo Shortcut creation failed.
)

echo:

pause

exit 0
