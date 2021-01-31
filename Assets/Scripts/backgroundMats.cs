using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundMats : MonoBehaviour
{
    public GameObject background;
    public Material[] mats;


    int GetRandom(int count)
    {
        return Random.Range(0, count);
    }

    
    void Update()
    {
        background.GetComponent<MeshRenderer>().material = mats[GetRandom(mats.Length)];
    }
}
