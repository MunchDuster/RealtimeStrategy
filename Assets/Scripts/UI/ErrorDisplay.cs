using TMPro;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using PrimeTween;

namespace Munchy.UI
{
    /// <summary>
    /// Used to convey errors to the user
    /// A little popup at the top of the screen saying what went wrong
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class ErrorDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float waitPerWord = 0.2f;
        [SerializeField] private float maxWait = 5f;
        [SerializeField] private float minWait = 1f;

        [Header("Tween settings")]
        [SerializeField] private Vector2 showPosition;
        [SerializeField] private Vector2 hidePosition;
        [SerializeField] private float tweenTime = 0.3f;

        private CancellationTokenSource _source = new();
        private RectTransform _rect;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            Communication.Listen<ErrorMessage>(StartDisplayingError);
            gameObject.SetActive(false);
        }

        private void StartDisplayingError(ErrorMessage message)
        {
            _source.Cancel();
            _source.Dispose();
            _source = new();
            DisplayError(message.data, _source.Token).Forget();
        }

        private async UniTask DisplayError(string message, CancellationToken token)
        {
            text.text = message;
            gameObject.SetActive(true);
            await Tween.UIAnchoredPosition(_rect, showPosition, tweenTime).WithCancellation(token);

            float time = Mathf.Clamp(message.Split(' ').Length * waitPerWord, minWait, maxWait);
            await UniTask.WaitForSeconds(time,  cancellationToken: token);
            if (token.IsCancellationRequested) return; // Skip rest if cancelled

            await Tween.UIAnchoredPosition(_rect, hidePosition, tweenTime).WithCancellation(token);
            if (token.IsCancellationRequested) return; // Skip rest if cancelled

            text.text = string.Empty;
            gameObject.SetActive(false);
        }
    }
}