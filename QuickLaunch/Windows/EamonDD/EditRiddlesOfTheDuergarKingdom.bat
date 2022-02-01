@echo off

REM Copyright (c) 2014+ by Michael Penner.  All rights reserved.

cd ..\..\..\System\Bin

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
findstr /r "%DOTNET_SDK_REGEX%" %DOTNET_SDKS_TXT% >nul 2>&1
set DOTNET_SDK_FOUND=%ERRORLEVEL%
del /f /q %DOTNET_SDKS_TXT% >nul 2>&1

if "%DOTNET_SDK_FOUND%"=="0" (
	goto dotnet_found
)

set /p DOTNET_RUNTIME_REGEX=<%DOTNET_RUNTIME_REGEX_TXT%
dotnet --list-runtimes >%DOTNET_RUNTIMES_TXT% 2>&1
findstr /r "%DOTNET_RUNTIME_REGEX%" %DOTNET_RUNTIMES_TXT% >nul 2>&1
set DOTNET_RUNTIME_FOUND=%ERRORLEVEL%
del /f /q %DOTNET_RUNTIMES_TXT% >nul 2>&1

if "%DOTNET_RUNTIME_FOUND%"=="0" (
	goto dotnet_found
)

mode con: cols=100 lines=50
type %WINDOWS_PREREQUISITES_TXT%
pause
exit 1

:dotnet_found

dotnet .\EamonPM.WindowsUnix.dll -pfn RiddlesOfTheDuergarKingdom.dll -wd ..\..\Adventures\RiddlesOfTheDuergarKingdom -la -rge
exit 0
