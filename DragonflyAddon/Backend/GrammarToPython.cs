using System.Collections.Generic;
using System.Linq;
using VoiceControl;

namespace DragonflyAddon
{
    public class GrammarToPython
    {
        public PythonRules pythonRules;
        IRules ruleManager;

        public GrammarToPython(IRules ruleManager)
        {
            this.ruleManager = ruleManager;
            pythonRules = new PythonRules(ruleManager);
        }

        

        public string BuildGrammar(Identifier identifier, Dictionary<string,string> activations)
        {
            var references = ruleManager.GetAllReferneces(identifier.ToString ());
            var goodReferences = references.Where(x => Identifier.IsWellFormated(x)).ToList();
            var rules = goodReferences.Select(x => pythonRules[x]).ToList();
            List<string> contexts = new List<string>();
            var context = "";
            foreach (var activation in activations)
            {
                
                if (activation.Key == "AppContext")
                    contexts.Add($"AppContext({activation.Value})");
                if (activation.Key == "process_name") 
                    contexts.Add($"AppContext(executable=\"{activation.Value}\")");
                if (activation.Key == "window_title")
                    contexts.Add($"AppContext(title=\"{activation.Value}\")");
                if (activation.Key == "window_maximized")
                    contexts.Add($"AppContext(is_maximized=\"{activation.Value}\")");
            }
            if (contexts.Count!=0)
            {
                context = "grammar.set_context(" + string.Join("&", contexts)+")\n";
            }
            return "from dragonfly import *\n"
                + $"from Controllers.{identifier.Escaped} import {identifier.Escaped}"
                + "\n"

                + "class Rep(CompoundRule):\n"
                + "\tspec = \"<commands>\"\n"
                + $"\textras = [RuleRef({identifier.Escaped}, name = \"commands\")]\n"
                + "\tdef _process_recognition(self, node, extras):\n"
                + "\t\tcommand = extras[\"commands\"]\n"
                + "\t\tfor action in command:\n"

                + "\t\t\taction_op = getattr(action, \"execute\", None)\n"
                + "\t\t\tif callable(action_op):\n"
                + "\t\t\t\taction.execute()\n"
                + "\t\t\telse:\n"

                + "\t\t\t\tfor act in action:\n"
                + "\t\t\t\t\taction_op = getattr(act, \"execute\", None)\n"
                + "\t\t\t\t\tif callable(action_op):\n"
                + "\t\t\t\t\t\tact.execute()\n"

                + $"grammar = Grammar(\"test\")\n"
                + $"grammar.add_rule(Rep())\n"

                +context
                + "grammar.load()\n";
        }

        
    }
}
