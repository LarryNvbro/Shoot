using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameSettings))]
public class GameSettingsEditor : Editor
{
    [MenuItem("Assets/Create/Nvbro/GameSettings")]
    public static void CreateGameSettings()
    {
        GameSettings gameSettings = CreateInstance<GameSettings>();
        string path = System.IO.Path.Combine(NvbroSelectionTools.FolderPathFromSelection(), "GameSettings.asset");
        path = AssetDatabase.GenerateUniqueAssetPath(path);
        AssetDatabase.CreateAsset(gameSettings, path);
        Selection.activeObject = gameSettings;
        AssetDatabase.SaveAssets();
    }
}
