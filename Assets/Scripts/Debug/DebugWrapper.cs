using UnityEngine;

public static class DebugWrapper
{
    private const string editorMode = "UNITY_EDITOR";

    [System.Diagnostics.Conditional(editorMode)]
    public static void Log(object message)
    {
        Debug.Log(message);
    }

    [System.Diagnostics.Conditional(editorMode)]
    public static void LogWarning(object message)
    {
        Debug.LogWarning(message);
    }

    [System.Diagnostics.Conditional(editorMode)]
    public static void LogError(object message)
    {
        Debug.LogError(message);
    }

    [System.Diagnostics.Conditional(editorMode)]
    public static void Assert(bool condition)
    {
        Debug.Assert(condition);
    }

    [System.Diagnostics.Conditional(editorMode)]
    public static void Assert(bool condition, object message)
    {
        Debug.Assert(condition, message);
    }

    [System.Diagnostics.Conditional(editorMode)]
    public static void Assert(bool condition, string message)
    {
        Debug.Assert(condition, message);
    }
}
