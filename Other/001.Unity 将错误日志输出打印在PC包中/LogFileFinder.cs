using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
public class LogFileFinder
{
    #region Fields
    #endregion
    #region public
    [UnityEditor.Callbacks.OnOpenAsset(0)]
    static bool OnOpenAsset(int instanceId, int line)
    {
        string stackTrace = GetStackTrace();
        if (!string.IsNullOrEmpty(stackTrace) && stackTrace.Contains("SDDebug"))
        {
            Match matches = Regex.Match(stackTrace, @"\(at (.+)\)", RegexOptions.IgnoreCase);
            string pathLine = "";
            while (matches.Success)
            {
                pathLine = matches.Groups[1].Value;
                if (!pathLine.Contains("SDDebug.cs"))
                {
                    int splitIndex = pathLine.LastIndexOf(":");
                    string path = pathLine.Substring(0, splitIndex);
                    line = System.Convert.ToInt32(pathLine.Substring(splitIndex + 1));
                    string fullPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets"));
                    fullPath = fullPath + path;
                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fullPath.Replace('/', '\\'), line);
                    break;
                }
                matches = matches.NextMatch();
            }
            return true;
        }
        return false;
    }

    static string GetStackTrace()
    {
        var consoleWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
        var fieldInfo = consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
        var consoleWindowInstance = fieldInfo.GetValue(null);
        if (consoleWindowInstance != null)
        {
            if ((object)EditorWindow.focusedWindow == consoleWindowInstance)
            {
                var listViewStateType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ListViewState");
                fieldInfo = consoleWindowType.GetField("m_ListView", BindingFlags.Instance | BindingFlags.NonPublic);
                var listView = fieldInfo.GetValue(consoleWindowInstance);
                fieldInfo = listViewStateType.GetField("row", BindingFlags.Instance | BindingFlags.Public);
                int row = (int)fieldInfo.GetValue(listView);
                fieldInfo = consoleWindowType.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
                string activeText = fieldInfo.GetValue(consoleWindowInstance).ToString();
                return activeText;
            }
        }
        return null;
    }
    #endregion
}