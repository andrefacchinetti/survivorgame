//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System;
using UnityEngine;

namespace EasySky.WeatherArea
{
    [Serializable]
    public class WeatherAreaData
    {
        public string areaName = "Area";
        public Texture2D areaIcon;
        public Color areaColor;
        public WeatherPresetData presetData;
    }
}