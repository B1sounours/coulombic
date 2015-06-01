using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateScoreUI : MonoBehaviour {

    public Text scoreText;

	void Update () {
        scoreText.text = GameManager.GetGameManager().GetScore().ToString("0");
	}
}
