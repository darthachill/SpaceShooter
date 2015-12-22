using UnityEngine;
using System.Collections;

public class MagnetController : MonoBehaviour {

    public GameObject spherePub;
    public GameObject magnet;
    public float radiusMultipler;
    public float lastingSphereTime; 

    private int currentMagnets;
    private int maxMagnets;
    private GameObject sphere;
    private GameObject magnetingSphere;
    private float startingDistance;
    private Transform shipTransform;
    private MagnetGUI[] magnetGUIController;
	
    
    // Use this for initialization
	void Start () {
        radiusMultipler = 1;
        currentMagnets = 0;
        maxMagnets = 2;
        magnetGUIController = new MagnetGUI[maxMagnets];
        FindMagnetGUI();
        shipTransform = transform;

        for (int i = 0; i < currentMagnets; i++) { 
               magnetGUIController[i].showImage();      // Display magnets on GUI after start a game   
            }
    }
  
      
   public float getStartingdDistance()
     {
       return startingDistance;
     }
     	

    void FindMagnetGUI()
    {
        for (int i = 0; i < maxMagnets; i++)
            magnetGUIController[i] = GameObject.Find("Magnet" + i).GetComponent<MagnetGUI>();
    }


    public void setStartingDistance(float distance)
    {
        startingDistance = distance;
    }


    public void UseMagnet()
    {
         if (currentMagnets > 0)
        {
            magnetGUIController[currentMagnets - 1].hideImage();
            currentMagnets--;
            magnetingSphere = new GameObject();

             
            magnetingSphere.gameObject.transform.SetParent(shipTransform);
            magnetingSphere.name = "MagnetingSphere";
            magnetingSphere.AddComponent<SphereCollider>();
            SphereCollider colliderReference = magnetingSphere.GetComponent<SphereCollider>();
            colliderReference.radius = 3F * radiusMultipler;
            magnetingSphere.transform.position = shipTransform.position;
            magnetingSphere.tag = "PickUp"; // To avoid triggering shots
            sphere = Instantiate(spherePub);
            sphere.name = "MagnetingSphereClone";

            if (!sphere) Debug.Log("Error with sphere reference");
            sphere.transform.SetParent(shipTransform);
            sphere.transform.position = magnetingSphere.transform.position;
            sphere.transform.localScale =new Vector3( 6f * radiusMultipler, 6f * radiusMultipler, 6f * radiusMultipler);
            sphere.GetComponent<MeshRenderer>().enabled = true;

         
            StartCoroutine(DecreaseSphereTime());            
        }

    }


    IEnumerator DecreaseSphereTime()
    {
        float lastingTime = lastingSphereTime;
        while (lastingTime > 0)
        {

            lastingTime -= Time.deltaTime;                             
            yield return null;                                                 
        }

        sphere.GetComponent<MeshRenderer>().enabled = false;
        Destroy(sphere);
        Destroy(magnetingSphere);
    }


    public void AddMagnet()
    {

        if ( currentMagnets < maxMagnets)
        {
            currentMagnets++;
            magnetGUIController[currentMagnets - 1].showImage();
        }
       
    }


}
