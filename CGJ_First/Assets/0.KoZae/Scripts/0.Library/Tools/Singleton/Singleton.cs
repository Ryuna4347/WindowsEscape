using KZLib.Develop;
using System;

public class Singleton<T> where T : class
{
    private static readonly object sync = new object();
    private static volatile T instance = null;

    public static T In
    {
        get
        {
            if (instance == null)
            {
                CreateInstance();
            }

            return instance;
        }
    }
    
    static void CreateInstance()
    {
        lock (sync)
        {
            if (instance == null)
            {
                var type = typeof(T);

                // Ensure there are no public constructors...  
                var ctors = type.GetConstructors();
                if (ctors.Length > 0)
                {
                    throw new KZException($"{type.Name} has at least one accesible ctor making it impossible to enforce singleton behaviour");
                }

                instance = (T) Activator.CreateInstance(type,true);
            }
        }
    }

    public virtual void Destroy() 
    {
        if (instance != null)
        {
            instance = null;
        }
    }

    public static bool HasInstance => instance != null;
}