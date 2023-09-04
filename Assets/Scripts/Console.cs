using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using System.Data;
using System.Text.RegularExpressions;

namespace Damath
{
    public class Console : MonoBehaviour
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

        public Player Operator = null;
        public Dictionary<string, Command> Commands = new();
        public static Console Main { get; private set; }
        public bool IsEnabled = false;
        public string command;
        public Window Window = null;
        
        public MatchController Match = null;
        public Cell SelectedCell = null;
        public Dictionary<(int, int), Cell> Cellmap;

        [SerializeField] GameObject windowPrefab;
        TMP_InputField input;
        TextMeshProUGUI messages;

        void Awake()
        {
            if (Main != null && Main != this)
            {
                Destroy(this);
            } else
            {
                Main = this;
            }
        }

        public void OnEnable()
        {
        }
        
        public void OnDisable()
        {
            IsEnabled = false;
            Game.Events.OnMatchBegin -= ReceiveMatchInstance;
            Game.Events.OnBoardUpdateCellmap -= ReceiveCellmap;
        }

        /// <summary>
        /// Enables the console. Is disabled by default.
        /// </summary>
        public void Enable()
        {
            IsEnabled = true;
            Init();
        }

        void ReceiveMatchInstance(MatchController match)
        {
            Debug.Log("received " + match);
            Match = match;
        }

        public void SetOperator(Player player)
        {
            Operator = player;
        }

        void SubscribeToEvents()
        {
            Game.Events.OnMatchBegin += ReceiveMatchInstance;
            Game.Events.OnBoardUpdateCellmap += ReceiveCellmap;
        }

        void ReceiveCellmap(Dictionary<(int, int), Cell> cellmap)
        {
            Cellmap = cellmap;
        }

        void Init()
        {
            SubscribeToEvents();
            CreateWindow();
            input = Window.transform.Find("Input").GetComponent<TMP_InputField>();
            messages = Window.transform.Find("Message").GetComponent<TextMeshProUGUI>();

            input.onSubmit.AddListener(new UnityAction<string>(GetCommand)); 

            InitCommands();
            Log($"[CONSOLE]: Started");
        }

        void CreateWindow()
        {
            if (Window != null)
            {
                Window.Delete();
                Window = Game.UI.CreateWindow(windowPrefab);
            } else
            {
                Window = Game.UI.CreateWindow(windowPrefab);
            }
            Window.rect.pivot = new Vector2(0f, 0f);
            Window.rect.offsetMin = new Vector2(30f, 30f);
            Window.rect.offsetMax = new Vector2(400f, 160f);
            Window.Close();
        }

        public Command CreateCommand(string command, string description = "")
        {
            var args = command.Split(" ");
            string commandName = args[0];
            Command newCommand = new()
            {
                Name = args[0],
                Syntax = command,
                Description = description
            };

            int i = 0;
            foreach (string arg in args)
            {
                if (i == 0) continue;

                if (arg.Contains("|"))
                {
                    string[] splitArgs = arg.Split("|");
                }

                if (arg.Contains("<"))
                {
                    // Required
                    string substring = arg[1..^1];
                    newCommand.Arguments.Add((substring, Command.ArgType.Required));
                } else if (arg.Contains("["))
                {
                    // Optional
                    string substring = arg[1..^1];
                    newCommand.Arguments.Add((substring, Command.ArgType.Optional));
                }
                i++;
            }

            Commands.Add(commandName, newCommand); 
            return newCommand;           
        }

        void InitCommands()
        {
            CreateCommand("chat <message>",
                          "Send a message.").AddCallback(Command_Chat);

            CreateCommand("connect <address>",
                          "Connect to a match.").AddCallback(Command_Connect);

            CreateCommand("draw",
                          "Offer a draw.").AddCallback();
                        
            CreateCommand("forfeit",
                          "Forfeit match.").AddCallback();

            CreateCommand("help <command>").AddCallback(Command_Help);

            CreateCommand("host",
                          "Host current match.").AddCallback(Command_Host);

            CreateCommand("match <create> <classic|speed|custom>").AddCallback(Command_Match);

            CreateCommand("piece <s|s> <col> <row> [value]",
                          "Piece actions.").AddCallback(Command_Piece);

            CreateCommand("select <col> <row>",
                          "Select a cell.").AddCallback(Command_Select);
        }

