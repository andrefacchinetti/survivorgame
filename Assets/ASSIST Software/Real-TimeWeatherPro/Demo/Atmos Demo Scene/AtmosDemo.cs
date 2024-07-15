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
public class AtmosDemo : MonoBehaviour
{
    [SerializeField] private RealTimeWeatherManager _realTimeWeatherManager;

    private void OnValidate()         
    {
#if (!ATMOS_PRESENT)
        Debug.LogError("Install Atmos");
#else
        RealTimeWeatherRelativePath.UpdatePath();
        if(FindObjectOfType<AtmosModuleController>() == null)
        {
            _realTimeWeatherManager.ActivateAtmosSimulation();
        }
#endif
    }
}
