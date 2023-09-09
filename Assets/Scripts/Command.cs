using System.Collections.Generic;
using UnityEngine.Events;

namespace Damath
{
    public class Command
    {
        public enum ArgType {Required, Optional}
        public string Name = "";
        public string Syntax = "";
        public string Description = "";
        public List<(string, ArgType)> Arguments;
        public List<string> Parameters;
        public List<string> Aliases;
        public UnityAction<List<string>> Calls;

        public void AddParameters(List<string> Parameters)
        {
        }

        public void AddAlias(string alias)
        {
            Aliases.Add(alias);
        }
        
        public void AddCallback(UnityAction<List<string>> func = null)
        {
            if (func != null)
            {
                Calls = func;
            }
        }

        public void SetDescription(string value)
        {
            Description = value;
        }

        public void Invoke(List<string> args)
        {
            Calls(args);

            // foreach (var call in Calls)
            // {
            //     call.Invoke();
            // }
        }
    }
}
