using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject puzzleGameObject, creditsGameObject, sandboxGameObject;
    public Button[] mainButtons;

    private enum MenuModes
    {
        clear, puzzle, sandbox, credits
    }
    private MenuModes menuMode;

    // Use this for initialization
    void Start()
    {
        SetUIVisibility(puzzleGameObject, false);
        SetUIVisibility(sandboxGameObject, false);
        SetUIVisibility(creditsGameObject, false);
        SetupPuzzleSelectListeners();
        GameSettings.GetPlayerProfile();
    }

    private void SetMenuMode(MenuModes newMenuMode)
    {
        menuMode = newMenuMode;
        SetUIVisibility(puzzleGameObject, menuMode == MenuModes.puzzle);
        SetUIVisibility(sandboxGameObject, menuMode == MenuModes.sandbox);
        SetUIVisibility(creditsGameObject, menuMode == MenuModes.credits);
        if (newMenuMode == MenuModes.puzzle)
            UpdatePuzzleSelectAppearance();
    }

    private void SetupPuzzleSelectListeners()
    {
        int levelIndex = 0;
        foreach (GameObject child in ParentChildFunctions.GetAllChildren(puzzleGameObject))
        {
            Button button = child.GetComponent<Button>();
            if (button == null)
                continue;
            button.onClick.RemoveAllListeners();
            int notSureWhyThisIsNecessary = levelIndex;
            button.onClick.AddListener(() =>
            {
                //as you can see here, C# is a very pythonic language.
                ChoosePuzzleClick(notSureWhyThisIsNecessary);
            });
            levelIndex++;
        }
    }

    private void UpdatePuzzleSelectAppearance()
    {
        int levelIndex = 0;
        foreach (GameObject child in ParentChildFunctions.GetAllChildren(puzzleGameObject))
        {
            PuzzleButtonUI pb = child.GetComponent<PuzzleButtonUI>();
            if (pb == null)
                continue;
            pb.UpdateAppearance(levelIndex);
            levelIndex++;
        }
    }

    public void PuzzleClick()
    {
        SetMenuMode(MenuModes.puzzle);
    }

    public void SandboxClick()
    {
        SetMenuMode(MenuModes.sandbox);
    }

    public void CreditsClick()
    {
        SetMenuMode(MenuModes.credits);
    }

    public void ChoosePuzzleClick(int levelIndex)
    {
        Debug.Log("ChoosePuzzleClick '" + levelIndex + "'");
        int sceneIndex = LevelManager.GetSceneIndexFromLevelIndex(levelIndex);
        Application.LoadLevel(sceneIndex);
    }

    public void ExitClick()
    {
        Application.Quit();
    }

    public static void SetUIVisibility(GameObject visibilityGameObject, bool isVisible)
    {
        if (visibilityGameObject == null)
            return;

        foreach (GameObject child in ParentChildFunctions.GetAllChildren(visibilityGameObject, true))
        {
            if (child.GetComponent<Canvas>() != null)
                child.GetComponent<Canvas>().enabled = isVisible;
            if (child.GetComponent<Image>() != null)
                child.GetComponent<Image>().enabled = isVisible;
            if (child.GetComponent<Text>() != null)
                child.GetComponent<Text>().enabled = isVisible;
            if (child.GetComponent<Button>() != null)
                child.GetComponent<Button>().enabled = isVisible;
        }
    }
}
