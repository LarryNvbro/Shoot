using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class ExHierarchy
{
    static ExHierarchy()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
    }

    static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        Object obj = EditorUtility.InstanceIDToObject(instanceID);
        GameObject go = (GameObject)obj as GameObject;

        if (go == null)
            return;

        Rect rect = new Rect(selectionRect);
        rect.x += rect.width - 20;
        rect.width = 18;

        Color color = GUI.backgroundColor;

        if (!go.activeSelf)
        {
            GUI.backgroundColor = Color.red;
        }

        bool olgActive = go.activeSelf;
        bool newActive = GUI.Toggle(rect, go.activeSelf, string.Empty);
        if (olgActive != newActive)
        {
            Undo.RecordObject(go, "Set Active GameObject");
            go.SetActive(newActive);
            EditorUtility.SetDirty(go);
        }

        GUI.backgroundColor = color;

        rect.x -= 84;
        rect.width = 80;

        UIPanel panel = go.GetComponent<UIPanel>();


        GUIStyle textStyle = new GUIStyle();
        textStyle.fontStyle = FontStyle.Normal;
        textStyle.normal.textColor = Color.red;
        textStyle.alignment = TextAnchor.MiddleRight;
        textStyle.fontSize = 9;

        if (panel != null)
        {
            int delpth = panel.depth;
            string labelText = "P {0}";
            GUI.Label(rect, string.Format(labelText, delpth), textStyle);
            rect.x -= 84;
        }

        UIWidget widget = go.GetComponent<UIWidget>();

        textStyle.normal.textColor = Color.yellow;

        if (widget != null)
        {
            int delpth = widget.depth;
            string labelText = "{0} {1}";
            string text = widget.GetType().ToString();

            if (text.StartsWith("UI2D"))
            {
                text = text.Substring(4, 1);
            }
            else
            {
                text = text.Substring(2, 1);
            }

            GUI.Label(rect, string.Format(labelText, text, delpth), textStyle);
            rect.x -= 84;
        }

        Component[] components = go.GetComponents<Component>();
        bool existMissingComponent = false;
        for (int i = 0; i < components.Length; ++i)
        {
            if (components[i] == null)
            {
                existMissingComponent = true;
                break;
            }
        }

        if (existMissingComponent)
        {
            textStyle.normal.textColor = Color.red;
            GUI.Label(rect, "missing component", textStyle);
            rect.x -= 84;
        }

        //if (go.name == "Game Object Pool")
        //{
        //    textStyle.normal.textColor = Color.yellow;
        //    string txt = "{0} ({1})";
        //    string childCount = go.transform.childCount.ToString();
        //    int activeCount = 0;
        //    foreach (Transform child in go.transform)
        //    {
        //        if (child.gameObject.activeSelf)
        //            activeCount++;
        //    }
        //    GUI.Label(rect, string.Format(txt, childCount, activeCount.ToString()), textStyle);
        //    rect.x -= 84;
        //}

        //if (go.name == "FXManager")
        //{
        //    textStyle.normal.textColor = Color.yellow;
        //    string txt = "{0} ({1})";
        //    string childCount = go.transform.childCount.ToString();
        //    int activeCount = 0;
        //    foreach (Transform child in go.transform)
        //    {
        //        if (child.gameObject.activeSelf)
        //            activeCount++;
        //    }
        //    GUI.Label(rect, string.Format(txt, childCount, activeCount.ToString()), textStyle);
        //    rect.x -= 84;
        //}
    }
}
