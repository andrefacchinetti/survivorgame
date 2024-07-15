//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System.Threading;
using System.Threading.Tasks;
using EasySky.WeatherArea;
using UnityEngine;

namespace EasySky.Particles
{
    /// <summary>
    /// Controls all weather effects and the interpolation between weather presets
    /// </summary>
    public class WeatherEffectsController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private RainController _rainController;
        [SerializeField] private SnowController _snowController;
        [SerializeField] private HailController _hailController;
        [SerializeField] private RainPsController _rainPsController;
        [SerializeField] private DuststormController _duststormController;
        [SerializeField] private SnowPsController _snowPsController;
        [SerializeField] private HailPsController _hailPsController;
        [SerializeField] private DuststormPsController _duststormPsController;
        [SerializeField] private FogController _fogController;
        [SerializeField] private LightningsController _lightningsController;

        private Camera _camera;
        private WeatherPresetData _currentData;
        #endregion

        #region Properties
        public RainController RainController { get => _rainController; }
        public SnowController SnowController { get => _snowController; }
        public HailController HailController { get => _hailController; }
        public DuststormController DuststormController { get => _duststormController; }
        public RainPsController RainPsController { get => _rainPsController; }
        public SnowPsController SnowPsController { get => _snowPsController; }
        public HailPsController HailPsController { get => _hailPsController; }
        public DuststormPsController DuststormPsController { get => _duststormPsController; }
        public FogController FogController { get => _fogController; }
        public LightningsController LightningsController { get => _lightningsController; set => _lightningsController = value; }
        #endregion

        #region Unity Methods
        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (_camera != null)
            {
                transform.position = _camera.transform.position;
            }
        }
        #endregion

        #region Public Methods
        public void ApplyData(WeatherAreaData data)
        {
            _currentData = data.presetData;
            RainController.ApplyData(data.presetData.RainData);
            SnowController.ApplyData(data.presetData.SnowData);
            HailController.ApplyData(data.presetData.HailData);
            DuststormController.ApplyData(data.presetData.DuststormData);
            FogController.ApplyData(data.presetData.FogData);
            RainPsController.ApplyData(data.presetData.StandarRainData);
            SnowPsController.ApplyData(data.presetData.StandarSnowData);
            HailPsController.ApplyData(data.presetData.StandarHailData);
            DuststormPsController.ApplyData(data.presetData.StandardDuststormData);
            LightningsController.ApplyData(data.presetData.RainData.areLightningsEnabled, data.presetData.RainData.lightningsFrequency);
        }

        public async void InterpolateEffects(WeatherPresetData data, CancellationToken cancellationToken)
        {
            if (_currentData == null) return;

            var x = 0f;
            while (x <= 1)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    SetInterpolateData(_currentData, data, 1);
                    _currentData = data;
                    return;
                }

                x += GlobalConstantData.LerpIncrement;
                SetInterpolateData(_currentData, data, x);
                await Task.Delay(GlobalConstantData.LerpSpeed);
            }

            _currentData = data;
        }

        public void SetInterpolateData(WeatherPresetData curentData, WeatherPresetData targetData, float ammount)
        {
            RainController.InterpolateEffect(curentData.RainData, targetData.RainData, ammount);
            SnowController.InterpolateEffect(curentData.SnowData, targetData.SnowData, ammount);
            HailController.InterpolateEffect(curentData.HailData, targetData.HailData, ammount);
            DuststormController.InterpolateEffect(curentData.DuststormData, targetData.DuststormData, ammount);
            FogController.InterpolateEffect(curentData.FogData, targetData.FogData, ammount);
            RainPsController.InterpolateEffect(curentData.StandarRainData, targetData.StandarRainData, ammount);
            SnowPsController.InterpolateEffect(curentData.StandarSnowData, targetData.StandarSnowData, ammount);
            HailPsController.InterpolateEffect(curentData.StandarHailData, targetData.StandarHailData, ammount);
            DuststormPsController.InterpolateEffect(curentData.StandardDuststormData, targetData.StandardDuststormData, ammount);
            LightningsController.ApplyData(targetData.RainData.areLightningsEnabled, targetData.RainData.lightningsFrequency);
        }
        #endregion
    }
}