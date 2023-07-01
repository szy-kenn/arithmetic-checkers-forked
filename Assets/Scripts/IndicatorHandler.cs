using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IndicatorType {Circle, Box}

public class IndicatorHandler : MonoBehaviour
{
    public Indicator selector;
    public List<Indicator> active = new List<Indicator>();
    public List<Indicator> forced = new List<Indicator>();

    [Header("Prefabs")]
    [SerializeField] GameObject _selectorPrefab;
    [SerializeField] GameObject _circleIndicatorPrefab;
    [SerializeField] GameObject _boxIndicatorPrefab;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        InitSelector();
    }

    Indicator InitSelector()
    {
        var newSelector = Instantiate(_selectorPrefab);
        newSelector.SetActive(false);
        newSelector.name = $"Selector";
        newSelector.transform.SetParent(this.transform);
        newSelector.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        selector = newSelector.GetComponent<Indicator>();
        return selector;
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
        newIndicator.transform.localScale = new Vector3(1, 1, 1);
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
        newIndicator.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
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

    public void ChangeColor()
    {

    }

    void Start()
    {
        Game.Main.moveSelectionIndicator += MoveTo;
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

