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
    /// Controls the standard snow particles
    /// </summary>
    public class SnowPsController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private ParticleSystem _snow;
        [SerializeField] private ParticleSystemRenderer _snowRenderer;

        private StandardParticleData _currentData;
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
        public void ApplyData(StandardParticleData data)
        {
            var em = _snow.emission;
            em.rateOverTime = data.intensity;

            var main = _snow.main;
            main.startColor = data.particleColor;

            var shape = _snow.shape;
            shape.scale = data.spawnBoxSize;
            shape.position = data.spawnBoxCenter;

            _snowRenderer.material = data.particleMaterial;

            var colorLife = _snow.colorOverLifetime;
            colorLife.color = data.particleColor;

            EnableParticle(data.isActive);
            EnableWind(data.isWindInteractionActive);
            _currentData = data;
        }

        public void InterpolateEffect(StandardParticleData curentSnowData, StandardParticleData targetSnowData, float x)
        {
            _currentData = curentSnowData;
            if (!curentSnowData.isActive && !targetSnowData.isActive) return;

            var startIntensity = curentSnowData.isActive ? curentSnowData.intensity : 0;
            var endIntensity = targetSnowData.isActive ? targetSnowData.intensity : 0;

            var em = _snow.emission;
            em.rateOverTime = math.lerp(startIntensity, endIntensity, x);

            if (x >= 1)
            {
                ApplyData(targetSnowData);
            }
        }
        #endregion

        #region Private Methods
        private void EnableParticle(bool enable)
        {
            _snow.gameObject.SetActive(enable);
        }

        private void EnableWind(bool value)
        {
            var radians = math.radians(EasySkyWeatherManager.Instance.GlobalData.windDirection);
            var force = _snow.forceOverLifetime;
            var direction = new Vector2((float)math.cos(radians), (float)math.sin(radians)) * EasySkyWeatherManager.Instance.GlobalData.windSpeed;
            force.xMultiplier = value ? direction.x : 0;
            force.zMultiplier = value ? direction.y : 0;
        }

        private void OnUpdateWind()
        {
            if (!_currentData.isWindInteractionActive) return;

            var radians = math.radians(EasySkyWeatherManager.Instance.GlobalData.windDirection);
            var force = _snow.forceOverLifetime;
            var direction = new Vector2((float)math.cos(radians), (float)math.sin(radians)) * EasySkyWeatherManager.Instance.GlobalData.windSpeed;
            force.xMultiplier = direction.x;
            force.zMultiplier = direction.y;
        }
        #endregion
    }
}