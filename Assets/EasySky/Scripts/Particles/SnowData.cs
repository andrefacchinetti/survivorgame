//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System;
using UnityEngine;

namespace EasySky.Particles
{
    [Serializable]
    public class SnowData : WeatherEffectData
    {
        public Vector2 flipBookSize;
        [Range(-9.81f, 0)]
        public float gravity;
        [Range(1, 5)]
        public int turbulence;
        public bool isFullscreenEffectEnabled;
        [Range(0, 1)]
        public float fullscreenEffectIntensity;
        public float fulscreenEffectNoiseScale;
        [Range(0, 1)]
        public float fullscreenEffectCoverage;
        [Range(0, 1)]
        public float fullscreenEffectNoiseIntensity;
        public Color fulscreenEffectColor;
        public bool isNoiseSnowActive;
        public bool isTextureSnowActive;
        [Range(0, 1)]
        public float snowAmmount;
        public float patchScale;
        public Color snowColor;
        [Range(0, 1)]
        public float snowNormalStrength;
        [Range(0, 1)]
        public float snowBlendDistance;
        [Range(0, 1)]
        public float snowContrast;
        [Range(0, 1)]
        public float snowSpecularShine;
        public LayerMask layers;
    }
}
