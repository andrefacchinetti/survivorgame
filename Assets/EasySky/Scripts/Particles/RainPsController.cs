//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using Unity.Mathematics;
using UnityEngine;

namespace EasySky.Particles
{
    /// <summary>
    /// Controls the standard rain particle
    /// </summary>
    public class RainPsController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private ParticleSystem _rain;
        [SerializeField] private ParticleSystemRenderer _rainRenderer;
        [SerializeField] private ParticleSystemRenderer _rainRipples;

        private StandardParticleData _currentData;
        #endregion

        #region Unity Methods
        private void Start()
        {
            EasySkyWeatherManager.Instance.OnWindUpdated += OnWindUpdated;
        }

        private void OnDestroy()
        {
            EasySkyWeatherManager.Instance.OnWindUpdated -= OnWindUpdated;
        }
        #endregion

        #region Public Methods
        public void ApplyData(StandardParticleData data)
        {
            var em = _rain.emission;
            em.rateOverTime = data.intensity;

            var main = _rain.main;
            main.startColor = data.particleColor;

            var shape = _rain.shape;
            shape.scale = data.spawnBoxSize;
            shape.position = data.spawnBoxCenter;

            _rainRenderer.material = data.particleMaterial;

            var colorLife = _rain.colorOverLifetime;
            colorLife.color = data.particleColor;

            var size = _rain.main.startSize;
            size.constantMin = size.constantMax = data.particleSize;

            EnableParticle(data.isActive);
            EnableWind(data.isWindInteractionActive);
            _currentData = data;
        }

        public void InterpolateEffect(StandardParticleData curentRainData, StandardParticleData targetRainData, float progress)
        {
            _currentData = curentRainData;
            if (!curentRainData.isActive && !targetRainData.isActive) return;

            var startIntensity = curentRainData.isActive ? curentRainData.intensity : 0;
            var endIntensity = targetRainData.isActive ? targetRainData.intensity : 0;

            var em = _rain.emission;
            em.rateOverTime = math.lerp(startIntensity, endIntensity, progress);

            if (progress >= 1)
            {
                ApplyData(targetRainData);
            }
        }
        #endregion

        #region Private Methods
        private void EnableParticle(bool enable)
        {
            _rain.gameObject.SetActive(enable);
        }

        private void EnableWind(bool value)
        {
            var radians = math.radians(EasySkyWeatherManager.Instance.GlobalData.windDirection);
            var force = _rain.forceOverLifetime;
            var direction = new Vector2((float)math.cos(radians), (float)math.sin(radians)) * EasySkyWeatherManager.Instance.GlobalData.windSpeed;
            force.xMultiplier = value ? direction.x : 0;
            force.zMultiplier = value ? direction.y : 0;
        }

        private void OnWindUpdated()
        {
            if (!_currentData.isWindInteractionActive) return;

            var radians = math.radians(EasySkyWeatherManager.Instance.GlobalData.windDirection);
            var force = _rain.forceOverLifetime;
            var direction = new Vector2((float)math.cos(radians), (float)math.sin(radians)) * EasySkyWeatherManager.Instance.GlobalData.windSpeed;
            force.xMultiplier = direction.x;
            force.zMultiplier = direction.y;
        }
        #endregion
    }
}