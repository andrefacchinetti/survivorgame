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
    /// Controls the vfx snow particles and the interpolation between snow presets
    /// </summary>
    public class SnowController : MonoBehaviour
    {
        #region Private variables
        [SerializeField] private VisualEffect _snowEffect;

        private SnowData _currentData;
        #endregion

        #region Unity Methods
        private void Start()
        {
            EasySkyWeatherManager.Instance.OnWindUpdated += OnUpdateWind;
        }

        private void OnDestroy()
        {
            EasySkyWeatherManager.Instance.OnWindUpdated -= OnUpdateWind;
        }
        #endregion

        #region Public Methods
        public void ApplyData(SnowData snowData)
        {
            _snowEffect.SetFloat(IntensityString, snowData.intensity);
            _snowEffect.SetVector3(SpawnPositionString, snowData.spawnboxCenter);
            _snowEffect.SetVector3(SpawnSizeString, snowData.spawnBoxSize);
            _snowEffect.SetFloat(MinParticleSizeString, snowData.minParticleSize);
            _snowEffect.SetFloat(MaxParticleSizeString, snowData.maxParticleSize);
            _snowEffect.SetVector4(ParticleColorString, snowData.particleColor);
            _snowEffect.SetFloat(ColorBlendString, snowData.colorBlend);
            _snowEffect.SetFloat("Gravity", snowData.gravity);
            _snowEffect.SetInt("Turbulence Intensity", snowData.turbulence);
            _snowEffect.SetTexture("Particle Texture", snowData.particleTexture);
            _snowEffect.SetVector2("Flip Book Size", snowData.flipBookSize);
            _snowEffect.SetFloat("Particle Lifetime", snowData.lifetime);
            EnableEffect(snowData.isActive);
            EnableWind(snowData.isWindInteractionActive);
            _currentData = snowData;
        }

        public void InterpolateEffect(SnowData curentSnowData, SnowData targetSnowData, float progress)
        {
            _currentData = curentSnowData;
            if (!curentSnowData.isActive && !targetSnowData.isActive) return;

            var startIntensity = curentSnowData.isActive ? curentSnowData.intensity : 0;
            var endIntensity = targetSnowData.isActive ? targetSnowData.intensity : 0;

            _snowEffect.SetFloat(IntensityString, math.lerp(startIntensity, endIntensity, progress));
            _snowEffect.SetFloat(MinParticleSizeString, math.lerp(curentSnowData.minParticleSize, targetSnowData.minParticleSize, progress));
            _snowEffect.SetFloat(MaxParticleSizeString, math.lerp(curentSnowData.maxParticleSize, targetSnowData.maxParticleSize, progress));
            _snowEffect.SetVector4(ParticleColorString, Color.Lerp(curentSnowData.particleColor, targetSnowData.particleColor, progress));
            _snowEffect.SetFloat(ColorBlendString, math.lerp(curentSnowData.colorBlend, targetSnowData.colorBlend, progress));

            EnableEffect(curentSnowData.isActive || targetSnowData.isActive);
            EnableWind(targetSnowData.isWindInteractionActive);

            if (progress >= 1)
            {
                ApplyData(targetSnowData);
            }
        }
        #endregion

        #region Private Methods
        private void EnableEffect(bool enable)
        {
            _snowEffect.gameObject.SetActive(enable);
        }

        private void OnUpdateWind()
        {
            if (!_currentData.isWindInteractionActive) return;

            var radians = math.radians(EasySkyWeatherManager.Instance.GlobalData.windDirection);
            _snowEffect.SetVector2(WindSpeedString, new Vector2((float)math.cos(radians), (float)math.sin(radians)) * EasySkyWeatherManager.Instance.GlobalData.windSpeed);
        }

        private void EnableWind(bool value)
        {
            var radians = math.radians(EasySkyWeatherManager.Instance.GlobalData.windDirection);
            _snowEffect.SetVector2(WindSpeedString, value ? new Vector2((float)math.cos(radians), (float)math.sin(radians)) * EasySkyWeatherManager.Instance.GlobalData.windSpeed : Vector2.zero);
        }
        #endregion
    }
}