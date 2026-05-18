@echo off
echo ========================================================
echo Supertonic SAPI 5 Engine - Windows Installation Script
echo ========================================================
echo.
echo Make sure you are running this script as Administrator.
echo.

set DLL_NAME=Supertonic.SAPI5.Core.comhost.dll

if not exist "%~dp0bin\Release\net8.0\%DLL_NAME%" (
    echo [ERROR] Could not find %DLL_NAME%. 
    echo Please build the project in Release mode first:
    echo   dotnet build -c Release
    echo.
    pause
    exit /b 1
)

echo Registering %DLL_NAME% using regsvr32...
regsvr32 /s "%~dp0bin\Release\net8.0\%DLL_NAME%"

if %errorlevel% neq 0 (
    echo [ERROR] Failed to register COM host. Ensure you ran this script as Administrator.
    pause
    exit /b %errorlevel%
)

echo.
echo [SUCCESS] COM object registered successfully!
echo The Supertonic voice token should now be available in Windows Text-To-Speech settings.
echo.
pause
