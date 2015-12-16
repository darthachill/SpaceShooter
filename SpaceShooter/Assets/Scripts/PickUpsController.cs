using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AudioSource), typeof(Animator))]
public class PickUpsController : MonoBehaviour
{
    public int value;

    public bool recoveryPackage;
    public bool fuel;
    public bool ammo;
    public bool bomb;
    public bool shield;
    public bool star;
    public bool magnet;
    public bool silverCoin;
    public bool bulletTime;
    public GameObject gun;


    private AudioSource audioSource;
    private Animator animator;
    private const string collected = "collected";
    private bool isCollected;                               // its prevent to collect item more times


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.name.Equals("MagnetingSphere") && silverCoin)
        {

            //  Vector3 distanceVector = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
            Vector3 distanceVector = other.GetComponentInParent<Transform>().position - transform.position;
            float startingDistance = other.GetComponentInParent<MagnetController>().getStartingdDistance();
            GetComponent<Rigidbody>().velocity = distanceVector.normalized * startingDistance * startingDistance / distanceVector.magnitude * 40 * Time.deltaTime;


        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("MagnetingSphere") && silverCoin)
        {


            //this.GetComponent<Rigidbody>().velocity = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position) * 40* Time.deltaTime;
            this.GetComponent<Rigidbody>().velocity = (other.GetComponentInParent<Transform>().position - transform.position) * 40 * Time.deltaTime;
            // float distance = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).magnitude;
            float distance = (other.GetComponentInParent<Transform>().position - transform.position).magnitude;
            other.GetComponentInParent<MagnetController>().setStartingDistance(distance);
        }

        if (other.tag.Equals("Player") && !isCollected)
        {
            isCollected = true;

            PlayerController playerController = other.GetComponent<PlayerController>();          // Get reference to player controller

            if (recoveryPackage)
                playerController.IncreaseHealth(value);
            else if (fuel)
                playerController.Refuel(value);
            else if (ammo)
                playerController.AddAmmo(value);
            else if (bomb)
                other.GetComponent<BombController>().AddBomb(value);
            else if (shield)
                other.GetComponent<ShieldController>().ActiveShield(value);                      // get refenence to shield Controller and active player shield
            else if (star)
                GameMaster.instance.AddStars(value);                                             // increase player score and add star
            else if (bulletTime)
                GameMaster.instance.BulletTimeOn();                                              // change timeScale to achive bullettime effect
            else if (gun)                                                                        // if there is any gun attached
            {
                playerController.ChangeWeapons(ref gun);                                         // change player weapon 
                playerController.AddAmmo(value);                                                 // add aditional ammo
            }
            else if (magnet)
            {

                other.GetComponent<MagnetController>().AddMagnet();
            }
            else if (silverCoin)
            {

                GameMaster.instance.IncreaseScore(1);
            }


            if (audioSource)                                                                     // if there is sound attached
                audioSource.Play();
            if (animator)                                                                        // if there is Animator attached
                animator.SetTrigger(collected);
            else
                DestroyPickUp();
        }
    }


    void DestroyPickUp()                                                                         // Animation will use this method after play collected animation
    {
        Destroy(gameObject, audioSource.clip.length);
    }


    void DestroyNow()                                                                            // Animation will use this method after play collected animation
    {
        Destroy(gameObject);
    }

}   // Karol Sobanski
