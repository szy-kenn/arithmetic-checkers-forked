using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damath
{
    public enum IndicatorType {Circle, Box}

    public class IndicatorHandler : MonoBehaviour
    {
        public Indicator Selector = null;
        public List<Indicator> active = new List<Indicator>();
        public List<Indicator> forced = new List<Indicator>();

        [Header("Prefabs")]
        [SerializeField] GameObject _selectorPrefab;
        [SerializeField] GameObject _circleIndicatorPrefab;
        [SerializeField] GameObject _boxIndicatorPrefab;

        void OnEnable()
        {
            Game.Events.OnMatchBegin += Init;
            Game.Events.OnCellSelect += MoveSelector;
            Game.Events.OnRefresh += Hide;
        }
        void OnDisnable()
        {
            Game.Events.OnMatchBegin -= Init;
            Game.Events.OnCellSelect -= MoveSelector;
            Game.Events.OnRefresh -= Hide;
        }

        public void Init(Ruleset rules)
        {
            Debug.Log("A");
            InitSelector();
        }

        public void MoveSelector(Cell cell)
        {
            if (Selector != null)
            {
                Selector.Move(cell);
            }
        }

        public void Hide()
        {
            Selector.Hide();
            Clear();
        }

        Indicator InitSelector()
        {
            var newSelector = Instantiate(_selectorPrefab);
            newSelector.SetActive(false);
            newSelector.name = $"Selector";
            newSelector.transform.SetParent(this.transform);
            Selector = newSelector.GetComponent<Indicator>();
            return Selector;
        }

        public Indicator Create(IndicatorType indicatorType, Cell cell, Color color, bool ForceDisplay=false)
        {
            switch (indicatorType)
            {
                case IndicatorType.Circle:
                    return CreateCircle(indicatorType, cell, color, ForceDisplay);
                case IndicatorType.Box:
                    return CreateBox(indicatorType, cell, color, ForceDisplay);
                default:
                    return null;
            }
        }

        Indicator CreateCircle(IndicatorType indicatorType, Cell cell, Color color, bool ForceDisplay=false)
        {
            var newIndicator = Instantiate(_circleIndicatorPrefab, cell.transform.position, Quaternion.identity);
            Indicator c_indicator = newIndicator.GetComponent<Indicator>();
            SpriteRenderer c_spriteRenderer = newIndicator.GetComponent<SpriteRenderer>();
            RectTransform c_rect = newIndicator.GetComponent<RectTransform>();

            newIndicator.name = $"Indicator {indicatorType}";
            newIndicator.transform.SetParent(this.transform);
            newIndicator.transform.position = cell.transform.position;
            // newIndicator.transform.localScale = new Vector3(1f, 1f, 1f);
            c_indicator.ForceDisplay = ForceDisplay;
            c_spriteRenderer.color = color;
            newIndicator.SetActive(true);
            if (ForceDisplay) forced.Add(c_indicator);
            else active.Add(c_indicator);
            return c_indicator;
        }

        Indicator CreateBox(IndicatorType indicatorType, Cell cell, Color color, bool ForceDisplay=false)
        {
            var newIndicator = Instantiate(_boxIndicatorPrefab, cell.transform.position, Quaternion.identity);
            Indicator c_indicator = newIndicator.GetComponent<Indicator>();
            SpriteRenderer c_spriteRenderer = newIndicator.GetComponent<SpriteRenderer>();
            RectTransform c_rect = newIndicator.GetComponent<RectTransform>();

            newIndicator.name = $"Indicator {indicatorType}";
            newIndicator.transform.SetParent(this.transform);
            newIndicator.transform.position = cell.transform.position;
            // newIndicator.transform.localScale = new Vector3(1f, 1f, 1f);
            c_indicator.ForceDisplay = ForceDisplay;
            c_spriteRenderer.color = color;
            newIndicator.SetActive(true);
            if (ForceDisplay) forced.Add(c_indicator);
            else active.Add(c_indicator);
            return c_indicator;
        }

        public void Clear()
        {
            if (active.Count != 0)
            {
                foreach (var i in active)
                {
                    Destroy(i.gameObject);
                }
                active.Clear();
            }
        }

        public void ClearAll()
        {
            if (active.Count != 0)
            {
                foreach (var i in active)
                {
                    Destroy(i.gameObject);
                }
                active.Clear();
            }

            if (forced.Count != 0)
            {
                foreach (var i in forced)
                {
                    Destroy(i.gameObject);
                }
                forced.Clear();
            }
        }

        void Show(Indicator indicator=null) 
        {
            if (indicator != null)
            {
                indicator.gameObject.SetActive(true); 
                return;
            }

            if (active.Count == 0) return;
            foreach (var i in active)
            {
                i.gameObject.SetActive(true);
            }
        }

        void Hide(Indicator indicator=null)
        {
            if (indicator != null)
            {
                indicator.gameObject.SetActive(true); 
                return;
            }

            if (active.Count == 0) return;
            foreach (var i in active)
            {
                if (i == null) return;
                i.gameObject.SetActive(false);
            }
        }

        void MoveTo(Cell cell)
        {
            // selectionIndicatorPrefab.transform.position = cell.transform.position;
        }
    }
}