using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swordfish
{

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	//  Keep this object alive
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)GameObject.FindObjectOfType(typeof(T));

                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<T>();
                    obj.name = typeof(T).ToString();

                    DontDestroyOnLoad(obj);
                }
            }

            return _instance;
        }
    }
}

}