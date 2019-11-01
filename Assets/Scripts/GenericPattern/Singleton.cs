using System;

public abstract class Singleton<T> where T : class, new()
{
    private static readonly Lazy<T> _Instance = new Lazy<T>(() => new T());

    protected Singleton() { }

    public static T Instance
    {
        get { return _Instance.Value; }
    }
}
