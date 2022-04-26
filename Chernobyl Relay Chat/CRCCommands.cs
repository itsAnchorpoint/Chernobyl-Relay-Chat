using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Chernobyl_Relay_Chat
{
    class CRCCommands
    {
        private static readonly Regex commandRx = new Regex(@"^/(\S+)\s*(.*)$");

        private static readonly List<CRCCommand> commands = new List<CRCCommand>()
        {
            new CRCCommand("commands", "/commands", CRCStrings.Localize("command_commands"), 0, false, ShowCommands),
            new CRCCommand("help", CRCStrings.Localize("command_help_usage"), CRCStrings.Localize("command_help_help"), 1, false, ShowHelp),
            new CRCCommand("msg", CRCStrings.Localize("command_msg_usage"), CRCStrings.Localize("command_msg_help"), 2, true, SendQuery),
            new CRCCommand("nick", CRCStrings.Localize("command_nick_usage"), CRCStrings.Localize("command_nick_help"), 1, true, ChangeNick),
            new CRCCommand("reply", CRCStrings.Localize("command_reply_usage"), CRCStrings.Localize("command_reply_help"), 1, true, SendReply),
            new CRCCommand("pay", CRCStrings.Localize("command_pay_usage"), CRCStrings.Localize("command_pay_help"), 2, true, SendMoney),
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
            output.AddError(CRCStrings.Localize("command_not_recognized_1") + commandString + CRCStrings.Localize("command_not_recognized_2"));
        }

        private static void ShowCommands(List<string> args, ICRCSendable output)
        {
            output.AddInformation(CRCStrings.Localize("command_available_commands") + string.Join(", ", commands));
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
            output.AddError(CRCStrings.Localize("command_not_recognized_1") + args[0] + CRCStrings.Localize("command_not_recognized_2"));
        }

        private static void SendQuery(List<string> args, ICRCSendable output)
        {
            CRCClient.SendQuery(args[0], args[1]);
        }

        private static void SendMoney(List<string> args, ICRCSendable output)
        {
            if (CRCGame.DEBUG)
            {
                CRCGame.AddError(CRCStrings.Localize("crc_debug_check"));
                CRCDisplay.AddError(CRCStrings.Localize("crc_debug_check"));
                return;
            }

            if (CRCGame.disable || CRCGame.processID == -1)
            {
                output.AddError(CRCStrings.Localize("command_pay_notingame"));
            }
            else if (CRCClient.userData.ContainsKey(args[0]) && CRCClient.userData[args[0]].IsInGame == "False")
            {
                output.AddError(CRCStrings.Localize("command_pay_usernotingame"));              
            }
            else if (!Regex.IsMatch(args[1], @"^\d+$"))
            {
                output.AddError("\"" + args[1] + CRCStrings.Localize("command_pay_notanumber"));
            }
            else
            {
                CRCClient.SendMoney(args[0], args[1]);
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
                output.AddError(CRCStrings.Localize("command_reply_error"));
        }
    }
}
