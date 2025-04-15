using UnityEngine;

public class SingletonManager : MonoBehaviour
{
    public static SingletonManager instance;

    void Awake()
    {
        Singleton();
    }

    void Singleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}