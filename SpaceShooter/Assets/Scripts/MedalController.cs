using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MedalController : MonoBehaviour
{
    AudioSource audiosource;
    Animator animator;

    public Sprite medalSprite;


    private Image image;


    void Start()
    {
        animator = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();
        image = GetComponent<Image>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            animator.SetBool("ShowMedal", true);
    }


    public void ShowMedalOnBoard()         // MedalAword invoke this
    {
        animator.SetBool("ShowMedal", true);
    }


    void ChangeTexture()                    // Animation invoke this
    {
        audiosource.Play();
        image.sprite = medalSprite;
    }


    void StopAnimation()                    // Animation invoke this
    {
        animator.SetBool("ShowMedal", false);
    }

}   // Karol Sobański
