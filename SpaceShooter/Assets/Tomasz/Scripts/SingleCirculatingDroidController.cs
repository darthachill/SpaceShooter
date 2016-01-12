using UnityEngine;
using System.Collections;
using System;
public class SingleCirculatingDroidController : ObjectController
{
    [HideInInspector]
    public Transform playerTransform;
    [HideInInspector]
    public float travelingTime;

    private SlowWeapon slowWeapon;
    private MoveDroidLaser moveDroidLaser;
    private CirculatingDroidController circulatingDroidController;
    private Transform opponentTransform;
    private Transform weaponTransform; // where particle are spawned


    void Start()
    {

          slowWeapon =  visualWeapons[0].weapon.GetComponent<SlowWeapon>();
           moveDroidLaser =  slowWeapon.bullet.GetComponent<MoveDroidLaser>(); 
        
        circulatingDroidController = playerTransform.GetComponent<CirculatingDroidController>();
        weaponTransform = transform.GetChild(0);
        StartCoroutine(RotateDroid());
    }


    public IEnumerator RotateDroid()
    {
        slowWeapon.Shot(false);
        while (isAlive)
        {
           
          if (circulatingDroidController.opponentTransform && isShooting)
            {

                transform.LookAt(circulatingDroidController.opponentTransform);
                weaponTransform.localPosition =  Vector3.forward / 2;
              
          transform.LookAt(circulatingDroidController.opponentTransform);
         moveDroidLaser = visualWeapons[0].weapon.GetComponent<SlowWeapon>().bullet.GetComponent<MoveDroidLaser>();
       moveDroidLaser.target = circulatingDroidController.opponentTransform;
       visualWeapons[0].weapon.GetComponent<SlowWeapon>().bullet.GetComponent<MoveDroidLaser>().target = circulatingDroidController.opponentTransform; 
       visualWeapons[0].weapon.GetComponent<SlowWeapon>().Shot(false);
               // GetComponent<SlowWeapon>().Shot(false);
     
   }

            transform.RotateAround(playerTransform.position, playerTransform.up, 360 * Time.deltaTime / travelingTime);

            yield return null;
        }
    }


    public override void TakeDamage(float damage, Vector3 damagePosition)
    {
        throw new NotImplementedException();
    }


    protected override void CheckBoundry()
    {
        throw new NotImplementedException();
    }


    public override void IEConstandDamageByTimeAdditional()
    {
        throw new NotImplementedException();
    }
}
