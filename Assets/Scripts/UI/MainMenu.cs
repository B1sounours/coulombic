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
    }

    private void SetMenuMode(MenuModes newMenuMode)
    {
        menuMode = newMenuMode;
        SetUIVisibility(puzzleGameObject, menuMode == MenuModes.puzzle);
        SetUIVisibility(sandboxGameObject, menuMode == MenuModes.sandbox);
        SetUIVisibility(creditsGameObject, menuMode == MenuModes.credits);
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

    public void ChoosePuzzleClick(int levelID)
    {
        Application.LoadLevel(levelID);
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
