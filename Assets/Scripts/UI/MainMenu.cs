using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject puzzleContainer, creditsContainer, sandboxContainer, optionsContainer;
    public Button[] mainButtons;

    private enum MenuModes
    {
        clear, puzzle, sandbox, credits, options
    }
    private MenuModes menuMode;

    // Use this for initialization
    void Start()
    {
        SetMenuMode(MenuModes.clear);
        SetupPuzzleSelectListeners();
        PlayerProfile.GetPlayerProfile();
    }

    private void SetMenuMode(MenuModes newMenuMode)
    {
        menuMode = newMenuMode;
        SetUIVisibility(puzzleContainer, menuMode == MenuModes.puzzle);
        SetUIVisibility(sandboxContainer, menuMode == MenuModes.sandbox);
        SetUIVisibility(creditsContainer, menuMode == MenuModes.credits);
        SetUIVisibility(optionsContainer, menuMode == MenuModes.options);
        if (newMenuMode == MenuModes.puzzle)
            UpdatePuzzleSelectAppearance();
    }

    private void SetupPuzzleSelectListeners()
    {
        int levelIndex = 0;
        foreach (GameObject child in ParentChildFunctions.GetAllChildren(puzzleContainer))
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
        foreach (GameObject child in ParentChildFunctions.GetAllChildren(puzzleContainer))
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
        SoundManager.PlaySound(GameSounds.click);
        SetMenuMode(MenuModes.puzzle);
    }

    public void SandboxClick()
    {
        SoundManager.PlaySound(GameSounds.click);
        if (PlayerProfile.GetPlayerProfile().IsSandboxUnlocked())
            Application.LoadLevel("sandbox");
        else
            SetMenuMode(MenuModes.sandbox);
    }

    public void CreditsClick()
    {
        SoundManager.PlaySound(GameSounds.click);
        SetMenuMode(MenuModes.credits);
    }

    public void OptionsClick()
    {
        SoundManager.PlaySound(GameSounds.click);
        SetMenuMode(MenuModes.options);
    }

    public void ChoosePuzzleClick(int levelIndex)
    {
        SoundManager.PlaySound(GameSounds.click);
        int sceneIndex = LevelManager.GetSceneIndexFromLevelIndex(levelIndex);
        Application.LoadLevel(sceneIndex);
    }

    public void ExitClick()
    {
        Application.Quit();
    }

    public void ClearScoresClick()
    {
        PlayerProfile.GetPlayerProfile().ResetProfile();
        SoundManager.PlaySound(GameSounds.click);
        SetMenuMode(MenuModes.clear);
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
