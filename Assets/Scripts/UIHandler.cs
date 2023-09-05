using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace Damath
{
    public class UIHandler : MonoBehaviour
    {
        public static UIHandler Main;
        [SerializeField] Canvas Canvas;
        public List<Sprite> icons;
        public Dictionary<string, Sprite> Icons = new();
        
        [Header("Prefabs")]
        public GameObject windowPrefab;
        public GameObject choicePrefab;

        void Awake()
        {
            Main = this;
        }

        void Start()
        {
            
        }

        public void PlayTransition()
        {
            
        }

        public void AddIcon(string name, Sprite sprite)
        {
            Icons.Add(name, sprite);
        }

        /// <summary>
        /// Creates an empty window.
        /// </summary>
        public Window CreateWindow()
        {
            var newWindow = Instantiate(windowPrefab);
            newWindow.transform.SetParent(Canvas.transform);
            return newWindow.GetComponent<Window>();
        }
        
        /// <summary>
        /// Creates a window given a window prefab.
        /// </summary>
        public Window CreateWindow(GameObject prefab)
        {
            var newWindow = Instantiate(prefab);
            newWindow.transform.SetParent(Canvas.transform);
            return newWindow.GetComponent<Window>();
        }
    }
}
