using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class randomPosRotSpawner : MonoBehaviour
{
    public camSnapshot snapCam;
    public GameObject[] spawnees;
    public GameObject background;
    public Transform spawnPoint;
    public int minObjects = 10;
    public int maxObjects = 50;
    public int trainingImages = 0;
    public int validationImages = 0;
    public Renderer rend;
    public Material whiteEmMat;
    public bool save = false;
    int randomInt;
    Vector3 scale;
    Material originalMaterial;
    int iterationNumber = 0;
    string trainString = "train";
    string valString = "val";

    // Update is called once per frame
    void Update()
    {
        if (trainingImages > Time.frameCount-1)
        {
            SpawnRandom(trainString);
        }
        else if (trainingImages + validationImages > Time.frameCount-1)
        {
            SpawnRandom(valString);
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    SpawnRandom();
        //}
    }

    int GetRandom(int count)
    {
        return Random.Range(0, count);
    }

    Vector3 GetRandomSpawnPoint()
    {
        float newX = Random.Range(-5.5f, 5.5f);
        float newY = Random.Range(0f, 10f);
        float newZ = Random.Range(-5.5f, 5.5f);
        return (new Vector3(newX, newY, newZ));
    }

    Vector3 GetRandomScale()
    {
        float sx = Random.Range(2f, 4f);
        return (new Vector3(sx, sx, sx));
    }

    Vector3 GetRandomRotation()
    {
        Vector3 euler = transform.eulerAngles;
        euler.x = Random.Range(-40f, 40f);
        euler.y = Random.Range(0f, 0f);
        euler.z = Random.Range(-40f, 40f);
        return (euler);
    }

    void SpawnRandom(string imageType)
    {
        int objectsToSpawn = Random.Range(minObjects, maxObjects);
        List<GameObject> objectsThisTime = new List<GameObject>();
        while (objectsToSpawn > 0)
        {
            objectsToSpawn -= 1;
            //Check if valid spawn position
            bool validPosition = false;
            Vector3 newPosition = Vector3.zero;
            float checkRadius = 2.5f;
            int maxSpawnAttemptsPerObstacle = 10;
            int spawnAttempts = 0;
            while (!validPosition && spawnAttempts < maxSpawnAttemptsPerObstacle)
            {
                spawnAttempts++;
                newPosition = GetRandomSpawnPoint();
                validPosition = true;
                if (Physics.CheckSphere(newPosition, checkRadius))
                {
                    validPosition = false;
                }
            }

            if (validPosition && spawnAttempts < maxSpawnAttemptsPerObstacle)
            {
                randomInt = GetRandom(spawnees.Length);
                GameObject newGO = (GameObject)Instantiate(spawnees[randomInt], newPosition, spawnPoint.rotation);
                newGO.transform.localScale = GetRandomScale();
                Vector3 newRot = GetRandomRotation();
                newGO.transform.eulerAngles = newRot;
                objectsThisTime.Add(newGO);
            }
        }
        print(objectsThisTime.Count);
        
        //Taking pictures
        if (save)
        {
            snapCam.CallTakeSnapshotAll(iterationNumber, imageType);     //Picture of the whole setting
        }
        RendOff(objectsThisTime);
        background.layer = LayerMask.NameToLayer("LightIgnored");
        //individual cones pics
        int iterationNumberPerObjectInImg = 0;
        foreach (GameObject GO in objectsThisTime)
        {
            rend = GO.GetComponent<Renderer>();
            rend.enabled = true;
            originalMaterial = GO.GetComponent<MeshRenderer>().material;    //find the original material
            GO.GetComponent<MeshRenderer>().material = whiteEmMat;  //change material to plain white
            if (save)
            {
                snapCam.CallTakeSnapshotIndividual(GO, iterationNumber, iterationNumberPerObjectInImg, imageType);
            }
            GO.GetComponent<MeshRenderer>().material = originalMaterial;    //return the material to the original material
            rend.enabled = false;
            Destroy(GO);
            iterationNumberPerObjectInImg++;
        }
        RendOn(objectsThisTime);
        background.layer = LayerMask.NameToLayer("Default");
        iterationNumber++;
    }

    void RendOff(List<GameObject> objs)
    {
        foreach (var GO in objs)
        {
            rend = GO.GetComponent<Renderer>();
            rend.enabled = false;
        }
    }

    void RendOn(List<GameObject> objs)
    {
        foreach (var GO in objs)
        {
            rend = GO.GetComponent<Renderer>();
            rend.enabled = true;
        }
    }
}
