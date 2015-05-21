using UnityEngine;
using System.Collections;

public enum GameSounds
{
    click = 0, depleted = 1
}

public static class SoundManager
{
    private static AudioClip[] sounds;
    private static bool isInitialized = false;

    private static void Initialize()
    {
        isInitialized = true;
        sounds = new AudioClip[2];
        sounds[0] = Resources.Load<AudioClip>("sounds/click");
        sounds[1] = Resources.Load<AudioClip>("sounds/depleted");
    }

    public static void PlaySound(GameSounds gameSound)
    {
        GameObject player = GameObject.Find("player");
        PlaySound(gameSound, player.transform.position);
    }

    public static void PlaySound(GameSounds gameSound,Vector3 position)
    {
        if (!isInitialized)
            Initialize();
        AudioSource.PlayClipAtPoint(sounds[(int)gameSound], position);
    }
}
