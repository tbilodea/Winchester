using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

    protected static T instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (T)FindObjectOfType<T>();

                if(instance == null)
                {
                    Debug.LogError("Could not find instance of type" + typeof(T));
                }
            }

            return instance;
        }
    }

}
