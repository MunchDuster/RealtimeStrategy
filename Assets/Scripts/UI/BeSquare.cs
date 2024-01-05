using UnityEngine;

namespace Munchy.UI
{
    /// <summary>
    /// Teeny weeny script to make UI a square,
    /// Use in editor only
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class BeSquare : MonoBehaviour
    {
        [Tooltip("If true: Will make width same as height\nElse: Will make height same as width.")]
        [SerializeField] private bool matchHeight = true;
        public void MakeSquare()
        {
            var rect = GetComponent<RectTransform>();

            rect.sizeDelta = matchHeight ?
                new Vector2(rect.sizeDelta.y, rect.sizeDelta.y) :
                new Vector2(rect.sizeDelta.x, rect.sizeDelta.x);
        }

        private void Awake()
        {
            Destroy(this); //Destroy this script when playing, NOT THE GAMEOBJECT
        }
    }
}