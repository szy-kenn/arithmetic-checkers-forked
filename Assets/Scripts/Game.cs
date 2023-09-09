using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Damath
{
    public enum Mode {Standard, Speed, Custom}
    /// <summary>
    /// Main game controller.
    /// </summary>
    public class Game : MonoBehaviour
    {
        public static Game Main { get; private set; }
        public static EventManager Events { get; private set; }
        public static Console Console { get; private set; }
        public static NetworkManager Network { get; private set; }
        public static UIHandler UI { get; private set; }
        public static AudioManager Audio { get; private set; }
        protected bool IsAlive;
        public bool IsPaused;
        public bool HasMatch { get; private set; }
        [field: SerializeField] public bool IsHosting { get; private set; }
        public Ruleset Ruleset = null;
        public string Nickname = "Player";
        public List<Lobby> Lobbies = new();
        public MatchController Match { get; private set; }

        void Awake()
        {
            if (Main != null && Main != this)
            {
                Destroy(this);
            } else
            {
                Main = this;
                Events = GetComponentInChildren<EventManager>();
                Console = GetComponentInChildren<Console>();
                UI = GetComponentInChildren<UIHandler>();
                Audio = GetComponentInChildren<AudioManager>();
                Network = GameObject.FindGameObjectWithTag("Network").GetComponent<NetworkManager>();
            }
        }

        void Start()
        {
            IsAlive = true;
            HasMatch = false;

            if (Settings.EnableConsole)
            {
                Console.Enable();
            }

            Events.OnMatchCreate += SetMatch;
        }
        
        public void Pause(bool value)
        {
            IsPaused = value;
        }

        public void LoadScene(string scene, bool playTransition = false, float delayInSeconds = 0f)
        {
            try
            {
                if (playTransition)
                {
                    // Play transition
                    StartCoroutine(Load(scene, delayInSeconds));
                } else
                {
                    SceneManager.LoadScene(scene);
                }
            } catch (NullReferenceException e)
            {
                Debug.Log("Scene does not exist" + e);
            }
        }

        IEnumerator Load(string scene, float delayInSeconds)
        {
            yield return new WaitForSeconds(delayInSeconds);
            SceneManager.LoadScene(scene);
        }

        /// <summary>
        /// Creates a match given a ruleset.
        /// </summary>
        public void CreateMatch(Ruleset.Type mode)
        {
            Ruleset = new Ruleset(mode);
            LoadScene("Match", playTransition: true);
        }

        private void SetMatch(MatchController match)
        {
            HasMatch = true;
            Match = match;
        }

        /// <summary>
        /// Host current match.
        /// </summary>
        public void Host()
        {
            if (HasMatch)
            {
                Network.StartHost();

                Lobby lobby = CreateLobby(Ruleset.Mode);
                Events.LobbyHost(lobby);
            } else
            {
                Console.Log("No match created. Create one first with /match create <mode>");
            }
        }

        public void Join(int lobbyId)
        {
            Lobby toJoin = Main.Lobbies[lobbyId];
            Lobbies.Clear();
            Lobbies.Add(toJoin);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="isPrivate"></param>
        /// <returns></returns>
        public static Lobby CreateLobby(Ruleset.Type mode, bool isPrivate = false)
        {
            Lobby newLobby = new(Main.Lobbies.Count, isPrivate);
            newLobby.SetRuleset(mode);
            Events.LobbyCreate(newLobby);
            Console.Log($"Hosted lobby {newLobby.Id} with match {mode}");
            Main.Lobbies.Add(newLobby);
            return newLobby;
        }

        /// <summary>
        /// Starts the match.
        /// </summary>
        public void StartMatch()
        {
            if (HasMatch)
            {
                Match.Init();
            }
        }

        public void SetNickname(string value)
        {
            Nickname = value;
        }
    }
}
