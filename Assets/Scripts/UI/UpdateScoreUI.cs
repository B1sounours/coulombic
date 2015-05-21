using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateScoreUI : MonoBehaviour {

    private GameManager gameManager;
    public Text scoreText;

    private GameManager GetGameManager()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        return gameManager;
    }

	void Update () {
        scoreText.text = GetGameManager().GetScore().ToString("0");
	}
}
