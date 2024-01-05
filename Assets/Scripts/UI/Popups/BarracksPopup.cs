using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
using Munchy.Units;
using Munchy.Units.Entities;
using Munchy.Units.Buildings;
using Munchy.UI;

namespace Munchy.UI.Popups
{
    public class BarracksPopup : MonoBehaviour, Popup
    {
        [SerializeField] private string queueEmptyText = "Queue empty";
        [SerializeField] private string queueHeaderText = "Queue:";
        [SerializeField] private TextMeshProUGUI queueText;
        [SerializeField] private PrefabTab selectionTab;

        private Barracks _barracks;

        private void Awake()
        {
            selectionTab.onSelected += OnTabSelected;
        }

        public void Init(Unit building)
        {
            _barracks = building as Barracks;
        }

        private void OnTabSelected(GameObject prefab)
        {
            Entity entity = prefab.GetComponent<Entity>();

            if (!entity)
            {
                Debug.LogError($"{prefab.name} has no entity!");
                return;
            }

            _barracks.TryAddToQueue(entity);
        }

        public void RefreshQueueText(List<Unit> queue)
        {
            if (queue.Count == 0)
                queueText.text = queueEmptyText;
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(queueHeaderText);
                foreach (Entity entity in queue)
                    stringBuilder.Append(entity.displayName);

                queueText.text = stringBuilder.ToString();
            }
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}