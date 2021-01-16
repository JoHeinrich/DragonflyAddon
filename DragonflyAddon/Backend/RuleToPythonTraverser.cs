using System;
using System.Collections.Generic;
using VoiceControl;

namespace DragonflyAddon
{
    public class RuleToPythonTraverser : IGrammarTraverser<string>
    {
   

        public string ComputeOption(IEnumerable<string> inner)
        {
            return $"Alternative([{string.Join(",", inner) }])";
        }

        public string ComputeSequence(IEnumerable<string> inner)
        {
            return $"Sequence([{string.Join(",", inner)}])";
        }

        public string ComputeOptional(string inner)
        {
            return $"Optional({inner})";
        }

        public string ComputeRepeat(int min, int max, string inner)
        {
            return $"Repetition({inner},min={min},max={max})";
        }

        public string ComputeReference(string name)
        {
            return $"RuleRef({new Identifier( name).Escaped})";
        }

        public string ComputeDictation(string language)
        {
            return "Impossible()";
        }

        public string ComputeNumber(int min, int max)
        {
            return "Impossible()";
        }

        public string ComputeLiteral(string text)
        {
            if (ContainsInvalidCharacter(text)) return "Impossible()";
            return $"Literal(\"{text}\")";
        }

        public string ComputeBad(string error)
        {
            return "Impossible()";
        }

        public static bool IsValidCharacter(char x)
        {
            return ('A' <= x && x <= 'Z') || ('a' <= x && x <= 'z');
        }
               
        public static bool ContainsInvalidCharacter(string input)
        {
            foreach (var c in input)
            {
                if (!IsValidCharacter(c)) return true;
            }
            return false;
        }
    }
}
