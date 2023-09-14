using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEditor;

namespace Damath
{
    public class Tooltip : MonoBehaviour, IHoverable
    {
        public bool Enable = true;
        public bool IsVisible;
        public float ShowDelay = 1f;
        public float HideDelay = 0f;
        public bool FadeTransition = true;
        public float FadeInDuration = 0.25f;
        public float FadeOutDuration = 0.25f;
        public bool IsHovered { get; set; }
        [SerializeField] private IUIElement element;
        [SerializeField] private RectTransform rect;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI tmpUGUI;

        void Update()
        {
            // move this somewhere else
            if (IsVisible)
            {
                gameObject.transform.position = new Vector2(Input.mousePosition.x + (rect.sizeDelta.x * 0.55f),
                                                            Input.mousePosition.y - (rect.sizeDelta.y * 0.55f));  
            }
        }


        void Start()
        {
            if (!Enable) return;

            image.color = new Color(image.color.r, image.color.g, image.color.b , 0f);
            tmpUGUI.color = new Color(tmpUGUI.color.r, tmpUGUI.color.g, tmpUGUI.color.b , 0f);
            gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsHovered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsHovered = false;
        }

        public void SetText(string text)
        {
            tmpUGUI.text = text;
        }

        public async void SetVisible(bool value)
        {
            if (!Enable) return;
            if (element == null) return;

            if (value)
            {  
                LeanTween.cancel(gameObject);
                if (!IsVisible) await Task.Delay((int)ShowDelay * 1000);
                if (!element.IsHovered) return;
                SetVisibility(true);
                LeanTween.value(gameObject, image.color.a, 1f, FadeInDuration)
                .setOnUpdate((i) =>
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, i);
                });
                    
                LeanTween.value(gameObject, tmpUGUI.color.a, 1f, FadeInDuration)
                .setOnUpdate((i) =>
                {
                    tmpUGUI.color = new Color(tmpUGUI.color.r, tmpUGUI.color.g, tmpUGUI.color.b, i);
                });
            } else
            {
                LeanTween.cancel(gameObject);
                await Task.Delay((int)HideDelay * 1000);

                LeanTween.value(gameObject, image.color.a, 0f, FadeOutDuration)
                .setOnUpdate((i) =>
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, i);
                });
                    
                LeanTween.value(gameObject, tmpUGUI.color.a, 0f, FadeOutDuration)
                .setOnUpdate((i) =>
                {
                    tmpUGUI.color = new Color(tmpUGUI.color.r, tmpUGUI.color.g, tmpUGUI.color.b, i);
                })
                .setOnComplete(SetVisibility)
                .setOnCompleteParam(false);
            }
        }

        void SetVisibility(object obj)
        {
            IsVisible = (bool)obj;
            gameObject.SetActive((bool)obj);
        }

        public void SetActive(bool value)
        {
            Enable = value;
        }
        
        public void SetElement(IUIElement obj)
        {
            element = obj;
        }

        public void SetColor(Color color)
        {
            image.color = color;
        }
    }
}