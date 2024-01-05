using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Munchy.Units.Buildings;

namespace Munchy.UI
{
    /// <summary>
    /// For the build menu buidling buttons
    /// </summary>
    public class BuildingTab : PrefabTab
    {
        /// <summary>
        /// Called by the item button onclick
        /// </summary>
        public override void OnClick(GameObject clickedPrefab)
        {
            Placer.Instance.Prefab = clickedPrefab.GetComponent<Building>();
        }
    }
}