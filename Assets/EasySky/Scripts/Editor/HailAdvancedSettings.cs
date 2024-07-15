//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System;
using EasySky.Utils;
using EasySky.WeatherArea;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasySky.Editor
{
    public class HailAdvancedSettings : SingletonEditorWindow<HailAdvancedSettings>
    {
        #region Private Variables
        private Slider _intensity;
        private Vector3Field _areaCenter;
        private Vector3Field _areaSize;
        private Slider _minParticleSize;
        private Slider _maxParticleSize;
        private ObjectField _particleTexture;
        private ColorField _particleColor;
        private Slider _particleColorBlend;
        private Toggle _particleEnabled;
        #endregion

        #region Protected Variables
        protected override string _ussPath { get; set; } = "/Editor/UI/HailAdvanceEditor.uxml";
        protected override string _windowName { get; set; } = "Hail Advanced Settings";
        #endregion

        #region Public Methods
        public override void InitializeForm(EasySkyWeatherManager weatherManager, WeatherPresetData presetData, Action onCloseWindowAction)
        {
            base.InitializeForm(weatherManager, presetData, onCloseWindowAction);
            GetParticleFields();
            SetParticleInputsData();
            RegisterParticleChanges();
        }
        #endregion

        #region Private Methods
        private void GetParticleFields()
        {
            _particleEnabled = rootVisualElement.Q<Toggle>("EnabledParticle");
            _intensity = rootVisualElement.Q<Slider>("ParticleIntensity");
            _areaCenter = rootVisualElement.Q<Vector3Field>("ParticleCenter");
            _areaSize = rootVisualElement.Q<Vector3Field>("ParticleSize");
            _minParticleSize = rootVisualElement.Q<Slider>("MinParticleSize");
            _maxParticleSize = rootVisualElement.Q<Slider>("MaxParticleSize");
            _particleTexture = rootVisualElement.Q<ObjectField>("ParticleTexture");
            _particleColor = rootVisualElement.Q<ColorField>("ParticleColor");
            _particleColorBlend = rootVisualElement.Q<Slider>("ColorBlend");
        }

        private void SetParticleInputsData()
        {
            _particleEnabled.SetValueWithoutNotify(_selectedPresetData.HailData.isActive);
            _intensity.SetValueWithoutNotify(_selectedPresetData.HailData.intensity);
            _areaCenter.SetValueWithoutNotify(_selectedPresetData.HailData.spawnboxCenter);
            _areaSize.SetValueWithoutNotify(_selectedPresetData.HailData.spawnBoxSize);
            _minParticleSize.SetValueWithoutNotify(_selectedPresetData.HailData.minParticleSize);
            _maxParticleSize.SetValueWithoutNotify(_selectedPresetData.HailData.maxParticleSize);
            _particleTexture.SetValueWithoutNotify(_selectedPresetData.HailData.particleTexture);
            _particleColor.SetValueWithoutNotify(_selectedPresetData.HailData.particleColor);
            _particleColorBlend.SetValueWithoutNotify(_selectedPresetData.HailData.colorBlend);
        }
        private void RegisterParticleChanges()
        {
            _particleEnabled.RegisterCallback<ChangeEvent<bool>>((evt) => SetParticleData());
            _intensity.RegisterCallback<ChangeEvent<float>>((evt) => SetParticleData());
            _areaCenter.RegisterCallback<ChangeEvent<Vector3>>((evt) => SetParticleData());
            _areaSize.RegisterCallback<ChangeEvent<Vector3>>((evt) => SetParticleData());
            _minParticleSize.RegisterCallback<ChangeEvent<float>>((evt) => SetParticleData());
            _maxParticleSize.RegisterCallback<ChangeEvent<float>>((evt) => SetParticleData());
            _particleTexture.RegisterCallback<ChangeEvent<UnityEngine.Object>>((evt) => SetParticleData());
            _particleColor.RegisterCallback<ChangeEvent<Color>>((evt) => SetParticleData());
            _particleColorBlend.RegisterCallback<ChangeEvent<float>>((evt) => SetParticleData());

            var hailFlipbookSize = rootVisualElement.Q<Vector2Field>("HailFlipBookSize");
            hailFlipbookSize.value = _selectedPresetData.HailData.flipBookSize;
            hailFlipbookSize.RegisterCallback<ChangeEvent<Vector2>>((evt) =>
            {
                _selectedPresetData.HailData.flipBookSize = evt.newValue;
                _weatherManager.WeatherEffectsController.HailController.ApplyData(_selectedPresetData.HailData);
            });

            var hailSecondaryQuantity = rootVisualElement.Q<Slider>("SecondaryHailQantity");
            hailSecondaryQuantity.value = _selectedPresetData.HailData.intensity;
            hailSecondaryQuantity.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.HailData.secondaryParticleQuantity = evt.newValue;
                _weatherManager.WeatherEffectsController.HailController.ApplyData(_selectedPresetData.HailData);
            });

            var hailSecondaryParticleSize = rootVisualElement.Q<Slider>("SecondaryHailParticleSize");
            hailSecondaryParticleSize.value = _selectedPresetData.HailData.secondaryParticleSize;
            hailSecondaryParticleSize.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.HailData.secondaryParticleSize = evt.newValue;
                _weatherManager.WeatherEffectsController.HailController.ApplyData(_selectedPresetData.HailData);
            });

            var secondaryHailTexture = rootVisualElement.Q<ObjectField>("SecondaryHailTexture");
            secondaryHailTexture.value = _selectedPresetData.HailData.secondaryParticleTexture;
            secondaryHailTexture.RegisterCallback<ChangeEvent<UnityEngine.Object>>((evt) =>
            {
                if (evt.newValue == null) 
                { 
                    return; 
                }

                _selectedPresetData.HailData.secondaryParticleTexture = (Texture2D)evt.newValue;
                _weatherManager.WeatherEffectsController.HailController.ApplyData(_selectedPresetData.HailData);
            });

            var hailSpeed = rootVisualElement.Q<Slider>("HailSpeed");
            hailSpeed.value = _selectedPresetData.HailData.hailSpeed;
            hailSpeed.RegisterCallback<ChangeEvent<float>>((evt) =>
            { 
                _selectedPresetData.HailData.hailSpeed = evt.newValue;
                _weatherManager.WeatherEffectsController.HailController.ApplyData(_selectedPresetData.HailData);
            });

            var secondaryFlipBook = rootVisualElement.Q<Vector2Field>("SecondaryHailFlipBookSize");
            secondaryFlipBook.value = _selectedPresetData.HailData.secondaryFlipBookSize;
            secondaryFlipBook.RegisterCallback<ChangeEvent<Vector2>>((evt) => 
            {
                _selectedPresetData.HailData.secondaryFlipBookSize = evt.newValue;
                _weatherManager.WeatherEffectsController.HailController.ApplyData(_selectedPresetData.HailData);

            });

            var lifetime = rootVisualElement.Q<FloatField>("Lifetime");
            lifetime.value = _selectedPresetData.HailData.lifetime;
            lifetime.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.HailData.lifetime = evt.newValue;
                _weatherManager.WeatherEffectsController.HailController.ApplyData(_selectedPresetData.HailData);
            });
        }

        private void SetParticleData()
        {
            _selectedPresetData.HailData.isActive = _particleEnabled.value;
            _selectedPresetData.HailData.intensity = _intensity.value;
            _selectedPresetData.HailData.spawnboxCenter = _areaCenter.value;
            _selectedPresetData.HailData.spawnBoxSize = _areaSize.value;
            _selectedPresetData.HailData.minParticleSize = _minParticleSize.value;
            _selectedPresetData.HailData.maxParticleSize = _maxParticleSize.value;
            _selectedPresetData.HailData.particleTexture = (Texture2D)_particleTexture.value;
            _selectedPresetData.HailData.particleColor = _particleColor.value;
            _selectedPresetData.HailData.colorBlend = _particleColorBlend.value;
            _weatherManager.WeatherEffectsController.HailController.ApplyData(_selectedPresetData.HailData);
            _weatherManager.FireDataUpdated();
        }
        #endregion
    }
}