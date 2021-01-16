using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VoiceControl;
namespace DragonflyAddon
{
    public class SimpleSpeechResult : ISpeechResult
    {
        public SimpleSpeechResult(string text)
        {
            Text = text;
            Words = text.Split(' ').ToList();
        }

        public string Text { get; }
        public List<string> Words { get; }

        public MemoryStream GetAudio(int startWord = 0, int count = 0)
        {
            throw new NotImplementedException();
        }
    }
}