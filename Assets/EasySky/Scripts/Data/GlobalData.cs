//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System;
using UnityEngine;

namespace EasySky.Data
{
    [CreateAssetMenu(fileName = "GlobalData", menuName = "Assets/GlobalData")]
    public class GlobalData : ScriptableObject
    {
        [HideInInspector] public DateTime globalTime;
        [HideInInspector] public float timeSpeedMultiplier;
        [HideInInspector] public float latitude;
        [HideInInspector] public float longitude;
        [HideInInspector] public float windSpeed;
        [HideInInspector] public float windDirection;
        [HideInInspector] public bool shouldUpdateTime;
        [HideInInspector] public float timeSpeed;
        [HideInInspector] public int seconds;
        [HideInInspector] public int minutes;
        [HideInInspector] public int hours;
        [HideInInspector] public int days;
        [HideInInspector] public int months;
        [HideInInspector] public int years;
    }
}