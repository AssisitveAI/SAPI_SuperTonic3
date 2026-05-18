using System;
using System.IO;
using Supertonic.SAPI5.Core;

namespace Supertonic.SAPI5.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Supertonic TTS SAPI 5 Test Console");
            
            // Adjust paths relative to the project root
            string baseDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../.."));
            string modelsDir = Path.Combine(baseDir, "assets", "onnx");
            string stylePath = Path.Combine(baseDir, "assets", "voice_styles", "M1.json");
            
            Console.WriteLine($"Loading ONNX from: {modelsDir}");
            Console.WriteLine($"Loading Voice Style from: {stylePath}");

            try
            {
                using var engine = new SupertonicTtsEngine(modelsDir, stylePath);
                Console.WriteLine($"Engine initialized successfully. Sample Rate: {engine.SampleRate}Hz");
                
                string text = "안녕하세요? 저는 슈퍼토닉 TTS 엔진입니다. 반갑습니다!";
                Console.WriteLine($"Synthesizing: {text}");

                var watch = System.Diagnostics.Stopwatch.StartNew();
                byte[] pcmData = engine.Synthesize(text, lang: "ko", totalStep: 8);
                watch.Stop();
                
                Console.WriteLine($"Synthesis completed in {watch.ElapsedMilliseconds}ms. Output size: {pcmData.Length} bytes.");

                // Write raw PCM to a proper WAV file for testing
                string outPath = "output.wav";
                WriteWav(outPath, pcmData, engine.SampleRate);
                Console.WriteLine($"Saved to {outPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing engine: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        static void WriteWav(string path, byte[] pcmData, int sampleRate)
        {
            using var fs = new FileStream(path, FileMode.Create);
            using var bw = new BinaryWriter(fs);

            bw.Write(new[] { 'R', 'I', 'F', 'F' });
            bw.Write(36 + pcmData.Length);
            bw.Write(new[] { 'W', 'A', 'V', 'E' });
            bw.Write(new[] { 'f', 'm', 't', ' ' });
            bw.Write(16); // Subchunk1Size
            bw.Write((short)1); // AudioFormat (1 = PCM)
            bw.Write((short)1); // NumChannels
            bw.Write(sampleRate); // SampleRate
            bw.Write(sampleRate * 2); // ByteRate
            bw.Write((short)2); // BlockAlign
            bw.Write((short)16); // BitsPerSample
            bw.Write(new[] { 'd', 'a', 't', 'a' });
            bw.Write(pcmData.Length);
            bw.Write(pcmData);
        }
    }
}
