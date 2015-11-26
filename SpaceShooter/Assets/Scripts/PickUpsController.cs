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


    void OnTriggerEnter(Collider other)
    {
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
                GameMaster.instance.IncreaseScore(value);                                        // increase player score
            else if (bulletTime)
                GameMaster.instance.BulletTimeOn();                                              // change timeScale to achive bullettime effect
            else if (gun)                                                                        // if there is any gun attached
            {
                playerController.ChangeWeapons(ref gun);                                         // change player weapon 
                playerController.AddAmmo(value);                                                 // add aditional ammo
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
            Destroy(gameObject);
    }

}   // Karol Sobanski
