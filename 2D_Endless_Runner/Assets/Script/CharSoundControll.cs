using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSoundControll : MonoBehaviour
{
    //jump sound
    public AudioClip jump;
    //HIghlight
    // audio highligh
    public AudioClip scoreHighlight;
    //anim higliht
    private Animator anim;
    private bool hl;

    private AudioSource audioPlayer;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
    }
    //anim play
    private void Update()
    {
        if (hl == true)
        {
            anim.SetBool("HIghlight", true);
            hl = false;
        }
        else
        {
            anim.SetBool("HIghlight", false);
        }
    }
    public void PlayJump()
    {
        audioPlayer.PlayOneShot(jump);
    }

    public void PlayScoreHighlight()
    {
        audioPlayer.PlayOneShot(scoreHighlight);
        hl = true;
    }
}