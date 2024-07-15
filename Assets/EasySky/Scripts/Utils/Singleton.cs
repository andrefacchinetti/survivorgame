//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace EasySky.Utils
{
    /// <summary>
    /// The following class represents the base structure for a Singleton class of type T
    /// </summary>
    /// <typeparam name="T">The type of class that will be singleton</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : EasySkyWeatherManager
    {
        protected static T _instance;
        private static object _lock = new System.Object();

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
#if UNITY_EDITOR
                        _instance = StageUtility.GetCurrentStageHandle().FindComponentOfType<T>();
#else
                        _instance = FindObjectOfType<T>();
#endif
                        _instance?.SetWeatherPreset(_instance.WeatherAreaController.GlobalPresetData.presetData);

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debug.LogError("[Singleton] Something went really wrong " +
                                " - there should never be more than 1 singleton!" +
                                " Reopening the scene might fix it.");
                            return _instance;
                        }
                    }
                    return _instance;
                }
            }
        }

        protected virtual void Start()
        {
            if (_instance != null)
            {
                if (_instance != this)
                {
#if UNITY_EDITOR
                    var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                    var onPrefabModeSelected = prefabStage != null && prefabStage.mode == PrefabStage.Mode.InIsolation;
                    if (onPrefabModeSelected)
                    {
                        _instance = prefabStage.prefabContentsRoot.GetComponent<T>();
                    }
                    else
                    {
                        DestroyImmediate(gameObject);
                    }
#else
                    DestroyImmediate(gameObject);
#endif
                    return;
                }
            }
        }

        private void OnDestroy()
        {
            _instance = null;
        }
    }
}