@echo off

REM Copyright (c) 2014+ by Michael Penner.  All rights reserved.

setlocal

timeout /t 1 /nobreak >nul 2>&1

cd /d "%~dp0"

set "SCRIPT_DIR=%cd%"

powershell -ExecutionPolicy Bypass -Command "if ('%SCRIPT_DIR%' -notmatch '(?-i).*\\Eamon-CS[^\\]*\\QuickLaunch\\Windows$') { exit 1 }" >nul 2>&1

set VALID_SCRIPT_DIR=%ERRORLEVEL%

if "%VALID_SCRIPT_DIR%"=="1" (
	echo:
	echo This batch file must be located in a valid Eamon CS repository.
	echo:
	pause
	exit 1
)

cd ..\..

set "BASE_PATH=%cd%"

cd System\Bin

set "LOCK_PATH=.\DETECT_DOTNET_AND_CONFIGURE.LOCK"
set "CLEANUP_SCRIPT_PATH=.\DETECT_DOTNET_AND_CONFIGURE_CLEANUP.ps1"

if exist "%LOCK_PATH%" (
	echo:
	echo Found DETECT_DOTNET_AND_CONFIGURE.LOCK file in System\Bin, waiting . . .
)

:acquire_lock

powershell -ExecutionPolicy Bypass -Command "try { $lockPath = '%LOCK_PATH%'; New-Item -ItemType File -Path $lockPath -ErrorAction Stop > $null; exit 0; } catch { exit 1; }" >nul 2>&1

set LOCK_ACQUIRED=%ERRORLEVEL%

if "%LOCK_ACQUIRED%"=="1" (
    timeout /t 1 >nul 2>&1
    goto acquire_lock
)

echo $lockfile = '%LOCK_PATH%' > "%CLEANUP_SCRIPT_PATH%"
echo while ($true) { >> "%CLEANUP_SCRIPT_PATH%"
echo     $parent = Get-CimInstance Win32_Process -Filter "ProcessId=$pid" ^| Select-Object -ExpandProperty ParentProcessId >> "%CLEANUP_SCRIPT_PATH%"
echo     if (!(Get-Process -Id $parent -ErrorAction SilentlyContinue)) { break } >> "%CLEANUP_SCRIPT_PATH%"
echo     Start-Sleep -Seconds 1 >> "%CLEANUP_SCRIPT_PATH%"
echo } >> "%CLEANUP_SCRIPT_PATH%"
echo Remove-Item -Path $lockfile -Force -ErrorAction SilentlyContinue >> "%CLEANUP_SCRIPT_PATH%"
echo Remove-Item -Path '%CLEANUP_SCRIPT_PATH%' -Force >> "%CLEANUP_SCRIPT_PATH%"

start /min powershell -WindowStyle Hidden -ExecutionPolicy Bypass -File "%CLEANUP_SCRIPT_PATH%"

set "WINDOWS_PREREQUISITES_TXT=.\WINDOWS_PREREQUISITES.TXT"
set "DOTNET_SDK_REGEX_TXT=.\DOTNET_SDK_REGEX.TXT"
set "DOTNET_RUNTIME_REGEX_TXT=.\DOTNET_RUNTIME_REGEX.TXT"

set UN=%RANDOM%

set "DOTNET_SDKS_TXT=.\DOTNET_SDKS_%UN%.TXT"
set "DOTNET_RUNTIMES_TXT=.\DOTNET_RUNTIMES_%UN%.TXT"

if not exist "%WINDOWS_PREREQUISITES_TXT%" (
	goto dotnet_found
)

set /p DOTNET_SDK_REGEX=<"%DOTNET_SDK_REGEX_TXT%"
dotnet --list-sdks >"%DOTNET_SDKS_TXT%" 2>&1
powershell -ExecutionPolicy Bypass -Command "$result = (Get-Content '%DOTNET_SDKS_TXT%') -match '%DOTNET_SDK_REGEX%'; if ($result) { exit 0 } else { exit 1 }" >nul 2>&1
set DOTNET_SDK_FOUND=%ERRORLEVEL%
del /f /q "%DOTNET_SDKS_TXT%" >nul 2>&1

if "%DOTNET_SDK_FOUND%"=="0" (
	goto dotnet_found
)

set /p DOTNET_RUNTIME_REGEX=<"%DOTNET_RUNTIME_REGEX_TXT%"
dotnet --list-runtimes >"%DOTNET_RUNTIMES_TXT%" 2>&1
powershell -ExecutionPolicy Bypass -Command "$result = (Get-Content '%DOTNET_RUNTIMES_TXT%') -match '%DOTNET_RUNTIME_REGEX%'; if ($result) { exit 0 } else { exit 1 }" >nul 2>&1
set DOTNET_RUNTIME_FOUND=%ERRORLEVEL%
del /f /q "%DOTNET_RUNTIMES_TXT%" >nul 2>&1

if "%DOTNET_RUNTIME_FOUND%"=="0" (
	goto dotnet_found
)

mode con: cols=110 lines=50
type "%WINDOWS_PREREQUISITES_TXT%"
pause
exit 2

