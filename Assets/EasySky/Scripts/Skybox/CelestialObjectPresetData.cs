//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using EasySky.Utils;
using System;
using UnityEngine;

namespace EasySky.Skybox
{
    [Serializable]
    public class CelestialObjectPresetData
    {
        public string presetName;
        public Texture2D starIcon;
        public Color lightColor;
        public float scale;
        public Vector2 offset;
        public bool isActive;
        public int lightTemperature;
        public float lightIntensity;
        public float nightLightIntensity = 100;
        public Color flareTint;
        public Color surfaceTint;
        public bool isInstantiated = false;
        public float flareSize;
        public float flareFalloff;
        public Texture2D starTexture;
        public bool castShadows;
        public Color presetColor;
        public string identifier;
        public Resolutions resolution;
        public float shadowDimmer;

        public CelestialObjectPresetData(CelestialObjectPresetData data)
        {
            presetName = data.presetName;
            starIcon = data.starIcon;
            lightColor = data.lightColor;
            scale = data.scale;
            offset = data.offset;
            isActive = data.isActive;
            lightTemperature = data.lightTemperature;
            lightIntensity = data.lightIntensity;
            flareTint = data.flareTint;
            surfaceTint = data.surfaceTint;
            isInstantiated = false;
            flareSize = data.flareSize;
            flareFalloff = data.flareFalloff;
            starTexture = data.starTexture;
            castShadows = data.castShadows;
            presetColor = data.presetColor;
            nightLightIntensity = data.nightLightIntensity;
            resolution = data.resolution;
            shadowDimmer = data.shadowDimmer;
        }
    }
}