using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damath
{
    public enum IndicatorType {Circle, Box}

    public class IndicatorHandler : MonoBehaviour
    {
        public Ruleset Rules { get; private set; }
        public Indicator Selector = null;
        public List<Indicator> Active = new List<Indicator>();
        public List<Indicator> Forced = new List<Indicator>();
        public List<Move> ValidMoves = new List<Move>();

        [Header("Prefabs")]
        [SerializeField] GameObject _selectorPrefab;
        [SerializeField] GameObject _circleIndicatorPrefab;
        [SerializeField] GameObject _boxIndicatorPrefab;

        void OnEnable()
        {
            Game.Events.OnMatchBegin += Init;
            Game.Events.OnRulesetCreate += ReceiveRuleset;
            Game.Events.OnCellSelect += MoveSelector;
            Game.Events.OnCellDeselect += Refresh;
            Game.Events.OnPieceSelect += ShowSelector;
            Game.Events.OnPieceSelect += Clear;
            Game.Events.OnPieceWait += IndicateValidMoves;
            Game.Events.OnPieceDone += ClearAll;
            Game.Events.OnRequireCapture += IndicateCapturingPieces;
            Game.Events.OnBoardUpdateMoves += UpdateValidMoves;
            Game.Events.OnRefresh += ClearAll;
            Game.Events.OnChangeTurn += IndicateCapturingPieces;
        }
        void OnDisable()
        {
            Game.Events.OnMatchBegin -= Init;
            Game.Events.OnRulesetCreate -= ReceiveRuleset;
            Game.Events.OnCellSelect -= MoveSelector;
            Game.Events.OnCellDeselect -= Refresh;
            Game.Events.OnPieceSelect -= ShowSelector;
            Game.Events.OnPieceSelect -= Clear;
            Game.Events.OnPieceWait -= IndicateValidMoves;
            Game.Events.OnPieceDone -= ClearAll;
            Game.Events.OnRequireCapture -= IndicateCapturingPieces;
            Game.Events.OnBoardUpdateMoves -= UpdateValidMoves;
            Game.Events.OnRefresh -= ClearAll;
            Game.Events.OnChangeTurn -= IndicateCapturingPieces;
        }

        public void Init(MatchController match)
        {
            InitSelector();
        }

        public void ReceiveRuleset(Ruleset rules)
        {
            Rules = rules;
        }

        public void MoveSelector(Cell cell)
        {
            if (Selector != null)
            {
                Selector.Move(cell);
            }
        }
        
        public void Refresh(Piece piece)
        {
            Selector.Hide();
            Clear();
        }

        public void Refresh(Cell cell)
        {
            Selector.Hide();
            Clear();
        }

        public void UpdateValidMoves(List<Move> moves)
        {
            this.ValidMoves = moves;
        }

        public void ShowSelector(Piece piece)
        {
            Selector.Show();
        }

        
        public void IndicateCapturingPieces(Side side)
        {
            IndicateCapturingPieces(true);
        }
        
        public void IndicateCapturingPieces(bool value)
        {
            if (!value) return;

            if (Rules.EnableMandatoryCapture)
            {
                if (ValidMoves.Count != 0)
                {
                    foreach (var move in ValidMoves)
                    {
                        CreateIndicator(IndicatorType.Box, move.originCell, new Color(0.25f, 0.75f, 0.42f), true);
                    }
                }
            }
        }

        public void IndicateValidMoves(Piece piece)
        {
            if (ValidMoves.Count != 0)
            {
                foreach (Move move in ValidMoves)
                {
                    CreateIndicator(IndicatorType.Circle, move.destinationCell, Color.yellow);
                }
            }
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

        public Indicator CreateIndicator(IndicatorType indicatorType, Cell cell, Color color, bool ForceDisplay=false)
        {
            switch (indicatorType)
            {
                case IndicatorType.Circle:
                    return CreateIndicatorCircle(indicatorType, cell, color, ForceDisplay);
                case IndicatorType.Box:
                    return CreateIndicatorBox(indicatorType, cell, color, ForceDisplay);
                default:
                    return null;
            }
        }

        Indicator CreateIndicatorCircle(IndicatorType indicatorType, Cell cell, Color color, bool ForceDisplay=false)
        {
            var newIndicator = Instantiate(_circleIndicatorPrefab, cell.transform.position, Quaternion.identity);
            Indicator c_indicator = newIndicator.GetComponent<Indicator>();
            SpriteRenderer c_spriteRenderer = newIndicator.GetComponent<SpriteRenderer>();
            RectTransform c_rect = newIndicator.GetComponent<RectTransform>();

            newIndicator.name = $"Indicator {indicatorType}";
            newIndicator.transform.SetParent(this.transform);
            newIndicator.transform.position = cell.transform.position;
            // newIndicator.transform.localScale = new Vector3(1f, 1f, 1f);
            (c_indicator.Col, c_indicator.Row) = (cell.Col, cell.Row);
            c_indicator.ForceDisplay = ForceDisplay;
            c_spriteRenderer.color = color;
            newIndicator.SetActive(true);
            if (ForceDisplay) Forced.Add(c_indicator);
            else Active.Add(c_indicator);
            return c_indicator;
        }

        Indicator CreateIndicatorBox(IndicatorType indicatorType, Cell cell, Color color, bool ForceDisplay=false)
        {
            var newIndicator = Instantiate(_boxIndicatorPrefab, cell.transform.position, Quaternion.identity);
            Indicator c_indicator = newIndicator.GetComponent<Indicator>();
            SpriteRenderer c_spriteRenderer = newIndicator.GetComponent<SpriteRenderer>();
            RectTransform c_rect = newIndicator.GetComponent<RectTransform>();

            newIndicator.name = $"Indicator {indicatorType}";
            newIndicator.transform.SetParent(this.transform);
            newIndicator.transform.position = cell.transform.position;
            // newIndicator.transform.localScale = new Vector3(1f, 1f, 1f);
            (c_indicator.Col, c_indicator.Row) = (cell.Col, cell.Row);
            c_indicator.ForceDisplay = ForceDisplay;
            c_spriteRenderer.color = color;
            newIndicator.SetActive(true);
            if (ForceDisplay) Forced.Add(c_indicator);
            else Active.Add(c_indicator);
            return c_indicator;
        }
        

        public void Clear()
        {
            if (Active.Count != 0)
            {
                foreach (var i in Active)
                {
                    Destroy(i.gameObject);
                }
                Active.Clear();
            }
        }

        public void Clear(Piece piece)
        {
            if (Active.Count != 0)
            {
                foreach (var i in Active)
                {
                    Destroy(i.gameObject);
                }
                Active.Clear();
            }
        }

        public void ClearAll(Piece piece)
        {
            ClearAll();
        }

        public void ClearAll()
        {
            Selector.Hide();

            if (Active.Count != 0)
            {
                foreach (var i in Active)
                {
                    Destroy(i.gameObject);
                }
                Active.Clear();
            }

            if (Forced.Count != 0)
            {
                foreach (var i in Forced)
                {
                    Destroy(i.gameObject);
                }
                Forced.Clear();
            }
        }

        void Show(Indicator indicator = null) 
        {
            if (indicator != null)
            {
                indicator.gameObject.SetActive(true); 
                return;
            }

            if (Active.Count == 0) return;
            foreach (var i in Active)
            {
                i.gameObject.SetActive(true);
            }
        }

        void Hide(Indicator indicator = null)
        {
            if (indicator != null)
            {
                indicator.gameObject.SetActive(true); 
                return;
            }

            if (Active.Count == 0) return;
            foreach (var i in Active)
            {
                if (i == null) return;
                i.gameObject.SetActive(false);
            }
        }
    }
}