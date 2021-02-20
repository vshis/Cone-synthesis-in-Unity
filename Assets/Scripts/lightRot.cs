using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightRot : MonoBehaviour
{
    public GameObject target;
    public Light diriectionalLight;

    void Start()
    {
        diriectionalLight = GetComponent<Light>();
    }

    void Update()
    {
        diriectionalLight.intensity = Random.Range(0.5f, 2f);
        transform.RotateAround(target.transform.position, Vector3.up, Random.Range(0, 360f));
    }
}
