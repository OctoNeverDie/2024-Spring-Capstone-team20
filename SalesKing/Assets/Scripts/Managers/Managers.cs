using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers instance; 
    public static Managers Instance 
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Managers>();

                if (instance == null)
                {
                    GameObject singleton = new GameObject();
                    instance = singleton.AddComponent<Managers>();
                    singleton.name = typeof(Managers).ToString() + " (Singleton)";

                    DontDestroyOnLoad(singleton);
                }
            }
            return instance;
        }
    } 

    public GameObject ManagersGO;


    void Awake()
    {
        Init();
        ManagersGO = transform.gameObject;
    }

    void Init()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    
    public void ClearChildManagers()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

}