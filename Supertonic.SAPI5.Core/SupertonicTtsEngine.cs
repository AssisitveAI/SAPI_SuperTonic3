using System;
using System.IO;
using System.Collections.Generic;

namespace Supertonic.SAPI5.Core
{
    public class SupertonicTtsEngine : IDisposable
    {
        private TextToSpeech _tts;
        private Style _voiceStyle;

        public SupertonicTtsEngine(string modelsDirectory, string voiceStylePath)
        {
            if (!Directory.Exists(modelsDirectory))
            {
                throw new DirectoryNotFoundException($"Models directory not found: {modelsDirectory}");
            }
            if (!File.Exists(voiceStylePath))
            {
                throw new FileNotFoundException($"Voice style file not found: {voiceStylePath}");
            }

            // Load TTS engine
            // UseGpu parameter is false
            _tts = Helper.LoadTextToSpeech(modelsDirectory, false);

            // Load Voice Style
            _voiceStyle = Helper.LoadVoiceStyle(new List<string> { voiceStylePath }, verbose: false);
        }

        /// <summary>
        /// Synthesize speech from text
        /// </summary>
        /// <param name="text">The input text</param>
        /// <param name="lang">Language code, e.g., "ko", "en"</param>
        /// <param name="totalStep">Inference steps (quality vs speed)</param>
        /// <param name="speed">Playback speed multiplier</param>
        /// <returns>Raw 16-bit PCM audio byte array</returns>
        public byte[] Synthesize(string text, string lang = "ko", int totalStep = 8, float speed = 1.0f)
        {
            var (wav, duration) = _tts.Call(text, lang, _voiceStyle, totalStep, speed);
            return ConvertFloatArrayToPcm(wav);
        }
        
        public int SampleRate => _tts?.SampleRate ?? 24000;

        private byte[] ConvertFloatArrayToPcm(float[] audioData)
        {
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms);

            foreach (var sample in audioData)
            {
                float clamped = Math.Max(-1.0f, Math.Min(1.0f, sample));
                short intSample = (short)(clamped * 32767);
                writer.Write(intSample);
            }

            return ms.ToArray();
        }

        public void Dispose()
        {
            // Note: Helper.cs does not implement IDisposable for TextToSpeech,
            // we will let the garbage collector handle it or update Helper.cs later.
        }
    }
}
