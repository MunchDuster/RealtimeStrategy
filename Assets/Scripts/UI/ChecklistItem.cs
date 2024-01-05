using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Munchy.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]

    public class ChecklistItem : MonoBehaviour
    {
        public UnityEvent onComplete;
        private TextMeshProUGUI text;

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        public void SetCompleted()
        {
            text.text = $"<s>{text.text}</s>";
            onComplete?.Invoke();
        }
    }
}