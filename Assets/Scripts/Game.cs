using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Damath
{
    /// <summary>
    /// Game manager.
    /// </summary>
    public class Game : MonoBehaviour
    {
        public static Game Main { get; private set; }
        public static EventManager Events { get; private set; }
        public static Console Console { get; private set; }
        public static UIHandler UI { get; private set; }
        public static AudioManager Audio { get; private set; }
        protected bool IsAlive;
        public bool IsPaused;
        public bool HasMatch { get; private set; }
        public Ruleset ruleset = null;

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
            }
        }

        void Start()
        {
            IsAlive = true;

            // Initialize event manager

            if (Settings.EnableConsole)
            {
                Console.Enable();
            }
            // Initialize console
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
        public void CreateMatch(Ruleset ruleset, bool start = false)
        {
            this.ruleset = ruleset;

            if (start) StartMatch();
        }

        public void CreateCustom(Ruleset ruleset)
        {
            //
        }

        public void Host()
        {
            //
        }

        /// <summary>
        /// Starts an instance of the match.
        /// </summary>
        public void StartMatch()
        {
            HasMatch = true;
            LoadScene("Match", playTransition: true);
        }
    }
}
