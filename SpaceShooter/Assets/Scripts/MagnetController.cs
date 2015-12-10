using UnityEngine;
using System.Collections;

public class MagnetController : MonoBehaviour {

    private int currentMagnets;
    private int maxMagnets;
    private float lastingSphereTime;
    public  GameObject spherePub;
    private GameObject sphere;
    public GameObject magnet;//
    private MagnetGUI[] magnetGUIController;
	// Use this for initialization
	void Start () {
        currentMagnets = 0;
        maxMagnets = 2;
        magnetGUIController = new MagnetGUI[maxMagnets];
        FindMagnetGUI();
        for (int i = 0; i < currentMagnets; i++) { 
           magnetGUIController[i].showImage();      // Display magnets on GUI after start a game   
   }
}
    
	
	// Update is called once per frame
	void Update () {
      
    }

    void FindMagnetGUI()
    {
        for (int i = 0; i < maxMagnets; i++)
            magnetGUIController[i] = GameObject.Find("Magnet" + i).GetComponent<MagnetGUI>();
    }

    public void UseMagnet()
    {
        if (currentMagnets > 0)
        {
            magnetGUIController[currentMagnets - 1].hideImage();
            currentMagnets--;
            // GameMaster.instance.playerHolder.
            GameObject magnetingSphere = new GameObject();
            magnetingSphere.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
            magnetingSphere.name = "MagnetingSphere";
            magnetingSphere.AddComponent<SphereCollider>();
            SphereCollider colliderReference = magnetingSphere.GetComponent<SphereCollider>();
              colliderReference.radius = 3F;
            magnetingSphere.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
            magnetingSphere.tag = "PickUp"; // To avoid triggering shots
            sphere = Instantiate(spherePub);
            if (!sphere) Debug.Log("Error with sphere reference");
            sphere.transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
            sphere.transform.position = magnetingSphere.transform.position;
            sphere.transform.localScale =new Vector3( 6f, 6f, 6f);
                  sphere.GetComponent<MeshRenderer>().enabled = true;
            lastingSphereTime = 5f;
            StartCoroutine(DecreaseSphereTime());
            Destroy(magnetingSphere, 5f);
      
        }

    }

    IEnumerator DecreaseSphereTime()
    {
        while (lastingSphereTime > 0)
        {

            lastingSphereTime -= Time.deltaTime;                             
            yield return null;                                                 
        }


        sphere.GetComponent<MeshRenderer>().enabled = false;                                 
    }
    public void AddMagnet()
    {
      
        if ( currentMagnets < maxMagnets)
        {
            
            currentMagnets++;
            magnetGUIController[currentMagnets - 1].showImage();

            //Debug.Log("currentMagnets++");
        }
       // pokazywanie bomb w GUI

    }
}
