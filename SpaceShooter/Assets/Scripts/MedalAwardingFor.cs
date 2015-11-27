using UnityEngine;
using System.Collections;

public class MedalAwardingFor : MonoBehaviour
{

    [System.Serializable]
    public class OnePointIncrease
    {
        public MedalController medalController;     // it controlls showing medal on board
        private int max = 1;                        // how many enemies player must to kill to get medal
        private int current;                        // how many enemies player already killed;
        private bool isMedalAwarded;                // flag prevents to get few times this same medal


        public void CheckMedalAwarding()
        {
            if (isMedalAwarded) return;             // if player already has this medal, do nothing

            current++;                              // add point for invoke this method;
            if (current >= max)                     // if player  exceeded max limit
            {
                medalController.ShowMedalOnBoard(); // give player metal (Show on board)
                isMedalAwarded = true;              // prevents to having more than one medal
            }
        }
    };

    [SerializeField]
    private OnePointIncrease kill;
    [SerializeField]
    private OnePointIncrease stars;

    void Start()
    {

    }


    public void KillingEnemies()
    {
        kill.CheckMedalAwarding();
    }


    public void CollectingStars()
    {
        stars.CheckMedalAwarding();
    }
}   // Karol Sobański
