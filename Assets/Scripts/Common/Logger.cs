using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Common
{
    public class Logger : Singleton<Logger>
    {
        [SerializeField]
        private TextMeshProUGUI DebugAreaText;

        [SerializeField]
        private bool EnableDebug;

        [SerializeField]
        private int MaxLines = 15;

        private void Awake()
        {
            if (DebugAreaText == null)
            {
                DebugAreaText = GetComponent<TextMeshProUGUI>();
            }
            DebugAreaText.text = string.Empty;
        }

        private void OnEnable()
        {
            DebugAreaText.enabled = EnableDebug;
            enabled = EnableDebug;

            if (enabled)
            {
                DebugAreaText.text += $"<color=\"white\">{DateTime.Now:HH:mm:ss.fff} {GetType().Name} enabled</color>\n";
            }
        }

        public void LogInfo(string message)
        {
            ClearLines();

            DebugAreaText.text += $"<color=\"green\">{DateTime.Now:HH:mm:ss.fff} {message}</color>\n";
        }

        public void LogError(string message)
        {
            ClearLines();
            DebugAreaText.text += $"<color=\"red\">{DateTime.Now:HH:mm:ss.fff} {message}</color>\n";
        }

        public void LogWarning(string message)
        {
            ClearLines();
            DebugAreaText.text += $"<color=\"yellow\">{DateTime.Now:HH:mm:ss.fff} {message}</color>\n";
        }

        private void ClearLines()
        {
            if (DebugAreaText.text.Split('\n').Length >= MaxLines)
            {
                DebugAreaText.text = string.Empty;
            }
        }
    }
}