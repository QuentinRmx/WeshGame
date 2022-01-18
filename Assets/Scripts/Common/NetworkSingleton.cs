using Unity.Netcode;
using UnityEngine;

public class NetworkSingleton<T> : NetworkBehaviour where T: Component
{

    internal static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance != null) return _instance;
            T[] objs = FindObjectsOfType(typeof(T)) as T[];
            if (objs is { Length: > 0 })
                _instance = objs[0];
            if (objs is { Length: > 1 })
            {
                Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
            }

            if (_instance != null) return _instance;
            var obj = new GameObject
            {
                name = $"_{typeof(T).Name}"
            };
            
            _instance = obj.AddComponent<T>();
            return _instance;
        }
    }


}