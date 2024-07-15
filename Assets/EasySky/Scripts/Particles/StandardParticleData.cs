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
    public class StandardParticleData
    {
        public float intensity;
        public Color particleColor;
        public Vector3 spawnBoxCenter;
        public Vector3 spawnBoxSize;
        public Material particleMaterial;
        public Material subEmiterMaterial;
        public bool isActive;
        public float particleSize;
        public bool isWindInteractionActive;
    }
}