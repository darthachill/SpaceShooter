using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;                       // Lists

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    public Boundry boundry;                             // area where ships can move


    [HideInInspector]
    public Transform hierarchyGuard;                    // to keep all created (Clone) in one Transform


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
    public float silverCoinSpawnTime = 10;

    [Header("GameSettings")]
    public Vector3 playerSpawnSpot;
    public float startWait = 3;
    public float endWait = 1;

    [Range(0, 100)]
    public int dropPercent;                                       // how offen item will drop from enemies
    public bool isEnemySpawn = true;
    public bool isPickUpSpawn = true;


    private int nextGoldenScore;                                  // nextGoldenScore is sum of all previous goldenScores
    private int score;                                            // how many scores player has
    private bool IsplayerAlive;
    private const string scoreAnimation = "GoldLabel";
    private BulletTime bulletTime;                                // reference to BulletTime script
    private MedalAwardingFor medalAwardingFor;                    // reference to the medal Awarding;
    private HighScoreController highScoreController;              // reference to highScore Controller it will e invoke after end game to save scores;
    private List<Transform> objectsList = new List<Transform>();  // list to keep references to all spawned objects, it will be helpful to destroy them after player death
    private GameObject playerShip;                                // player ship reference
    private Pause pause;                                          // reference to Pause script


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        nextGoldenScore = goldenScore;                            // initial first golden Score
        scoreText.text = "Score: 0";

        hierarchyGuard = new GameObject("HierarchyGuard").transform;
        highScoreController = GameObject.FindGameObjectWithTag("HighScore").GetComponent<HighScoreController>();  // Get reference to hight score

        // Get references
        bulletTime = GetComponent<BulletTime>();
        medalAwardingFor = GetComponent<MedalAwardingFor>();
        pause = GetComponent<Pause>();
    }



    public void NewGame()                              // Hangar canvas will invoke this
    {
        StartCoroutine(GameLoop());
    }


    public IEnumerator GameLoop()
    {
        yield return StartCoroutine(StartGame());      // wait before StartGame will finish
        yield return StartCoroutine(PlayGame());       // wait before PlayGame  will finish
        yield return StartCoroutine(EndGame());        // wait before endGame   will Finish
    }


    IEnumerator StartGame()
    {
        Reset();                                       // reset HUD elements

        CursorController.instance.HideCursor();        // hide cursor on screen
        pause.CallMenu();                              // now player can stop game by pressing escape

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
        StartCoroutine(SilverCoinSpawner());

        while (CheckIfPlayerIsAlive())                                                        // if player will die quit the loop
            yield return null;
    }


    IEnumerator EndGame()
    {
        isEnemySpawn = false;                                                                                         // stop spawning enemy
        isPickUpSpawn = false;                                                                                        // stop spawning pick ups
        yield return new WaitForSeconds(endWait);                                                                     // wait some time that player will se his death

        medalAwardingFor.SaveMedals();                                                                                // save all player medals information that will be necessary to display in gameOver screen
        highScoreController.SaveScoreInHighScore(score);                                                              // save player score in highscore

        GameObject.FindGameObjectWithTag("GameOver").GetComponent<GameOverController>().UpdateStats(score);           // update stats on gameOver canvas;
        StopAllCoroutines();                                                                                          // it prevents to spawn objects after end the game

        CursorController.instance.ShowCursor();                                                                       // player has possibility to move the cursor
        pause.StopPause();                                                                                            // now player can't call the pause menu
    }


    public void ChooseShip(GameObject newShip)                   // Button will invoke this
    {
        playerShip = newShip;
    }


    void Reset()
    {
        medalAwardingFor.Reset();
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
        objectsList.Add(newDrop.transform);                                               // add to object list new drop item
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


    public void ClearScenee()
    {
        foreach (Transform o in objectsList)                                              // ho through all list with objects
            if (o)                                                                        // if object is't destroy yet
                Destroy(o.gameObject);                                                    // destroy it
    }


    void SpawnPlayer()
    {
        IsplayerAlive = true;
        Instantiate(playerShip, playerSpawnSpot, Quaternion.identity);
    }


    IEnumerator RandomObjectSpawner(GameObject[] objectsToSpawn, Boundry objectsPosition, float objectSpawnTime)
    {
        while (IsplayerAlive)                                                                                                                   // spawn objects all the time
        {
            yield return new WaitForSeconds(objectSpawnTime);
            GameObject randObject = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];                                                     // choose random object
            Vector3 randPosition = new Vector3(Random.Range(-objectsPosition.left, objectsPosition.right), 0, objectsPosition.up);              // choose random object position
            GameObject newObject = Instantiate(randObject, randPosition, Quaternion.identity) as GameObject;                                    // create new object
            objectsList.Add(newObject.transform);                                                                                               // add object to list
            newObject.transform.SetParent(hierarchyGuard);                                                                                      // parent Enemy to  hierarchyGuard
        }
    }


    IEnumerator SilverCoinSpawner()
    {
        while (IsplayerAlive)                                                                                                                   // spawn objects all the time
        {
            yield return new WaitForSeconds(silverCoinSpawnTime);
            GetComponent<SilverCoinSpawnController>().SpawnCoins();                                                                             // parent Enemy to  hierarchyGuard
        }
    }


    IEnumerator RandomObjectSpawner(GameObject objectToSpawn, Boundry objectsPosition, float objectSpawnTime)                                   // method for only one gameobject
    {
        while (IsplayerAlive)                                                                                                                   // spawn objects all the time
        {
            yield return new WaitForSeconds(objectSpawnTime);
            Vector3 randPosition = new Vector3(Random.Range(-objectsPosition.left, objectsPosition.right), 0, objectsPosition.up);              // choose random object position
            GameObject newObject = Instantiate(objectToSpawn, randPosition, Quaternion.identity) as GameObject;                                 // create new object
            objectsList.Add(newObject.transform);                                                                                               // add object to list
            newObject.transform.SetParent(hierarchyGuard);                                                                                      // parent Enemy to  hierarchyGuard
        }
    }
}   // Karol Sobanski