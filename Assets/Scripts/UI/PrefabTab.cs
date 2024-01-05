using System;
using System.Linq;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Munchy.Units;

namespace Munchy.UI
{
    /// <summary>
    /// Generic tab handler, abstracts the selection process (although this version relies heavily on inspector setup
    /// Tab being a list of options that can be selected
    /// </summary>
    public class PrefabTab : MonoBehaviour
    {
        private Dictionary<Unit, PrefabTabItem> items = new();
        private List<Unit> units = new();

        public Action<GameObject> onSelected;

        protected virtual void Awake()
        {
            foreach (Transform child in transform)
            {
                PrefabTabItem item = child.GetComponent<PrefabTabItem>();
                item.onSelected += OnClick;
                Unit unit = item.prefab.GetComponent<Unit>();
                units.Add(unit);
                items.Add(unit, item);
            }
        }

        private void Start()
        {
            // Delay one frame for rect transform to resize
            UniTask.DelayFrame(1).ContinueWith(() =>
            {
                PhotoBooth.AddToQueue(units, items.Values.First().Size, OnPhotoTaken);
            });
        }

        private void OnPhotoTaken(Unit unit, Texture[] textures)
        {
            items[unit].SetTextures(textures);
        }

        /// <summary>
        /// Called by the item button onclick, supplied with its prefab
        /// </summary>
        public virtual void OnClick(GameObject prefab)
        {
            Debug.Log($"{prefab.name} clicked!");
            onSelected?.Invoke(prefab);
        }
    }
}