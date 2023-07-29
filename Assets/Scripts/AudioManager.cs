using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damath
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Main { get; private set; }
        public Dictionary<string, AudioSource> sources = new Dictionary<string, AudioSource>();
        public float defaultVolume = 1f;

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

        public void AddSource(string name, AudioSource audioSource)
        {
            sources.Add(name, audioSource);
        }

        public void PlaySound(string name)
        {
            sources[name].Play();
        }
    }
}