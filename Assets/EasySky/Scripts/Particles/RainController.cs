//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;
using static EasySky.GlobalConstantData;

namespace EasySky.Particles
{
    /// <summary>
    /// Controls the rain vfx particles and the interpolation between rain presets
    /// </summary>
    public class RainController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private VisualEffect _rainEffect;
        [SerializeField] private RipplesController _ripplesController;

        private RainData _currentData;
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            EasySkyWeatherManager.Instance.OnWindUpdated += OnUpdateWind;
        }

        private void OnDestroy()
        {
            EasySkyWeatherManager.Instance.OnWindUpdated -= OnUpdateWind;
        }
        #endregion

        #region Public Methods
        public void ApplyData(RainData rainData)
        {
            _rainEffect.SetFloat(IntensityString, rainData.intensity);
            _rainEffect.SetVector3(SpawnPositionString, rainData.spawnboxCenter);
            _rainEffect.SetVector3(SpawnSizeString, rainData.spawnBoxSize);
            _rainEffect.SetFloat(MinParticleSizeString, rainData.minParticleSize);
            _rainEffect.SetFloat(MaxParticleSizeString, rainData.maxParticleSize);
            _rainEffect.SetVector4(ParticleColorString, rainData.particleColor);
            _rainEffect.SetFloat(ColorBlendString, rainData.colorBlend);
            _rainEffect.SetTexture("Particle Texture", rainData.particleTexture);
            _rainEffect.SetFloat("Particle Lifetime", rainData.particleLifetime);
            _rainEffect.SetFloat("Gravity", rainData.gravity);
            _rainEffect.SetFloat("Particle Lifetime", rainData.lifetime);
            EnableRain(rainData.isActive);
            EnableWind(rainData.isWindInteractionActive);
            _currentData = rainData;
            _ripplesController.ApplyDataToRipples(rainData);
        }

        public void EnableRain(bool enable)
        {
            _rainEffect.gameObject.SetActive(enable);
        }

        public void InterpolateEffect(RainData curentRainData, RainData targetRainData, float progress)
        {
            _currentData = curentRainData;
            if (!curentRainData.isActive && !targetRainData.isActive) return;

            var startIntensity = curentRainData.isActive ? curentRainData.intensity : 0;
            var endIntensity = targetRainData.isActive ? targetRainData.intensity : 0;

            _rainEffect.SetFloat(IntensityString, math.lerp(startIntensity, endIntensity, progress));
            _rainEffect.SetFloat(MinParticleSizeString, math.lerp(curentRainData.minParticleSize, targetRainData.minParticleSize, progress));
            _rainEffect.SetFloat(MaxParticleSizeString, math.lerp(curentRainData.maxParticleSize, targetRainData.maxParticleSize, progress));
            _rainEffect.SetVector4(ParticleColorString, Color.Lerp(curentRainData.particleColor, targetRainData.particleColor, progress));
            _rainEffect.SetFloat(ColorBlendString, math.lerp(curentRainData.colorBlend, targetRainData.colorBlend, progress));
            _ripplesController.ApplyDataToRipples(targetRainData);

            EnableRain(curentRainData.isActive || targetRainData.isActive);
            EnableWind(targetRainData.isWindInteractionActive);

            if (progress >= 1)
            {
                ApplyData(targetRainData);
            }
        }
        #endregion

        #region Private Methods
        private void OnUpdateWind()
        {
            if (!_currentData.isWindInteractionActive) return;

            var radians = math.radians(EasySkyWeatherManager.Instance.GlobalData.windDirection);
            _rainEffect.SetVector2(WindSpeedString, new Vector2((float)math.cos(radians), (float)math.sin(radians)) * EasySkyWeatherManager.Instance.GlobalData.windSpeed);
        }

        private void EnableWind(bool value)
        {
            var radians = math.radians(EasySkyWeatherManager.Instance.GlobalData.windDirection);
            _rainEffect.SetVector2(WindSpeedString, value ? new Vector2((float)math.cos(radians), (float)math.sin(radians)) * EasySkyWeatherManager.Instance.GlobalData.windSpeed : Vector2.zero);
        }
        #endregion
    }
}