using UnityEngine;
using UnityEditor;
using System.Collections;

public class NvbroSelectionTools
{
    public static string FolderPathFromSelection()
    {
        var selection = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        string path = "Assets";
        if (selection.Length > 0) {
            path = AssetDatabase.GetAssetPath(selection[0]);
            var dummypath = System.IO.Path.Combine(path, "fake.asset");
            var assetpath = AssetDatabase.GenerateUniqueAssetPath(dummypath);
            if (assetpath != "") {
                return path;
            } else {
                return System.IO.Path.GetDirectoryName(path);
            }
        }
        return path;
    }
}
