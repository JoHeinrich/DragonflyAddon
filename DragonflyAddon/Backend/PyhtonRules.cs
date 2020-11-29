using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoiceControl;

namespace DragonflyAddon
{
    public interface IFixedProvider<T> : IProvider<Identifier, T> { }

    public class PythonRuleProvider: IProvider<string>
    {
        RuleToPythonTraverser builder = new RuleToPythonTraverser();
        IRules ruleManager;
        public PythonRuleProvider(IRules ruleManager)
        {
            this.ruleManager = ruleManager;
        }

        public string Get(string identifier)
        {
            try
            {
                var id = new Identifier(identifier);
                string definition = (id.Assembly == "DragonflyAddon") ?
                DragonReference(id) :
                CommandReference(id);
                return definition;
            }
            catch (Exception e)
            {
                return "from dragonfly import *\n"
                + "\n"
                + "\n"
                + $"class {identifier.Replace(".","_")}_class(BasicRule): \n"
                + "\texported = False\n"
                + "\t" + "element = Impossible() \n"
                + $"{identifier.Replace(".", "_")} = {identifier.Replace(".", "_")}_class()"

                ;
            }

        }

        private string DragonReference(Identifier identifier)
        {
            return $"from Rules.{identifier.Name} import {identifier.Name}\n"
                + $"class {identifier.Escaped}_class({identifier.Name}):\n"
                + "\texported = False\n"
                + $"{ identifier.Escaped} = {identifier.Escaped}_class()"
                ;
        }

        private string CommandReference(Identifier identifier)
        {
            IRule rule = ruleManager.Get(identifier.ToString());

            if(rule!=null)
            {
                string pythonRule = rule.Traverse(builder);
            
                List<string> imports = ruleManager
                    .GetDirectReferneces(identifier.ToString())
                    .Where(x=>x!=identifier.ToString())
                    .Select(x => CreateImport(new Identifier(x)))
                    .ToList();


                
                return "from dragonfly import *\n"
                + string.Join("\n", imports)
                + "\n"
                + "\n"
                + $"class {identifier.Escaped}_class(BasicRule): \n"
                + "\texported = False\n"
                + "\t" + "element = " + pythonRule + "\n"
                + $"{ identifier.Escaped} = {identifier.Escaped}_class()"
                ;
            }
            else
            {
                return "from dragonfly import *\n"
                + "\n"
                + "\n"
                + $"class {identifier.Escaped}_class(BasicRule): \n"
                + "\texported = False\n"
                + "\t" + "element = Impossible() \n"
                + $"{ identifier.Escaped} = {identifier.Escaped}_class()"
                ;
            }

        }
        public string CreateImport(Identifier identifier) => $"from {identifier.Escaped} import {identifier.Escaped}";

        public IEnumerable<string> Available => ruleManager.Available.Select(x => x);
    }

    public class PythonRules:ProviderBuffer<string,string>
    {

        public PythonRules(IRules ruleManager):base(new PythonRuleProvider(ruleManager))
        {

        }
    }
}
