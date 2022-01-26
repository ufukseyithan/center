using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game.UI {
    [RequireComponent(typeof(ScrollRect))]
    public class HorizontalScrollSnap : MonoBehaviour, IDragHandler, IEndDragHandler {
        [Header("Settings")]
        [SerializeField]
        [Tooltip("Width of the individual items in the content. Beware all the items should have the same width.")]
        float itemWidth;

        RectTransform rectTransform;
        ScrollRect scrollRect;

        int itemCount;
        bool isDragging;

        void Awake() {
            rectTransform = GetComponent<RectTransform>();
            scrollRect = GetComponent<ScrollRect>();
        }

        void Start() {
            scrollRect.inertia = false;
            itemCount = scrollRect.content.childCount;
        }

        void Update() {
            float contentXPos = Mathf.Abs(scrollRect.content.anchoredPosition.x);

            for (int i = itemCount; i-- > 0;) {
                if ((i * itemWidth - 50) <= contentXPos) {
                    if (!isDragging)
                        LerpContentToPosition(GetItemPosition(i));

                    Core.GameManager.Instance.SelectedLevel = i + 1;

                    break;
                }   
            }
        }

        public float GetItemPosition(int itemIndex) => -itemIndex * itemWidth;

        public void SetContentPositionToItem(int itemIndex) => SetContentPosition(GetItemPosition(itemIndex));

        public void SetContentPosition(float position) {
            // Debug.Log(scrollRect);
            
            scrollRect.content.anchoredPosition = new Vector2(position, rectTransform.anchoredPosition.y);
        }

        public void LerpContentToPosition(float position) => SetContentPosition(Mathf.Lerp(scrollRect.content.anchoredPosition.x, position, scrollRect.elasticity));

        public void OnDrag(PointerEventData data) => isDragging = true;
    
        public void OnEndDrag(PointerEventData data) => isDragging = false;
    }
}