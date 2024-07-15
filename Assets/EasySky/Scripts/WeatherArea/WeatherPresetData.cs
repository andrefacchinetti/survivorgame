//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using EasySky.Clouds;
using EasySky.Particles;
using EasySky.Utils;
using UnityEngine;

namespace EasySky.WeatherArea
{
    [CreateAssetMenu(fileName = "WeatherPresetData", menuName = "Assets/WeatherPresetData")]
    public class WeatherPresetData : ScriptableObject
    {
        public VolumetricCloudPresetData VolumetricCloudPresetData;
        public LayerCloudPresetData LayerCloudPresetData;
        public RainData RainData;
        public HailData HailData;
        public SnowData SnowData;
        public FogData FogData;
        public DuststormData DuststormData;
        public StandardParticleData StandarRainData;
        public StandardParticleData StandarSnowData;
        public StandardParticleData StandarHailData;
        public StandardParticleData StandardDuststormData;
        public string PresetName;
        public Texture2D Icon;
        public Color PresetColor;
        public uint SelectedParticle;
        public WeatherTypes WeatherType;

        public void SetToDefault(WeatherPresetData data)
        {
            VolumetricCloudPresetData = data.VolumetricCloudPresetData;
            LayerCloudPresetData = data.LayerCloudPresetData;
            RainData = data.RainData;
            SnowData = data.SnowData;
            HailData = data.HailData;
            FogData = data.FogData;
            PresetName = data.PresetName;
            PresetColor = data.PresetColor;
            Icon = data.Icon;
            DuststormData = data.DuststormData;
            StandarRainData = data.StandarRainData;
            StandarSnowData = data.StandarSnowData;
            StandarHailData = data.StandarHailData;
            StandardDuststormData = data.StandardDuststormData;
        }
    }
}