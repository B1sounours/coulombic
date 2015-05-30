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
            bool isCompleted = levelIndex % 3 == 0;
            int score = 100 + levelIndex;

            this.puzzleTitle.text = level.title;
            completionCheck.enabled = isCompleted;
            bestScore.text = isCompleted ? "best: " + score : "";
        }
        else
        {
            puzzleTitle.text = "";
            bestScore.text = "";
            completionCheck.enabled = false;
        }
    }

}
