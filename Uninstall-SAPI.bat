@echo off
echo ========================================================
echo Supertonic SAPI 5 Engine - Windows Uninstallation Script
echo ========================================================
echo.
echo Make sure you are running this script as Administrator.
echo.

set DLL_NAME=Supertonic.SAPI5.Core.comhost.dll

if not exist "%~dp0bin\Release\net8.0\%DLL_NAME%" (
    echo [ERROR] Could not find %DLL_NAME%. 
    echo Ensure the built DLL exists in the Release directory to unregister it.
    echo.
    pause
    exit /b 1
)

echo Unregistering %DLL_NAME% using regsvr32...
regsvr32 /u /s "%~dp0bin\Release\net8.0\%DLL_NAME%"

if %errorlevel% neq 0 (
    echo [ERROR] Failed to unregister COM host. Ensure you ran this script as Administrator.
    pause
    exit /b %errorlevel%
)

echo.
echo [SUCCESS] COM object unregistered successfully!
echo The Supertonic voice token has been removed from your system.
echo.
pause
