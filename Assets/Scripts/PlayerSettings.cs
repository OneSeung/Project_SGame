using UnityEngine;
using System.IO;

public class PlayerSettings
{
    private static PlayerSettings _instance;
    public static PlayerSettings Instance
    {
        get
        {
            if (_instance == null)
                _instance = new PlayerSettings();
            return _instance;
        }
    }

    // Hp
    public int Hp = 5;

    // Move
    public float WalkSpeed = 5.0f;
    public float RunSpeed = 10.0f;

    // Stamina
    public float StaminaMax = 100.0f;
    public float StaminaMinus = 1.0f;
    public float StaminaPlus = 10.0f;
    public float StaminaPlusDelay = 5.0f;

    // Mentality
    public float Mentality = 100.0f;
    public float MentalityRange = 5.0f;
    public int MentalityDebuff = 49;

    private PlayerSettings()
    {
        _LoadFromJson();
    }

    private void _LoadFromJson()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "PlayerSettings.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, this);
            Debug.Log("PlayerSettings loaded from JSON.");
        }
        else
        {
            Debug.Log("PlayerSettings.json not found. Creating default JSON.");
            _SaveToJson(path);
        }
    }

    private void _SaveToJson(string path)
    {
        string json = JsonUtility.ToJson(this, true);
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, json);
        Debug.Log("PlayerSettings.json created with default values.");
    }
}



