using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace SimpleWindowSystem
{
    public class WindowSystem : MonoBehaviour
    {
        public bool NeedFocus { get; private set; }
        public UnityEvent<bool> NeedFocusChanged { get; } = new();

        [SerializeField] private InputSystemUIInputModule inputSystemUIInputModule;
        [SerializeField] private Window firstWindowPrefab;
        [SerializeField] private Text debugLabel;
        private readonly List<Window> _windows = new();

        public void Start()
        {
            UpdateDevice(InputSystem.devices.FirstOrDefault());
            inputSystemUIInputModule.move.action.performed += EventReceived;
            inputSystemUIInputModule.submit.action.performed += EventReceived;
            inputSystemUIInputModule.cancel.action.performed += EventReceived;
            inputSystemUIInputModule.leftClick.action.performed += EventReceived;
            inputSystemUIInputModule.middleClick.action.performed += EventReceived;
            inputSystemUIInputModule.rightClick.action.performed += EventReceived;

            inputSystemUIInputModule.cancel.action.performed += OnCancel;

            Open(firstWindowPrefab);
        }

        public void Open(Window windowPrefab)
        {
            _windows.LastOrDefault()?.Deactivate();

            var newWindowInstance = Instantiate(windowPrefab, transform);
            newWindowInstance.Activate(NeedFocus);

            _windows.Add(newWindowInstance);
        }

        public void Close(Window windowInstance)
        {
            _windows.Remove(windowInstance);
            Destroy(windowInstance.gameObject);

            var frontWindow = _windows.Last();
            if (!frontWindow.IsActive)
            {
                frontWindow.Activate(NeedFocus);
            }
        }

        private void EventReceived(InputAction.CallbackContext context)
        {
            UpdateDevice(context.control.device);
        }

        private void UpdateDevice(InputDevice device)
        {
            debugLabel.text = $"CurrentDevice: {device.name}";

            var newNeedFocus = device is not Pointer; // Pointerはマウスとかタッチスクリーンとか
            if (NeedFocus == newNeedFocus) return;

            NeedFocus = newNeedFocus;
            NeedFocusChanged.Invoke(NeedFocus);
            if (NeedFocus && EventSystem.current.currentSelectedGameObject == null)
            {
                _windows.LastOrDefault()?.Activate(NeedFocus);
            }
            else if (!NeedFocus && EventSystem.current.currentSelectedGameObject != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        private void OnCancel(InputAction.CallbackContext obj)
        {
            var target = _windows.LastOrDefault();
            if (target == null) return;

            target.CloseRequestByCancelButton();
        }

        public void Update()
        {
            if (!NeedFocus)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
}
