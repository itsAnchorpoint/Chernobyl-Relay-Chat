using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Chernobyl_Relay_Chat
{
    class CRCCommands
    {
        private static readonly Regex commandRx = new Regex(@"^/(\S+)\s*(.*)$");

        private static readonly List<CRCCommand> commands = new List<CRCCommand>()
        {
            new CRCCommand("commands", "/commands", "Displays all available commands.", 0, false, ShowCommands),
            new CRCCommand("help", "/help [command] Use /commands to see all available commands.", "Displays information about the given command.", 1, false, ShowHelp),
            new CRCCommand("msg", "/msg [nick] [message]", "Sends a private message to another user.", 2, true, SendQuery),
            new CRCCommand("nick", "/nick [nick]", "Changes your nickname.", 1, true, ChangeNick),
            new CRCCommand("reply", "/reply [message]", "Sends a private message to the last user who sent you one.", 1, true, SendReply),
            new CRCCommand("pay", "/pay [nick] [amount]", "Send specified amount of in-game money to the player. Must be in-game.", 2, true, SendMoney),
        };

        public static void ProcessCommand(string message, ICRCSendable output)
        {
            Match commandMatch = commandRx.Match(message);
            string commandString = commandMatch.Groups[1].Value;
            string args = commandMatch.Groups[2].Value;
            foreach (CRCCommand command in commands)
            {
                if (command.Name == commandString)
                {
                    command.Process(args, output);
                    return;
                }
            }
            output.AddError("Command \"" + commandString + "\" not recognized. Use /commands to see all available commands.");
        }

        private static void ShowCommands(List<string> args, ICRCSendable output)
        {
            output.AddInformation("Available commands: " + string.Join(", ", commands));
        }

        private static void ShowHelp(List<string> args, ICRCSendable output)
        {
            foreach (CRCCommand command in commands)
            {
                if (command.Name == args[0])
                {
                    output.AddInformation(command.Help);
                    return;
                }
            }
            output.AddError("Command \"" + args[0] + "\" not recognized. Use /commands to see all available commands.");
        }

        private static void SendQuery(List<string> args, ICRCSendable output)
        {
            CRCClient.SendQuery(args[0], args[1]);
        }

        private static void SendMoney(List<string> args, ICRCSendable output)
        {
            if (CRCGame.disable || CRCGame.processID == -1)
            {
                output.AddError("You must be in-game to be able to send money!");
            }
            else if (Regex.IsMatch(args[1], @"^\d+$"))
            {
                CRCClient.SendMoney(args[0], args[1]);
            }
            else
            {
                output.AddError("\"" + args[1] + "\" is not a number. Please, input an amount of money you want to send.");
            }
        }

        private static void ChangeNick(List<string> args, ICRCSendable output)
        {
            string nick = args[0].Replace(' ', '_');
            string result = CRCStrings.ValidateNick(nick);
            if (result != null)
                output.AddError(result);
            else
                CRCClient.ChangeNick(nick);
        }

        private static void SendReply(List<string> args, ICRCSendable output)
        {
            if (!CRCClient.SendReply(args[0]))
                output.AddError("You can't reply if you haven't been sent any messages yet.");
        }
    }
}
