using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PuzzleButtonUI : MonoBehaviour
{
    public Text puzzleTitle, bestScore;
    public Image completionCheck;

    public void UpdateAppearance(int levelIndex)
    {
        Level level = LevelManager.GetLevel(levelIndex);
        if (level.exists)
        {
            PlayerProfile pp = PlayerProfile.GetPlayerProfile();
            bool isWin = pp.GetWin(levelIndex) ;
            float score = pp.GetScore(levelIndex);

            this.puzzleTitle.text = level.title;
            completionCheck.enabled = isWin;
            bestScore.text = isWin && score>0 ? "best: " + score.ToString("0") : "";
        }
        else
        {
            puzzleTitle.text = "";
            bestScore.text = "";
            completionCheck.enabled = false;
        }
    }

}
