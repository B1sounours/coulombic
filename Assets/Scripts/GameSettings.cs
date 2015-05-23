using UnityEngine;
using System.Collections;

public static class GameSettings
{
    public static float magnetInterval = 0.005f;
    public static float minimumMagnetInterval = 0.005f;

    public static int maximumCharge = 1000;

    public static int targetFPS = 30;

    private static bool[] showTips;

    private static bool isStarted = false;
    public static bool GetShowTip(int levelIndex)
    {
        StartIfNotStarted();
        if (levelIndex >= showTips.Length)
            Debug.LogError("GetShowTip levelIndex=" + levelIndex + " showTips.length=" + showTips.Length);
        return showTips[levelIndex];
    }

    public static void SetShowTip(int levelIndex, bool newValue)
    {
        StartIfNotStarted();
        showTips[levelIndex] = newValue;
    }

    private static void StartIfNotStarted()
    {
        if (isStarted)
            return;
        isStarted = true;
        showTips = new bool[LevelManager.GetLevelCount()];
        for (int i = 0; i < showTips.Length; i++)
            showTips[i] = true;
    }
}
