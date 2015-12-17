using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MagnetGUI : MonoBehaviour {

    private Image magnetImage;
    

    void Awake()
    {        
        magnetImage = GetComponent<Image>();
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
