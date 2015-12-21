using UnityEngine;
using System.Collections;


public class StaminaController : MonoBehaviour
{
    [HideInInspector]
    public AudioClip clipReadyToUse;                        // when stamina is full make noise. Clip will charged to script not to object


    [SerializeField]
    private int maxStamina = 100;                            // max amount of stamina that player has
    [SerializeField]
    private int skillStamina = 80;                          // how many stamina skill will use
    [SerializeField]
    private float staminaIncreaseSpeed = 1;                 // how fast stamina will increase with time
    [SerializeField]
    private bool isIncreaseWithTime;
    [SerializeField]
    private bool isIncreaseWithKill;

    private bool isStaminaFull=true;
    private bool isIncreasingWithTimeOn;
    private float currentStamina;                           // how many stamina player has in this moment
    private AudioSource audioSource;
    private VisualBar staminaBar;


    void OnEnable()
    {
        staminaBar = GameObject.FindWithTag("StaminaBar").GetComponent<VisualBar>();
        GameMaster.instance.staminaController = this;
        audioSource = staminaBar.gameObject.GetComponent<AudioSource>();

        currentStamina = maxStamina;
        staminaBar.UpdateBar(currentStamina, maxStamina);

        if (isIncreaseWithTime)
            StartCoroutine(IncreaseWithTime());

        if (isIncreaseWithKill)                                         //s can be both option active Time and kill
            GameMaster.instance.StaminaByKill = true;

        ReadyToUse();
    }


    IEnumerator IncreaseWithTime()
    {
        isIncreasingWithTimeOn = true;
        isStaminaFull = false;

        while (isIncreasingWithTimeOn)
        {
            currentStamina += Time.deltaTime * staminaIncreaseSpeed;    // increase stamina with time

            if (currentStamina > maxStamina)                            // if current stamina value is greatest than max
            {
                ReadyToUse();
                isIncreasingWithTimeOn = false;                         // stop increase stamina
            }

            staminaBar.UpdateBar(currentStamina, maxStamina);           // update stamina bar on HUD

            yield return null;                                          // refresh screen
        }
    }


    public void IncreaseWithKilling(int points)
    {
        currentStamina += points;

        if (currentStamina >= maxStamina)                                // if current stamina value is greatest than max
            ReadyToUse();

        staminaBar.UpdateBar(currentStamina, maxStamina);
    }


    public void UseSkill()
    {
        if (isStaminaFull)
        {
            isStaminaFull = false;
            staminaBar.BlueIcone(false);
        }

        if (currentStamina <= skillStamina) return;

        currentStamina -= skillStamina;
        staminaBar.UpdateBar(currentStamina, maxStamina);

        if (isIncreaseWithTime && !isIncreasingWithTimeOn)
            StartCoroutine(IncreaseWithTime());
    }


    void ReadyToUse()
    {
        if (audioSource.clip != clipReadyToUse)
            audioSource.clip = clipReadyToUse;

        if (!isStaminaFull)
            audioSource.Play();

        currentStamina = maxStamina;                            // to make sure that is not greatest that max
        staminaBar.BlueIcone(true);                             // active blue icone on screen
        isStaminaFull = true;
    }
}   // Karol Sobański
