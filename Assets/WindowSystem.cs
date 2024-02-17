using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace SimpleWindowSystem
{
    public class WindowSystem : MonoBehaviour
    {
        [SerializeField] private InputSystemUIInputModule inputSystemUIInputModule;
        [SerializeField] private Window firstWindowPrefab;
        [SerializeField] private Text debugLabel;
        private readonly List<Window> _windows = new();
        private bool _needFocus;

        public void Start()
        {
            UpdateDevice(InputSystem.devices.FirstOrDefault());
            inputSystemUIInputModule.move.action.performed += EventReceived;
            inputSystemUIInputModule.submit.action.performed += EventReceived;
            inputSystemUIInputModule.cancel.action.performed += EventReceived;
            inputSystemUIInputModule.leftClick.action.performed += EventReceived;
            inputSystemUIInputModule.middleClick.action.performed += EventReceived;
            inputSystemUIInputModule.rightClick.action.performed += EventReceived;

            Open(firstWindowPrefab);
        }

        public void Open(Window windowPrefab)
        {
            _windows.LastOrDefault()?.Deactivate();

            var newWindowInstance = Instantiate(windowPrefab, transform);
            newWindowInstance.Activate(_needFocus);

            _windows.Add(newWindowInstance);
        }

        public void Close(Window windowInstance)
        {
            _windows.Remove(windowInstance);
            Destroy(windowInstance.gameObject);

            var frontWindow = _windows.Last();
            if (!frontWindow.IsActive)
            {
                frontWindow.Activate(_needFocus);
            }
        }

        private void EventReceived(InputAction.CallbackContext context)
        {
            UpdateDevice(context.control.device);
        }

        private void UpdateDevice(InputDevice device)
        {
            debugLabel.text = $"CurrentDevice: {device.name}";

            var needFocus = device is Keyboard or Gamepad;
            if (_needFocus == needFocus) return;

            _needFocus = needFocus;
            if (_needFocus && EventSystem.current.currentSelectedGameObject == null)
            {
                _windows.LastOrDefault()?.Activate(_needFocus);
            }
            else if (!_needFocus && EventSystem.current.currentSelectedGameObject != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
}
