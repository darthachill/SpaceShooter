using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SilverCoinSpawnController : MonoBehaviour {

    private Vector3 spawnPosistionVector;
    private bool areCoinsOnTheField;
    public GameObject SilverCoinGuard;
 
      GameObject silverCoin;
    GameObject coinTmp;
   private float randomX;
  private  float randomZ;
    public void SpawnCoins(GameObject silverCoin)
    {
        randomX = Random.Range(-17, 5); 
        randomZ = Random.Range(1, 15);

       //  silverCoin = GameObject.Find("SilverCoinPickUp");
     // silverCoin = Resources.Load("Prefab/PicUps/SilverCoinPickUp") as GameObject;
      
       spawnPosistionVector = new Vector3(randomX, 0, randomZ);
        for (int i =0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                {
                coinTmp = Instantiate(silverCoin);
                coinTmp.gameObject.SetActive(true);
                coinTmp.transform.position = spawnPosistionVector + new Vector3(i, 0, j);
                coinTmp.transform.SetParent(SilverCoinGuard.transform);
              
                 }
      
    }

    IEnumerator TenSecondsTimer()
    {
       
        float lastingTime = 10f;
        while (lastingTime > 0)
        {

            lastingTime -= Time.deltaTime;
            yield return null;
        }
        spawnPosistionVector = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    // Use this for initialization
    void Start () {
        SilverCoinGuard = new GameObject();
        SilverCoinGuard.name = "SilverCoinGuard";
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
