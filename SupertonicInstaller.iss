; Supertonic SAPI 5 Engine Inno Setup Script
; This script generates a Windows Setup (.exe) to automatically install and register the SAPI engine.

[Setup]
AppName=Supertonic SAPI5 Engine
AppVersion=1.0
AppPublisher=AssistiveAI
DefaultDirName={pf}\Supertonic
DefaultGroupName=Supertonic SAPI5 Engine
OutputDir=InstallerOutput
OutputBaseFilename=Supertonic_SAPI_Setup
Compression=lzma
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64

[Files]
; 1. Copy the compiled DLLs (Ensure you build in Release mode before running Inno Setup)
Source: "Supertonic.SAPI5.Core\bin\Release\net8.0\*"; DestDir: "{app}\bin"; Flags: ignoreversion recursesubdirs createallsubdirs

; 2. Copy the model files. 
; (You must manually place your downloaded ONNX and JSON files into a 'models' folder next to this .iss file before compiling)
Source: "models\*"; DestDir: "{app}\models"; Flags: ignoreversion recursesubdirs createallsubdirs

[Run]
; Automatically register the COM host during installation
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\bin\Supertonic.SAPI5.Core.comhost.dll"""; StatusMsg: "Registering Supertonic SAPI Voice..."; Flags: runhidden

[UninstallRun]
; Automatically unregister the COM host before uninstallation
Filename: "{sys}\regsvr32.exe"; Parameters: "/u /s ""{app}\bin\Supertonic.SAPI5.Core.comhost.dll"""; StatusMsg: "Unregistering Supertonic SAPI Voice..."; Flags: runhidden
