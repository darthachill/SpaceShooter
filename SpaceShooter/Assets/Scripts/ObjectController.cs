using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class VisualWeapon                             // class keeps information about what kind of weapon object has and where can be spawn
{
    public GameObject weapon;
    public Transform weaponSpawn;

    [HideInInspector]
    public Weapon weaponScripts;
    public VisualWeapon(GameObject weapon, Transform weaponSpawn)
    {
        this.weapon = weapon;
        this.weaponSpawn = weaponSpawn;
    }
}


[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public abstract class ObjectController : MonoBehaviour
{
    [Header("Main Statistics")]
    public int maxHealth = 100;
    public float horizontalMove = 0f;
    public float verticalMove = -1f;
    public GameObject destroyExplosion;               // destroy explosion particle effect
    public AudioSource destroySound;
   

    [Header("Weapons")]
    public List<VisualWeapon> visualWeapons = new List<VisualWeapon>();
    [HideInInspector]
    public int weaponLevel = 1;


    [Header("Control")]
    [SerializeField]
    protected bool isShooting = true;                    // some weapons can disable object ability to shot
    [SerializeField]
    protected bool isMoving = true;                      // some weapons can disable object ability to move

    protected Boundry boundryPosition;
    protected Rigidbody rigidbody;
    protected VisualBar healthBar;
    protected int currentHealth;
    protected bool isAlive = true;                                    // flag prevents killing object many times after his dead
    protected bool isShieldActive;                                    // when shield is active object can't die


    protected virtual void Start()
    {
        boundryPosition = GameMaster.instance.boundry;                // set boundry form GameMaster
        rigidbody = GetComponent<Rigidbody>();                        // get reference to health bar
        currentHealth = maxHealth;

        if (visualWeapons.Count > 0)                                  // get Reference to all Weapons
            for (int i = 0; i < visualWeapons.Count; i++)
                visualWeapons[i].weaponScripts = WeaponCreator(ref visualWeapons[i].weapon, ref visualWeapons[i].weaponSpawn);
    }


    public IEnumerator DisableShotAndMove(float howLong)      // some weapons projectiles can disable object ability to move and shoot for while
    {
        if (!isShieldActive)                                  // if shield isn't active
        {
            rigidbody.velocity = Vector3.zero;                // set object velocity to zero

            isShooting = isMoving = false;
            yield return new WaitForSeconds(howLong);
            isShooting = isMoving = true;
        }
        else
            yield return null;
    }


    protected Weapon WeaponCreator(ref GameObject weapon, ref Transform weaponSpawn)
    {
        GameObject newWeapon = Instantiate(weapon, weaponSpawn.position, weapon.transform.rotation) as GameObject;             // Create Weapon in weapon spown pint
        newWeapon.transform.SetParent(weaponSpawn);                                                                            // parentto spawn weapon point
        return newWeapon.GetComponent<Weapon>();                                                                               // return Weapon script
    }


    protected virtual IEnumerator DestroyEffect()
    {
        if (destroyExplosion != null)
        {

            GameObject newExplosion = Instantiate(destroyExplosion, transform.position, transform.rotation) as GameObject;     // Create explosion
            newExplosion.transform.SetParent(GameMaster.instance.hierarchyGuard);
        }

        if (destroySound != null)
        {
            destroySound.Play();

            while (destroySound.isPlaying)
                yield return null;
        }

        yield return null;
    }


    protected virtual void Move()
    {
        Vector3 movement = new Vector3(horizontalMove, 0, verticalMove);
        rigidbody.velocity = movement;
    }


    public abstract void TakeDamage(int damage, Vector3 damagePosition);
    protected abstract void CheckBoundry();                             // enemy and player have  own boundry

}   //Karol Sobanski