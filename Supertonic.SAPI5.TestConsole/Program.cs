using System;
using Supertonic.SAPI5.Core;

namespace Supertonic.SAPI5.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Supertonic TTS SAPI 5 Test Console");
            
            // Assume models are placed in a 'models' directory alongside the executable
            string modelsDir = "models";
            
            try
            {
                using var engine = new SupertonicTtsEngine(modelsDir);
                Console.WriteLine("Engine initialized successfully.");
                
                // Test synthesis
                // byte[] audio = engine.Synthesize("Hello world");
                // File.WriteAllBytes("output.pcm", audio);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing engine: {ex.Message}");
            }
        }
    }
}
