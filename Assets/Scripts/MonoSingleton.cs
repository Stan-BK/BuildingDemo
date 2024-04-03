using UnityEngine;


public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    public bool global = true;
    static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType<T>();
            }
            return instance;
        }

    }
    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (global) DontDestroyOnLoad(this.gameObject);
        this.OnStart();
    }

    protected virtual void OnStart()
    {

    }
}