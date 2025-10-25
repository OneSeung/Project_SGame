using UnityEngine;

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
    public float MentalityRange = 5.0f;
    public int MentalityDebuff = 49;

    private PlayerSettings() { }
}


