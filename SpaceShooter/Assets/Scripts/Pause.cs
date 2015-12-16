using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour
{
    public void ActivePauseControll()         // GameMaster in newGame state will invoke this
    {
        StartCoroutine(IEWaitForKeyPress());   
    }


    public void InactivePauseControll()      // After end the game will end, GameMaster will disable possibility to call the pause menu by invoke this
    {
        StopCoroutine(IEWaitForKeyPress());
    }


    IEnumerator IEWaitForKeyPress()
    {
        while (true)                         
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CursorController.instance.SetCursorLock();
                CallMenu();
            }
            yield return null;
        }
    }


    void CallMenu()
    {

    }
}
