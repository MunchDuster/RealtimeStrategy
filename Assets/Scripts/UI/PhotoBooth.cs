using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Munchy.Units;
using Munchy.Extensions;
using UnityEngine.Rendering;

namespace Munchy.UI
{
    /// <summary>
    /// Draws the preview images for units
    /// </summary>
    public class PhotoBooth : MonoBehaviour
    {
        private static PhotoBooth instance;

        private struct QueuedData
        {
            public List<Unit> units;
            public Vector2Int size;
            public Action<Unit, Texture[]> setter;

            public QueuedData(List<Unit> units, Vector2Int size, Action<Unit, Texture[]> setter)
            {
                this.units = units;
                this.size = size;
                this.setter = setter;
            }
        }

        [SerializeField] private new Camera camera;
        [SerializeField] private Transform cameraPivot;
        [SerializeField] private int picturesPerItem = 8;
        [SerializeField][Range(0f, 2f)] private float heightMultiplier;
        [SerializeField][Range(0f, 2f)] private float zBoundsMultiplier;
        [SerializeField][Range(0f, 2f)] private float xyBoundsMultiplier;

        private List<QueuedData> queue = new();
        private GameObject prefabInstance;
        private bool isRunning;
        private RenderTexture render;
        private Texture[] textures;

        public static void AddToQueue(List<Unit> units, Vector2Int size, Action<Unit, Texture[]> setter)
        {
            instance.queue.Add(new(units, size, setter));

            if (!instance.isRunning)
                instance.TakePhotos().Forget();
        }

        private async UniTask TakePhotos()
        {
            isRunning = true;

            RenderTexture savedRenderTexture = RenderTexture.active; // Save the original active render texture

            // Main loop
            while (queue.Count > 0)
            {
                QueuedData data = queue[0];

                int height = data.size.x;
                int width = data.size.y;

                RefreshCameraSettings(width, height);

                foreach (Unit unit in data.units)
                {
                    SpawnUnit(unit);
                    SetupTextures(unit, width, height);

                    for (int i = 0; i < picturesPerItem; i++)
                    {
                        textures[i] = Render(textures[i], width, height);
                        UpdatePivotRotation(i);
                    }

                    data.setter(unit, textures);
                    DestroyImmediate(prefabInstance); // Destroy waits a frame so do this instead
                }

                queue.RemoveAt(0); //Finished queued task
            }

            RenderTexture.active = savedRenderTexture; // Replace the original active Render Texture.
            isRunning = false;
        }
        private void Awake()
        {
            instance = this;
        }

        private Renderer SpawnUnit(Unit unit)
        {
            prefabInstance = Instantiate(unit.gameObject);
            prefabInstance.SetLayerRecursively(camera.cullingMask);
            Destroy(prefabInstance.GetComponent<Unit>());

            Renderer renderer = prefabInstance.GetComponent<Renderer>();
            UpdateCameraPosition(prefabInstance.GetComponent<Collider>());

            return renderer;
        }

        private void SetupTextures(Unit unit, int width, int height)
        {
            textures = new Texture[picturesPerItem];
            for (int i = 0; i < textures.Length; i++)
                textures[i] = new Texture2D(width, height, TextureFormat.ARGB32, 1, true);
        }

        private void UpdatePivotRotation(int i)
        {
            cameraPivot.localRotation = Quaternion.Euler(0, i * 360f / picturesPerItem, 0);
        }

        private void UpdateCameraPosition(Collider target)
        {
            Vector3 position = Vector3.zero;

            Bounds bounds = target.bounds;

            position.y = bounds.extents.y;

            // Distance needed to fit object vertically
            float yDist = bounds.extents.y / Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad / 2f);
            // Distance needed to fit object horizontally
            float xDist = bounds.extents.x / Mathf.Tan(camera.fieldOfView * camera.aspect * Mathf.Deg2Rad / 2f);

            position.z = -(Mathf.Max(yDist, xDist) * xyBoundsMultiplier + bounds.extents.z * zBoundsMultiplier);

            camera.transform.position = position + heightMultiplier * bounds.extents.y * Vector3.up;
            camera.transform.LookAt(bounds.center);
        }

        private void RefreshCameraSettings(int width, int height)
        {
            render = new(width, height, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            RenderTexture.active = render;
            camera.targetTexture = render;
            camera.aspect = (float)width / height;
        }
        private Texture Render(Texture texture, int width, int height)
        {
            camera.Render();
            Graphics.CopyTexture(render, texture); // THE MIP COUNTS MUST MATCH OR THIS WON'T WORK

            return texture; 
        }
    }
}