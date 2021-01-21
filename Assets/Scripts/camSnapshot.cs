using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class camSnapshot : MonoBehaviour
{
    Camera snapCam;

    int index = 0;

    public int resWidth = 512;
    public int resHeight = 512;
    // Start is called before the first frame update
    void Start()
    {
        snapCam = GetComponent<Camera>();

    }

    public void CallTakeSnapshotAll(int iterationNum, string imageType)
    { 
        Texture2D snapshot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        snapCam.Render();
        RenderTexture.active = snapCam.targetTexture;
        snapshot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        byte[] bytes = snapshot.EncodeToPNG();
        string fileName = SnapshotNameIndex();
        index++;
        System.IO.File.WriteAllBytes(fileName, bytes);
        if (System.IO.File.Exists(fileName))
        {
            string newFileName = $"img_{iterationNum.ToString().PadLeft(5, '0')}";
            System.IO.File.Move(fileName, (string.Format("C:/apps/synthConesTest/captures/{0}/{1}.png", imageType, newFileName)));
        }
    }

    public void CallTakeSnapshotIndividual(int iterationNum, int objNum, string imageType)
    {
        Texture2D snapshot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        snapCam.Render();
        RenderTexture.active = snapCam.targetTexture;
        snapshot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        byte[] bytes = snapshot.EncodeToPNG();
        string fileName = SnapshotNameIndex();
        index++;
        System.IO.File.WriteAllBytes(fileName, bytes);
        if (System.IO.File.Exists(fileName))
        {
            string newFileName = $"img_{iterationNum.ToString().PadLeft(5, '0')}_obj_{objNum.ToString().PadLeft(3, '0')}";
            System.IO.File.Move(fileName, (string.Format("C:/apps/synthConesTest/captures/{0}/individual/{1}.png", imageType, newFileName)));
        }
    }

    string SnapshotNameIndex()
    {
        return string.Format("{0}/Snapshots/snap_{1}x{2}_{3}_{4}.png", Application.dataPath, resWidth, resHeight, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), index);
    }
}
