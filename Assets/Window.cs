using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SimpleWindowSystem
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Window : MonoBehaviour
    {
        public bool IsActive { get; private set; }

        [SerializeField] private Selectable firstSelected;
        [SerializeField] private bool restoreFocus;
        [SerializeField] private bool closeByCancelButton;
        private CanvasGroup _canvasGroup;
        private WindowSystem _windowSystem;
        private GameObject _restoreTarget;

        public void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _windowSystem = GetComponentInParent<WindowSystem>();
        }

        public void Activate(bool needFocus)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            if (needFocus)
            {
                EventSystem.current.SetSelectedGameObject(_restoreTarget != null ? _restoreTarget : firstSelected.gameObject);
            }

            IsActive = true;
        }

        public void Deactivate()
        {
            if (restoreFocus)
            {
                var selected = EventSystem.current.currentSelectedGameObject;
                if (selected != null && selected.transform.IsChildOf(transform))
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

        public void CloseRequestByCancelButton()
        {
            if (!closeByCancelButton) return;
            Close();
        }
    }
}
