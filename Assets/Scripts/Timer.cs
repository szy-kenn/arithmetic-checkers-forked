using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Damath
{
    public enum Format {MM_SS, SS}

    public class Timer : MonoBehaviour
    {
        public Format Format = Format.MM_SS;
        public float startingTimeInSeconds = 0f;
        public float currentTime = 0f;
        public bool IsEnabled = false;
        public bool IsRunning = false;
        public List<UnityAction> callbacks = new List<UnityAction>();
        public TextMeshProUGUI text = null;


        void Update()
        {
            if (!IsRunning) return;
            
            currentTime -= 1 * Time.deltaTime;

            if (text != null)
            {
                text.text = Format switch
                {
                    Format.MM_SS => ToMM_SS(),
                    Format.SS => ToSS(),
                    _ => ToSS()
                };
            }

            if (currentTime < 0)
            {
                Finish();
            }
        }

        public void Init(float value)
        {
            currentTime = value;
        }

        public void SetFormat(Format format)
        {
            Format = format;
        }

        public string ToMM_SS()
        {
            TimeSpan time = TimeSpan.FromSeconds(currentTime);

            return time.ToString("mm':'ss");
        }

        public string ToSS()
        {
            TimeSpan time = TimeSpan.FromSeconds(currentTime);

            return time.ToString("ss");
        }
        
        public void Begin()
        {
            if (IsRunning) return;
            IsRunning = true;
        }

        public void Stop()
        {
            if (!IsRunning) return;
            IsRunning = false;
        }

        public void Reset(bool start = false)
        {
            currentTime = startingTimeInSeconds;
            IsRunning = start;
        }
        
        
        public void Finish()
        {
            IsRunning = false;

            if (callbacks.Count == 0) return;

            foreach (var c in callbacks)
            {
                c();
            }
        }

        public float GetTime()
        {
            return currentTime;
        }

        public void SetTime(float valueInSeconds)
        {
            startingTimeInSeconds = valueInSeconds;
            currentTime = valueInSeconds;
        }

        public void ChangeTime(float valueInSeconds)
        {
            currentTime = valueInSeconds;
        }
        
        public void Invoke()
        {
            foreach (var c in callbacks)
            {
                c();
            }
        }
        
        public void AddTime(float valueInSeconds)
        {
            currentTime += valueInSeconds;
        }
        
        public void RemoveTime(float valueInSeconds)
        {
            currentTime -= valueInSeconds;
        }

        public void AddCallback(UnityAction function)
        {
            callbacks.Add(function);
        }

        public void SetText(TextMeshProUGUI value)
        {
            this.text = value;
        }
    }
}