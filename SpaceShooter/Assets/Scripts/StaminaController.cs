using UnityEngine;
using System.Collections;


public class StaminaController : MonoBehaviour
{
    [SerializeField]
    private int maxStamina= 100;                            // max amount of stamina that player has
    [SerializeField]
    private int skillStamina = 80;                          // how many stamina skill will use
    [SerializeField]
    private float staminaIncreaseSpeed = 1;                // how fast stamina will increase with time
    [SerializeField]
    private bool isIncreaseWithTime;
    [SerializeField]
    private bool isIncreaseWithKill;


    private bool isIncreasingWithTimeOn;
    private float currentStamina;                      // how many stamina player has in this moment
    public VisualBar staminaBar;

    // sound fail
    // sound correct

    void OnEnable()
    {
        staminaBar = GameObject.FindWithTag("StaminaBar").GetComponent<VisualBar>();
        GameMaster.instance.staminaController = this;

        currentStamina = maxStamina;
        staminaBar.UpdateBar(currentStamina, maxStamina);

        if (isIncreaseWithTime)
            StartCoroutine(IncreaseWithTime());

        if (isIncreaseWithKill)                      // can be both option active Time and kill
            GameMaster.instance.StaminaByKill = true;
    }


    IEnumerator IncreaseWithTime()
    {
        isIncreasingWithTimeOn = true;

        while (isIncreasingWithTimeOn)
        {
            currentStamina += Time.deltaTime * staminaIncreaseSpeed;    // increase stamina with time

            if (currentStamina > maxStamina)                            // if current stamina value is greatest than max
            {
                currentStamina = maxStamina;                            // to make sure that current stamina isn't greatest than max value assign max stamina to current;
                isIncreasingWithTimeOn = false;                         // stop increase stamina
            }

            staminaBar.UpdateBar(currentStamina, maxStamina);           // update stamina bar on HUD

            yield return null;                                          // refresh screen
        }
    }


    public void IncreaseWithKilling(int points)
    {
        currentStamina += points;

        if (currentStamina > maxStamina)                            // if current stamina value is greatest than max
            currentStamina = maxStamina;

        print(staminaBar + " " + gameObject.name );
        staminaBar.UpdateBar(currentStamina, maxStamina);
    }


    public void UseSkill()
    {
        if (currentStamina <= skillStamina) return;

        currentStamina -= skillStamina;
        staminaBar.UpdateBar(currentStamina, maxStamina);

        if (isIncreaseWithTime && !isIncreasingWithTimeOn)
            StartCoroutine(IncreaseWithTime());
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Y))
            IncreaseWithKilling(10);
    }
}   // Karol Sobański
