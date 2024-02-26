using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace SimpleWindowSystem
{
    public class WindowSystem : MonoBehaviour
    {
        [SerializeField] private InputSystemUIInputModule inputSystemUIInputModule;
        [SerializeField] private Window firstWindowPrefab;
        private readonly List<Window> _windows = new();

        public void Start()
        {
            inputSystemUIInputModule.cancel.action.performed += OnCancel;

            Open(firstWindowPrefab);
        }

        public void Open(Window windowPrefab)
        {
            _windows.LastOrDefault()?.Deactivate();

            var newWindowInstance = Instantiate(windowPrefab, transform);
            newWindowInstance.Activate();

            _windows.Add(newWindowInstance);
        }

        public void Close(Window windowInstance)
        {
            _windows.Remove(windowInstance);
            Destroy(windowInstance.gameObject);

            var frontWindow = _windows.Last();
            if (!frontWindow.IsActive)
            {
                frontWindow.Activate();
            }
        }

        private void OnCancel(InputAction.CallbackContext obj)
        {
            var target = _windows.LastOrDefault();
            if (target == null) return;

            target.CloseRequestByCancelButton();
        }
    }
}
