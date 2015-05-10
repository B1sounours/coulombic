using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public bool startPaused = true;
    private bool isPaused = true;

    void Start()
    {
        SetGamePause(startPaused);
    }

    public void SetGamePause(bool isPaused)
    {
        this.isPaused = isPaused;
        //Time.timeScale = isPaused ? 0 : 1;

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
