using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class backgroundMats : MonoBehaviour
{
    public GameObject background;
    public Material[] mats;
    public Material[] skyboxes;
    Material newMat;
    float textureScale = 7f;

    int GetRandom(int count)
    {
        return Random.Range(0, count);
    }
    
    void Update()
    {
        RenderSettings.skybox = skyboxes[GetRandom(skyboxes.Length)];
        newMat = mats[GetRandom(mats.Length)];
        background.GetComponent<MeshRenderer>().material = newMat;
        //textureScale = Random.Range(7f, 7f);
        background.GetComponent<Renderer>().material.mainTextureScale = new Vector2(textureScale, textureScale);
    }
}
