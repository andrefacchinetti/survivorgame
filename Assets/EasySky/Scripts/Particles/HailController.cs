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
    /// Controls the hail particles parameters and the interpolation between the hail presets
    /// </summary>
    public class HailController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private VisualEffect _hailEffect;

        private HailData _currentData;
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
        public void ApplyData(HailData hailData)
        {
            _hailEffect.SetFloat(IntensityString, hailData.intensity);
            _hailEffect.SetVector3(SpawnPositionString, hailData.spawnboxCenter);
            _hailEffect.SetVector3(SpawnSizeString, hailData.spawnBoxSize);
            _hailEffect.SetFloat(MinParticleSizeString, hailData.minParticleSize);
            _hailEffect.SetFloat(MaxParticleSizeString, hailData.maxParticleSize);
            _hailEffect.SetVector4(ParticleColorString, hailData.particleColor);
            _hailEffect.SetFloat(ColorBlendString, hailData.colorBlend);
            _hailEffect.SetFloat(SecondarySizeString, hailData.secondaryParticleSize);
            _hailEffect.SetTexture("Secondary Particle Texture", hailData.secondaryParticleTexture);
            _hailEffect.SetFloat("Secondary Quantity", hailData.secondaryParticleQuantity);
            _hailEffect.SetVector2("Secondary Flip Book Size", hailData.secondaryFlipBookSize);
            _hailEffect.SetFloat("Hail Speed", hailData.hailSpeed);
            _hailEffect.SetVector2("Flip Book Size", hailData.flipBookSize);
            _hailEffect.SetFloat("Particle Lifetime", hailData.lifetime);
            _hailEffect.SetTexture("Particle Texture", hailData.particleTexture);
            EnableEffect(hailData.isActive);
            EnableWind(hailData.isWindInteractionActive);
            _currentData = hailData;
        }

        public void InterpolateEffect(HailData curentHailData, HailData targetHailData, float progress)
        {
            _currentData = curentHailData;
            if (!curentHailData.isActive && !targetHailData.isActive) return;

            var startIntensity = curentHailData.isActive ? curentHailData.intensity : 0;
            var endIntensity = targetHailData.isActive ? targetHailData.intensity : 0;

            _hailEffect.SetFloat(IntensityString, math.lerp(startIntensity, endIntensity, progress));
            _hailEffect.SetFloat(MinParticleSizeString, math.lerp(curentHailData.minParticleSize, targetHailData.minParticleSize, progress));
            _hailEffect.SetFloat(MaxParticleSizeString, math.lerp(curentHailData.maxParticleSize, targetHailData.maxParticleSize, progress));
            _hailEffect.SetVector4(ParticleColorString, Color.Lerp(curentHailData.particleColor, targetHailData.particleColor, progress));
            _hailEffect.SetFloat(ColorBlendString, math.lerp(curentHailData.colorBlend, targetHailData.colorBlend, progress));

            if (progress >= 1)
            {
                ApplyData(targetHailData);
            }
        }
        #endregion

        #region Private Methods
        private void EnableEffect(bool enable)
        {
            _hailEffect.gameObject.SetActive(enable);
        }

        private void OnUpdateWind()
        {
            if (!_currentData.isWindInteractionActive) return;

            var radians = math.radians(EasySkyWeatherManager.Instance.GlobalData.windDirection);
            _hailEffect.SetVector2(WindSpeedString, new Vector2((float)math.cos(radians), (float)math.sin(radians)) * EasySkyWeatherManager.Instance.GlobalData.windSpeed);
        }

        private void EnableWind(bool value)
        {
            var radians = math.radians(EasySkyWeatherManager.Instance.GlobalData.windDirection);
            _hailEffect.SetVector2(WindSpeedString, value ? new Vector2((float)math.cos(radians), (float)math.sin(radians)) * EasySkyWeatherManager.Instance.GlobalData.windSpeed : Vector2.zero);
        }
        #endregion
    }
}