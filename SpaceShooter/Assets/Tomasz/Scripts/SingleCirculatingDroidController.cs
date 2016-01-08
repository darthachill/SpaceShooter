using UnityEngine;
using System.Collections;
using System;
public class SingleCirculatingDroidController : ObjectController
{

    public Transform playerTransform;
    public float travelingTime;
    public bool isDroidAlive;

    private SlowWeapon slowWeapon;
    private MoveDroidLaser moveDroidLaser;
    private CirculatingDroidController circulatingDroidController;
    private Transform opponentTransform;


    void Start()
    {
        isDroidAlive = true;
        slowWeapon = GetComponent<SlowWeapon>();
        moveDroidLaser = slowWeapon.bullet.GetComponent<MoveDroidLaser>();
        circulatingDroidController = playerTransform.GetComponent<CirculatingDroidController>();
        opponentTransform = circulatingDroidController.opponentTransform;
        StartCoroutine(RotateDroid());
    }


   public IEnumerator RotateDroid()
    {
        while (isDroidAlive) {

           
            if (circulatingDroidController.opponentTransform && isShooting)
            {
                transform.LookAt(circulatingDroidController.opponentTransform);
                moveDroidLaser. target = circulatingDroidController.opponentTransform;
                slowWeapon.Shot(false);
            }

            transform.RotateAround(playerTransform.position, transform.up, 360 * Time.deltaTime / travelingTime);

            yield return null;
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
