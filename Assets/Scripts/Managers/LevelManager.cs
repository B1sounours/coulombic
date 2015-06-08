using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level
{
    public string title, goal, tip;
    public bool exists;
    public Level(string title, string goal, string tip)
    {
        this.title = title;
        this.goal = goal;
        this.tip = tip;
        exists = true;
    }
    public Level()
    {
        this.title = "Mystery Challenge!";
        this.goal = "The details of this challenge are a mystery.";
        this.tip = "";
        exists = false;
    }
}

public static class LevelManager
{
    private static bool isStarted = false;
    private static List<Level> levels;
    private static Level sandboxLevel;

    private static void StartIfNotStarted()
    {
        if (isStarted)
            return;
        isStarted = true;


        AddLevel("Welcome!",
            "What do you think will happen?",
            "In this game, objects that are the same push each other apart. Objects that are different pull together. Have a look around. What do you think will happen to these objects when you turn on time? To turn on time, press 'G' or spacebar.");
        AddLevel("Looking",
            "Turn around.",
            "If you turn on time you'll notice all the objects are coming towards you. Why is that? You need to move your mouse to look around. Then press 'G' or spacebar.");
        AddLevel("Moving",
            "Move around.",
            "When you start time, a few cool things will happen. But you won't see anything from here! To move around, press the keyboard keys 'WASD'. Also, 'Q' and 'E' moves you up and down. Start time by pressing 'G' and then move around.");

        AddLevel("Opposites Attract",
            "Stop the objects from flying apart.",
            "In this challenge, all the objects have the same charge. One! Since they're the same, they push away from each other. But if you left click on them, you can remove electrons and that changes their charge.");
        AddLevel("Likes Repel",
            "Make the objects fly apart as fast as possible.",
            "In this challenge, you can add OR remove electrons to change the charge. What's the best way to make the objects fly apart the fastest? Your score will be higher if they move faster. Press 'T' to select a tool.");
        AddLevel("Charge Dancing",
            "Make all objects find their partner, and dance their way to victory.",
            "In this challenge, you'll need to add and remove electrons. The dancers on the left need to be the same, and the dancers on the right need to be the same.");

        AddLevel("Better Bonds",
            "What do you think will happen?",
            "This world has many weakly charged objects, some strongly charged objects, and one very strongly charged object. What will happen when you press go? Why do some objects stick together, but later decide not to? In this level you can also make any changes you like. Just watch closely!");
        AddLevel("Tug of War",
            "Pull the big sphere so it falls to the left, not the right.",
            "You can now double the charge on objects. Should you double each cube once, or double one cube three times? And which cube? Know this: if a charge is twice as far away, it has one fourth the pull. If it's three times farther away, it has one ninth the pull. Or if it is R times farther away, it has 1/(R*R) the pull.");
        AddLevel("Ion Scoop",
            "Pull in as many particles as possible into the big sphere.",
            "You won't be able to catch them all, but how can you catch the most?");
    }

    public static Level GetSandboxLevel()
    {
        return new Level("Sandbox",
                "Experiment however you like!",
                "Press 'T' to open the sandbox menu. If you press 'R' to reset, all objects and changes you made before pressing 'G' will go back to how they were when you made them. Also, resets will erase all changes you made after you turned time on.");
    }

    public static bool IsValidLevelIndex(int levelIndex)
    {
        return levelIndex >= 0 && levelIndex < GetLevelCount();
    }

    private static void AddLevel(string title, string goal, string tip)
    {
        if (levels == null)
            levels = new List<Level>();

        Level newLevel = new Level(title, goal, tip);
        levels.Add(newLevel);
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

    public static Level GetLevel(int levelIndex)
    {
        StartIfNotStarted();
        if (levelIndex >= levels.Count)
            return new Level();
        return levels[levelIndex];
    }
}
