//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace EasySky.Skybox
{
    /// <summary>
    /// Controls a directional light
    /// </summary>
    public class CelestialObjectController : MonoBehaviour
    {
        #region Private Pariables
        [SerializeField] private HDAdditionalLightData _directionalLight;
        #endregion

        #region Public Methods
        public void SetData(CelestialObjectPresetData starsPresetData)
        {
            _directionalLight.color = starsPresetData.lightColor;
            _directionalLight.angularDiameter = starsPresetData.scale;
            _directionalLight.surfaceTexture = starsPresetData.starTexture;
            _directionalLight.flareTint = starsPresetData.flareTint;
            _directionalLight.surfaceTint = starsPresetData.surfaceTint;
            _directionalLight.SetColor(starsPresetData.lightColor, starsPresetData.lightTemperature);
            _directionalLight.flareSize = starsPresetData.flareSize;
            _directionalLight.flareFalloff = starsPresetData.flareFalloff;
            _directionalLight.EnableShadows(starsPresetData.castShadows);
            _directionalLight.shadowResolution.level = (int) starsPresetData.resolution;
            _directionalLight.shadowDimmer = starsPresetData.shadowDimmer;
        }

        public void SetLightIntensity(float intensity)
        {
            _directionalLight.intensity = intensity;
        }
        #endregion
    }
}