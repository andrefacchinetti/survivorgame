//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System.Threading;
using System.Threading.Tasks;
using EasySky.WeatherArea;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace EasySky.Particles
{
    /// <summary>
    /// Controls the rain and snow fullscreen effects and the interpolation between the presets
    /// </summary>
    public class FullscreenEffectController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private CustomPassVolume _rainPass;
        [SerializeField] private CustomPassVolume _icePass;
        [SerializeField] private CustomPassVolume _snowLayerTriplanar;
        [SerializeField] private CustomPassVolume _snowLayerNoise;

        private RainData _currentRainEffectData;
        private SnowData _currentSnowEffectData;
        #endregion

        #region Public Methods
        public void ChangeRainEffectState(bool state)
        {
            _rainPass.enabled = state;
        }

        public void ChangeIceEffectState(bool state)
        {
            _icePass.enabled = state;
        }

        public void ChangeSnowTriplanarState(bool state)
        {
            _snowLayerTriplanar.enabled = state;
        }

        public void ChangeSnowNoiseState(bool state)
        {
            _snowLayerNoise.enabled = state;
        }


        public void ApplyDataToRain(RainData data)
        {
            var fullscreenPass = _rainPass.customPasses[0] as FullScreenCustomPass;
            if (fullscreenPass == null) return;

            ChangeRainEffectState(data.isFullscreenEffectEnabled);
            fullscreenPass.fullscreenPassMaterial.SetFloat("_Intensity", data.fullscreenEffectIntensity);
            fullscreenPass.fullscreenPassMaterial.SetFloat("_DropTiling", data.fulscreenEffectDropTiling);
            fullscreenPass.fullscreenPassMaterial.SetFloat("_DripSize", data.fullscreenEffectDripSize);

            _currentRainEffectData = data;
        }

        public void ApplyDataToSnow(SnowData data)
        {
            var fullscreenPass = _icePass.customPasses[0] as FullScreenCustomPass;
            if (fullscreenPass == null) return;

            ChangeIceEffectState(data.isFullscreenEffectEnabled);
            fullscreenPass.fullscreenPassMaterial.SetFloat("_Intensity", data.fullscreenEffectIntensity);
            fullscreenPass.fullscreenPassMaterial.SetFloat("_Noise_Scale", data.fulscreenEffectNoiseScale);
            fullscreenPass.fullscreenPassMaterial.SetFloat("_Coverage", data.fullscreenEffectCoverage);
            fullscreenPass.fullscreenPassMaterial.SetFloat("_Noise_Fringe_Intesity", data.fullscreenEffectNoiseIntensity);
            fullscreenPass.fullscreenPassMaterial.SetColor("_Color", data.fulscreenEffectColor);

            _currentSnowEffectData = data;
        }

        public void ApplyDataToSnowLayer(SnowData data)
        {
            var fullscreenPass = _snowLayerTriplanar.customPasses[0] as DrawRenderersCustomPass;
            if (fullscreenPass == null) return;

            ChangeSnowNoiseState(data.isNoiseSnowActive);
            ChangeSnowTriplanarState(data.isTextureSnowActive);

            SetPassData(fullscreenPass, data);

            fullscreenPass = _snowLayerNoise.customPasses[0] as DrawRenderersCustomPass;
            if (fullscreenPass == null) return;

            SetPassData(fullscreenPass, data);
        }

        public void ApplyData(WeatherAreaData data)
        {
            ApplyDataToSnow(data.presetData.SnowData);
            ApplyDataToRain(data.presetData.RainData);
            ApplyDataToSnowLayer(data.presetData.SnowData);
        }

        public async void InterpolateEffects(RainData rainData, SnowData snowData, CancellationToken cancellationToken)
        {
            var rainFullscreenPass = _rainPass.customPasses[0] as FullScreenCustomPass;
            if (rainFullscreenPass == null) return;

            var shouldUpdateRain = _currentRainEffectData.isFullscreenEffectEnabled || rainData.isFullscreenEffectEnabled;

            var rainStartIntensity = _currentRainEffectData.isFullscreenEffectEnabled ? _currentRainEffectData.fullscreenEffectIntensity : 0;
            var rainEndIntensity = rainData.isFullscreenEffectEnabled ? rainData.fullscreenEffectIntensity : 0;

            var icefullscreenPass = _icePass.customPasses[0] as FullScreenCustomPass;
            if (icefullscreenPass == null) return;

            var shouldUpdateSnow = _currentSnowEffectData.isFullscreenEffectEnabled || snowData.isFullscreenEffectEnabled;

            var iceStartIntensity = _currentSnowEffectData.isFullscreenEffectEnabled ? _currentSnowEffectData.fullscreenEffectIntensity : 0;
            var iceEndIntensity = snowData.isFullscreenEffectEnabled ? snowData.fullscreenEffectIntensity : 0;

            var shouldUpdateSnowLayer = false;
            var fullscreenPass = new DrawRenderersCustomPass();
            if (_currentSnowEffectData.isTextureSnowActive || snowData.isTextureSnowActive)
            {
                fullscreenPass = _snowLayerTriplanar.customPasses[0] as DrawRenderersCustomPass;
                shouldUpdateSnowLayer = true;
                ChangeSnowTriplanarState(true);
            }

            if (_currentSnowEffectData.isNoiseSnowActive || snowData.isNoiseSnowActive)
            {
                fullscreenPass = _snowLayerNoise.customPasses[0] as DrawRenderersCustomPass;
                shouldUpdateSnowLayer = true;
                ChangeSnowNoiseState(true);
            }

            if(snowData.isNoiseSnowActive || snowData.isTextureSnowActive)
            {
                fullscreenPass.layerMask = snowData.layers;
            }


            var snowStartIntensity = _currentSnowEffectData.isNoiseSnowActive || _currentSnowEffectData.isTextureSnowActive ? _currentSnowEffectData.snowAmmount : 0;
            var snowEndIntensity = snowData.isNoiseSnowActive || snowData.isTextureSnowActive ? snowData.snowAmmount : 0;

            if (_currentSnowEffectData.isTextureSnowActive || _currentSnowEffectData.isNoiseSnowActive)
            {
                fullscreenPass.overrideMaterial.SetFloat("_Patch_Scale", _currentSnowEffectData.patchScale);
            }

            if (snowData.isNoiseSnowActive || snowData.isTextureSnowActive)
            {
                fullscreenPass.overrideMaterial.SetFloat("_Patch_Scale", snowData.patchScale);
            }

            var progress = 0f;
            while (progress <= 1)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    ApplyDataToRain(rainData);
                    ApplyDataToSnow(snowData);
                    return;
                }

                if (shouldUpdateRain)
                {
                    ChangeRainEffectState(true);
                    rainFullscreenPass.fullscreenPassMaterial.SetFloat("_Intensity", math.lerp(rainStartIntensity, rainEndIntensity, progress));
                    rainFullscreenPass.fullscreenPassMaterial.SetFloat("_DropTiling", math.lerp(_currentRainEffectData.fulscreenEffectDropTiling, rainData.fulscreenEffectDropTiling, progress));
                    rainFullscreenPass.fullscreenPassMaterial.SetFloat("_DripSize", math.lerp(_currentRainEffectData.fullscreenEffectDripSize, rainData.fullscreenEffectDripSize, progress));
                }

                if (shouldUpdateSnow)
                {
                    ChangeIceEffectState(true);
                    icefullscreenPass.fullscreenPassMaterial.SetFloat("_Intensity", math.lerp(iceStartIntensity, iceEndIntensity, progress));
                    icefullscreenPass.fullscreenPassMaterial.SetFloat("_Noise_Scale", math.lerp(_currentSnowEffectData.fulscreenEffectNoiseScale, snowData.fulscreenEffectNoiseScale, progress));
                    icefullscreenPass.fullscreenPassMaterial.SetFloat("_Coverage", math.lerp(_currentSnowEffectData.fullscreenEffectCoverage, snowData.fullscreenEffectCoverage, progress));
                    icefullscreenPass.fullscreenPassMaterial.SetFloat("_Noise_Fringe_Intesity", math.lerp(_currentSnowEffectData.fullscreenEffectNoiseIntensity, snowData.fullscreenEffectNoiseIntensity, progress));
                    icefullscreenPass.fullscreenPassMaterial.SetColor("_Color", Color.Lerp(_currentSnowEffectData.fulscreenEffectColor, snowData.fulscreenEffectColor, progress));
                }

                if (shouldUpdateSnowLayer)
                {
                    fullscreenPass.overrideMaterial.SetFloat("_Snow_Amount", math.lerp(snowStartIntensity, snowEndIntensity, progress));
                    fullscreenPass.overrideMaterial.SetColor("_Snow_Color_Tint", Color.Lerp(_currentSnowEffectData.snowColor, snowData.snowColor, progress));
                    fullscreenPass.overrideMaterial.SetFloat("_Normal_Strength", math.lerp(_currentSnowEffectData.snowNormalStrength, snowData.snowNormalStrength, progress));
                    fullscreenPass.overrideMaterial.SetFloat("_Snow_Blend_Distance", math.lerp(_currentSnowEffectData.snowBlendDistance, snowData.snowBlendDistance, progress));
                    fullscreenPass.overrideMaterial.SetFloat("_Snow_Contrast", math.lerp(_currentSnowEffectData.snowContrast, snowData.snowContrast, progress));
                    fullscreenPass.overrideMaterial.SetFloat("_Normal_Strength", math.lerp(_currentSnowEffectData.snowNormalStrength, snowData.snowNormalStrength, progress));
                }
                progress += GlobalConstantData.LerpIncrement;
                await Task.Delay(GlobalConstantData.LerpSpeed);
            }

            ApplyDataToRain(rainData);
            ApplyDataToSnow(snowData);
            ApplyDataToSnowLayer(snowData);
        }
        #endregion

        #region Private Methods
        private void SetPassData(DrawRenderersCustomPass pass, SnowData data)
        {
            pass.overrideMaterial.SetFloat("_Snow_Amount", data.snowAmmount);
            pass.overrideMaterial.SetFloat("_Patch_Scale", data.patchScale);
            pass.overrideMaterial.SetColor("_Snow_Color_Tint", data.snowColor);
            pass.overrideMaterial.SetFloat("_Normal_Strength", data.snowNormalStrength);
            pass.overrideMaterial.SetFloat("_Snow_Blend_Distance", data.snowBlendDistance);
            pass.overrideMaterial.SetFloat("_Snow_Contrast", data.snowContrast);
            pass.overrideMaterial.SetFloat("_Snow_Specular_Shine", data.snowSpecularShine);
            pass.layerMask = data.layers;
        }
        #endregion
    }
}