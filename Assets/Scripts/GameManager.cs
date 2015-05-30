using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public bool startPaused = true;
    private bool isPaused = true;
    public GameObject templeRegion;
    public int levelIndex = 0;
    public float startingScore = 0;
    public bool templeHasScoreAbsorb = false;
    private float score = 0;
    private float elapsedTime = 0;
    private bool hasSuccessAppeared = false, hasFailureAppeared=false;

    [System.Serializable]
    public class VictoryCondition
    {
        public float time = 5;
        public float minScore = 0;
        public float failTime = 15;
    }
    public VictoryCondition successCondition;

    void Start()
    {
        StartUI();
        SetGamePause(startPaused);
        MakeTemple();
        score = startingScore;
    }

    public float GetScore()
    {
        return score;
    }

    public void AddScore(float points)
    {
        score += points;
    }

    private void MakeTemple()
    {
        GameObject prefab = Resources.Load<GameObject>("prefabs/temple");
        GameObject temple = Instantiate(prefab);
        ScoreAbsorb sa= temple.AddComponent<ScoreAbsorb>();
        sa.baseValue = 0;
        sa.startVisible = true;
        temple.transform.parent = templeRegion.transform;
        temple.transform.position = new Vector3(0, 0, 0);
    }

    private void StartUI()
    {
        GameObject prefab = Resources.Load<GameObject>("prefabs/GameplayCanvas");
        GameObject gameplayGameObject = Instantiate(prefab);
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

    public float GetVictoryTimerDuration()
    {
        return successCondition.time;
    }

    public bool GetIsPaused()
    {
        return isPaused;
    }

    public bool PlayerHasWon()
    {
        return elapsedTime > successCondition.time && score >= successCondition.minScore;
    }

    public void PlayerWins()
    {
        FindObjectOfType<GameplayUI>().SetGameMenuMode(GameMenuModes.success);
        SoundManager.PlaySound(GameSounds.victory);
        hasSuccessAppeared = true;
    }

    public void PlayerLoses()
    {
        FindObjectOfType<GameplayUI>().SetGameMenuMode(GameMenuModes.fail);
        SoundManager.PlaySound(GameSounds.fail);
        hasFailureAppeared = true;
    }

    public bool PlayerHasLost()
    {
        return elapsedTime > successCondition.failTime && !PlayerHasWon();
    }

    void Update()
    {
        if (!isPaused)
        {
            elapsedTime += Time.deltaTime;
            if (!hasSuccessAppeared && PlayerHasWon())
                PlayerWins();
            if (!hasFailureAppeared && PlayerHasLost())
                PlayerLoses();
        }
    }
}
