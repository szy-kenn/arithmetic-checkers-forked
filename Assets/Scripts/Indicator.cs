using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damath
{
    public class Indicator : MonoBehaviour
    {
        public Color Color;
        public Sprite Sprite;
        public bool ForceDisplay = false;
        public RectTransform rectTransform;
        public SpriteRenderer spriteRenderer;

        void Awake()
        {
            Sprite = GetComponent<Sprite>();
            rectTransform = GetComponent<RectTransform>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Move(Cell cell)
        {
            gameObject.SetActive(true);
            this.transform.position = cell.transform.position;
        }

        public void Delete()
        {
            Destroy(this);
        }

        public void SetColor(Color value)
        {
            spriteRenderer.color = value;
        }
    }
}