using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public AudioSource select;
    public AudioSource move;
    public AudioSource capture;
    public float volume = 1f;
    public AudioClip invalid;

    void Awake()
    {
        Game.Main.onPieceClick += Select;
        Game.Main.onPieceMove += Move;
        Game.Main.onPieceCapture += Capture;
    }

    public void Select()
    {
        select.Play();
    }

    public void Move()
    {
        move.Play();
    }
    
    public void Capture()
    {
        capture.Play();
    }


}

