using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Speech.Synthesis.TtsEngine;
using Microsoft.Win32;

namespace Supertonic.SAPI5.Core
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct WAVEFORMATEX
    {
        public short wFormatTag;
        public short nChannels;
        public int nSamplesPerSec;
        public int nAvgBytesPerSec;
        public short nBlockAlign;
        public short wBitsPerSample;
        public short cbSize;
    }

    [ComVisible(true)]
    [Guid("4F5A6B7C-8D9E-4F1A-B2C3-D4E5F6A7B8C9")]
    public class SupertonicSapiEngine : TtsEngineSsml
    {
        private SupertonicTtsEngine _engine;
        private string _lang;
        private int _sampleRate = 44100;

        public SupertonicSapiEngine() : base("")
        {
            // Dynamically resolve models directory relative to where this DLL is installed
            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string baseDir = Path.GetDirectoryName(assemblyPath) ?? @"C:\Supertonic\bin";
            
            // Assume the installer places 'models' directory at the same level as 'bin'
            // or inside the same directory. Let's point to the parent directory's 'models' folder.
            string rootDir = Directory.GetParent(baseDir)?.FullName ?? baseDir;
            string modelsDir = Path.Combine(rootDir, "models");
            string stylePath = Path.Combine(modelsDir, "voice_styles", "M1.json");
            
            _lang = "ko"; 

            try
            {
                if (Directory.Exists(modelsDir) && File.Exists(stylePath))
                {
                    _engine = new SupertonicTtsEngine(modelsDir, stylePath);
                    _sampleRate = _engine.SampleRate;
                }
            }
            catch (Exception)
            {
                // Must not throw during COM instantiation
            }
        }

        public override IntPtr GetOutputFormat(SpeakOutputFormat preferedFormat, IntPtr targetWaveFormat)
        {
            WAVEFORMATEX wfx = new WAVEFORMATEX
            {
                wFormatTag = 1, // PCM
                nChannels = 1, // Mono
                nSamplesPerSec = _sampleRate,
                wBitsPerSample = 16,
                cbSize = 0
            };
            wfx.nBlockAlign = (short)(wfx.nChannels * (wfx.wBitsPerSample / 8));
            wfx.nAvgBytesPerSec = wfx.nSamplesPerSec * wfx.nBlockAlign;

            IntPtr pWfx = Marshal.AllocCoTaskMem(Marshal.SizeOf(wfx));
            Marshal.StructureToPtr(wfx, pWfx, false);
            
            return pWfx;
        }

        public override void AddLexicon(Uri uri, string mediaType, ITtsEngineSite site) { }

        public override void RemoveLexicon(Uri uri, ITtsEngineSite site) { }

        public override void Speak(TextFragment[] fragment, IntPtr waveHeader, ITtsEngineSite site)
        {
            if (_engine == null) return;

            foreach (var frag in fragment)
            {
                if (frag.State.Action == TtsEngineAction.Speak)
                {
                    string textToSpeak = frag.TextToSpeak;

                    if (string.IsNullOrWhiteSpace(textToSpeak))
                        continue;

                    // SAPI Rate: -10 to +10
                    int sapiRate = site.Rate;
                    float speed = 1.0f;
                    if (sapiRate > 0)
                        speed = 1.0f + (sapiRate / 10.0f);
                    else if (sapiRate < 0)
                        speed = 1.0f + (sapiRate / 20.0f);

                    // Fire Sentence Boundary Event
                    var evt = new SpeechEventInfo(
                        (short)TtsEventId.SentenceBoundary,
                        0,
                        0,
                        IntPtr.Zero
                    );
                    site.AddEvents(new[] { evt }, 1);

                    try
                    {
                        byte[] pcmData = _engine.Synthesize(textToSpeak, _lang, 8, speed);

                        // Pin the array to pass its pointer
                        GCHandle handle = GCHandle.Alloc(pcmData, GCHandleType.Pinned);
                        try
                        {
                            site.Write(handle.AddrOfPinnedObject(), pcmData.Length);
                        }
                        finally
                        {
                            handle.Free();
                        }
                    }
                    catch (Exception)
                    {
                        // Ignore synthesis errors to avoid crashing host
                    }
                }
            }
        }

        [ComRegisterFunction]
        public static void Register(Type t)
        {
            string clsid = t.GUID.ToString("B");
            string tokensKey = @"SOFTWARE\Microsoft\Speech\Voices\Tokens\SupertonicVoice";
            
            using (var key = Registry.LocalMachine.CreateSubKey(tokensKey))
            {
                key.SetValue("", "Supertonic Voice (Korean)");
                key.SetValue("CLSID", clsid);
                key.SetValue("409", "Supertonic Voice (Korean)"); 
                key.SetValue("412", "Supertonic Voice (Korean)"); 
                
                using (var attrs = key.CreateSubKey("Attributes"))
                {
                    attrs.SetValue("Gender", "Male");
                    attrs.SetValue("Name", "Supertonic");
                    attrs.SetValue("Language", "412"); // 412 is Korean
                    attrs.SetValue("Vendor", "Supertonic");
                }
            }
        }

        [ComUnregisterFunction]
        public static void Unregister(Type t)
        {
            Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Speech\Voices\Tokens\SupertonicVoice", false);
        }
    }
}
