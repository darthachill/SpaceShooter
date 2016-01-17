using UnityEngine;
using System.Collections;

public class DestroyByBoundry : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("CirculatingDroid(Clone)"))  // to eliminate problem with destroyed droids near boundry
            return;

        GameMaster.instance.RemoveObject(other.transform);
		Destroy(other.gameObject); 
    }
}   // Karol Sobanski
