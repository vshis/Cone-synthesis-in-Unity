using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class takeScreenshotCamera : MonoBehaviour
{
    Camera ssCam;
    public int resWidth = 512;
    public int resHeight = 512;
    static string ssDirPath = "C:/apps/synthConesTest/screenshots";
    public bool save = false; 

    void Start()
    {
        ssCam = GetComponent<Camera>();
    }

    private void OnPostRender()
    {
        if (save)
        {
            Texture2D screenshot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            RenderTexture.active = ssCam.targetTexture;
            screenshot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0, false);
            screenshot.Apply();
            byte[] bytes = screenshot.EncodeToPNG();
            string fileName = string.Format("{0}/Screenshot_{1}.png", ssDirPath, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
            System.IO.File.WriteAllBytes(fileName, bytes);
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}