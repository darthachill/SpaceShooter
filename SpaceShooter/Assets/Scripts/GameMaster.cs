using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    public Boundry boundry;                             // area where ships can move


    [HideInInspector]
    public Transform hierarchyGuard;                    // to keep all created (Clone) in one Transform
    public GameObject playerHolder;                     // player ship reference

    [Header("UI")]
    [HideInInspector]
    public int goldenScore = 100;                       // max score value, when label change his color to gold for a while
    public Text scoreText;                              // player score label on screen


    [Header("Spawn Objects")]
    public GameObject[] enemies;
    public GameObject[] formations;
    public GameObject[] pickUps;
    public GameObject[] dropItems;
    public GameObject fuelTank;
    public GameObject ammo;


    [Header("Spawn Positions")]
    public Boundry pickUpPosition;
    public Boundry enemyPosition;

    [Header("Time between spawning objects")]
    public float enemySpawnTime = 3;
    public float formationSpawnTime = 5;
    public float pickUpTime = 3;
    public float fuelSpawnTime;
    public float ammoSpawnTime;

    [Space(5)]
    private int nextGoldenScore;                       // nextGoldenScore is sum of all previous goldenScores
    private int score;                                 // how many scores player has
    private bool IsplayerAlive;

    [Header("GameSettings")]
    public float startWait = 3;
    public float endWait = 1;

    [Range(0, 100)]
    public int dropPercent;                            // how offen item will drop from enemies
    public bool isEnemySpawn = true;
    public bool isPickUpSpawn = true;

    private const string scoreAnimation = "GoldLabel";
    private BulletTime bulletTime;                     // reference to BulletTime script
    private MedalAwardingFor medalAwardingFor;         // reference to the medal Awarding;
    private HighScoreController highScoreController;   // reference to highScore Controller it will e invoke after end game to save scores;
    


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(scoreText);


        nextGoldenScore = goldenScore;           // initial first golden Score
        scoreText.text = "Score: 0";

        bulletTime = GetComponent<BulletTime>();
        medalAwardingFor = GetComponent<MedalAwardingFor>();

        hierarchyGuard = new GameObject("HierarchyGuard").transform;
        highScoreController = GameObject.FindGameObjectWithTag("HighScore").GetComponent<HighScoreController>();  // Get reference to hight score
    }


    public void NewGame()                             // New Game canvas will invoke this
    {
        Reset();
        StartCoroutine(GameLoop());
    }


    public IEnumerator GameLoop()
    {
        yield return StartCoroutine(StartGame());     // wait before StartGame will finish
        yield return StartCoroutine(PlayGame());      // wait before PlayGame  will finish
        yield return StartCoroutine(EndGame());       // wait before endGame   will Finish
    }


    IEnumerator StartGame()
    {
        isEnemySpawn = true;
        isPickUpSpawn = true;

        SpawnPlayer();                                 // Create Player on Map
        yield return new WaitForSeconds(startWait);    // wait before player ship will appeare on screen
    }


    IEnumerator PlayGame()
    {
        if (isEnemySpawn)
            StartCoroutine(RandomObjectSpawner(enemies, enemyPosition, enemySpawnTime));

        if (isPickUpSpawn)
        {
            StartCoroutine(RandomObjectSpawner(pickUps, pickUpPosition, pickUpTime));         // spawn random objects
            StartCoroutine(RandomObjectSpawner(fuelTank, pickUpPosition, fuelSpawnTime));     // spawn every  time fuel
            StartCoroutine(RandomObjectSpawner(ammo, pickUpPosition, fuelSpawnTime));         // spawn every  ytime ammo
        }

        while (CheckIfPlayerIsAlive())                                                        // if player will die quit the loop
            yield return null;
    }


    IEnumerator EndGame()
    {
        isEnemySpawn = false;
        isPickUpSpawn = false;
        yield return new WaitForSeconds(endWait);
        Menu gameOverMenu = GameObject.FindGameObjectWithTag("GameOver").GetComponent<Menu>();                        // get reference to menu
        gameOverMenu.gameObject.GetComponent<GatesController>().UpdateGateState(true);                                // set gates to open state
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<MenuManager>().ShowMenu(gameOverMenu);                // find MenuManager and switch menu to gameOver
        highScoreController.SaveScoreInHighScore(score);                                                              // save player score in highscore
    }


    void Reset()
    {
        score = 0;
        scoreText.text = "Score: 0";
    }


    public void BulletTimeOn()
    {
        bulletTime.StartCoroutine("TurnOnBulletTime");
    }


    public void PlayerDie()
    {
        IsplayerAlive = false;
    }


    public void IncreaseScore(int amount)                                                // for killing enemies
    {
        this.score += amount;
        scoreText.text = "Score: " + score;

        if (score > nextGoldenScore)
        {
            scoreText.GetComponent<Animator>().SetTrigger(scoreAnimation);
            nextGoldenScore += goldenScore;                                              // set new  value when label will change his color to gold
        }
    }


    public void DropRandomItem(Vector3 spawnPoint)
    {
        int randomPercent = Random.Range(0, 100);                                         // random percent to drop item

        if (randomPercent > dropPercent) return;                                          // if randomPercent is greatest than dropPErcent, create item

        GameObject newDrop = Instantiate(dropItems[Random.Range(0, dropItems.Length)], spawnPoint, Quaternion.identity) as GameObject;
        newDrop.transform.SetParent(hierarchyGuard);
    }


    public bool CheckIfPlayerIsAlive()                                                    // check if player is still alive
    {
        return IsplayerAlive ? true : false;
    }


    public void AddStars(int extraPoints)
    {
        medalAwardingFor.CollectingStars();         // check if player get Medal
        IncreaseScore(extraPoints);                 // increase player score
    }


    public void AddKilledEnemy()
    {
        medalAwardingFor.KillingEnemies();
    }



    void SpawnPlayer()
    {
        IsplayerAlive = true;
        Instantiate(playerHolder, new Vector3(0, 0, -4.0f), Quaternion.identity);
    }
    

    IEnumerator RandomObjectSpawner(GameObject[] objectsToSpawn, Boundry objectsPosition, float objectSpawnTime)
    {
        while (IsplayerAlive)                                                                                                                       // spawn objects all the time
        {

            yield return new WaitForSeconds(objectSpawnTime);
            GameObject randObject = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];                                                     // choose random object
            Vector3 randPosition = new Vector3(Random.Range(-objectsPosition.left, objectsPosition.right), 0, objectsPosition.up);              // choose random object position
            GameObject newObject = Instantiate(randObject, randPosition, Quaternion.identity) as GameObject;                                    // create new object
            newObject.transform.SetParent(hierarchyGuard);                                                                                      // parent Enemy to  hierarchyGuard

        }
    }
    

    IEnumerator RandomObjectSpawner(GameObject objectsToSpawn, Boundry objectsPosition, float objectSpawnTime)                                  // method for only one gameobject
    {
        while (IsplayerAlive)                                                                                                                   // spawn objects all the time
        {
            yield return new WaitForSeconds(objectSpawnTime);
            Vector3 randPosition = new Vector3(Random.Range(-objectsPosition.left, objectsPosition.right), 0, objectsPosition.up);              // choose random object position
            GameObject newObject = Instantiate(objectsToSpawn, randPosition, Quaternion.identity) as GameObject;                                // create new object
            newObject.transform.SetParent(hierarchyGuard);                                                                                      // parent Enemy to  hierarchyGuard
        }
    }
}   // Karol Sobanski