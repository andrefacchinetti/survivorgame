//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System;
using EasySky.Particles;
using EasySky.Utils;
using EasySky.WeatherArea;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasySky.Editor
{
    public class StandardParticleAdvancedEditor : SingletonEditorWindow<StandardParticleAdvancedEditor>
    {
        #region Private Variables
        private Toggle _standardParticleEnabled;
        private ColorField _standarParticleColor;
        private FloatField _standardParticleSize;
        private Vector3Field _standardAreaCenter;
        private Vector3Field _standardAreaSize;
        private ObjectField _standardParticleMaterial;
        private Slider _standardParticleIntensity;
        private int _selectedParticle;
        #endregion

        #region Protected Variables
        protected override string _ussPath { get; set; } = "/Editor/UI/StandardParticleAdvancedEditor.uxml";
        protected override string _windowName { get; set; } = "Standard Particle Advanced Editor";
        #endregion

        #region Public Methods
        public void InitializeForm(EasySkyWeatherManager weatherManager, WeatherPresetData presetData, int selectedParticle, Action onCloseWindowAction)
        {
            base.InitializeForm(weatherManager, presetData, onCloseWindowAction);
            _selectedParticle = selectedParticle;
            GetStandarParticleFields();
            SetStandardParticleInputData(_selectedParticle);
            RegisterStandardParticlesFields();
        }
        #endregion

        #region Private Methods
        private void GetStandarParticleFields()
        {
            _standardParticleEnabled = rootVisualElement.Q<Toggle>("StandardParticleEnabled");
            _standarParticleColor = rootVisualElement.Q<ColorField>("StandardParticleColor");
            _standardParticleSize = rootVisualElement.Q<FloatField>("StandarParticleSize");
            _standardAreaCenter = rootVisualElement.Q<Vector3Field>("StandarSpawnCenter");
            _standardAreaSize = rootVisualElement.Q<Vector3Field>("StandarSpawnSize");
            _standardParticleMaterial = rootVisualElement.Q<ObjectField>("StandarParticleMaterial");
            _standardParticleIntensity = rootVisualElement.Q<Slider>("StandardParticleIntensity");
        }

        private void SetStandardParticleInputData(int index)
        {
            switch (index)
            {
                case 0: //rain
                    SetParticleInputData(_selectedPresetData.StandarRainData);
                    break;
                case 1: //snow
                    SetParticleInputData(_selectedPresetData.StandarSnowData);
                    break;
                case 2: //hail
                    SetParticleInputData(_selectedPresetData.StandarHailData);
                    break;
                case 3: //duststorm
                    SetParticleInputData(_selectedPresetData.StandardDuststormData);
                    break;
            }
        }

        private void SetParticleInputData(StandardParticleData data)
        {
            _standardParticleEnabled.SetValueWithoutNotify(data.isActive);
            _standarParticleColor.SetValueWithoutNotify(data.particleColor);
            _standardParticleSize.SetValueWithoutNotify(data.particleSize);
            _standardAreaCenter.SetValueWithoutNotify(data.spawnBoxCenter);
            _standardAreaSize.SetValueWithoutNotify(data.spawnBoxSize);
            _standardParticleMaterial.SetValueWithoutNotify(data.particleMaterial);
            _standardParticleIntensity.SetValueWithoutNotify(data.intensity);
        }

        private void RegisterStandardParticlesFields()
        {
            _standardParticleEnabled.RegisterCallback<ChangeEvent<bool>>((evt) => SetStandardParticleData(_selectedParticle));
            _standarParticleColor.RegisterCallback<ChangeEvent<Color>>((evt) => SetStandardParticleData(_selectedParticle));
            _standardParticleSize.RegisterCallback<ChangeEvent<float>>((evt) => SetStandardParticleData(_selectedParticle));
            _standardAreaCenter.RegisterCallback<ChangeEvent<Vector3>>((evt) => SetStandardParticleData(_selectedParticle));
            _standardAreaSize.RegisterCallback<ChangeEvent<Vector3>>((evt) => SetStandardParticleData(_selectedParticle));
            _standardParticleMaterial.RegisterCallback<ChangeEvent<UnityEngine.Object>>((evt) => SetStandardParticleData(_selectedParticle));
            _standardParticleIntensity.RegisterCallback<ChangeEvent<float>>((evt) => SetStandardParticleData(_selectedParticle));
        }

        private void SetStandardParticleData(int index)
        {
            switch (index)
            {
                case 0: //rain
                    SetParticleData(_selectedPresetData.StandarRainData);
                    _weatherManager.WeatherEffectsController.RainPsController.ApplyData(_selectedPresetData.StandarRainData);
                    break;
                case 1: //snow
                    SetParticleData(_selectedPresetData.StandarSnowData);
                    _weatherManager.WeatherEffectsController.SnowPsController.ApplyData(_selectedPresetData.StandarSnowData);
                    break;
                case 2: //hail
                    SetParticleData(_selectedPresetData.StandarHailData);
                    _weatherManager.WeatherEffectsController.HailPsController.ApplyData(_selectedPresetData.StandarHailData);
                    break;
                case 3: //duststorm
                    SetParticleData(_selectedPresetData.StandardDuststormData);
                    _weatherManager.WeatherEffectsController.DuststormPsController.ApplyData(_selectedPresetData.StandardDuststormData);
                    break;
            }
            _weatherManager.FireDataUpdated();
        }

        private void SetParticleData(StandardParticleData particleData)
        {
            particleData.isActive = _standardParticleEnabled.value;
            particleData.particleColor = _standarParticleColor.value;
            particleData.particleSize = _standardParticleSize.value;
            particleData.spawnBoxCenter = _standardAreaCenter.value;
            particleData.spawnBoxSize = _standardAreaSize.value;
            particleData.particleMaterial = (Material)_standardParticleMaterial.value;
            particleData.intensity = _standardParticleIntensity.value;
        }
        #endregion
    }
}