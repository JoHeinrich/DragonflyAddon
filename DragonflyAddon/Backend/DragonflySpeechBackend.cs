using System;
using System.Collections.Generic;
using System.IO;
using VoiceControl;

namespace DragonflyAddon
{
    public class DragonflySpeechBackend : ISpeechBackend
    {
        private readonly IRules rules;
        private readonly ILog log;
        private readonly IProfileConfiguration profileConfiguration;
        private readonly IPaths paths;
        public string ControllerDirectory;
        public string GrammarDirectory;

        bool listening;

        public event OnRecognitionHandler OnRecognition = x=> { };

        public bool Listen
        {
            get
            {
                return listening;
            }
            set
            {
                listening = value;
            }
        }

        public DragonflySpeechBackend(IRules rules, ILog log,IProfileConfiguration profileConfiguration,IPaths paths)
        {
            this.rules = rules;
            this.log = log;
            this.profileConfiguration = profileConfiguration;
            this.paths = paths;
            
            GrammarDirectory = paths.GetPath("Dragonfly");
            ControllerDirectory = Path.Combine(GrammarDirectory,"Controllers");
        }

        DragonflyPythonListener activeListener;

        Dictionary<string,DragonflyPythonListener> listeners = new Dictionary<string,DragonflyPythonListener>();


        public void Add(IGrammar grammar)
        {
            GrammarToPython ruleToPython = new GrammarToPython(rules);
            string text = ruleToPython.BuildGrammar(new Identifier( grammar.Identifier),profileConfiguration.GetActivationConditions(grammar.Identifier));
            PrepareDirectory();
            WriteRuleFiles(ruleToPython.pythonRules);
            var grammarPath = Path.Combine(GrammarDirectory, $"_{new Identifier(grammar.Identifier).Escaped}.py");
            File.WriteAllText(grammarPath, text);
            DragonflyPythonListener listener = new DragonflyPythonListener(grammarPath, log);
            if(listeners.ContainsKey(grammar.Identifier))
            {
                var old = listeners[grammar.Identifier];
                old.Dispose();
                if (activeListener == old) activeListener = null;
                listeners.Remove(grammar.Identifier);
            }
            listeners.Add(grammar.Identifier,listener);

            //listener.RegisterGrammar();
            if (activeListener ==null )
            {
                listener.OnRecognition+= x =>
                {
                    var args = new OnRecognitionEventArgs { confidence = 1, identifier = grammar.Identifier, speechResult = new SimpleSpeechResult(x) };
                    OnRecognition(args);
                };
                activeListener = listener;
            }
   
            log.RunTask(listener.RegisterGrammar);

        }




        void PrepareDirectory()
        {
            Directory.CreateDirectory(ControllerDirectory);
            var RuleDirectory = paths.GetPath("Dragonfly/Rules");
            File.Create(Path.Combine(ControllerDirectory, "__init__.py")).Close();
            File.Create(Path.Combine(RuleDirectory, "__init__.py")).Close();
        }

        void WriteRuleFiles(PythonRules pythonRules)
        {
            foreach (var rule in pythonRules.Buffered)
            {
                File.WriteAllText(Path.Combine(ControllerDirectory,new Identifier(rule).Escaped + ".py"), pythonRules[rule]);
            }
        }

        public bool Simulate(string input)
        {
            throw new NotImplementedException();
        }

        public void Remove(IGrammar grammar)
        {
            throw new NotImplementedException();
        }

        public void SetGrammarActivation(IGrammar grammar, bool enabled)
        {
            
        }

        public void Dispose()
        {
            foreach (var listener in listeners.Values)
            {
                listener.Dispose();
            }
        }
    }
}