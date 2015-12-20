using UnityEngine;
using System.Collections;
using System;

public class CirculatingDroidController :ObjectController {

    public float r;
    public float t;
    public GameObject droidPrefab;


    private GameObject circulatingSphere;
    private float x;
    private float z;
    private Transform opponentTransform;

    void Start () {
        circulatingSphere = Instantiate(droidPrefab);
        circulatingSphere.transform.SetParent(transform);   // set playership transform as parent transform
        circulatingSphere.transform.localPosition = new Vector3(0f, 0f, 0f);
        x = r;
        z = 0f;
        circulatingSphere.transform.Translate(r, 0f, 0f);
        circulatingSphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
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
               distance = (circulatingSphere.transform.position - child.position).magnitude;
               if(distance <= smallestDistance)
                {
                    enemyTransform = child;
                    smallestDistance = distance;
                }
            }
        }

        return enemyTransform;
    }


    void Update () {
       
        circulatingSphere.transform.RotateAround(transform.position, Vector3.up, 360 * Time.deltaTime / t);

        if (Input.GetKeyDown(KeyCode.C))  // TEST
        {
            opponentTransform = findClosestOponent();
            Debug.Log(opponentTransform.name);
         }

    }


    public override void TakeDamage(int damage, Vector3 damagePosition)
    {
        throw new NotImplementedException();
    }

    protected override void CheckBoundry()
    {
        throw new NotImplementedException();
    }
}
