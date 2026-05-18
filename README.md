# Supertonic SAPI 5 Engine
**Developed by KAIST Assistive AI Lab (재활인공지능연구실)**

A SAPI 5 (Speech Application Programming Interface) compatible Text-to-Speech (TTS) engine for Windows, powered by the open-source [Supertonic 3](https://supertonictts.com/) ONNX model from Supertone Inc.

This project wraps the high-quality Supertonic 3 TTS engine in a COM interface via `.NET 8 COM Hosting`. This allows it to be used by any Windows screen reader (e.g., NVDA, JAWS, Narrator) or accessibility application that relies on SAPI 5, drastically improving the quality of synthetic voices available to visually impaired users.

## Acknowledgements & Licensing
* **SAPI Wrapper Code**: Developed and maintained by KAIST Assistive AI Lab (재활인공지능연구실) (MIT License).
* **Supertonic 3 Core**: The underlying TTS architecture, `Helper.cs` components, and ONNX models are developed by **Supertone Inc.** (MIT License for code, refer to Supertone's official repository for model licensing terms).

## Architecture
* **Core**: `Supertonic.SAPI5.Core` (.NET 8.0)
  * Implements `System.Speech.Synthesis.TtsEngine.TtsEngineSsml` to integrate with SAPI 5.
  * Utilizes `Microsoft.ML.OnnxRuntime` to perform fast, offline inference using Supertonic's models.
* **Test Console**: `Supertonic.SAPI5.TestConsole`
  * A command-line utility for testing synthesis quality and latency without needing full SAPI registration.

## Getting Started

### 1. Download Models
Due to their large size, the ONNX model files are not included in this repository. You must download the Supertonic 3 model files:
1. Create a folder at `C:\Supertonic\models` (or your preferred installation path).
2. Download the `.onnx` models from Hugging Face: [Supertone/supertonic-3](https://huggingface.co/Supertone/supertonic-3/tree/main/onnx) and place them in the `models` directory.
3. Download a Voice Style file (e.g., `M1.json`) and place it in `models\voice_styles\`.

### 2. Build the Project
Because this is a SAPI 5 COM component, you must build it for Windows.
```bash
# Build the core library in Release mode
dotnet build Supertonic.SAPI5.Core -c Release
```

### 3. Registering the SAPI Engine (Windows Only)
When built on Windows, .NET 8 creates a `Supertonic.SAPI5.Core.comhost.dll` which acts as the unmanaged bridge for SAPI.
1. Open an **Administrator Command Prompt**.
2. Navigate to the project root directory.
3. Run `Install-SAPI.bat`. This will run `regsvr32` and safely insert the SAPI Voice Token into the Windows Registry.

### 4. Creating an Installer (Inno Setup)
For distribution, we provide an Inno Setup script (`SupertonicInstaller.iss`):
1. Build the project in Release mode.
2. Place the downloaded ONNX models into a `models` folder next to the `.iss` file.
3. Open `SupertonicInstaller.iss` in [Inno Setup](https://jrsoftware.org/isinfo.php) and compile. 
4. Distribute the resulting `.exe` installer.

## Usage
Once registered, the voice will appear as **"Supertonic Voice (Korean)"** in your Windows Text-to-Speech settings and in screen readers like NVDA.

To unregister and remove it, simply run:
```cmd
Uninstall-SAPI.bat
```
