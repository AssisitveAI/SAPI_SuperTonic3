# SAPI_SuperTonic3

A SAPI 5 (Speech Application Programming Interface) compatible Text-to-Speech (TTS) engine for Windows, powered by the [Supertonic 3](https://supertonictts.com/) ONNX model.

This project wraps the high-quality Supertonic 3 TTS engine in a COM interface via `.NET 8 COM Hosting`, allowing it to be used by any Windows screen reader (e.g., NVDA, JAWS) or TTS application that relies on SAPI 5.

## Architecture

* **Core**: `Supertonic.SAPI5.Core` (.NET 8.0)
  * Implements `System.Speech.Synthesis.TtsEngine.TtsEngineSsml` to integrate with SAPI 5.
  * Utilizes `Microsoft.ML.OnnxRuntime` to perform fast, offline inference using Supertonic's `ConvNeXt` models.
* **Test Console**: `Supertonic.SAPI5.TestConsole`
  * A command-line utility for testing synthesis quality and latency without needing full SAPI registration.

## Getting Started

### 1. Download Models
Due to their large size, the ONNX model files are not included in this repository. You must download the Supertonic 3 model files and place them in the correct directory.
1. Create a folder at `C:\Supertonic\models`.
2. Download the `.onnx` models from Hugging Face: [Supertone/supertonic-3](https://huggingface.co/Supertone/supertonic-3/tree/main/onnx) and place them in `C:\Supertonic\models`.
3. Download a Voice Style file (e.g., `M1.json`) and place it in `C:\Supertonic\models\voice_styles\`.

### 2. Build the Project
Because this is a SAPI 5 COM component, you must build it specifically for Windows.

```bash
# Build the core library in Release mode
dotnet build Supertonic.SAPI5.Core -c Release
```

### 3. Registering the SAPI Engine (Windows Only)
When built on Windows, .NET 8 creates a `Supertonic.SAPI5.Core.comhost.dll` which acts as the unmanaged bridge for SAPI.

1. Open an **Administrator Command Prompt**.
2. Navigate to the project root directory.
3. Run `Install-SAPI.bat`:
   ```cmd
   Install-SAPI.bat
   ```
4. This script will run `regsvr32` and trigger our `[ComRegisterFunction]` which safely inserts the SAPI Voice Token into the Windows Registry at:
   `HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech\Voices\Tokens\SupertonicVoice`

### 4. Usage
Once registered, the voice will appear as **"Supertonic Voice (Korean)"** in your Windows Text-to-Speech settings and in screen readers like NVDA.

To unregister and remove it, simply run:
```cmd
Uninstall-SAPI.bat
```

## Development & Testing
You can use the Test Console to debug synthesis logic before registering the engine:
```bash
cd Supertonic.SAPI5.TestConsole
dotnet run
```
This will produce an `output.wav` file to verify the audio quality.

## Notice
The ONNX models and the core inference methodology belong to Supertone Inc. This wrapper is simply a bridge to utilize these models seamlessly within the Windows accessibility ecosystem.
