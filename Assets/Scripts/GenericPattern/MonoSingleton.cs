using System.Reflection;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _Instance = null;
    private static GameObject singletonContainer = null;

    protected MonoSingleton() { }

    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType(typeof(T)) as T;
                
                if (_Instance == null)
                {
                    singletonContainer = new GameObject();
                    singletonContainer.name = typeof(T).ToString() + "(Singleton)";
                    _Instance = singletonContainer.AddComponent(typeof(T)) as T;
                    DebugWrapper.LogWarning(
                        $"Lazy할 필요가 없을 경우 미리 생성하여 관리하는 방법도 있습니다. 없을 경우, {typeof(T)}를 Lazy하게 생성합니다.");

                    DontDestroyOnLoad(singletonContainer);
                }
            }
         
            return _Instance;
        }
    }

    // virtual initialize method (Mainly, Object Reference Init)
    protected virtual void Initialize() { }

    protected void OnApplicationQuit()
    {
        _Instance = null;
    }
}

