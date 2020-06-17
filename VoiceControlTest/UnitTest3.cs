using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VoiceControl;
using Capture = System.Text.RegularExpressions.Capture;

namespace VoiceControlTest
{
    class DragonflyCompiler
    {
        class Token
        {
            int position;
        }
        class ReferenceToken:Token
        {
            string reference;
        }
        class TextToken : Token
        {
            string text;
        }
        class OptionToken : Token
        {
            bool open;
        }
        class OptionalToken : Token
        {
            bool open;
        }
        public string RegularExpression()
        {
            string[] tokens = {
             @"(?<optional>\[|\])",
             @"(?<option>\(|\))",
             @"(?<reference>\<w*\>)",
             @"(?<text>\w*)",
            };
            return "(?<value>" + string.Join("|", tokens) + ")+";
        }
        public bool Check(CaptureCollection captures)
        {
            Stack<string> stack = new Stack<string>();
            foreach (Capture capture in captures)
            {
                
                switch (capture.Value)
                {
                    
                    case "[":
                    case "(":
                    //case "<":
                        stack.Push(capture.Value);
                        break;
                    case "]":
                    case ")":
                        //case ">":
                        if (stack.Count == 0) return false;
                       if( stack.Pop()!= capture.Value)return false ;
                        break;
                      
                    case "|":
                       if( stack.Peek()!= "(")return false ;

                        break;

                    default:
                        break;
                }
            }
            return stack.Count == 0;
        }

        public Rule Compile(string input)
        {
            Stack<Rule> stack = new Stack<Rule>();
            Series inner = new Series();
            Rule roote = inner;
            
            Match match = Regex.Match(input, RegularExpression());

            foreach (Capture value in match.Captures)
            {
                switch (value.Value)
                {
                    
                    case "[":
                        inner = new Series();
                        Repetition repetition = new Repetition(inner);
                        stack.Push(repetition);
                        stack.Push(inner);
                        break;
                    case "]":
                        if (!(stack.Pop() is Series)) return null;
                        if (!(stack.Pop() is Repetition)) return null;
                        break;
                    case "(":
                        inner = new Series();
                        stack.Push(new Option(inner));
                        stack.Push(inner);
                        break;
                    case ")":
                        if (!(stack.Pop() is Series)) return null;
                        if (!(stack.Pop() is Option)) return null;
                        break;
                    case "|":
                        if (!(stack.Pop() is Series)) return null;
                        if (!(stack.Peek() is Option option)) return null;
                        inner = new Series();
                        option.Add(inner);
                        break;

                    default:
                        inner.Append(new Words(value.Value));
                        break;
                }
            }

            return roote;
        }
    }
    [TestClass]
    public class UnitTest3
    {
        DragonflyCompiler compiler = new DragonflyCompiler();

        [TestMethod]
        [DataRow("()", true)]
        [DataRow("[]", true)]
        [DataRow("<>", true)]
        [DataRow("test", true)]

        public void TestParsing(string input, bool matches)
        {
            Assert.AreEqual(matches, Regex.IsMatch(input, compiler.RegularExpression()));
        }

        [TestMethod]
        [DataRow("()", true)]
        [DataRow("[]", true)]
        [DataRow("<>", true)]
        [DataRow("test", true)]

        [DataRow(")(", false )]
        [DataRow("][", false )]
        [DataRow("[)", false )]
        [DataRow("(]", false )]
        public void TestBrackets(string input, bool matches)
        {
            Assert.AreEqual(matches,compiler.Check( Regex.Match(input, compiler.RegularExpression()).Groups["value"].Captures));
        }
    }
}
