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
    /// Controls the duststorm particles
    /// </summary>
    public class DuststormController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private VisualEffect _duststormEffect;

        private DuststormData _currentData;
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
        public void EnableEffect(bool enable)
        {
            _duststormEffect.gameObject.SetActive(enable);
        }

        public void ApplyData(DuststormData data)
        {
            _duststormEffect.SetFloat("Spawn Rate", data.intensity);
            _duststormEffect.SetVector3(SpawnPositionString, data.spawnboxCenter);
            _duststormEffect.SetVector3(SpawnSizeString, data.spawnBoxSize);
            _duststormEffect.SetVector4(ParticleColorString, data.particleColor);
            _duststormEffect.SetFloat(ColorBlendString, data.colorBlend);
            _duststormEffect.SetFloat("Particle Size", data.particleSize);
            _duststormEffect.SetVector3("Spawn Position", data.spawnboxCenter);
            _duststormEffect.SetVector3("Spawn Box Size", data.spawnBoxSize);
            _duststormEffect.SetTexture("Texture", data.particleTexture);
            _duststormEffect.SetVector2("FlipBook Size", data.flipBookSize);
            EnableEffect(data.isActive);
            EnableWind(data.isWindInteractionActive);
            _currentData = data;
        }

        public void EnableWind(bool value)
        {
            var radians = math.radians(EasySkyWeatherManager.Instance.GlobalData.windDirection);
            _duststormEffect.SetVector2(WindSpeedString, value ? new Vector2((float)math.cos(radians), (float)math.sin(radians)) * EasySkyWeatherManager.Instance.GlobalData.windSpeed : Vector2.zero);
        }

        public void InterpolateEffect(DuststormData curentDustormData, DuststormData targetDuststormData, float progress)
        {
            _currentData = curentDustormData;
            if (!curentDustormData.isActive && !targetDuststormData.isActive)
            {
                return;
            }

            var startIntensity = curentDustormData.isActive ? curentDustormData.intensity : 0;
            var endIntensity = targetDuststormData.isActive ? targetDuststormData.intensity : 0;

            _duststormEffect.SetFloat("Spawn Rate", math.lerp(startIntensity, endIntensity, progress));
            _duststormEffect.SetFloat("Particle Size", math.lerp(curentDustormData.particleSize, targetDuststormData.particleSize, progress));
            _duststormEffect.SetVector4(ParticleColorString, Color.Lerp(curentDustormData.particleColor, targetDuststormData.particleColor, progress));
            _duststormEffect.SetFloat(ColorBlendString, math.lerp(curentDustormData.colorBlend, targetDuststormData.colorBlend, progress));

            if (progress >= 1)
            {
                ApplyData(targetDuststormData);
            }
        }
        #endregion

        #region Private Methods
        private void OnUpdateWind()
        {
            if (!_currentData.isWindInteractionActive)
            { 
                return; 
            }

            var radians = math.radians(EasySkyWeatherManager.Instance.GlobalData.windDirection);
            _duststormEffect.SetVector2(WindSpeedString, new Vector2((float)math.cos(radians), (float)math.sin(radians)) * EasySkyWeatherManager.Instance.GlobalData.windSpeed);
        }
        #endregion
    }
}