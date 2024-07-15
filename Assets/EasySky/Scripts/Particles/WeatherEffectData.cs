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
    public class WeatherEffectData
    {
        public bool isActive;
        [Range(0, 10)]
        public float intensity;
        public Vector3 spawnboxCenter;
        public Vector3 spawnBoxSize;
        public float particleLifetime;
        [Range(0, 1)]
        public float minParticleSize;
        [Range(0, 1)]
        public float maxParticleSize;
        public Texture particleTexture;
        public Color particleColor;
        [Range(0, 1)]
        public float colorBlend;
        public bool isWindInteractionActive;
        public float lifetime;
    }
}