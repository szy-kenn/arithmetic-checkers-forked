using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

namespace Damath
{
    public class UIHandler : MonoBehaviour
    {
        public static UIHandler Main;
        [SerializeField] Canvas Canvas;
        public List<Sprite> icons;
        public Dictionary<string, Sprite> Icons = new();
        
        [Header("Prefabs")]
        [SerializeField] private UnityEngine.UI.Image dim;
        public GameObject windowPrefab;
        public GameObject choiceWindowPrefab;
        public GameObject choicePrefab;
        private bool IsDimmed;
        [SerializeField] private Tooltip tooltipPrefab;

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

        public Window CreateChoiceWindow()
        {
            var newChoiceWindow = Instantiate(choiceWindowPrefab);
            newChoiceWindow.transform.SetParent(Canvas.transform);
            newChoiceWindow.SetActive(false);
            return newChoiceWindow.GetComponent<Window>();
        }

        /// <summary>
        /// Creates an empty window.
        /// </summary>
        public Window CreateWindow()
        {
            var newWindow = Instantiate(windowPrefab, new Vector3(0f, 0f, Constants.ZLocationWindow), Quaternion.identity, Canvas.transform);
            newWindow.SetActive(false);
            return newWindow.GetComponent<Window>();
        }
        
        /// <summary>
        /// Creates a window given a window prefab.
        /// </summary>
        public Window CreateWindow(GameObject prefab)
        {
            var newWindow = Instantiate(prefab, new Vector3(0f, 0f, Constants.ZLocationWindow), Quaternion.identity, Canvas.transform);
            return newWindow.GetComponent<Window>();
        }

        /// <summary>
        /// Add a tooltip for a UIElement.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public void ShowTooltip(IUIElement element, string text)
        {
            tooltipPrefab.SetElement(element);
            tooltipPrefab.SetText(text);
            tooltipPrefab.SetActive(true);
        }

        public void HideTooltip()
        {
            tooltipPrefab.SetActive(false);
        }

        public Tooltip CreateTooltip(IUIElement element, string text, Color color)
        {
            var newTooltip = Instantiate(tooltipPrefab, Input.mousePosition, Quaternion.identity, Canvas.transform);
            Tooltip m_tooltip = newTooltip.GetComponent<Tooltip>();
            m_tooltip.SetElement(element);
            m_tooltip.SetText(text);
            m_tooltip.SetColor(color);
            return m_tooltip;
        }

        public void Dim(float time)
        {
            if (IsDimmed)
            {
                LeanTween.value(dim.gameObject, dim.color.a, 0f, time)
                .setEaseOutExpo()
                .setOnUpdate( (i) =>
                {
                    dim.color = new (dim.color.r, dim.color.g, dim.color.b, i);
                });
            } else
            {
                LeanTween.value(dim.gameObject, 0f, 0.25f, time)
                .setEaseOutExpo()
                .setOnUpdate( (i) =>
                {
                    dim.color = new (dim.color.r, dim.color.g, dim.color.b, i);
                });
            }
            IsDimmed = !IsDimmed;
        }
    }
}
