using UnityEngine;
using UnityEngine.UI;

namespace SimpleWindowSystem
{
    [RequireComponent(typeof(Button))]
    public class CommonButton : MonoBehaviour
    {
        private Button _button;
        private ColorBlock _buttonColorForGamepadOrKeyboard;
        private ColorBlock _buttonColorForMouse;

        public void Awake()
        {
            _button = GetComponent<Button>();

            _buttonColorForGamepadOrKeyboard = _button.colors;
            _buttonColorForGamepadOrKeyboard.highlightedColor = _button.colors.normalColor;
            _buttonColorForGamepadOrKeyboard.selectedColor = _button.colors.highlightedColor;

            _buttonColorForMouse = _button.colors;
            _buttonColorForMouse.highlightedColor = _button.colors.highlightedColor;
            _buttonColorForMouse.selectedColor = _button.colors.normalColor;

            var windowSystem = GetComponentInParent<WindowSystem>();
            UpdateButtonColor(windowSystem.NeedFocus);
            windowSystem.NeedFocusChanged.AddListener(UpdateButtonColor);
        }

        private void UpdateButtonColor(bool needFocus)
        {
            _button.colors = needFocus ? _buttonColorForGamepadOrKeyboard : _buttonColorForMouse;
        }
    }
}