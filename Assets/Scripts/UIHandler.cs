using System.Collections.Generic;
using UnityEngine;

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
        public GameObject choiceWindowPrefab;
        public GameObject choicePrefab;
        public GameObject tooltipPrefab;

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
        public Tooltip CreateTooltip(IUIElement element, string text)
        {
            var newTooltip = Instantiate(tooltipPrefab, Input.mousePosition, Quaternion.identity, Canvas.transform);
            Tooltip m_tooltip = newTooltip.GetComponent<Tooltip>();
            m_tooltip.SetElement(element);
            m_tooltip.SetText(text);
            return m_tooltip;
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
    }
}
