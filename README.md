# Chernobyl Relay Chat Rebirth
An IRC-based chat app for Anomaly, originally developed by TKGP for CoC. Features an independent client as well as in-game chat, automatic death messages, and compatibility with all other addons.

# [Download](https://github.com/8r2y5/Chernobyl-Relay-Chat-Rebirth/releases/tag/0.7.1)

## Note
Currently (since [0.5.0](https://github.com/8r2y5/Chernobyl-Relay-Chat-Rebirth/releases/tag/0.5.0) release) russian localisation is done using translator.

# Features
## Faction colors in chat
![Preview](./preview/faction_colors.png)

## Ability to identify with IRC server
You can provide password in `Options`.  
To register your nick type, `/msg NickServ REGISTER [password] [email]` and follow the instructions.

## Commands
| Command    | Description                                     | Usage                   | Note                                                                           |
|------------|-------------------------------------------------|-------------------------|--------------------------------------------------------------------------------|
| `/block`   | Blocks interactions/messages with provided user | `/block [nick]`         |                                                                                |
| `/unblock` | Unblocks previously blocked user                | `/unblock [nick]`       |                                                                                |
| `/list`    | Shows list of currently blocked users           | `/list`                 |                                                                                |
| `/help`    | Shows help message for command                  | `/help block`           |                                                                                |
| `/commands` | Shows avaliable commands                        | `/commands`             |                                                                                |
| `/msg`      | Sends private message to user                   | `/msg [nick] [message]` |                                                                                |
| `/nick` | Changes you nicname in chat.                    | `/nick [new nick]`       | Nick cannot contain space, it's IRC limitation                                 |
| `/reply` | Replyes to last private message/dm | `/reply [message]` |                                                                                |
| `/r` | Alias for `/reply` | `/r [message]` |                                                                                |
| `/pay` | Transfers money to another user | `/pay [nick] [amount]` | Both need to be in-game. There is option to block money transfer in `Options`. |                       |

# Official CRCR Discord Server
[Join](https://discord.gg/KjNHXCkHr9) to get help, leave feedback or just to hang out! 

# Installation
1. Install the [.NET framework](https://www.microsoft.com/net/download/framework) if you don't have it already  
2. Extract the contents of the CRCR.zip wherever you like
3. Copy the included gamedata folder to your game directory (MOD MANAGERS WILL NOT WORK) 

# Usage
Run Chernobyl Relay Chat Rebirth.exe; the application must be running for in-game chat to work.  
After connecting, click the Options button to change your name and other settings, then launch Anomaly.  
Once playing, press Enter (by default) to bring up the chat interface and Enter again to send your message, or Escape to close without sending.  
You may use text commands from the game or client by starting with a /. Use /commands to see all available commands.  

# What's Planned  
- Advanced anti-spam and moderation features
- New interface
- (possibly) Own private IRC-server
- New In-game chat GUI and (possibly) integrating it in Anomaly's 3D PDA  

# Original Credits
TKGP: Original CRC  
EveNaari: Huge help with C#, Microsoft Visual Studio  
GitHub: Octokit  
Max Hauser: semver  
Mirco Bauer: SmartIrc4Net  
nixx quality: GitHubUpdate  
  
av661194, OWL, XMODER, Anchorpoint: Russian translation  
Rebirth changes: Anchorpoint
