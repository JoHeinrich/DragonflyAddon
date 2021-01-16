using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VoiceControl;

namespace UnitTestProject
{


    [TestClass]
    public class BasicRuleToPythonTest
    {
        DragonflyToRules parser = new DragonflyToRules();
        RuleToPythonTraverser builder = new RuleToPythonTraverser();



        void Test(string trigger, string python)
        {

            var rule = parser.TriggerToRule(trigger);
            Test(rule, python);
        }

        void Test(Rule rule, string python)
        {
            string produced = rule.Traverse(builder);
            Assert.AreEqual(python, produced);
        }

        string LiteralPython = "Literal(\"test\")";

        [TestMethod]
        public void Literal()
        {
            Test("test", LiteralPython);
        }

        [TestMethod]
        public void Optional()
        {
            Test("[test]", "Optional(Literal(\"test\"))");
        }


        [TestMethod]
        public void Sequence()
        {
            Test("[test]test", $"Sequence([Optional({LiteralPython}),{LiteralPython}])");
        }

        [TestMethod]
        public void Repetition()
        {
            Test(new Repetition(new Literal("test"), 4), $"Repetition({LiteralPython},min=1,max=4)");
        }

        [TestMethod]
        public void Reference()
        {
            Test(new Reference("DragonflyAddon.Rule.Window"), "RuleRef(DragonflyAddon_Rule_Window)");
        }



        TestRuleManager ruleManager = new TestRuleManager();

        void Rule(string identifier,Rule rule)
        {
            ruleManager.rules.Add(identifier, rule);
        }
        void Rule (string identifier, string trigger)
        {
            Rule(identifier, parser.TriggerToRule(trigger));
        }
        
        void PrepareDirectory(string ControllerDirectory)
        {
            Directory.CreateDirectory(ControllerDirectory);
            foreach (var file in Directory.EnumerateFiles(ControllerDirectory))
            {
                File.Delete(file);
            }
        }


        void TestGrammar()
        {
            DragonflySpeechBackend backend = new DragonflySpeechBackend(ruleManager,new ConsoleLog());
            PrepareDirectory(backend.ControllerDirectory);

            Grammar g = new Grammar("Core.Profile.test");
            backend.Add(g);
            var grammarPath = backend.GrammarDirectory + $"_{new Identifier(g.Identifier).Escaped}.py";
            string result = ExecutePythonCommand(grammarPath);
            Assert.IsTrue(result.Contains("No handlers could be found for logger"), result);
        }

        [TestMethod]
        public void SimpleFile()
        {
            Rule("Test.Rule.second", "second");
            Rule("Core.Profile.test", "test<Test.Rule.second>");
            TestGrammar();
        }

        [TestMethod]
        public void NullRule()
        {
            Rule("Core.Profile.test", "test<Test.Rule.nullRule>");
            TestGrammar();
        }


        [TestMethod]
        public void BadReference()
        {
            Rule("Core.Profile.test", "test<badref>");
            TestGrammar();
        }

        [TestMethod]
        public void BadDragonflyReference()
        {
            Rule("Core.Profile.test", "test<Dragonfly.test.bad>");
            TestGrammar();
        }

        [TestMethod]
        public void DragonfylReference()
        {
            Rule("DragonflyAddon.Rule.Window", "should not be here");
            Rule("Core.Profile.test", "test<DragonflyAddon.Rule.Window>");
            TestGrammar();
        }


        public static string ExecutePythonCommand(string command)
        {
            return ExecutePythonCommand(@"C:\Python27\python.exe", command);
        }
        public static string ExecutePythonCommand(string pythonPath, string command)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = pythonPath;
            start.Arguments = command;
            start.UseShellExecute = false;
            start.CreateNoWindow = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardInput = true;
            start.RedirectStandardError = true;
            using (Process process = Process.Start(start))
            {
                process.WaitForExit();
                using (StreamReader reader = process.StandardError)
                {
                    string result = reader.ReadToEnd();
                    return result;
                }
            }
        }

    }
}
    
