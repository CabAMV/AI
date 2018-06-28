using UnityEngine;
using System.Collections;


//NO TOCAR NUNCA

public class Singleton<T> : MonoBehaviour where T : Component
{
    static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    obj.hideFlags = HideFlags.DontSave;
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {

        if (instance == null)
            instance = this as T;

        else
        {
            if (this != instance)
            {
                Destroy(this.gameObject);
            }
        }
    }

}
