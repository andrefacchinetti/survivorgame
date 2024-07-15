//
// Copyright(c) 2023 Real-Time Weather ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using RealTimeWeather;
using RealTimeWeather.Managers;
using RealTimeWeather.WeatherControllers;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class EnviroDemo : MonoBehaviour
{
    [SerializeField] private RealTimeWeatherManager _realTimeWeatherManager;

    private void OnValidate()
    {
#if (!ENVIRO_PRESENT)
        Debug.LogError("Install Enviro");
#else
        RealTimeWeatherRelativePath.UpdatePath();
        if(FindObjectOfType<EnviroModuleController>() == null)
        {
            _realTimeWeatherManager.ActivateEnviroSimulation();
        }
#endif
    }
}
