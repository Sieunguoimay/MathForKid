using System;
using UnityEngine;

public class Singleton<TObject> : MonoBehaviour where TObject : MonoBehaviour
{
    private static TObject _instance;
    private static bool _destroyed = false;
    public static TObject Instance
    {
        get
        {
            if (_instance != null) return _instance;
            if (_destroyed) return null;
            
            _instance = new GameObject($"(Singleton){nameof(TObject)}").AddComponent<TObject>();

            DontDestroyOnLoad(_instance.gameObject);

            return _instance;
        }
    }

    private void OnDestroy()
    {
        _destroyed = true;
    }
}