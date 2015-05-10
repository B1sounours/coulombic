using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public GameObject puzzleGameObject,creditsGameObject;
    public Button[] mainButtons;


	// Use this for initialization
	void Start () {
        SetUIVisibility(puzzleGameObject, false);
        SetUIVisibility(creditsGameObject, false);
	}

    public void PuzzleClick()
    {
        SetUIVisibility(puzzleGameObject, true);
        SetUIVisibility(creditsGameObject, false);
    }

    public void CreditsClick()
    {
        SetUIVisibility(puzzleGameObject, false);
        SetUIVisibility(creditsGameObject, true);
    }

    public void ChoosePuzzleClick(int levelID)
    {
        Application.LoadLevel(levelID);
    }

    public void ExitClick()
    {
        Application.Quit();
    }

    public static void SetUIVisibility(GameObject visibilityGameObject,bool isVisible)
    {
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