:dotnet_found

setlocal EnableDelayedExpansion

set SYSTEM_CONFIGURED=0

if not exist ".\publish" (
	set SYSTEM_CONFIGURED=1
)

if not exist ".\runtimes" (
	set SYSTEM_CONFIGURED=1
)

if "%SYSTEM_CONFIGURED%"=="1" (
	echo:
	echo This Eamon CS repository has already been configured.
	echo:
	pause
	exit 3
)

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

echo:

xcopy ".\publish\%TARGET%\native\*" "." /I /S /Y

echo:

xcopy ".\runtimes\%TARGET%\native\*" "." /I /S /Y

set "EXE_PATH=%BASE_PATH%\System\Bin\EamonPM.Desktop.exe"
set "LNK_PATH=%BASE_PATH%\QuickLaunch\Windows\EamonPM.Desktop.exe.lnk"

powershell -ExecutionPolicy Bypass -Command "$WS = New-Object -ComObject WScript.Shell; $SC = $WS.CreateShortcut('%LNK_PATH%'); $SC.TargetPath = '%EXE_PATH%'; $result = $SC.Save(); exit $result" >nul 2>&1

set SHORTCUT_CREATED=%ERRORLEVEL%

echo:

if "%SHORTCUT_CREATED%"=="0" (

	echo Shortcut creation successful.

	del /f /q ".\publish\linux-arm64\native\EamonPM.Desktop" >nul 2>&1
	del /f /q ".\publish\linux-x64\native\EamonPM.Desktop" >nul 2>&1
	del /f /q ".\publish\osx-arm64\native\EamonPM.Desktop" >nul 2>&1
	del /f /q ".\publish\osx-x64\native\EamonPM.Desktop" >nul 2>&1
	del /f /q ".\publish\win-arm64\native\EamonPM.Desktop.exe" >nul 2>&1
	del /f /q ".\publish\win-x64\native\EamonPM.Desktop.exe" >nul 2>&1

	rd /q ".\publish\linux-arm64\native" >nul 2>&1
	rd /q ".\publish\linux-arm64" >nul 2>&1
	rd /q ".\publish\linux-x64\native" >nul 2>&1
	rd /q ".\publish\linux-x64" >nul 2>&1

	rd /q ".\publish\osx-arm64\native" >nul 2>&1
	rd /q ".\publish\osx-arm64" >nul 2>&1
	rd /q ".\publish\osx-x64\native" >nul 2>&1
	rd /q ".\publish\osx-x64" >nul 2>&1

	rd /q ".\publish\win-arm64\native" >nul 2>&1
	rd /q ".\publish\win-arm64" >nul 2>&1
	rd /q ".\publish\win-x64\native" >nul 2>&1
	rd /q ".\publish\win-x64" >nul 2>&1

	rd /q ".\publish" >nul 2>&1

	del /f /q ".\runtimes\linux-arm64\native\libHarfBuzzSharp.so" >nul 2>&1
	del /f /q ".\runtimes\linux-arm64\native\libSkiaSharp.so" >nul 2>&1
	del /f /q ".\runtimes\linux-x64\native\libHarfBuzzSharp.so" >nul 2>&1
	del /f /q ".\runtimes\linux-x64\native\libSkiaSharp.so" >nul 2>&1
	del /f /q ".\runtimes\osx\native\libAvaloniaNative.dylib" >nul 2>&1
	del /f /q ".\runtimes\osx\native\libHarfBuzzSharp.dylib" >nul 2>&1
	del /f /q ".\runtimes\osx\native\libSkiaSharp.dylib" >nul 2>&1

	del /f /q ".\runtimes\win-arm64\native\av_libglesv2.dll" >nul 2>&1
	del /f /q ".\runtimes\win-arm64\native\libHarfBuzzSharp.dll" >nul 2>&1
	del /f /q ".\runtimes\win-arm64\native\libSkiaSharp.dll" >nul 2>&1
	del /f /q ".\runtimes\win-x64\native\av_libglesv2.dll" >nul 2>&1
	del /f /q ".\runtimes\win-x64\native\libHarfBuzzSharp.dll" >nul 2>&1
	del /f /q ".\runtimes\win-x64\native\libSkiaSharp.dll" >nul 2>&1

	rd /q ".\runtimes\linux-arm64\native" >nul 2>&1
	rd /q ".\runtimes\linux-arm64" >nul 2>&1
	rd /q ".\runtimes\linux-x64\native" >nul 2>&1
	rd /q ".\runtimes\linux-x64" >nul 2>&1

	rd /q ".\runtimes\osx\native" >nul 2>&1
	rd /q ".\runtimes\osx" >nul 2>&1

	rd /q ".\runtimes\win-arm64\native" >nul 2>&1
	rd /q ".\runtimes\win-arm64" >nul 2>&1
	rd /q ".\runtimes\win-x64\native" >nul 2>&1
	rd /q ".\runtimes\win-x64" >nul 2>&1

	rd /q ".\runtimes" >nul 2>&1

	echo:

	echo Repository configuration successful.

) else (
	echo Shortcut creation failed.
)

echo:

pause

exit 0
