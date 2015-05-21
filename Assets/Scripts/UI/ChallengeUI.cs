using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChallengeUI : MonoBehaviour {
    GameManager gameManager;
    public Text title, goal,tip;

    private GameManager GetGameManager()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        return gameManager;
    }

	public void UpdateAppearance(){
        int levelIndex = FindObjectOfType<GameManager>().levelIndex;
        Level level=LevelManager.GetLevel(levelIndex);
        title.text = level.title;
        goal.text = level.goal;
        tip.text = level.tip;
    }
}
