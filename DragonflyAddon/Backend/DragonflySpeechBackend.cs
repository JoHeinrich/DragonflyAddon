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
        public string ControllerDirectory = @"C:\Users\laise\Documents\EasyVoiceCodeTest2\Dragonfly\Controllers\";
        public string GrammarDirectory = @"C:\Users\laise\Documents\EasyVoiceCodeTest2\Dragonfly\";

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

        public DragonflySpeechBackend(IRules rules, ILog log,IProfileConfiguration profileConfiguration)
        {
            this.rules = rules;
            this.log = log;
            this.profileConfiguration = profileConfiguration;
        }

        DragonflyPythonListener activeListener;

        Dictionary<string,DragonflyPythonListener> listeners = new Dictionary<string,DragonflyPythonListener>();


        public void Add(IGrammar grammar)
        {
            GrammarToPython ruleToPython = new GrammarToPython(rules);
            string text = ruleToPython.BuildGrammar(new Identifier( grammar.Identifier),profileConfiguration.GetActivationConditions(grammar.Identifier));
            PrepareDirectory();
            WriteRuleFiles(ruleToPython.pythonRules);
            var grammarPath = GrammarDirectory + $"_{new Identifier(grammar.Identifier).Escaped}.py";
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
            File.Create(ControllerDirectory + "__init__.py").Close();
        }
        
        void WriteRuleFiles(PythonRules pythonRules)
        {
            foreach (var rule in pythonRules.Buffered)
            {
                File.WriteAllText(ControllerDirectory + new Identifier(rule).Escaped + ".py", pythonRules[rule]);
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