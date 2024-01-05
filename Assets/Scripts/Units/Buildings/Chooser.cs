using System.Collections.Generic;
using UnityEngine;

namespace Munchy.Units.Buildings
{
    public class Chooser : MonoBehaviour
    {
        public static Chooser Instance;

        public static Dictionary<System.Type, Building> buildingPrefabs = new();

        [SerializeField] private GameObject[] buildingPrefabsList;


        private void Awake()
        {
            foreach (var prefab in buildingPrefabsList)
            {
                Building building = prefab.GetComponent<Building>();
                buildingPrefabs.Add(building.GetType(), building);
            }

            Instance = this;
        }

        public T Get<T>() where T : Building
        {
            if (buildingPrefabs.TryGetValue(typeof(T), out Building building))
                return building as T;
            else
            {
                Debug.LogError($"TYPE NOT LISTED: {typeof(T)}");
                return null;
            }
        }
    }
}