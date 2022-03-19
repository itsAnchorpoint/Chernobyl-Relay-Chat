using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chernobyl_Relay_Chat
{
    class CRCCommand
    {
        public string Name;
        public string Help;
        private string usage;
        private int argCount;
        private bool longArg;
        private Action<List<string>, ICRCSendable> action;

        private static Regex argRx = new Regex(@"\S+");

        public CRCCommand(string name, string setUsage, string setHelp, int setArgCount, bool setLongArg, Action<List<string>, ICRCSendable> setAction)
        {
            Name = name;
            usage = CRCStrings.Localize("command_usage") + setUsage;
            Help = setHelp + ' ' + usage;
            argCount = setArgCount;
            longArg = setLongArg;
            action = setAction;
        }

        public void Process(string input, ICRCSendable output)
        {
            List<string> args = new List<string>();
            MatchCollection argMatches = argRx.Matches(input);
            if (argMatches.Count < argCount || (argMatches.Count > argCount && !longArg))
            {
                output.AddError(usage);
                return;
            }
            for (int index = 0; index < argCount; index++)
            {
                if (index == (argCount - 1) && longArg)
                    args.Add(input.Substring(argMatches[index].Index));
                else
                    args.Add(argMatches[index].Value);
            }
            action(args, output);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
