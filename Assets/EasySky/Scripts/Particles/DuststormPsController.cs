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
    public class DuststormPsController : MonoBehaviour
    {
        #region Private Variable
        [SerializeField] private ParticleSystem _duststorm;
        [SerializeField] private ParticleSystemRenderer _duststormRenderer;

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
            var em = _duststorm.emission;
            em.rateOverTime = data.intensity;

            var main = _duststorm.main;
            main.startColor = data.particleColor;

            var shape = _duststorm.shape;
            shape.scale = data.spawnBoxSize;
            shape.position = data.spawnBoxCenter;

            _duststormRenderer.material = data.particleMaterial;
            _currentData = data;
            OnWindUpdated();
            EnableParticle(data.isActive);
        }

        public void InterpolateEffect(StandardParticleData curentDuststormData, StandardParticleData targetDuststormData, float progress)
        {
            _currentData = curentDuststormData;
            if (!curentDuststormData.isActive && !targetDuststormData.isActive) return;

            var startIntensity = curentDuststormData.isActive ? curentDuststormData.intensity : 0;
            var endIntensity = targetDuststormData.isActive ? targetDuststormData.intensity : 0;

            var em = _duststorm.emission;

            em.rateOverTime = math.lerp(startIntensity, endIntensity, progress);

            if (progress >= 1)
            {
                ApplyData(targetDuststormData);
            }
        }
        #endregion

        #region Private Methods
        private void EnableParticle(bool enable)
        {
            _duststorm.gameObject.SetActive(enable);
        }

        private void OnWindUpdated()
        {
            var force = _duststorm.forceOverLifetime;
            if (!_currentData.isWindInteractionActive)
            {
                force.xMultiplier = 0;
                force.zMultiplier = 0;
                return;
            }

            var radians = math.radians(EasySkyWeatherManager.Instance.GlobalData.windDirection);
            var direction = new Vector2((float)math.cos(radians), (float)math.sin(radians)) * EasySkyWeatherManager.Instance.GlobalData.windSpeed;
            force.xMultiplier = direction.x;
            force.zMultiplier = direction.y;
        }
        #endregion
    }
}