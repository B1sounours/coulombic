using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public bool startPaused = true;
    private bool isPaused = true;
    private bool hasSimulationBegun = false;
    public GameObject templeRegion;
    public int levelIndex = 0;
    public float startingScore = 0;
    public bool templeHasScoreAbsorb = false;
    public bool isSandboxMode = false;
    private float score = 0;
    private float elapsedTime = 0;
    private bool hasSuccessAppeared = false, hasFailureAppeared = false;

    private float mustSeeObjectTimer = 0;
    private Renderer mustSeeObjectRenderer;
    private Vector3 mustMoveAroundStartPosition;
    private GameObject player;
    private static GameManager gameManager;

    [System.Serializable]
    public class VictoryCondition
    {
        public float time = 5;
        public float minScore = 0;
        public float failTime = 15;
        public GameObject mustSeeObject;
        public bool mustMoveAround = false;
    }
    public VictoryCondition successCondition;

    void Start()
    {
        gameManager = this;
        StartUI();
        SetGamePause(startPaused);
        MakeTemple();
        score = startingScore;
        mustMoveAroundStartPosition = GetPlayer().transform.position;
        if (isSandboxMode && FindObjectOfType<SandboxManager>() == null)
            gameObject.AddComponent<SandboxManager>();
    }
    
    public static GameManager GetGameManager()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        return gameManager;
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
        ScoreAbsorb sa = temple.AddComponent<ScoreAbsorb>();
        sa.baseValue = 0;
        sa.startVisible = true;
        temple.transform.parent = templeRegion.transform;
        temple.transform.position = new Vector3(0, 0, 0);
    }

    private void StartUI()
    {
        GameObject prefab = Resources.Load<GameObject>("prefabs/UserInterface");
        GameObject gameplayGameObject = Instantiate(prefab);
        gameplayGameObject.transform.position = Vector3.zero;
    }

    public bool HasSimulationBegun()
    {
        return hasSimulationBegun;
    }

    public void SetGamePause(bool isPaused)
    {
        this.isPaused = isPaused;
        if (!isPaused && !HasSimulationBegun())
        {
            hasSimulationBegun = true;
            foreach (MovingChargedObject mco in FindObjectsOfType<MovingChargedObject>())
                mco.SetFrozenPosition(false);
        }

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

    private GameObject GetPlayer()
    {
        if (player == null)
            player = FindObjectOfType<FlightController>().gameObject;
        return player;
    }

    private bool PlayerHasMovedSignificantly()
    {
        return Vector3.Distance(mustMoveAroundStartPosition, GetPlayer().transform.position) > 10;
    }

    public bool PlayerHasWon()
    {
        if (successCondition.mustSeeObject != null && mustSeeObjectTimer < 1)
            return false;
        if (successCondition.mustMoveAround && !PlayerHasMovedSignificantly())
            return false;
        return elapsedTime > successCondition.time && score >= successCondition.minScore;
    }

    public void PlayerWins()
    {
        FindObjectOfType<GameplayUI>().SetGameMenuMode(GameMenuModes.success);
        SoundManager.PlaySound(GameSounds.victory);
        hasSuccessAppeared = true;
        SaveScore();
        PlayerProfile.GetPlayerProfile().SetWin(levelIndex, true);
    }

    public void PlayerLoses()
    {
        FindObjectOfType<GameplayUI>().SetGameMenuMode(GameMenuModes.fail);
        SoundManager.PlaySound(GameSounds.fail);
        hasFailureAppeared = true;
    }

    public bool PlayerHasLost()
    {
        return successCondition.failTime > 0 && elapsedTime > successCondition.failTime && !PlayerHasWon();
    }

    private Renderer GetMustSeeObjectRenderer()
    {
        if (mustSeeObjectRenderer == null)
            mustSeeObjectRenderer = successCondition.mustSeeObject.GetComponent<Renderer>();
        return mustSeeObjectRenderer;
    }

    public void SaveScore()
    {
        PlayerProfile pp = PlayerProfile.GetPlayerProfile();
        if (pp.GetScore(levelIndex) < score)
            pp.SetScore(levelIndex, score);
    }

    void Update()
    {
        if (!isPaused)
        {
            elapsedTime += Time.deltaTime;

            if (!isSandboxMode)
            {
                if (!hasSuccessAppeared && PlayerHasWon())
                    PlayerWins();
                if (!hasFailureAppeared && PlayerHasLost())
                    PlayerLoses();
            }
        }

        if (successCondition.mustSeeObject != null && GetMustSeeObjectRenderer().isVisible)
            mustSeeObjectTimer += Time.deltaTime;
    }
}
