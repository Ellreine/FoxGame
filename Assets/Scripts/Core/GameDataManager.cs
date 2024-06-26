using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LocationData
{
    public string Name;
    public int Level;
    public List<string> CheckpointNames = new List<string>();
}

public static class GameDataManager
{
    private const string LevelKey = "CurrentLevel";
    private const string CheckpointKey = "CurrentCheckpoint";

    public static void SaveLocationData(int level, string checkpointName)
    {
        PlayerPrefs.SetInt(LevelKey, level);
        PlayerPrefs.SetString(CheckpointKey, checkpointName);
        PlayerPrefs.Save();
    }

    public static int LoadLevel()
    {
        return PlayerPrefs.GetInt(LevelKey, 1); // По умолчанию первый уровень
    }

    public static string LoadCheckpoint()
    {
        return PlayerPrefs.GetString(CheckpointKey, string.Empty); // По умолчанию пустая строка
    }

    public static void ResetData()
    {
        PlayerPrefs.DeleteKey(LevelKey);
        PlayerPrefs.DeleteKey(CheckpointKey);
        PlayerPrefs.Save();
    }
}
