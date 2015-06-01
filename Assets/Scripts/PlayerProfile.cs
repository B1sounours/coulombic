using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerProfile
{
    //this contains, saves, and loads scores and settings that can be changed by the player.
    [System.Serializable]
    private class PlayerProfileData
    {
        public bool[] levelWins;
        public float[] levelScores;
        public bool[] showTips;
        public bool showSandboxTip = true;

        public PlayerProfileData()
        {
            int levelCount = LevelManager.GetLevelCount();
            levelWins = new bool[levelCount];
            levelScores = new float[levelCount];
            showTips = new bool[levelCount];
            for (int i = 0; i < levelCount; i++)
                showTips[i] = true;
        }
    }
    private PlayerProfileData playerProfileData;
    private static PlayerProfile playerProfile;

    public PlayerProfile()
    {
        Load();
    }

    public void ResetProfile()
    {
        playerProfileData = new PlayerProfileData();
        playerProfile.Save();
    }

    public static PlayerProfile GetPlayerProfile()
    {
        if (playerProfile == null)
            playerProfile = new PlayerProfile();
        return playerProfile;
    }

    public bool IsSandboxUnlocked()
    {
        return true || AreAllLevelsComplete();
    }

    public bool AreAllLevelsComplete()
    {
        for (int i = 0; i < playerProfileData.levelWins.Length; i++)
            if (!playerProfileData.levelWins[i])
                return false;
        return true;
    }

    public bool GetShowTip(int levelIndex, bool isSandbox)
    {
        return isSandbox ? playerProfileData.showSandboxTip : playerProfileData.showTips[levelIndex];
    }

    public bool GetWin(int levelIndex)
    {
        return playerProfileData.levelWins[levelIndex];
    }

    public float GetScore(int levelIndex)
    {
        return playerProfileData.levelScores[levelIndex];
    }

    public void SetShowTip(int levelIndex, bool showTip, bool isSandbox)
    {
        if (isSandbox)
            playerProfileData.showSandboxTip = showTip;
        else
            playerProfileData.showTips[levelIndex] = showTip;

        Save();
    }

    public void SetWin(int levelIndex, bool isWin)
    {
        playerProfileData.levelWins[levelIndex] = isWin;
        Save();
    }

    public void SetScore(int levelIndex, float score)
    {
        playerProfileData.levelScores[levelIndex] = score;
        Save();
    }

    private string GetPath()
    {
        return Application.persistentDataPath + Path.DirectorySeparatorChar + "playerProfile.dat";
    }

    private void Save()
    {

        if (playerProfileData == null)
        {
            Debug.LogError("PlayerProfile tried to save a null playerProfileData.");
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Create(GetPath());
            bf.Serialize(fs, playerProfileData);
            fs.Close();
        }
    }

    private void Load()
    {
        if (File.Exists(GetPath()))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(GetPath(), FileMode.Open);
            playerProfileData = (PlayerProfileData)bf.Deserialize(fs);
            fs.Close();
        }
        else
        {
            Debug.Log("PlayerProfile found no profile. Making new. " + GetPath());
            playerProfileData = new PlayerProfileData();
        }
    }
}