        /// <summary>
        /// Prompts invalid command usage.
        /// </summary>
        void PromptInvalid()
        {
            Log("Unknown command. Type /help for a list of available commands");
        }

        void PromptInvalid(string command)
        {
            Log($"Invalid command usage. Try /help {command}");
        }

        public void GetCommand(string input)
        {
            if (input == "") return;
            command = input;
            Run(command);
            this.input.text = "";
            this.input.Select();
        }

        public void Refresh()
        {
            input.text = "";
        }

        public void Log(object message)
        {
            if (message.GetType() != typeof(string))
            {
                Debug.Log(message);
            } else
            {
                messages.text += $"\n{message}";
            }
        }

        /// <summary>
        /// Invokes a console command.
        /// </summary>
        public void Run(string command)
        {
            if (command.Contains("/")) command = command.Replace("/", "");

            List<string> args = new(command.Split());
            Command toInvoke;

            try
            {
                toInvoke = Commands[args[0]];
                Game.Events.PlayerCommand(Operator);

                try
                {
                    toInvoke.Invoke(args);
                } catch
                {  
                    PromptInvalid(args[0]);
                    return;
                }
            } catch
            {
                PromptInvalid();
                return;
            }
        }

        // Console commands list
        #region 

        void Command_Chat(List<string> args)
        {
            Log($"Sent message: {args[1]}");
        }

        void Command_Connect(List<string> args)
        {
            string ip = args[1];
        }

        void Command_Help(List<string> args)
        {
            if (args.Count == 1)
            {
                // Run help command
                Log("List of available commands: ");
                // Iterate through command list keys
                
            } else
            {
                if (int.TryParse(args[1], out int page))
                {
                    Log($"Showing page {page}");
                } else
                {
                    Log($"Usage: " + Commands[args[1]].Syntax);
                    Log(Commands[args[1]].Description);
                }
            }
        }

        void Command_Host(List<string> args)
        {
            string ip = args[1];
        }

        void Command_Match(List<string> args)
        {
            if (args[1] == "create")
            {
                if (args[2] == "classic")
                {
                    Game.Main.CreateMatch(new Ruleset(Gamemode.Classic));
                } else if (args[2] == "speed")
                {
                    Game.Main.CreateMatch(new Ruleset(Gamemode.Speed));
                } else if (args[3] == "custom")
                {
                    Game.Main.CreateMatch(new Ruleset(Gamemode.Custom));
                } else
                {
                    PromptInvalid(args[0]);
                    return;
                }
                Log($"Created match with mode {args[2]}");
            } else if (args[1] == "start")
            {
                if (Game.Main.Ruleset == null)
                {
                    Log("No match created. Create one with /match create <mode>");
                } else
                {
                    if (Game.Main.HasMatch)
                    {
                        Log($"A match is already running.");
                    } else
                    {
                        Game.Main.StartMatch();
                    }
                }
            } else if (args[1] == "get")
            {
                Log(Match);
            }
        }

        void Command_Piece(List<string> args)
        {
            if (args[1] == "create")
            {
                //
            } else if (args[1] == "get")
            {
                Log(Match);
            }
        }

        void Command_Select(List<string> args)
        {
            (int col, int row) = (int.Parse(args[1]), int.Parse(args[2]));
            
            Game.Events.PlayerSelect(Operator);
            Game.Events.CellSelect(Cellmap[(col, row)]);
        }
        
        void Command_Move(List<string> args)
        {
            (int col, int row) = (int.Parse(args[1]), int.Parse(args[2]));
            
            Game.Events.PlayerSelect(Operator);
            Game.Events.CellSelect(Cellmap[(col, row)]);
        }
        
        #endregion
    }
}
