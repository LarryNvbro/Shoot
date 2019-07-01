using UnityEngine;

public class GameSettings : ScriptableObject
{
    public GameProperties gameProperties;
    public SpeedProperties speedProperties;

    private static GameSettings _instance;
    public static GameSettings Get()
    {
        if (_instance == null)
            _instance = Resources.Load<GameSettings>("GameSettings");
        return _instance;
    }
}

[System.Serializable]
public class GameProperties
{
    public int nicknameLength;
}

[System.Serializable]
public class SpeedProperties
{
    public float autoDaubDelay;
}
