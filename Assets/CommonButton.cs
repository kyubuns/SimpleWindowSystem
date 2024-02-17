using UnityEngine;
using UnityEngine.UI;

namespace SimpleWindowSystem
{
    [RequireComponent(typeof(Button))]
    public class CommonButton : MonoBehaviour
    {
        private Button _button;
        private WindowSystem _windowSystem;
        private bool _needFocus;
        private ColorBlock _buttonColorForGamepadOrKeyboard;
        private ColorBlock _buttonColorForMouse;

        public void Awake()
        {
            _button = GetComponent<Button>();
            _windowSystem = GetComponentInParent<WindowSystem>();

            _buttonColorForGamepadOrKeyboard = _button.colors;
            _buttonColorForGamepadOrKeyboard.highlightedColor = _button.colors.normalColor;
            _buttonColorForGamepadOrKeyboard.selectedColor = _button.colors.highlightedColor;

            _buttonColorForMouse = _button.colors;
            _buttonColorForMouse.highlightedColor = _button.colors.highlightedColor;
            _buttonColorForMouse.selectedColor = _button.colors.normalColor;
        }

        public void Update()
        {
            if (_needFocus != _windowSystem.NeedFocus)
            {
                _needFocus = _windowSystem.NeedFocus;
                _button.colors = _needFocus ? _buttonColorForGamepadOrKeyboard : _buttonColorForMouse;
            }
        }
    }
}