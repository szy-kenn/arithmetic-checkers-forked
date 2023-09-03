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
            public string Name = "none";
            public string Description
            {
                get { return Description; }
                set { Description = value; }
            }
            public List<(string, ArgType)> Arguments;
            public List<string> Parameters;
            public List<string> Aliases;
            public UnityAction<List<string>> Calls;

            public Command(string name)
            {
                Name = name;
            }

            public void AddParameters(List<string> Parameters)
            {
            }

            public void AddAlias(string alias)
            {
                Aliases.Add(alias);
            }
            
            public void AddCallback(UnityAction<List<string>> func)
            {
                Calls = func;
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

        public Dictionary<string, Command> Commands = new();
        public static Console Main { get; private set; }
        public bool IsEnabled = false;
        public string command;
        public Window window = null;
        
        public MatchController Match = null;

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
        }

        void Update()
        {
            if (IsEnabled)
            {
                if (Input.GetKeyDown(KeyCode.Tilde))
                {
                    if (window == null) return;
                    window.Toggle();
                }
            } else
            {
                if (Input.GetKeyDown(KeyCode.F1))
                {
                    if (IsEnabled) return;
                    Enable();
                }
            }
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

        void Init()
        {
            Game.Events.OnMatchBegin += ReceiveMatchInstance;

            CreateWindow();
            input = window.transform.Find("Input").GetComponent<TMP_InputField>();
            messages = window.transform.Find("Message").GetComponent<TextMeshProUGUI>();

            input?.onSubmit.AddListener(new UnityAction<string>(GetCommand)); 

            InitCommands();
            Log($"[CONSOLE]: Started");
        }

        void CreateWindow()
        {
            if (window != null)
            {
                window.Delete();
                window = Game.UI.CreateWindow(windowPrefab);
            } else
            {
                window = Game.UI.CreateWindow(windowPrefab);
            }
            window.rect.pivot = new Vector2(0f, 0f);
            window.rect.offsetMin = new Vector2(30f, 30f);
            window.rect.offsetMax = new Vector2(400f, 160f);
        }

        public Command CreateCommand(string command)
        {
            Debug.Log("creating command " + command);
            var args = command.Split(" ");
            string commandName = args[0];
            Command newCommand = new(commandName);

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
            CreateCommand("chat <message>").AddCallback(Command_Chat);
            CreateCommand("help [command]").AddCallback(Command_Help);
            CreateCommand("match <create|get>").AddCallback(Command_Match);
            // CreateCommand("ping [player]");
            CreateCommand("move <col> <row>").AddCallback(Command_Select);
            CreateCommand("piece <select|add|remove> <col> <row>").AddCallback(Command_Select);
            CreateCommand("select <col> <row>").AddCallback(Command_Select);
            CreateCommand("selectmove <col> <row> <toCol> <toRow>").AddCallback(Command_Select);

            // CreateCommand("test <number|string>");
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
        }

        public void Refresh()
        {
            input.text = "";
        }

        public static void Log(string message)
        {
            Debug.Log(message);
        }

        /// <summary>
        /// Invokes a console command.
        /// </summary>
        public void Run(string command)
        {
            if (command.Remove(0, 1) != "/")
            {
                PromptInvalid();
                return;
            }

            List<string> args = new(command.Split());

            Command toInvoke = Commands[args[0]];
            toInvoke.Invoke(args);
        }

        // Console commands list
        #region 

        void Command_Chat(List<string> args)
        {
            Debug.Log($"Sent message: {args[1]}");
        }

        void Command_Help(List<string> args)
        {
            if (args.Count == 1)
            {
                // Run help command
                Debug.Log("List of available commands: ");
                // Iterate through command list keys
                
            } else
            {
                if (int.TryParse(args[1], out int page))
                {
                    page = page;
                } else
                {
                    Debug.Log(Commands[args[0]].Description);
                }
            }

            Debug.Log($"Usage: /{args[1]}");
        }

        void Command_Match(List<string> args)
        {
            if (args[1] == "create")
            {
                //
            } else if (args[1] == "get")
            {
                Debug.Log(Match);
            }
        }

        void Command_Select(List<string> args)
        {
            foreach (var a in args)
            {
                Debug.Log(a);
            }
            (int col, int row) = (int.Parse(args[1]), int.Parse(args[2]));
            
            Match.SelectCell(col, row);
        }
        


        #endregion 
    }
}
