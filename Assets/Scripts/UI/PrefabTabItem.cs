using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Munchy.Units;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Munchy.UI
{
    public class PrefabTabItem : MonoBehaviour
    {
        private const string CostSprite = "<sprite name=\"coin\">";
        private const string FoodSprite = "<sprite name=\"food\">";

        public Action<GameObject> onSelected;

        [Header("Prefab Tab Item")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI foodText;
        [SerializeField] private RawImage image;
        [SerializeField] private float secondsPerSpin = 3f;

        [Header("Outer Refs")]
        public GameObject prefab;

        private RectTransform rect;
        private Texture[] textures;
        private int index;

        public Vector2Int Size => new Vector2Int((int)rect.rect.width, (int)rect.rect.height);

        private void Awake()
        {
            rect = image.GetComponent<RectTransform>();

            //Load data from prefab
            Unit unit = prefab.GetComponent<Unit>();
            nameText.text = unit.displayName;
            costText.text = $"{CostSprite}{unit.cost}";
            foodText.text = $"{FoodSprite}{unit.foodCost}";
        }

        public void SetTextures(Texture[] textures)
        {
            this.textures = textures;
            CycleTextures(destroyCancellationToken).Forget();
        }
        

        private async UniTask CycleTextures(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                image.texture = textures[index++];
                index %= textures.Length;
                await UniTask.WaitForSeconds(secondsPerSpin / textures.Length, cancellationToken: token);
            }
        }

        public void OnClicked()
        {
            Debug.Log($"{prefab.name} clicked (ITEM)!");
            onSelected?.Invoke(prefab);
        }
    }
}