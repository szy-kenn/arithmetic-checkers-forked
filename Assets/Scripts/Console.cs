using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace Damath
{
    public class Console : MonoBehaviour
    {
        public static Console Main { get; private set; }
        public Game Game { get; private set; }
        public bool IsEnabled = false;
        public string command;
        public Window window = null;

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
                this.Game = Game.Main;
            }
        }

        void Update()
        {
            if (IsEnabled)
            {
                if (Input.GetKeyDown(KeyCode.Tilde))
                {
                    if (window == null) return;
                    if (!window.IsVisible)
                    {
                        window.Open();
                    } else
                    {
                        window.Close();
                    }
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

        void Init()
        {
            CreateWindow();
            input = window.transform.Find("Input").GetComponent<TMP_InputField>();
            messages = window.transform.Find("Message").GetComponent<TextMeshProUGUI>();

            if (input != null)
            {
                input.onSubmit.AddListener(new UnityAction<string>(GetCommand));
            } 
            
            Log($"Started console.");
        }

        void CreateWindow()
        {
            if (window != null)
            {
                window.Delete();
                window = UIHandler.Main.CreateWindow(windowPrefab);
            } else
            {
                Debug.Log($"{UIHandler.Main.CreateWindow(windowPrefab)}");
                window = UIHandler.Main.CreateWindow(windowPrefab);
            }
            window.rect.pivot = new Vector2(0f, 0f);
            window.rect.offsetMin = new Vector2(30f, 30f);
            window.rect.offsetMax = new Vector2(400f, 160f);
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
            Refresh();
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
        void Run(string command)
        {
            if (command.Remove(0, 1) != "/") PromptInvalid();
            string[] args = command.Split();

            try
            {
                switch (args[0])
                {
                    case "match":
                        break;

                    case "help":
                        c_Help();
                        break;

                    case "move":
                    case "mov":

                        break;

                    case "selmove":
                    case "sm":
                        try
                        {
                            if (int.Parse(args[0]) >= 0 || int.Parse(args[0]) <= 7) return;
                            if (int.Parse(args[1]) >= 0 || int.Parse(args[1]) <= 7) return;
                            if (int.Parse(args[2]) >= 0 || int.Parse(args[2]) <= 7) return;
                            if (int.Parse(args[3]) >= 0 || int.Parse(args[3]) <= 7) return;
                            
                            c_Selmove(int.Parse(args[0]),
                                            int.Parse(args[1]), 
                                            int.Parse(args[2]),
                                            int.Parse(args[3]));
                        } catch
                        {
                            Log("Selections must be between coordinate 0 and 7.");
                        }
                        break;

                    default:
                        PromptInvalid();
                        break;
                }
            } catch
            {
                PromptInvalid(args[0]);
            }
        }

        // Console commands list
        #region 

        void c_Help()
        {
            Debug.Log("Help command ran");
        }

        void c_Match()
        {
            Debug.Log("Match command ran");
        }

        void c_Move(int toCol, int toRow)
        {
            Debug.Log("Move command ran");
        }

        void c_Select(int col, int row)
        {
            Debug.Log("Select command ran");
        }

        void c_Selmove(int fromCol, int fromRow, int toCol, int toRow)
        {
            Debug.Log("Selmove command ran");
        }

        #endregion 
    }
}
