using UnityEngine;

namespace SimpleWindowSystem
{
    public class SecondWindow : Window
    {
        public void OnClickClose()
        {
            Close();
        }

        public void OnClickDummy(string message)
        {
            Debug.Log($"OnClickDummy: {message}");
        }
    }
}