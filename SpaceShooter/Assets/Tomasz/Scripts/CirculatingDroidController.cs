using UnityEngine;
using System.Collections.Generic;


public class CirculatingDroidController : MonoBehaviour
{

    public float r;                     // radius of droid rotation circle
    public float t;                     // circuit travel time
    public GameObject droidPrefab;      //prefab for droid
    public List<GameObject> Droids;     //list of active droids
    public int droidCount = 0;          //for controll initiial droisd count in inspector
    public int maxDroids = 3;
    public float[] droidSpawnAngles;   //for having droids in good distance of each other

    [HideInInspector]
    public Transform opponentTransform;    //nearest oponent of the player


    private GameObject circulatingSphere;   //temporary game object that is added to droid list
    private SingleCirculatingDroidController singleCirculatingDroidController;  //reference used to set fields in single droid at start


    void Start()
    {
        for (int i = 0; i < droidCount; i++)
            AddDroid();
    }


    void Update()
    {
        if (Droids.Count > 0)
        {
            opponentTransform = findClosestOponent();        //finding nearest opponent of the player
        }
    }


    public void AddDroid()
    {
        circulatingSphere = Instantiate(droidPrefab);
        circulatingSphere.transform.SetParent(transform);   // set playership transform as parent transform
        singleCirculatingDroidController = circulatingSphere.GetComponent<SingleCirculatingDroidController>();

        circulatingSphere.transform.localPosition = new Vector3(r, 0f, 0f);     //set init position of new droid
        circulatingSphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        singleCirculatingDroidController.travelingTime = t;
        singleCirculatingDroidController.playerTransform = transform;       //set player transform reference in single droid
        Droids.Add(circulatingSphere);

        Droids[Droids.Count - 1].transform.position = Droids[0].transform.position;
        Droids[Droids.Count - 1].transform.rotation = Droids[0].transform.rotation;
        Droids[Droids.Count - 1].transform.RotateAround(transform.position, Vector3.up, droidSpawnAngles[Droids.Count - 1]);

    }


    public void DestroyAllDroids()          //method used in playerController when player die
    {
        foreach (GameObject droid in Droids)
        {
            Destroy(droid);                   //destroy all droids
        }
        Droids.Clear();                     //make droid list clear
    }


    private Transform findClosestOponent()
    {
        Transform enemyTransform = null;
        float distance;
        float smallestDistance = 10000f;

        foreach (Transform child in GameMaster.instance.hierarchyGuard)
        {
            if (child.tag == "Enemy")                                            // looking gameObject tagged Enemy
            {
                distance = (transform.position - child.position).magnitude;     //looking for smallest distance 
                if (distance <= smallestDistance)                               // to tagged "enemy" object in hierarhy guard
                {
                    enemyTransform = child;
                    smallestDistance = distance;
                }
            }
        }
        return enemyTransform;
    }

}
