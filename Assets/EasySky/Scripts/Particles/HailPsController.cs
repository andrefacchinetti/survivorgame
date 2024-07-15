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
    /// Controls the standard hail particle system
    /// </summary>
    public class HailPsController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private ParticleSystem _hail;
        [SerializeField] private ParticleSystemRenderer _hailRenderer;

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
            var em = _hail.emission;
            em.rateOverTime = data.intensity;

            var main = _hail.main;
            main.startColor = data.particleColor;

            var shape = _hail.shape;
            shape.scale = data.spawnBoxSize;
            shape.position = data.spawnBoxCenter;

            _hailRenderer.material = data.particleMaterial;

            var colorLife = _hail.colorOverLifetime;
            colorLife.color = data.particleColor;

            EnableParticle(data.isActive);
            EnableWind(data.isWindInteractionActive);
            _currentData = data;
        }

        public void InterpolateEffect(StandardParticleData curentHailData, StandardParticleData targetHailData, float progress)
        {
            _currentData = curentHailData;
            if (!curentHailData.isActive && !targetHailData.isActive) return;

            var startIntensity = curentHailData.isActive ? curentHailData.intensity : 0;
            var endIntensity = targetHailData.isActive ? targetHailData.intensity : 0;

            var em = _hail.emission;

            em.rateOverTime = math.lerp(startIntensity, endIntensity, progress);

            if (progress >= 1)
            {
                ApplyData(targetHailData);
            }
        }
        #endregion

        #region Private Methods
        private void EnableParticle(bool enable)
        {
            _hail.gameObject.SetActive(enable);
        }

        private void EnableWind(bool value)
        {
            var radians = math.radians(EasySkyWeatherManager.Instance.GlobalData.windDirection);
            var force = _hail.forceOverLifetime;
            var direction = new Vector2((float)math.cos(radians), (float)math.sin(radians)) * EasySkyWeatherManager.Instance.GlobalData.windSpeed;
            force.xMultiplier = value ? direction.x : 0;
            force.zMultiplier = value ? direction.y : 0;
        }

        private void OnWindUpdated()
        {
            if (!_currentData.isWindInteractionActive) return;

            var radians = math.radians(EasySkyWeatherManager.Instance.GlobalData.windDirection);
            var force = _hail.forceOverLifetime;
            var direction = new Vector2((float)math.cos(radians), (float)math.sin(radians)) * EasySkyWeatherManager.Instance.GlobalData.windSpeed;
            force.xMultiplier = direction.x;
            force.zMultiplier = direction.y;
        }
        #endregion
    }
}