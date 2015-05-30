using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum GameMenuModes
{
    gameplay, toolSelect, challengeInfo, success, fail
}

public class GameplayUI : MonoBehaviour
{
    public Image selectedToolImage;
    public Text selectedToolCount;
    public Toggle hideTipsToggle;

    public GameObject toolSelectContainer, gameplayContainer, challengeInfoContainer, successContainer, failContainer, selectedToolContainer, scoreContainer;

    private GameMenuModes gameMenuMode = GameMenuModes.gameplay;
    private GameManager gameManager;

    private static Sprite[] toolSprites;
    private Tools selectedTool = Tools.add;


    void Start()
    {
        if (GameSettings.GetShowTip(GetGameManager().levelIndex))
            SetGameMenuMode(GameMenuModes.challengeInfo);
        else
            SetGameMenuMode(GameMenuModes.gameplay);
        SelectAnyAvailableTool();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            SetGameMenuMode(gameMenuMode == GameMenuModes.gameplay ? GameMenuModes.toolSelect : GameMenuModes.gameplay);

        if (Input.GetKeyDown(KeyCode.G))
        {
            SetGameMenuMode(GameMenuModes.gameplay);
            FindObjectOfType<GameManager>().SetGamePause(false);
        }

        if (Input.GetKeyDown(KeyCode.R))
            RestartLevel();

        if (Input.GetKeyDown(KeyCode.C))
            SetGameMenuMode(gameMenuMode == GameMenuModes.gameplay ? GameMenuModes.challengeInfo : GameMenuModes.gameplay);

        if (Input.GetKeyDown(KeyCode.M))
            Application.LoadLevel("Main Menu");
    }

    private void RestartLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    private void SelectAnyAvailableTool()
    {
        ClickTool clickTool = FindObjectOfType<ClickTool>();
        for (int i = 0; i < clickTool.toolCharges.Length; i++)
            if (clickTool.toolCharges[i] > 0)
            {
                SelectTool(i);
                break;
            }
    }

    public void HideTipsButtonClick()
    {
        GameSettings.SetShowTip(GetGameManager().levelIndex, !hideTipsToggle.isOn);
    }

    public GameMenuModes GetGameMenuMode()
    {
        return gameMenuMode;
    }

    public Tools GetSelectedTool()
    {
        return selectedTool;
    }

    public void SetGameMenuMode(GameMenuModes newGameMenuMode)
    {
        gameMenuMode = newGameMenuMode;
        MainMenu.SetUIVisibility(gameplayContainer, gameMenuMode == GameMenuModes.gameplay);
        MainMenu.SetUIVisibility(selectedToolContainer, ShouldShowToolCharges());
        MainMenu.SetUIVisibility(scoreContainer, ShouldShowScore());
        MainMenu.SetUIVisibility(toolSelectContainer, gameMenuMode == GameMenuModes.toolSelect);
        MainMenu.SetUIVisibility(challengeInfoContainer, gameMenuMode == GameMenuModes.challengeInfo);
        MainMenu.SetUIVisibility(successContainer, gameMenuMode == GameMenuModes.success);
        MainMenu.SetUIVisibility(failContainer, gameMenuMode == GameMenuModes.fail);
        hideTipsToggle.isOn = !GameSettings.GetShowTip(GetGameManager().levelIndex);
        toolSelectContainer.GetComponent<ToolSelectUI>().UpdateAppearance();
        challengeInfoContainer.GetComponent<ChallengeUI>().UpdateAppearance();
        FindObjectOfType<MouseLook>().enabled = gameMenuMode == GameMenuModes.gameplay;
    }

    private bool ShouldShowScore()
    {
        return GetGameManager().successCondition.minScore>0 && gameMenuMode == GameMenuModes.gameplay;
    }

    private bool ShouldShowToolCharges()
    {
        ClickTool clickTool = FindObjectOfType<ClickTool>();
        return clickTool.HasAtLeastOneToolCharge() && gameMenuMode == GameMenuModes.gameplay;
    }

    public void StayHereButtonClick()
    {
        SoundManager.PlaySound(GameSounds.click);
        SetGameMenuMode(GameMenuModes.gameplay);
    }

    private GameManager GetGameManager()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        return gameManager;
    }

    public void NextLevelButtonClick()
    {
        SoundManager.PlaySound(GameSounds.click);
        int levelIndex = GetGameManager().levelIndex + 1;
        Application.LoadLevel(LevelManager.GetSceneIndexFromLevelIndex(levelIndex));
    }

    public void RestartLevelButtonClick()
    {
        SoundManager.PlaySound(GameSounds.click);
        RestartLevel();
    }

    public void ChallengeInfoButtonClick()
    {
        SoundManager.PlaySound(GameSounds.click);
        SetGameMenuMode(GameMenuModes.gameplay);
    }

    public void SelectToolClick(int toolID)
    {
        SelectTool(toolID);
        SoundManager.PlaySound(GameSounds.click);
        SetGameMenuMode(GameMenuModes.gameplay);
    }

    private void SelectTool(int toolID)
    {
        selectedTool = (Tools)toolID;
        UpdateSelectedToolAppearance();
    }

    public void UpdateSelectedToolAppearance()
    {
        int toolID = (int)selectedTool;
        selectedToolImage.sprite = GetToolSprite(toolID);

        ClickTool clickTool = FindObjectOfType<ClickTool>();
        int count = clickTool.toolCharges[toolID];
        selectedToolCount.text = ToolSelectUI.GetCountText(count, FindObjectOfType<ClickTool>());
    }

    private Sprite GetToolSprite(int toolID)
    {
        if (toolSprites == null)
        {
            toolSprites = new Sprite[4];
            for (int i = 0; i < 4; i++)
            {
                string path = "sprites/tool" + i;
                toolSprites[i] = Resources.Load<Sprite>(path);
                if (toolSprites[i] == null)
                    Debug.LogError(path + " is null");

            }
        }
        return toolSprites[toolID];
    }
}
