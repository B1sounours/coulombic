using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameSounds
{
    click = 0, depleted = 1, victory = 2, fail = 3, refuse=4
}

public static class SoundManager
{
    private static List<AudioClip> sounds;
    private static bool isInitialized = false;

    private static void Initialize()
    {
        isInitialized = true;
        sounds = new List<AudioClip>();

        sounds.Add(Resources.Load<AudioClip>("sounds/click"));
        sounds.Add(Resources.Load<AudioClip>("sounds/depleted"));
        sounds.Add(Resources.Load<AudioClip>("sounds/victory"));
        sounds.Add(Resources.Load<AudioClip>("sounds/fail"));
        sounds.Add(Resources.Load<AudioClip>("sounds/depleted"));
    }

    public static void PlaySound(GameSounds gameSound)
    {        
        GameObject player = GameObject.Find("player");
        PlaySound(gameSound, player.transform.position);
    }

    public static void PlaySound(GameSounds gameSound, Vector3 position)
    {
        if (!isInitialized)
            Initialize();
        AudioSource.PlayClipAtPoint(sounds[(int)gameSound], position);
    }
}
