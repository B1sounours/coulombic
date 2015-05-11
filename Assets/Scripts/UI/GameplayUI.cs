using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum GameMenuModes
{
    gameplay, toolSelect
}

public class GameplayUI : MonoBehaviour
{
    public Image selectedToolImage;
    public Text selectedToolCount;
    public GameObject toolSelectContainer, gameplayContainer;

    private GameMenuModes gameMenuMode = GameMenuModes.gameplay;

    private static Sprite[] toolSprites;
    private Tools selectedTool = Tools.add;

    void Start()
    {
        SetGameMenuMode(GameMenuModes.gameplay);
        SelectTool(0);
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
            Application.LoadLevel(Application.loadedLevel);

        if (Input.GetKeyDown(KeyCode.M))
            Application.LoadLevel("Main Menu");
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
        MainMenu.SetUIVisibility(toolSelectContainer, gameMenuMode == GameMenuModes.toolSelect);
        toolSelectContainer.GetComponent<ToolSelectUI>().UpdateAppearance();
        FindObjectOfType<MouseLook>().enabled = gameMenuMode == GameMenuModes.gameplay;
    }

    public void SelectToolClick(int toolID)
    {
        SelectTool(toolID);
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
        int count = FindObjectOfType<ClickTool>().toolCharges[toolID];
        selectedToolCount.text = ToolSelectUI.GetCountText(count,FindObjectOfType<ClickTool>());
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
