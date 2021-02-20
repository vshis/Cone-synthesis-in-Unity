using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;

public class organisedSpawner : MonoBehaviour
{
    public camSnapshot snapCam;
    public GameObject[] spawnees;
    public GameObject[] props;
    public GameObject background;
    public GameObject roadSurface;
    public Transform spawnPoint;
    public GameObject verticalBackground;
    public int minObjects = 10;
    public int maxObjects = 50;
    public int minProps = 0;
    public int maxProps = 0;
    public int trainingImages = 0;
    public int validationImages = 0;
    public int testImages = 0;
    public Renderer rend;
    public Material whiteEmMat;
    public Material blackEmMat;
    public bool save = false;
    int randomInt;
    Vector3 scale;
    Material originalMaterial;
    int iterationNumber = 0;
    string trainString = "train";
    string valString = "valid";
    string testString = "test";
    static string capturesDir = "C:/apps/synthConesTest/captures";
    System.IO.DirectoryInfo di = new DirectoryInfo(capturesDir);  //this takes the Directory.info format of the directory where all files in the captures directory and its subdirectories will be deleted
    Stopwatch sw = Stopwatch.StartNew();

    void Start()
    {
        RendOffSingle(verticalBackground);
        if (save)
        {
            foreach (FileInfo imageFile in di.EnumerateFiles("*.*", SearchOption.AllDirectories))
            {
                imageFile.Delete();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (trainingImages > Time.frameCount - 1)
        {
            Resources.UnloadUnusedAssets();
            SpawnRandom(trainString);
        }
        else if (trainingImages + validationImages > Time.frameCount - 1)
        {
            Resources.UnloadUnusedAssets();
            SpawnRandom(valString);
        }
        else if (trainingImages + validationImages + testImages > Time.frameCount - 1)
        {
            Resources.UnloadUnusedAssets();
            SpawnRandom(testString);
        }
        else if (trainingImages + validationImages + testImages == Time.frameCount - 1)
        {
            print(string.Format("Synthesis completed. Synthesised {0} images in hh:mm:ss {1}", Time.frameCount - 1, sw.Elapsed));
        }
        else
        {
            Resources.UnloadUnusedAssets();
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    int GetRandom(int count)
    {
        return Random.Range(0, count);
    }

    Vector3 GetRandomSpawnPoint(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
    {
        float newX = Random.Range(minX, maxX);
        float newY = Random.Range(minY, maxY);
        float newZ = Random.Range(minZ, maxZ);
        return (new Vector3(newX, newY, newZ));
    }

    Vector3 ConesSpawnPoint(float minX, float maxX, float minY, float maxY, float minZ, float maxZ) //cones spawn on the road surface
    {
        float newX = Random.Range(minX, maxX);
        if (newX > -10f)
        {
            newX += 20f;
        }
        float newY = Random.Range(minY, maxY);
        float newZ = Random.Range(minZ, maxZ);
        return (new Vector3(newX, newY, newZ));
    }

    Vector3 PropsSpawnPoint(float minX, float maxX, float minY, float maxY, float minZ, float maxZ) //other props spawn outside the road surface
    {
        float newX = Random.Range(minX, maxX);
        if (newX > -18f)
        {
            newX += 36f;
        }
        float newY = Random.Range(minY, maxY);
        float newZ = Random.Range(minZ, maxZ);
        return (new Vector3(newX, newY, newZ));
    }

    Vector3 GetRandomScale(float minScale, float maxScale)
    {
        float sx = Random.Range(minScale, maxScale);
        return (new Vector3(sx, sx, sx));
    }

    Vector3 GetRandomRotation(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
    {
        Vector3 euler = transform.eulerAngles;
        euler.x = Random.Range(minX, maxX);
        euler.y = Random.Range(minY, maxY);
        euler.z = Random.Range(minZ, maxZ);
        return (euler);
    }

    List<GameObject> SpawnProps(int propsToSpawn)
    {
        List<GameObject> propsList = new List<GameObject>();
        while (propsToSpawn > 0)
        {
            propsToSpawn -= 1;
            //Check if valid spawn position
            bool validPosition = false;
            Vector3 newPosition = Vector3.zero;
            float checkRadius = 15f;
            int maxSpawnAttemptsPerObstacle = 10;
            int spawnAttempts = 0;
            while (!validPosition && spawnAttempts < maxSpawnAttemptsPerObstacle)
            {
                spawnAttempts++;
                //newPosition = GetRandomSpawnPoint(-40f, 26f, 0f, 0f, 0f, 70f);
                newPosition = PropsSpawnPoint(-40f, 4f, 0f, 0f, -10f, 90f);
                validPosition = true;
                if (Physics.CheckSphere(newPosition, checkRadius, 9))
                {
                    validPosition = false;
                }
            }

            if (validPosition && spawnAttempts < maxSpawnAttemptsPerObstacle)
            {
                randomInt = GetRandom(props.Length);
                GameObject newProp = (GameObject)Instantiate(props[randomInt], newPosition, spawnPoint.rotation);
                newProp.transform.localScale = GetRandomScale(0.5f, 1.5f);
                Vector3 newRot = GetRandomRotation(0f, 0f, -180f, 180f, 0f, 0f);
                newProp.transform.eulerAngles = newRot;
                propsList.Add(newProp);
            }
        }
        return propsList;
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
            float checkRadius = 8f;
            int maxSpawnAttemptsPerObstacle = 10;
            int spawnAttempts = 0;
            while (!validPosition && spawnAttempts < maxSpawnAttemptsPerObstacle)
            {
                spawnAttempts++;
                //newPosition = GetRandomSpawnPoint(-40f, 40f, 0f, 0f, 0f, 50f);
                newPosition = ConesSpawnPoint(-14f, -6f, 0f, 0f, -10f, 60f);
                validPosition = true;
                if (Physics.CheckSphere(newPosition, checkRadius, 9))
                {
                    validPosition = false;
                }
            }

            if (validPosition && spawnAttempts < maxSpawnAttemptsPerObstacle)
            {
                randomInt = GetRandom(spawnees.Length);
                GameObject newGO = (GameObject)Instantiate(spawnees[randomInt], newPosition, spawnPoint.rotation);
                newGO.transform.localScale = GetRandomScale(4f, 4f);
                Vector3 newRot = GetRandomRotation(0f, 0f, -180f, 180f, 0f, 0f);
                newGO.transform.eulerAngles = newRot;
                objectsThisTime.Add(newGO);
            }
        }
        List<GameObject> propsThisTime = SpawnProps(Random.Range(minProps, maxProps));
        print(string.Format("Total of {0} cones and {1} props spawned.", objectsThisTime.Count, propsThisTime.Count));

        //Taking pictures
        if (save)
        {
            snapCam.CallTakeSnapshotAll(iterationNumber, imageType, capturesDir);     //Picture of the whole setting
        }
        materialBlack(objectsThisTime);
        materialBlack(propsThisTime);
        materialBlackSingle(background);
        materialBlackSingle(roadSurface);
        RendOnSingle(verticalBackground);
        //individual cones pics
        int iterationNumberPerObjectInImg = 0;
        foreach (GameObject GO in objectsThisTime)
        {
            originalMaterial = GO.GetComponent<MeshRenderer>().material;    //find the original material
            materialWhiteSingle(GO);
            if (save)
            {
                snapCam.CallTakeSnapshotIndividual(GO, iterationNumber, iterationNumberPerObjectInImg, imageType, capturesDir);
            }
            GO.GetComponent<MeshRenderer>().material = originalMaterial;    //return the material to the original material           
            iterationNumberPerObjectInImg++;
        }
        destroyListOfGO(objectsThisTime);
        destroyListOfGO(propsThisTime);
        RendOffSingle(verticalBackground);
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

    void RendOffSingle(GameObject obj)
    {
        rend = obj.GetComponent<Renderer>();
        rend.enabled = false;
    }

    void RendOnSingle(GameObject obj)
    {
        rend = obj.GetComponent<Renderer>();
        rend.enabled = true;
    }

    void materialBlackSingle(GameObject obj)
    {
        Material[] mats = obj.GetComponent<MeshRenderer>().materials;
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = blackEmMat;
        }
        obj.GetComponent<MeshRenderer>().materials = mats;
    }

    void materialWhiteSingle(GameObject obj)
    {
        Material[] mats = obj.GetComponent<MeshRenderer>().materials;
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = whiteEmMat;
        }
        obj.GetComponent<MeshRenderer>().materials = mats;
    }

    void materialBlack(List<GameObject> objs)
    {
        foreach (var GO in objs)
        {
            Material[] mats = GO.GetComponent<MeshRenderer>().materials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = blackEmMat;
            }
            GO.GetComponent<MeshRenderer>().materials = mats;
        }
    }

    void materialWhite(List<GameObject> objs)
    {
        foreach (var GO in objs)
        {
            Material[] mats = GO.GetComponent<MeshRenderer>().materials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = whiteEmMat;
            }
            GO.GetComponent<MeshRenderer>().materials = mats;
        }
    }

    void destroyListOfGO(List<GameObject> objs)
    {
        foreach (var GO in objs)
        {
            Destroy(GO);
        }
    }
}
