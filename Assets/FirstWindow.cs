using UnityEngine;

namespace SimpleWindowSystem
{
    public class FirstWindow : Window
    {
        [SerializeField] private Window secondWindowPrefab;

        public void OnClickOpen()
        {
            Open(secondWindowPrefab);
        }

        public void OnClickDummy(string message)
        {
            Debug.Log($"OnClickDummy: {message}");
        }
    }
}