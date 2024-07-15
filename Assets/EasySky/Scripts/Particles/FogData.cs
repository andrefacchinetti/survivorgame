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
    public class FogData
    {
        public bool isEnabled;
        public float baseHeight;
        public float maxHeight;
        public float attenuationDistance;
        public Color color;
        public float maxFogDistance;
    }
}