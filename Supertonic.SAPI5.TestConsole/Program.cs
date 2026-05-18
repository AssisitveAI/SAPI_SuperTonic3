using System;
using System.Reflection;
using System.Speech.Synthesis.TtsEngine;

class Test
{
    static void Main()
    {
        var type = typeof(TtsEngineSsml);
        Console.WriteLine("TtsEngineSsml constructors:");
        foreach (var c in type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            Console.WriteLine(c);

        Console.WriteLine("\nFragmentState properties:");
        foreach (var p in typeof(FragmentState).GetProperties())
            Console.WriteLine(p.Name + " : " + p.PropertyType);

        Console.WriteLine("\nSpeechEventInfo properties/fields:");
        foreach (var p in typeof(SpeechEventInfo).GetProperties())
            Console.WriteLine(p.Name);

        Console.WriteLine("\nITtsEngineSite methods:");
        foreach (var m in typeof(ITtsEngineSite).GetMethods())
            Console.WriteLine(m);
    }
}
