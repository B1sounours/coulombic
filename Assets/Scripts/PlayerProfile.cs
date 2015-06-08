using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

//this contains, saves, and loads scores and settings that can be changed by the player.
[System.Serializable]
public class PlayerProfileData
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

public class PlayerProfile
{

    private PlayerProfileData playerProfileData;
    private static PlayerProfile playerProfile;
    private static string keyString = "playerProfileData";

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
        return AreAllLevelsComplete();
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
        if (!LevelManager.IsValidLevelIndex(levelIndex))
            return 0;
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
        XmlSerializer serializer = new XmlSerializer(typeof(PlayerProfileData));

        using (StringWriter sw = new StringWriter())
        {
            serializer.Serialize(sw, playerProfileData);
            //Debug.Log(sw.ToString());
            PlayerPrefs.SetString(keyString, sw.ToString());
        }
    }

    private void Load()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PlayerProfileData));
        string text = PlayerPrefs.GetString(keyString);
        if (text.Length == 0)
        {
            Debug.Log("Did not find PlayerProfileData in PlayerPrefs. Creating new profile.");
            playerProfileData = new PlayerProfileData();
        }
        else
        {
            using (var reader = new System.IO.StringReader(text))
            {
                playerProfileData = serializer.Deserialize(reader) as PlayerProfileData;
            }
        }
    }

}
