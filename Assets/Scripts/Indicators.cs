using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IndicatorType {Move}

public class Indicators : MonoBehaviour
{
    public Color defaultColor = Color.yellow;
    public Selector Selector;

    public List<GameObject> indicators;

    [Header("Defaults")]
    [SerializeField] GameObject _moveIndicator;

    public void Create(IndicatorType indicatorType, Cell cell)
    {

        switch (indicatorType)
        {
            case IndicatorType.Move:
                    var newIndicator = Instantiate(_moveIndicator, cell.transform.position, Quaternion.identity);
                    newIndicator.name = $"Indicator {indicatorType}";
                    newIndicator.transform.SetParent(this.transform);
                    SpriteRenderer spriteRenderer = newIndicator.GetComponent<SpriteRenderer>();
                    RectTransform rect = newIndicator.GetComponent<RectTransform>();
                    newIndicator.transform.position = cell.transform.position;
                    newIndicator.transform.localScale = new Vector3(1, 1, 1);
                    newIndicator.SetActive(true);
                    indicators.Add(newIndicator);
                break;
            default:
                break;
        }
    }

    public void Clear()
    {
        foreach (GameObject indicator in indicators)
        {
            Destroy(indicator);
        }
    }

    public void Create(IndicatorType indicatorType, Cell cell, Color indicatorColor)
    {
        // var newIndicator = Instantiate();
    }

    public void ChangeColor()
    {

    }

    void Start()
    {
        Game.Main.showAllIndicators += Show;
        Game.Main.hideAllIndicators += Hide;
        Game.Main.moveSelectionIndicator += MoveTo;
        Game.Main.refresh += Hide;
    }

    void Show() 
    {
        _moveIndicator.SetActive(true);
    }

    void Hide()
    {
        _moveIndicator.SetActive(false);
    }

    void MoveTo(Cell cell)
    {
        // selectionIndicatorPrefab.transform.position = cell.transform.position;
    }
}

