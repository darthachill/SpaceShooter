using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    public Text scoreText;
    public AudioSource pointsSnd;


    public void UpdateStats(int score)
    {
        scoreText.text = "0";                                                                            // clear score on screen
        Menu gameOverMenu = GetComponent<Menu>();                                                        // get reference to menu
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<MenuManager>().ShowMenu(gameOverMenu);   // find MenuManager and switch menu to gameOver
        StartCoroutine(GetComponent<MedalLoader>().LoadAllMedals());                                     // load all medals to gameover canvas
        GetComponent<GatesController>().UpdateGateState(true);                                           // set gates to open state

        if (score > 0)
            StartCoroutine(UpdateScore(score));
    }


    IEnumerator UpdateScore(int score)
    {
        pointsSnd.Play();                                                                                // play score counter snd
        yield return new WaitForSeconds(0.5f);

        float clipLength = pointsSnd.clip.length - 0.65f;                                                
        float current = 0;

        float pointsPerFrame = score / clipLength;             // how many poits will add to score every frame

        while ((score - current) > 0)          
        {
            current += pointsPerFrame * Time.deltaTime;
            scoreText.text = ((int)current).ToString();

            yield return null;
        }

        scoreText.text = score.ToString();
    }
}   // Karol Sobański
