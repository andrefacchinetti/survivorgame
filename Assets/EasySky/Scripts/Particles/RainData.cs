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
    public class RainData : WeatherEffectData
    {
        [Range(0, 50)]
        public float normalEvaluationDistance;
        public bool areRipplesActive;
        public bool isFullscreenEffectEnabled;
        [Range(0, 1)]
        public float fullscreenEffectIntensity;
        public float fulscreenEffectDropTiling;
        public float fullscreenEffectDripSize;
        [Range(0, 1)]
        public float fullscreenEffectSpeed;
        [Range(0, 1)]
        public float ripplesCoverage;
        public float ripplesTiling;
        [Range(0, 1)]
        public float ripplesSpeed;
        [Range(0, 1)]
        public float ripplesScale;
        [Range(0, 1)]
        public float ripplesNormalIntensity;
        public bool areLightningsEnabled;
        public float lightningsFrequency;
        public LayerMask layers;
        public float gravity;
    }
}