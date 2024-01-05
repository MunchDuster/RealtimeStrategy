using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class WebCam : MonoBehaviour
{
    [SerializeField] private RawImage otherImage;
    [SerializeField] private RenderTexture renderTexture;
    WebCamTexture webCamTexture;

    // Start is called before the first frame update
    void Start()
    {
        webCamTexture = new WebCamTexture();
        RawImage image = GetComponent<RawImage>();
        image.texture = webCamTexture;
        webCamTexture.Play();
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Texture2D newTexture = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.ARGB32, false );
            Graphics.CopyTexture(webCamTexture, newTexture);
            newTexture.Apply();
            otherImage.texture = newTexture;

            File.WriteAllBytes("C:\\Users\\Malachai\\Desktop\\BINGUS.png", newTexture.EncodeToPNG());
        }
    }
}
