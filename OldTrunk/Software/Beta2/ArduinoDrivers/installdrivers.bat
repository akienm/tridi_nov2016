@echo off
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
::
:: SoundFit Arduino Driver Installer v3 2014-0507 Akien MacIain
:: Copyright (C) SoundFit LLC, 2014
::
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: first check for a help request
if %1.==/?. goto help
if %1.==/h. goto help
if %1.==/H. goto help
if %1.==-?. goto help
if %1.==-h. goto help
if %1.==-H. goto help

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: now check for the actual required driver component files
if NOT EXIST arduino.inf goto nodrivers
if NOT EXIST arduino.cat goto nodrivers

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: now set up and check for the installer executable for this hardware platform
set aia=INVALIDARCH
if %PROCESSOR_ARCHITECTURE%.==AMD64. set aia=dpinst-amd64.exe
if %PROCESSOR_ARCHITECTURE%.==x86. set aia=dpinst-x86.exe
if %aia%.==INVALIDARCH. goto archerr
if not exist %aia% goto nodrivers

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Check to see if any debug flags were indicated
if %1.==/d. goto debug
if %1.==/D. goto debug
if %1.==/o. goto oldway
if %1.==/O. goto oldway

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: now do the install
%aia% /q /sw /sa
goto done

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: this is the verbose version of the install above
:: it's used for debugging only.
:debug
%aia% /q /c
pause
goto done

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: we never land here, this is the old way of doing the install
:oldway
%SystemRoot%\System32\InfDefaultInstall.exe .\arduino.inf
pause
goto done

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: these are the error notification routines
:nodrivers
Echo.
echo Driver installation for the SugarCube's embedded control unit failed.
echo.
echo The batch file %0 was launched without having the active directory
echo containing the driver files. The active directory is:
echo.
cd
echo.
echo.
pause

:archerr
echo.
echo Driver installation for the SugarCube's embedded control unit failed.
echo.
echo An unknown PROCESSOR_ARCHITECTURE of %PROCESSOR_ARCHITECTURE% was found in
echo the environment. Please contact support with this message. Thank you.
echo.
echo.
pause
goto done

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: usage
:help
echo.
echo. %0 is the SoundFit Arduino Driver Installer Batch File.
echo. 
echo Usage:
echo %0    - Silently install drivers
echo %0 /? - display this message
echo %0 /D - Display install debug information
echo %0 /O - Install using the system INF installer EXE instead of Arduino's
echo.
echo Must be in the same directory as these files:
echo    arduino.cat
echo    arduino.inf
echo    dpinst-amd64.exe
echo    dpinst-x86.exe
echo.

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: and this means we're done!
:done
