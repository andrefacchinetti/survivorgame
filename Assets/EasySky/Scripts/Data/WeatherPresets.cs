//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System.Collections.Generic;
using EasySky.WeatherArea;
using UnityEngine;

namespace EasySky.Data
{
    [CreateAssetMenu(fileName = "WeatherPresets", menuName = "Assets/WeatherPresets")]
    public class WeatherPresets : ScriptableObject
    {
        [SerializeField] private List<WeatherPresetData> weatherPresets;

        public List<WeatherPresetData> WeatherPresetsList { get => weatherPresets; }
    }
}