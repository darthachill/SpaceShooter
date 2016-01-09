using UnityEngine;
using System.Collections.Generic;


public class CirculatingDroidController : MonoBehaviour
{

    public float r;
    public float t;     // circuit travel time
    public GameObject droidPrefab;
    public List<GameObject> Droids;
    public int droidCount = 0; //for controll initiial droids in inspector
    public int maxDroids = 3;
    public float[] droidSpawnAngles;
    public Transform opponentTransform;    //nearest oponent
    public GameObject circulatingSphere;   //prefab for droid


    private SingleCirculatingDroidController singleCirculatingDroidController;


    void Start()
    {
        for (int i = 0; i < droidCount; i++)
            AddDroid();
    }
    

     void Update()
    {
        if (Droids.Count > 0)
        {
           opponentTransform = findClosestOponent();
        }
            
    }

    public void AddDroid()
    {
        circulatingSphere = Instantiate(droidPrefab);
        circulatingSphere.transform.SetParent(transform);   // set playership transform as parent transform
        singleCirculatingDroidController = circulatingSphere.GetComponent<SingleCirculatingDroidController>();
        circulatingSphere.transform.localPosition = new Vector3(r, 0f, 0f);
        circulatingSphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        singleCirculatingDroidController.travelingTime = t;
        singleCirculatingDroidController.playerTransform = transform;

        Droids.Add(circulatingSphere);
        Droids[Droids.Count - 1].transform.position = Droids[0].transform.position;
        Droids[Droids.Count - 1].transform.rotation = Droids[0].transform.rotation;
        Droids[Droids.Count - 1].transform.RotateAround(transform.position, Vector3.up, droidSpawnAngles[Droids.Count - 1]);
        
    }


    public void DestroyAllDroids()
    {
        foreach (GameObject droid in Droids)
        {
            circulatingSphere.GetComponent<SingleCirculatingDroidController>().isDroidAlive = false;
            Destroy(droid);
        }
        Droids.Clear();
    }


    Transform findClosestOponent()
    {
        Transform enemyTransform = null;
        float distance;
        float smallestDistance = 10000f;

        foreach (Transform child in GameMaster.instance.hierarchyGuard)
        {
            if (child.tag == "Enemy")
            {
                distance = (transform.position - child.position).magnitude;
                if (distance <= smallestDistance)
                {
                    enemyTransform = child;
                    smallestDistance = distance;
                }
            }
        }
        return enemyTransform;
    }
    
}
