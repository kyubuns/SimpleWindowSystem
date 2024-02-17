using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SimpleWindowSystem
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Window : MonoBehaviour
    {
        [SerializeField] private Selectable firstSelected;
        [SerializeField] private bool restoreFocus;

        public bool IsActive { get; private set; }
        private CanvasGroup _canvasGroup;
        private WindowSystem _windowSystem;
        private GameObject _restoreTarget;

        public void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _windowSystem = GetComponentInParent<WindowSystem>();
        }

        public void Activate()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            EventSystem.current.SetSelectedGameObject(_restoreTarget != null ? _restoreTarget : firstSelected.gameObject);

            IsActive = true;
        }

        public void Deactivate()
        {
            if (restoreFocus)
            {
                var selected = EventSystem.current.currentSelectedGameObject;
                if (selected.transform.IsChildOf(transform))
                {
                    _restoreTarget = selected;
                }
            }
            _canvasGroup.alpha = 0.5f;
            _canvasGroup.interactable = false;

            IsActive = false;
        }

        protected void Open(Window windowPrefab)
        {
            _windowSystem.Open(windowPrefab);
        }

        protected void Close()
        {
            _windowSystem.Close(this);
        }
    }
}
