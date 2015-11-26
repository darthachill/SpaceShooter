using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighScoreController : MonoBehaviour
{
    public Text[] scoreText;


    string hightScore = "HighScore";


    public void ResetHighScores()
    {
        for (int i = 0; i < scoreText.Length; i++)
            PlayerPrefs.SetInt((hightScore + (i + 1).ToString()), i);
    }


    public void SaveScoreInHighScore(int currentScore)
    {
        int i = 1;
        int place = 0;

        while (i <= scoreText.Length)                                      // go through all scores
        {
            if (currentScore > PlayerPrefs.GetInt(hightScore + i))         // if current score is greatest than this score in high Score
                place = i;

            if (place > 0)
            {
                for (int j = scoreText.Length; j > place; j--)
                {
                    int previous = PlayerPrefs.GetInt(hightScore + (j - 1));     // save latest score
                    //print("zapisz : " + j + "Score: " + PlayerPrefs.GetInt(hightScore + j) + "Zamien z: " +  (j-1) + " " +  PlayerPrefs.GetInt(hightScore + (j - 1).ToString()));
                    PlayerPrefs.SetInt(hightScore + (j).ToString(), previous);
                }

                PlayerPrefs.SetInt(hightScore + place, currentScore);
                break;                                                     // Quit the loop
            }
            i++;                                                           // check another score
        }
        UploadScores();                                                    // upload Text score in high Score menu
    }


    void UploadScores()
    {
        for (int i = 0; i < scoreText.Length; i++)
            scoreText[i].text = PlayerPrefs.GetInt(hightScore + (i + 1).ToString()).ToString();
    }
}
