using UnityEngine;

//! 씬이 사라지면 같이 소멸한다.

public class SingletonOnce<T> : MonoBehaviour where T : Component
{
    protected static T instance = null;
    public static T In
    {
        get
        {
            if (applicationIsQuitting)
            {
                return null;
            }
            else
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                }

                return instance;
            }
        }
    }

    void Awake()
    {
        DoAwake();
    }

    public static bool HasInstance => !IsDestroyed;
    public static bool IsDestroyed => instance == null;

    protected virtual void DoAwake() { }

    protected static bool applicationIsQuitting = false;
    protected virtual void OnApplicationQuit() { applicationIsQuitting = true; }
}