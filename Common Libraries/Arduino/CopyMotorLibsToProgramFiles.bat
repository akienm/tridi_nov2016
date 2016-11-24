@echo off

:::::::::::::::::::::::::::::::::::::::::::::::::::::::
::
:: This batch file copies the motor libraries to 
:: the Program Files dir
::
:::::::::::::::::::::::::::::::::::::::::::::::::::::::

:::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Sort out the target directory
set target=%ProgramFiles%
if NOT %ProgramFiles(x86)%.==. set target=%ProgramFiles(x86)%

:::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Validate target dir
if exist "%target%\Arduino\arduino.exe" goto validateexist
echo.
echo %0 Is for copying motor libraries once the Arduino software is
echo installed. At this time, we cannot find the file 
echo "%target%\Arduino\arduino.exe"
echo.
echo Please install the Arduino software and try again.
echo.
pause
goto done

:::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Validate source dir
:validateexist
if not exist ".\AccelStepper\AccelStepper.cpp" goto libsnotfound
if not exist ".\AdafruitMotorShieldLibraryMaster\AFMotor.cpp" goto libsnotfound
goto docopy

:::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: source dir errors
:libsnotfound
echo.
echo %0 Is for copying motor libraries once the Arduino software is
echo installed. At this time, we cannot find the motor library files 
echo which we expected to find in subdirectories below this one.
echo.
echo Please update Subversion and try again.
echo.
pause
goto done

:::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Do the copy
:: /s subdirs
:: /c continue on error
:: /q quiet
:: /h hidden too
:: /r read only too
:: /y overwrite
:: /I target is a directory
:docopy
echo Copying library files, please wait...
md "%target%\Arduino\libraries\AccelStepper" > null 2>&1
xcopy ".\AccelStepper\*.*" "%target%\Arduino\libraries\AccelStepper" /s /c /q /h /r /y /I
md "%target%\Arduino\libraries\AdafruitMotorShieldLibraryMaster" > NUll  2>&1
xcopy ".\AdafruitMotorShieldLibraryMaster\*.*" "%target%\Arduino\libraries\AdafruitMotorShieldLibraryMaster" /s /c /q /h /r /y /I

:done
