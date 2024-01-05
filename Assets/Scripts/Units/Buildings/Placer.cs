using System;
using UnityEngine;
using Photon.Pun;
using Munchy.Extensions;


namespace Munchy.Units.Buildings
{
    public class Placer : MonoBehaviour
    {
        public static Placer Instance { get; private set; }

        private static Quaternion rotation = Quaternion.Euler(-90, 0, 0); // because blender model.

        [Header("Main Settings")]
        [SerializeField] private LayerMask dontBuildOnLayerMask;
        [SerializeField] private LayerMask placingLayer;
        [SerializeField] private bool toggleGrid = true;
        [SerializeField] private float gridSize = 1;
        [SerializeField] private float lerpSpeed = 10;
        [SerializeField] [Range(0.1f, 1f)] private float snappiness = 0.5f;

        [Space(10)]
        [Header("Ray cast settings")]
        [SerializeField] private float maxDist;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private KeyCode multiPlaceKey;
        [SerializeField] private Material placingMaterialOk;
        [SerializeField] private Material placingMaterialBad;

        /// <summary>
        /// Will automatically exit placing mode when this is set to null
        /// Expects item to have a Building attached
        /// </summary>
        public Building Prefab
        {
            set
            {
                if (_demoInstance) Destroy(_demoInstance.gameObject);

                if (!value) // Resetting
                {
                    _prefab = null;
                    return;
                }

                if (!value.CanSpawn()) // Cant place building
                {
                    _prefab = null;
                    return;
                }

                _prefab = value;

                _demoInstance = Instantiate(value.gameObject, Vector3.zero, rotation).GetComponent<Building>();
                _demoInstance.gameObject.SetLayerRecursively(placingLayer);
                _demoInstance.enabled = false;
            }
        }

        private Building _demoInstance;
        private Building _prefab;
        private Vector3 point;

        private void Awake()
        {
            Instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            if (!_prefab) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDist, layerMask))
            {
                if(toggleGrid)
                    hit.point = PutOnGrid(hit.point);

                point = Vector3.Lerp(point, hit.point, lerpSpeed * Time.deltaTime / Mathf.Max(snappiness, Vector3.Distance(point, hit.point)));

                if (Input.GetMouseButtonDown(0))
                    SpawnItem(hit.point);
                else
                {
                    _demoInstance.transform.position = point;

                    Material mat = dontBuildOnLayerMask.Contains(hit.collider.gameObject.layer) ? placingMaterialBad : placingMaterialOk;
                    _demoInstance.SetMaterial(mat);
                }
            }
        }

        void SpawnItem(Vector3 point)
        {
            PhotonNetwork.Instantiate(_prefab.gameObject.name, point, rotation);
            Prefab = Input.GetKey(multiPlaceKey) ? _prefab : null; //Force refresh
        }

        private Vector3 PutOnGrid(Vector3 point)
        {
            return new Vector3(
                Mathf.Round(point.x * gridSize) / gridSize,
                Mathf.Round(point.y * gridSize) / gridSize,
                Mathf.Round(point.z * gridSize) / gridSize
                );
        }
    }
}