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
    public class HailData : WeatherEffectData
    {
        public Vector2 flipBookSize;
        [Range(0, 1)]
        public float secondaryParticleSize;
        public Texture secondaryParticleTexture;
        [Range(0, 10)]
        public float secondaryParticleQuantity;
        public Vector2 secondaryFlipBookSize;
        public float secondaryIntensity;
        public float hailSpeed;
    }
}