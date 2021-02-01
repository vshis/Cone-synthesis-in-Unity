using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camMovement : MonoBehaviour
{
    public GameObject snapshotsCamera;

    Vector3 GetRandomRotation()
    {
        Vector3 euler = transform.eulerAngles;
        euler.x = Random.Range(-10f, 20f);
        euler.y = 0f;
        euler.z = 0f;
        return (euler);
    }

    void Update()
    {
        snapshotsCamera.transform.position = new Vector3(0, Random.Range(5f, 10f), -20);
        snapshotsCamera.transform.eulerAngles = GetRandomRotation();
    }
}
