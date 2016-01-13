using UnityEngine;
using System.Collections;
using System;
public class SingleCirculatingDroidController : ObjectController
{
    [HideInInspector]
    public Transform playerTransform;    //used to rotate droid around

    [HideInInspector]
    public float travelingTime;         //time of making full circle travel


    private CirculatingDroidController circulatingDroidController;  //used to have updated Transform of nearest opponent


    void Start()
    {
        base.Start();

        circulatingDroidController = playerTransform.GetComponent<CirculatingDroidController>();
        StartCoroutine(RotateDroid());
    }


    public IEnumerator RotateDroid()        //rotate single droid and shot to opponents
    {
        while (isAlive)
        {

            if (circulatingDroidController.opponentTransform && isShooting)     // if there is opponent in hierarhyGuard
            {
                transform.LookAt(circulatingDroidController.opponentTransform);     // have opponent in front of droid
                Shot();
            }

              transform.RotateAround(playerTransform.position, playerTransform.up, 360 * Time.deltaTime / travelingTime); //rotate around player

            yield return null;
        }
    }



    protected virtual void Shot()
    {
        for (int i = 0; i < visualWeapons.Count; i++)             // Shot from all weapons
            visualWeapons[i].weaponScripts.Shot(false);
    }


    public override void TakeDamage(float damage, Vector3 damagePosition)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            Death();
    }


    protected void Death()
    {
        Destroy(gameObject);
    }

    protected override void CheckBoundry()      //droid has to stick near player
    {
        throw new NotImplementedException();
    }


    public override void IEConstandDamageByTimeAdditional()
    {
        throw new NotImplementedException();
    }
}
