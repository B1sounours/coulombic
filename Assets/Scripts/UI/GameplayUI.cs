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
    public Canvas toolSelectCanvas;

    private GameMenuModes gameMenuMode = GameMenuModes.gameplay;

    private static Sprite[] toolSprites;
    private int selectedTool = 1; //add:0  subtract:1  divide:2  multiply:3

    void Start()
    {
        SetGameMenuMode(GameMenuModes.gameplay);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            SetGameMenuMode(gameMenuMode == GameMenuModes.gameplay ? GameMenuModes.toolSelect : GameMenuModes.gameplay);

        if (Input.GetKeyDown(KeyCode.G))
            FindObjectOfType<GameManager>().SetGamePause(false);

        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(Application.loadedLevel);
    }

    public void SetGameMenuMode(GameMenuModes newGameMenuMode)
    {
        gameMenuMode = newGameMenuMode;

        MainMenu.SetVisibility(toolSelectCanvas, gameMenuMode == GameMenuModes.toolSelect);
        MainMenu.SetVisibility(GetComponent<Canvas>(), gameMenuMode == GameMenuModes.gameplay);
        FindObjectOfType<MouseLook>().enabled = gameMenuMode == GameMenuModes.gameplay;
    }

    public void SelectToolClick(int toolID)
    {
        SelectTool(toolID);
    }

    private void SelectTool(int toolID)
    {
        selectedTool = toolID;
        selectedToolImage.sprite = GetToolSprite(toolID);
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
