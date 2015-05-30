using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerProfile
{
    [System.Serializable]
    private class PlayerProfileData
    {
        public bool[] levelWins;
        public float[] levelScores;

        public PlayerProfileData()
        {
            int levelCount = LevelManager.GetLevelCount();
            levelWins = new bool[levelCount];
            levelScores = new float[levelCount];
        }
    }
    private PlayerProfileData playerProfileData;

    public PlayerProfile()
    {
        Load();
    }

    public bool GetWin(int levelIndex)
    {
        return playerProfileData.levelWins[levelIndex];
    }

    public float GetScore(int levelIndex)
    {
        return playerProfileData.levelScores[levelIndex];
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
