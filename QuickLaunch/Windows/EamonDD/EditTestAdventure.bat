@echo off
cd ..\..\..\System\Bin
dotnet .\EamonPM.WindowsUnix.dll -pfn EamonRT.dll -wd ..\..\Adventures\TestAdventure -la -rge
