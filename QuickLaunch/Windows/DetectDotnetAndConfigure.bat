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

echo:
echo Configuring Eamon CS repository . . .

cd ..\..

set "BASE_PATH=%cd%"

cd System\Bin

set "LOCK_PATH=.\DETECT_DOTNET_AND_CONFIGURE.LOCK"
set "CLEANUP_SCRIPT_PATH=.\DETECT_DOTNET_AND_CONFIGURE_CLEANUP.ps1"

set "WAITED_ON_LOCK=0"

if exist "%LOCK_PATH%" (
	set "WAITED_ON_LOCK=1"
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
echo:
echo Configuring Eamon CS repository . . .
if "%WAITED_ON_LOCK%"=="1" (
	echo:
	echo Found DETECT_DOTNET_AND_CONFIGURE.LOCK file in System\Bin, waiting . . .
)
type "%WINDOWS_PREREQUISITES_TXT%"
echo:
pause
exit 2

:dotnet_found

setlocal EnableDelayedExpansion

set SYSTEM_CONFIGURED=0

if not exist ".\_avalonia.zip" (
	set SYSTEM_CONFIGURED=1
)

if not exist ".\_win-arm64.zip" (
	set SYSTEM_CONFIGURED=1
)

if not exist ".\_win-x64.zip" (
	set SYSTEM_CONFIGURED=1
)

if "%SYSTEM_CONFIGURED%"=="1" (
	echo:
	echo This repository has already been configured.
	echo:
	pause
	exit 3
)

:menu

echo:
echo Please select your system type:
echo:
echo   [1] Windows (Intel/AMD) - win-x64
echo   [2] Windows (Arm)       - win-arm64
echo:
set /p CHOICE=Enter your choice (1 or 2): 

if "%CHOICE%"=="1" set TARGET=win-x64
if "%CHOICE%"=="2" set TARGET=win-arm64

if not defined TARGET (
	echo:
	echo Invalid choice. Please enter 1 or 2.
	goto menu
)

echo:

powershell -ExecutionPolicy Bypass -Command "try { Expand-Archive -Path '.\_avalonia.zip' -DestinationPath '.' -Force; exit 0 } catch { exit 1 }" >nul 2>&1

set AVALONIA_UNZIPPED=%ERRORLEVEL%

if "%AVALONIA_UNZIPPED%"=="0" (
    echo [+] Avalonia package^(s^) unzip successful.
) else (
    echo [-] Avalonia package^(s^) unzip failed.
)

powershell -ExecutionPolicy Bypass -Command "try { Expand-Archive -Path '.\_%TARGET%.zip' -DestinationPath '.' -Force; exit 0 } catch { exit 1 }" >nul 2>&1

set PLATFORM_UNZIPPED=%ERRORLEVEL%

if "%PLATFORM_UNZIPPED%"=="0" (
    echo [+] Platform-specific package^(s^) unzip successful.
) else (
    echo [-] Platform-specific package^(s^) unzip failed.
)

set "EXE_PATH=%BASE_PATH%\System\Bin\EamonPM.Desktop.exe"
set "LNK_PATH=%BASE_PATH%\QuickLaunch\Windows\EamonPM.Desktop.exe.lnk"

powershell -ExecutionPolicy Bypass -Command "$WS = New-Object -ComObject WScript.Shell; $SC = $WS.CreateShortcut('%LNK_PATH%'); $SC.TargetPath = '%EXE_PATH%'; $result = $SC.Save(); exit $result" >nul 2>&1

set SHORTCUT_CREATED=%ERRORLEVEL%

if "%SHORTCUT_CREATED%"=="0" (

	echo [+] Shortcut creation successful.

	del /f /q ".\_avalonia.zip" >nul 2>&1
	del /f /q ".\_linux-arm64.zip" >nul 2>&1
	del /f /q ".\_linux-x64.zip" >nul 2>&1
	del /f /q ".\_osx.zip" >nul 2>&1
	del /f /q ".\_osx-arm64.zip" >nul 2>&1
	del /f /q ".\_osx-x64.zip" >nul 2>&1
	del /f /q ".\_win-arm64.zip" >nul 2>&1
	del /f /q ".\_win-x64.zip" >nul 2>&1

	echo [+] Repository configuration successful.

) else (
	echo [-] Shortcut creation failed.
)

echo:

echo Setup completed.

echo:

pause

exit 0
