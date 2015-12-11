using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MagnetGUI : MonoBehaviour {


    private Image magnetImage;
   // private const string bombUsed = "magnetUsed";
    //private const string bombCollected = "magnetCollected";
    // Use this for initialization
    void Start () {
	
	}
    void Awake()
    {
        
        magnetImage = GetComponent<Image>();
      
    }
	
	// Update is called once per frame
	void Update () {
	
	}
   public void showImage()
    {
        
        magnetImage.enabled = true;
    }
  public  void hideImage()
    {
        magnetImage.enabled = false;
    }

}
