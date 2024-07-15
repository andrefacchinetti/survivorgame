//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace EasySky.Particles
{
    /// <summary>
    /// Controls the fog parameters and the interpolation between fog presets
    /// </summary>
    public class FogController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private Volume _volume;

        private Fog _fog;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            SetupFog();
        }
        #endregion

        #region Public Methods
        public void SetupFog()
        {
            _volume.sharedProfile.TryGet(out _fog);
        }

        /// <summary>
        /// Used during other Unity Store integrations
        /// </summary>
        public void ChangeFogState(bool state)
        {
            _fog.enabled.value = state;
        }

        /// <summary>
        /// Used during other Unity Store integrations
        /// </summary>
        public void ChangeVisibility(float distance)
        {
            _fog.meanFreePath.value = distance;
        }

        public void ApplyData(FogData data)
        {
            _fog.maxFogDistance.value = data.maxFogDistance;
            _fog.baseHeight.value = data.baseHeight;
            _fog.maximumHeight.value = data.maxHeight;
            _fog.tint.value = data.color;
            _fog.albedo.value = data.color;
            _fog.meanFreePath.value = data.attenuationDistance;
            _fog.enableVolumetricFog.value = data.isEnabled;
            _fog.enabled.value = data.isEnabled;
        }

        public void InterpolateEffect(FogData currentData, FogData targetData, float progress)
        {
            ChangeFogState(targetData.isEnabled || currentData.isEnabled);
            _fog.maxFogDistance.value = math.lerp(currentData.maxFogDistance, targetData.maxFogDistance, progress);
            _fog.baseHeight.value = math.lerp(currentData.baseHeight, targetData.baseHeight, progress);
            _fog.maximumHeight.value = math.lerp(currentData.maxHeight, targetData.maxHeight, progress);
            _fog.tint.value = Color.Lerp(currentData.color, targetData.color, progress);
            _fog.meanFreePath.value = math.lerp(currentData.attenuationDistance, targetData.attenuationDistance, progress);

            if (progress >= 1)
            {
                ApplyData(targetData);
            }
        }
        #endregion
    }

}