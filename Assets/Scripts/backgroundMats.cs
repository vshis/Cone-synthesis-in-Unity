using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundMats : MonoBehaviour
{
    public GameObject background;
    public Material[] mats;
    Material newMat;
    float textureScale = 50f;

    int GetRandom(int count)
    {
        return Random.Range(0, count);
    }
    
    void Update()
    {
        newMat = mats[GetRandom(mats.Length)];
        background.GetComponent<MeshRenderer>().material = newMat;
        textureScale = Random.Range(20f, 50f);
        background.GetComponent<Renderer>().material.mainTextureScale = new Vector2(textureScale, textureScale);
    }
}
