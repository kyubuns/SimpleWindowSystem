using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SimpleWindowSystem
{
    public class CommonButton : Selectable, IPointerClickHandler, ISubmitHandler, IPointerMoveHandler
    {
        [Serializable]
        public class ButtonClickedEvent : UnityEvent {}

        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private ButtonClickedEvent m_OnClick = new();

        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color selectedColor = Color.yellow;
        [SerializeField] private Color pressedColor = Color.cyan;
        private Graphic _targetGraphic;

        protected CommonButton()
        {
        }

        public ButtonClickedEvent onClick
        {
            get => m_OnClick;
            set => m_OnClick = value;
        }

        protected override void Awake()
        {
            base.Awake();
            _targetGraphic = GetComponent<Graphic>();
        }

        private async void Press()
        {
            if (!IsActive() || !IsInteractable())
            {
                return;
            }

            var eventSystem = EventSystem.current;
            _targetGraphic.color = pressedColor;
            eventSystem.gameObject.SetActive(false);
            await Task.Delay(1000);
            eventSystem.gameObject.SetActive(true);
            _targetGraphic.color = selectedColor;

            m_OnClick.Invoke();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            Press();
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            Press();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            _targetGraphic.color = selectedColor;
            base.OnSelect(eventData);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            _targetGraphic.color = normalColor;
            base.OnDeselect(eventData);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                return;
            }
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}