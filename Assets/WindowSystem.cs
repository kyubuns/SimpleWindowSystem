using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SimpleWindowSystem
{
    public class WindowSystem : MonoBehaviour
    {
        [SerializeField] private Window firstWindowPrefab;
        private readonly List<Window> _windows = new();

        public void Start()
        {
            Open(firstWindowPrefab);
        }

        public void Open(Window windowPrefab)
        {
            if (_windows.Any())
            {
                _windows.Last().Deactivate();
            }

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
    }
}
