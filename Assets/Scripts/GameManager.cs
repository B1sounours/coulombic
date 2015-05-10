using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    private bool isPaused=true;

    void Start()
    {
        SetGamePause(true);
    }

    public void SetGamePause(bool isPaused)
    {
        this.isPaused = isPaused;
        Time.timeScale = isPaused ? 0 : 1;

        foreach (ChargedObject co in FindObjectsOfType<ChargedObject>())
            co.enabled = !isPaused;
        foreach (MovingChargedObject mco in FindObjectsOfType<MovingChargedObject>())
            mco.enabled = !isPaused;
    }

    public bool GetIsPaused()
    {
        return isPaused;
    }
}
