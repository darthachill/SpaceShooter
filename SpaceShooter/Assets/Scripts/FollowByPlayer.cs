using UnityEngine;
using System.Collections;

public class FollowByPlayer : MonoBehaviour
{
    public float rotateSpeed = 500;

    private Transform player;


    void Start()
    {
       // player = GameObject.FindGameObjectWithTag("Player").transform;
      
        if (GameObject.Find("Eagle(Clone)"))
        {
            player = GameObject.Find("Eagle(Clone)").transform;
            //  magnetingSphere.gameObject.transform.SetParent(GameObject.Find("Eagle(Clone)").transform);
        }
        else if (GameObject.Find("Shadow(Clone)"))
        {
            player = GameObject.Find("Shadow(Clone)").transform;
            //  magnetingSphere.gameObject.transform.SetParent(GameObject.Find("Shadow(Clone)").transform);

        }
        //edited by Tomasz 2015:12:14 18:45 (to many "players" on scene)
    }


    void Update()
    {
        if (GameMaster.instance.CheckIfPlayerIsAlive())                                                                                                                  // if player is still alive
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position), rotateSpeed * Time.deltaTime);      // fallow player
    }
}   // Karol Sobanski
