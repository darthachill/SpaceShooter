using UnityEngine;
using System.Collections;


public class EnemyController : ObjectController
{
    public int pointForKill;                          // points for player
    [HideInInspector]
    public Material white;
    public bool isTakingCollisionDamage;              // take damage with colider with player
    public int DamageByCollision;                     // how many damage player will take with collision

    // object take damage to player when hit
    [Tooltip("How many frames object will change his material to white after taking damage")]
    private float whiteFrames = 2;
    private MeshRenderer[] meshRenderers;             // all renderer in model will be change to white color after taking damage
    private Material[] orginalMaterials;


    protected override void Start()
    {
        base.Start();

        SetMaterialArrays();
        healthBar = GameObject.Find("EnemyHealth").GetComponent<VisualBar>();
    }


    protected virtual void Update()
    {
        if (isShooting)                                                         // check if enemy can shoot
            if (GameMaster.instance.CheckIfPlayerIsAlive())                     // check if player is still alive 
                Shot();
            else
                isShooting = false;                                             // if player is death stop shooting

        if (isMoving)
            Move();
    }


    public void DisplayHealthBar()                                              // Player has access to this method,  if he see anamy he will invoke method
    {
        healthBar.DisplayBar(transform);                                        // Display visual health bar on screen
        healthBar.UpdateBar(currentHealth, maxHealth);                          // Update health bar values
    }


    public override void TakeDamage(int damage, Vector3 damagePosition)         // Player has access to this method, 
    {
        if (!isAlive) return;                                                   // if enemy is killed  do nothing


        StartCoroutine(SwitchMaterial());                                       // switch material for while to white
        ExplosionController.instance.RandomExplosionEffect(damagePosition);

        currentHealth -= damage;
        healthBar.UpdateBar(currentHealth, maxHealth);

        if (currentHealth <= 0)
            StartCoroutine(DestroyEffect());
    }


    public void SetShooting(bool state)                                         // GameMaster will invoke this
    {
        if(isAlive)                                                             // if enemy is still alive
            isShooting = state;
    }


    protected override IEnumerator DestroyEffect()
    {
        for (int i = 0; i < meshRenderers.Length; i++)                          // disable all meshh renderers
            if (meshRenderers[i])
                meshRenderers[i].enabled = false;

        isAlive = false;

        GetComponent<Collider>().enabled = false;                               // disable colliders

        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>(); // get  all particles in children

        foreach (ParticleSystem particle in particles)                          // disable all particle system
            particle.Stop();

        isShooting = false;                                                     // disable moving
        isMoving = false;                                                       // disable shooting

        GameMaster.instance.DropRandomItem(transform.position);                 // choose random item to drop
        GameMaster.instance.AddKilledEnemy(pointForKill);                       // Add Killed enemy to stats and send points for him

        yield return StartCoroutine(base.DestroyEffect());                      // play musics and particle effects

        Death();
    }


    protected virtual void Death()                                              // virtual because sometimes we will destroy parent (RocketTower)
    {
        GameMaster.instance.RemoveObject(transform);
        Destroy(gameObject);
    }


    protected override void CheckBoundry()                                      // Enemy Boundry
    {
        rigidbody.position = new Vector3(
        Mathf.Clamp(rigidbody.position.x, boundryPosition.left, boundryPosition.right),
        rigidbody.transform.position.y,
        rigidbody.transform.position.z);
    }


    protected virtual void Shot()
    {
        for (int i = 0; i < visualWeapons.Count; i++)                            // Shot from all weapons
            visualWeapons[i].weaponScripts.Shot(false);
    }


    void SetMaterialArrays()
    {
        meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();      // get all mesh renderes in objects

        int size = meshRenderers.Length;
        orginalMaterials = new Material[size];                                   // set array size

        for (int i = 0; i < size; i++)
            orginalMaterials[i] = meshRenderers[i].material;
    }


    IEnumerator SwitchMaterial()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
            if (meshRenderers[i])
                meshRenderers[i].material = white;

        for (int i = 0; i < whiteFrames; i++)                                     // wait whiteFrames before change material again
            yield return null;

        for (int i = 0; i < meshRenderers.Length; i++)
            if (meshRenderers[i])
                meshRenderers[i].material = orginalMaterials[i];
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && isTakingCollisionDamage)
        {
            other.GetComponent<PlayerController>().TakeDamage(DamageByCollision, transform.position);         // take damage to player
            TakeDamage(maxHealth, transform.position);                                                        // destroy object after collision
        }
    }

}   // Karol Sobanski


