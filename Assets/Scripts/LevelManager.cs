using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Level
{
    public string title, goal, tip;
    public Level(string title, string goal, string tip)
    {
        this.title = title;
        this.goal = goal;
        this.tip = tip;
    }
}

public static class LevelManager
{
    private static bool isStarted = false;
    private static List<Level> levels;

    private static void StartIfNotStarted()
    {
        if (isStarted)
            return;
        isStarted = true;

        levels = new List<Level>();
        levels.Add(new Level("Opposites Attract",
            "Stop the objects from flying apart.",
            "In this challenge, all the objects have the same charge. One! Since they're the same, they push away from each other. But if you left click on them, you can remove electrons and that changes their charge."));
        levels.Add(new Level("Likes Repel",
            "Make the objects fly apart as fast as possible.",
            "In this challenge, you can add OR remove electrons to change the charge. And the cube will never move. What's the best way to make the objects fly apart the fastest? Your score adds up their speed."));
        levels.Add(new Level("Charge Dancing",
            "Make all objects find their partner, and dance their way to victory.",
            "In this challenge, you'll need to add and remove electrons. The dancers on the left need to be the same, and the dancers on the right need to be the same."));
        
        levels.Add(new Level("Tug of War",
            "Pull the big sphere so it falls to the left, not the right.",
            "You can now double the charge on objects. Should you double each cube once, or double one cube three times? And which cube? Know this: if a charge is twice as far away, it has one fourth the pull. If it's three times farther away, it has one ninth the pull. Or if it is R times farther away, it has 1/(R*R) the pull."));
    }

    public static int GetSceneIndexFromLevelIndex(int levelIndex)
    {
        return levelIndex + 1;
    }

    public static int GetLevelCount()
    {
        StartIfNotStarted();
        return levels.Count;
    }

    public static Level GetLevel(int index)
    {
        StartIfNotStarted();
        if (index >= levels.Count)
            return new Level("Mystery Challenge!", "The details of this challenge are a mystery...", "");
        return levels[index];
    }

    public static string GetLevelTitle(int index)
    {
        StartIfNotStarted();
        return levels[index].title;
    }
}
