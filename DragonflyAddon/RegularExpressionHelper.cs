using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VoiceControl
{
    public class RegularExpressionHelper
    {

        public static Dictionary<string, string> RegularExpressionDictionary(string input,string pattern,int key,int value)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            var match = Regex.Match(input,pattern);
            while (match.Success)
            {
                var block = match.Groups[key].Value;
                if (!data.ContainsKey(block))
                {
                    data.Add(block, match.Groups[value].Value);
                }
                else
                {
                    Console.WriteLine($"Provider already contains {block}");
                }
                match = match.NextMatch();
            }
            return data;
        }
        
    }
}
