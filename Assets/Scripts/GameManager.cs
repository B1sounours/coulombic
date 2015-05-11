﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public bool startPaused = true;
    private bool isPaused = true;

    void Start()
    {
        StartUI();
        SetGamePause(startPaused);
    }

    private void StartUI()
    {
        GameObject prefab = Resources.Load<GameObject>("prefabs/GameplayCanvas");
        GameObject gameplayGameObject= Instantiate(prefab);
    }

    public void SetGamePause(bool isPaused)
    {
        this.isPaused = isPaused;

        foreach (ChargedObject co in FindObjectsOfType<ChargedObject>())
            co.enabled = !isPaused;
        foreach (MovingChargedObject mco in FindObjectsOfType<MovingChargedObject>())
            mco.enabled = !isPaused;
        foreach (Rigidbody rigidbody in FindObjectsOfType<Rigidbody>())
            rigidbody.constraints = isPaused ? RigidbodyConstraints.FreezeAll : 0;
    }

    public bool GetIsPaused()
    {
        return isPaused;
    }
}