using UnityEngine;

namespace Damath
{
    public class Selector : MonoBehaviour
    {
        public int Col, Row;
        public Color Color = Colors.SkyBlue;
        RectTransform rect;
        SpriteRenderer spriteRenderer;
        
        void Awake()
        {
            rect = GetComponent<RectTransform>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        void Start()
        {
            Game.Events.OnPieceSelect += Move;
            Game.Events.OnPieceDeselect += Hide;
            Game.Events.OnPieceCapture += Hide;
            Game.Events.OnPieceDone += Hide;
        }
        
        void OnDisable()
        {
            Game.Events.OnPieceSelect -= Move;
            Game.Events.OnPieceDeselect -= Hide;
            Game.Events.OnPieceCapture += Hide;
            Game.Events.OnPieceDone -= Hide;
        }

        public void Move(Piece piece)
        {
            Show();
            transform.position = piece.Cell.transform.position;
        }
        
        public void Show()
        {
            spriteRenderer.enabled = true;
        }

        public void Hide()
        {
            spriteRenderer.enabled = false;
        }

        public void Hide(Piece piece)
        {
            Hide();
        }
        public void Hide(Move move)
        {
            Hide();
        }
    }
}
