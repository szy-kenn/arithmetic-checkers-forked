using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Damath
{
    public class Tooltip : MonoBehaviour, IUIElement, IHoverable
    {
        [field: SerializeField] public bool IsVisible { get; set; }   
        [TextArea] public string Text;
        public float ShowDelay = 1f;
        public float HideDelay = 0f;
        public bool FadeTransition = true;
        public float FadeInDuration = 0.25f;
        public float FadeOutDuration = 0.25f;
        public bool Enable = true;
        public bool IsHovered { get; set; }
        private RectTransform rect;
        private IUIElement element;
        private Image image;
        private TextMeshProUGUI tmpUGUI;

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            image = GetComponent<Image>();
            tmpUGUI = GetComponentInChildren<TextMeshProUGUI>();
            tmpUGUI.text = Text;
        }

        void Update()
        {
            if (IsVisible)
            {
                gameObject.transform.position = new Vector2(Input.mousePosition.x + (rect.sizeDelta.x * 0.55f),
                                                            Input.mousePosition.y - (rect.sizeDelta.y * 0.55f));  
            }
        }

        void Start()
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b , 0f);
            tmpUGUI.color = new Color(tmpUGUI.color.r, tmpUGUI.color.g, tmpUGUI.color.b , 0f);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsHovered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsHovered = false;
        }

        public async void SetVisible(bool value)
        {
            if (!Enable) return;
            if (element == null) return;

            if (value)
            {  
                LeanTween.cancel(gameObject);
                await Task.Delay((int)ShowDelay * 1000);
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
            this.element = obj;
        }

        public void SetText(string value)
        {
            tmpUGUI.text = value;
        }

        public void SetColor(Color color)
        {
            image.color = color;
        }
    }
}