# SAPI_SuperTonic3

Windows SAPI5 Text-to-Speech (TTS) engine using the lightning-fast Supertonic TTS model (via ONNX Runtime).

## Overview

This project aims to bridge the modern Supertonic TTS engine with standard Windows applications by implementing a SAPI 5 COM interface. This enables screen readers (like NVDA, JAWS) and other standard Windows TTS consumers to utilize the Supertonic voice seamlessly on your local machine.

## Architecture

- **SAPI 5 Interface**: Implemented in C# via COM Interop (or C++).
- **Inference Engine**: ONNX Runtime.
- **Model**: Supertonic TTS (Speech Autoencoder, Text-to-Latent, Duration Predictor).

## Development Roadmap

1. **Phase 1**: Core Inference Testing (Supertonic ONNX in C#)
2. **Phase 2**: SAPI 5 COM Interface Implementation
3. **Phase 3**: Windows Registry Registration
4. **Phase 4**: Refinement and Audio Control (Rate/Volume/Pitch mapping)

## Prerequisites

- Windows SDK
- .NET Framework 4.8 / .NET 6+
- ONNX Runtime (`Microsoft.ML.OnnxRuntime`)

## License

MIT License (Subject to Supertonic TTS model licensing)
