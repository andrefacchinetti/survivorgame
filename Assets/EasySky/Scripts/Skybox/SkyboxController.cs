//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace EasySky.Skybox
{
    public class SkyboxController : MonoBehaviour
    {
        #region Constants
        private const int DEFAULT_EMISSION_MULTIPLIER = 2;
        #endregion

        #region Private Variables
        [SerializeField] private Volume _volume;
        [SerializeField] private SkyboxData _skyboxData;

        private PhysicallyBasedSky _sky;
        #endregion

        #region Properties
        public SkyboxData SkyboxData { get => _skyboxData; }
        #endregion

        #region Unity Methods
        private void Start()
        {
            SetupSky();
        }

        private void Update()
        {
            var time = EasySkyWeatherManager.Instance.GlobalData.globalTime;
            var dayTime = time.Second + time.Minute * 60 + time.Hour * 3600;

            _sky.spaceEmissionMultiplier.value = DEFAULT_EMISSION_MULTIPLIER + DEFAULT_EMISSION_MULTIPLIER * _skyboxData.data[_skyboxData.selectedData].animationCurve.Evaluate(dayTime / 86400f);
        }
        #endregion

        #region Public Methods
        public void SetupSky()
        {
            _volume.sharedProfile.TryGet(out _sky);
        }

        public void SetSky(int index)
        {
            _sky.spaceEmissionTexture.value = _skyboxData.data[index].cubemap;
        }
        #endregion
    }
}