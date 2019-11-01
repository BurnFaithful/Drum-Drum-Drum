using UnityEngine;
using UnityEditor;

public class TestBMSSettingWindow : EditorWindow
{
    private static TestBMSSettingWindow window;
    private string bmsDirectory;
    private string bmsName;

    [MenuItem("Test/Music Test")]
    public static void RunMainGame()
    {
        window = GetWindow(typeof(TestBMSSettingWindow)) as TestBMSSettingWindow;
        window.name = "BMS Setting Window";
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("BMS Setting", EditorStyles.boldLabel);
        bmsDirectory = EditorGUILayout.TextField("BMS Directory : ", bmsDirectory);
        bmsName = EditorGUILayout.TextField("BMS File Name(With Extension) : ", bmsName);

        if (GUILayout.Button("Submit"))
        {
            window.Close();
        }
    }
}
