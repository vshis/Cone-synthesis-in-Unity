using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class roadSurfaceChange : MonoBehaviour
{
    public GameObject roadSurface;
    public Material[] mats;
    Material newMat;
    float textureScale = 40f;

    int GetRandom(int count)
    {
        return Random.Range(0, count);
    }

    void Update()
    {
        newMat = mats[GetRandom(mats.Length)];
        roadSurface.GetComponent<MeshRenderer>().material = newMat;
        //textureScale = Random.Range(7f, 7f);
        roadSurface.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1f, textureScale);
    }
}

