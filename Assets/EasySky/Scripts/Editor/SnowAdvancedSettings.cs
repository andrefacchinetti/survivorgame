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
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasySky.Editor
{
    public class SnowAdvancedSettings : SingletonEditorWindow<SnowAdvancedSettings>
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
        private Toggle _noiseSnowEnabled;
        private Toggle _textureSnowEnabled;
        #endregion

        #region Protected Variables
        protected override string _ussPath { get; set; } = "/Editor/UI/SnowAdvanceEditor.uxml";
        protected override string _windowName { get; set; } = "Snow Advanced Settings";
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
            _particleEnabled.SetValueWithoutNotify(_selectedPresetData.SnowData.isActive);
            _intensity.SetValueWithoutNotify(_selectedPresetData.SnowData.intensity);
            _areaCenter.SetValueWithoutNotify(_selectedPresetData.SnowData.spawnboxCenter);
            _areaSize.SetValueWithoutNotify(_selectedPresetData.SnowData.spawnBoxSize);
            _minParticleSize.SetValueWithoutNotify(_selectedPresetData.SnowData.minParticleSize);
            _maxParticleSize.SetValueWithoutNotify(_selectedPresetData.SnowData.maxParticleSize);
            _particleTexture.SetValueWithoutNotify(_selectedPresetData.SnowData.particleTexture);
            _particleColor.SetValueWithoutNotify(_selectedPresetData.SnowData.particleColor);
            _particleColorBlend.SetValueWithoutNotify(_selectedPresetData.SnowData.colorBlend);
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

            var snowFlipbookSize = rootVisualElement.Q<Vector2Field>("SnowFlipBookSize");
            snowFlipbookSize.value = _selectedPresetData.SnowData.flipBookSize;
            snowFlipbookSize.RegisterCallback<ChangeEvent<Vector2>>((evt) =>
            {
                _selectedPresetData.SnowData.flipBookSize = evt.newValue;
                _weatherManager.WeatherEffectsController.SnowController.ApplyData(_selectedPresetData.SnowData);
            });

            var snowGravity = rootVisualElement.Q<Slider>("SnowGravity");
            snowGravity.value = _selectedPresetData.SnowData.gravity;
            snowGravity.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.SnowData.gravity = evt.newValue;
                _weatherManager.WeatherEffectsController.SnowController.ApplyData(_selectedPresetData.SnowData);
            });

            var snowTurbulence = rootVisualElement.Q<SliderInt>("SnowTurbulence");
            snowTurbulence.value = _selectedPresetData.SnowData.turbulence;
            snowTurbulence.RegisterCallback<ChangeEvent<int>>((evt) =>
            {
                _selectedPresetData.SnowData.turbulence = evt.newValue;
                _weatherManager.WeatherEffectsController.SnowController.ApplyData(_selectedPresetData.SnowData);
            });

            var fullscreenToggle = rootVisualElement.Q<Toggle>("FullscreenEffect");
            fullscreenToggle.value = _selectedPresetData.SnowData.isFullscreenEffectEnabled;
            fullscreenToggle.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                _selectedPresetData.SnowData.isFullscreenEffectEnabled = evt.newValue;
                _weatherManager.FullscreenEffectController.ChangeIceEffectState(evt.newValue);
                _weatherManager.FireDataUpdated();
            });

            var effectIntensity = rootVisualElement.Q<Slider>("EffectIntensity");
            effectIntensity.value = _selectedPresetData.SnowData.fullscreenEffectIntensity;
            effectIntensity.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.SnowData.fullscreenEffectIntensity = evt.newValue;
                _weatherManager.FullscreenEffectController.ApplyDataToSnow(_selectedPresetData.SnowData);
            });

            var effectNoiseScale = rootVisualElement.Q<FloatField>("EffectNoiseScale");
            effectNoiseScale.value = _selectedPresetData.SnowData.fulscreenEffectNoiseScale;
            effectNoiseScale.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.SnowData.fulscreenEffectNoiseScale = evt.newValue;
                _weatherManager.FullscreenEffectController.ApplyDataToSnow(_selectedPresetData.SnowData);
            });

            var effectCoverage = rootVisualElement.Q<Slider>("EffectCoverage");
            effectCoverage.value = _selectedPresetData.SnowData.fullscreenEffectCoverage;
            effectCoverage.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.SnowData.fullscreenEffectCoverage = evt.newValue;
                _weatherManager.FullscreenEffectController.ApplyDataToSnow(_selectedPresetData.SnowData);
            });

            var effectNoiseIntensity = rootVisualElement.Q<Slider>("EffectNoiseIntensity");
            effectNoiseIntensity.value = _selectedPresetData.SnowData.fullscreenEffectNoiseIntensity;
            effectNoiseIntensity.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.SnowData.fullscreenEffectNoiseIntensity = evt.newValue;
                _weatherManager.FullscreenEffectController.ApplyDataToSnow(_selectedPresetData.SnowData);
            });

            var effectColor = rootVisualElement.Q<ColorField>("EffectColor");
            effectColor.value = _selectedPresetData.SnowData.fulscreenEffectColor;
            effectColor.RegisterCallback<ChangeEvent<Color>>((evt) =>
            {
                _selectedPresetData.SnowData.fulscreenEffectColor = evt.newValue;
                _weatherManager.FullscreenEffectController.ApplyDataToSnow(_selectedPresetData.SnowData);
            });


            _noiseSnowEnabled = rootVisualElement.Q<Toggle>("NoiseSnowToggle");
            _textureSnowEnabled = rootVisualElement.Q<Toggle>("TextureSnowToggle");
            _noiseSnowEnabled.value = _selectedPresetData.SnowData.isNoiseSnowActive;
            _textureSnowEnabled.value = _selectedPresetData.SnowData.isTextureSnowActive;

            _noiseSnowEnabled.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                _selectedPresetData.SnowData.isNoiseSnowActive = evt.newValue;
                _selectedPresetData.SnowData.isTextureSnowActive = false;
                _textureSnowEnabled.SetValueWithoutNotify(false);
                _weatherManager.FullscreenEffectController.ChangeSnowNoiseState(evt.newValue);
                _weatherManager.FullscreenEffectController.ChangeSnowTriplanarState(false);
            });

            _textureSnowEnabled.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                _selectedPresetData.SnowData.isTextureSnowActive = evt.newValue;
                _selectedPresetData.SnowData.isNoiseSnowActive = false;
                _noiseSnowEnabled.SetValueWithoutNotify(false);
                _weatherManager.FullscreenEffectController.ChangeSnowNoiseState(false);
                _weatherManager.FullscreenEffectController.ChangeSnowTriplanarState(evt.newValue);
            });

            var snowAmmont = rootVisualElement.Q<Slider>("SnowAmmount");
            snowAmmont.value = _selectedPresetData.SnowData.snowAmmount;
            snowAmmont.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.SnowData.snowAmmount = evt.newValue;
                _weatherManager.FullscreenEffectController.ApplyDataToSnowLayer(_selectedPresetData.SnowData);
            });

            var snowPatchScale = rootVisualElement.Q<FloatField>("SnowPatchScale");
            snowPatchScale.value = _selectedPresetData.SnowData.patchScale;
            snowPatchScale.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.SnowData.patchScale = evt.newValue;
                _weatherManager.FullscreenEffectController.ApplyDataToSnowLayer(_selectedPresetData.SnowData);
            });

            var snowColor = rootVisualElement.Q<ColorField>("SnowColor");
            snowColor.value = _selectedPresetData.SnowData.snowColor;
            snowColor.RegisterCallback<ChangeEvent<Color>>((evt) =>
            {
                _selectedPresetData.SnowData.snowColor = evt.newValue;
                _weatherManager.FullscreenEffectController.ApplyDataToSnowLayer(_selectedPresetData.SnowData);
            });

            var snowNormalStrength = rootVisualElement.Q<Slider>("SnowNormalStrength");
            snowNormalStrength.value = _selectedPresetData.SnowData.snowNormalStrength;
            snowNormalStrength.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.SnowData.snowNormalStrength = evt.newValue;
                _weatherManager.FullscreenEffectController.ApplyDataToSnowLayer(_selectedPresetData.SnowData);
            });

            var snowBlendDistance = rootVisualElement.Q<Slider>("SnowBlendDistance");
            snowBlendDistance.value = _selectedPresetData.SnowData.snowBlendDistance;
            snowBlendDistance.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.SnowData.snowBlendDistance = evt.newValue;
                _weatherManager.FullscreenEffectController.ApplyDataToSnowLayer(_selectedPresetData.SnowData);
            });

            var snowContrast = rootVisualElement.Q<Slider>("SnowContrast");
            snowContrast.value = _selectedPresetData.SnowData.snowContrast;
            snowContrast.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.SnowData.snowContrast = evt.newValue;
                _weatherManager.FullscreenEffectController.ApplyDataToSnowLayer(_selectedPresetData.SnowData);
            });

            var snowSpecularShine = rootVisualElement.Q<Slider>("SpecularShine");
            snowSpecularShine.value = _selectedPresetData.SnowData.snowSpecularShine;
            snowSpecularShine.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.SnowData.snowSpecularShine = evt.newValue;
                _weatherManager.FullscreenEffectController.ApplyDataToSnowLayer( _selectedPresetData.SnowData);
            });

            var snowLayer = rootVisualElement.Q<LayerMaskField>("Layer");
            snowLayer.value = _selectedPresetData.SnowData.layers;
            snowLayer.RegisterCallback<ChangeEvent<int>>((evt) =>
            {
                _selectedPresetData.SnowData.layers = evt.newValue;
                _weatherManager.FullscreenEffectController.ApplyDataToSnowLayer(_selectedPresetData.SnowData);
            });

            var lifetime = rootVisualElement.Q<FloatField>("Lifetime");
            lifetime.value = _selectedPresetData.SnowData.lifetime;
            lifetime.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.SnowData.lifetime = evt.newValue;
                _weatherManager.WeatherEffectsController.SnowController.ApplyData(_selectedPresetData.SnowData);
            });
        }

        private void SetParticleData()
        {
            _selectedPresetData.SnowData.isActive = _particleEnabled.value;
            _selectedPresetData.SnowData.intensity = _intensity.value;
            _selectedPresetData.SnowData.spawnboxCenter = _areaCenter.value;
            _selectedPresetData.SnowData.spawnBoxSize = _areaSize.value;
            _selectedPresetData.SnowData.minParticleSize = _minParticleSize.value;
            _selectedPresetData.SnowData.maxParticleSize = _maxParticleSize.value;
            _selectedPresetData.SnowData.particleTexture = (Texture2D)_particleTexture.value;
            _selectedPresetData.SnowData.particleColor = _particleColor.value;
            _selectedPresetData.SnowData.colorBlend = _particleColorBlend.value;
            _weatherManager.WeatherEffectsController.SnowController.ApplyData(_selectedPresetData.SnowData);
            _weatherManager.FireDataUpdated();
            EditorUtility.SetDirty(_selectedPresetData);
        }
        #endregion
    }
}