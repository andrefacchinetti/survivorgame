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
    public class RainAdvancedSettings : SingletonEditorWindow<RainAdvancedSettings>
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
        private Toggle _cameraEffect;
        private Slider _effectIntensity;
        private FloatField _effectDripTiling;
        private FloatField _effectDripSize;
        private Slider _effectSpeed;
        private Slider _ripplesCoverage;
        private FloatField _ripplesTiling;
        private Slider _ripplesSpeed;
        private Slider _ripplesScale;
        private Slider _ripplesNormalIntensity;
        private Toggle _lightning;
        private Slider _lightningFreq;
        #endregion

        #region Protected Variables
        protected override string _ussPath { get; set; } = "/Editor/UI/RainAdvanceEditor.uxml";
        protected override string _windowName { get; set; } = "Rain Advanced Settings";
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
            _cameraEffect = rootVisualElement.Q<Toggle>("FullscreenEffect");
            _effectIntensity = rootVisualElement.Q<Slider>("EffectIntensity");
            _effectDripTiling = rootVisualElement.Q<FloatField>("EffectDropsTiling");
            _effectDripSize = rootVisualElement.Q<FloatField>("EffectDropsSize");
            _ripplesCoverage = rootVisualElement.Q<Slider>("RipplesCoverage");
            _ripplesTiling = rootVisualElement.Q<FloatField>("RipplesTiling");
            _ripplesSpeed = rootVisualElement.Q<Slider>("RipplesSpeed");
            _ripplesScale = rootVisualElement.Q<Slider>("RippleScale");
            _ripplesNormalIntensity = rootVisualElement.Q<Slider>("RipplesNormalIntensity");
            _effectSpeed = rootVisualElement.Q<Slider>("EffectSpeed");
            _lightning = rootVisualElement.Q<Toggle>("LightnEnabled");
            _lightningFreq = rootVisualElement.Q<Slider>("LightnFreq");
        }

        private void SetParticleInputsData()
        {
            _intensity.SetValueWithoutNotify(_selectedPresetData.RainData.intensity);
            _areaCenter.SetValueWithoutNotify(_selectedPresetData.RainData.spawnboxCenter);
            _areaSize.SetValueWithoutNotify(_selectedPresetData.RainData.spawnBoxSize);
            _minParticleSize.SetValueWithoutNotify(_selectedPresetData.RainData.minParticleSize);
            _maxParticleSize.SetValueWithoutNotify(_selectedPresetData.RainData.maxParticleSize);
            _particleTexture.SetValueWithoutNotify(_selectedPresetData.RainData.particleTexture);
            _particleColor.SetValueWithoutNotify(_selectedPresetData.RainData.particleColor);
            _particleColorBlend.SetValueWithoutNotify(_selectedPresetData.RainData.colorBlend);
            _particleEnabled.SetValueWithoutNotify(_selectedPresetData.RainData.isActive);
            _cameraEffect.SetValueWithoutNotify(_selectedPresetData.RainData.isFullscreenEffectEnabled);
            _effectIntensity.SetValueWithoutNotify(_selectedPresetData.RainData.fullscreenEffectIntensity);
            _effectDripTiling.SetValueWithoutNotify(_selectedPresetData.RainData.fulscreenEffectDropTiling);
            _effectDripSize.SetValueWithoutNotify(_selectedPresetData.RainData.fullscreenEffectDripSize);
            _ripplesCoverage.SetValueWithoutNotify(_selectedPresetData.RainData.ripplesCoverage);
            _ripplesTiling.SetValueWithoutNotify(_selectedPresetData.RainData.ripplesTiling);
            _ripplesSpeed.SetValueWithoutNotify(_selectedPresetData.RainData.ripplesSpeed);
            _ripplesScale.SetValueWithoutNotify(_selectedPresetData.RainData.ripplesScale);
            _ripplesNormalIntensity.SetValueWithoutNotify(_selectedPresetData.RainData.normalEvaluationDistance);
            _effectSpeed.SetValueWithoutNotify(_selectedPresetData.RainData.fullscreenEffectSpeed);
            _lightning.SetValueWithoutNotify(_selectedPresetData.RainData.areLightningsEnabled);
            _lightningFreq.SetValueWithoutNotify(_selectedPresetData.RainData.lightningsFrequency);
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

            var rainRipplesToggle = rootVisualElement.Q<Toggle>("RainRipples");
            rainRipplesToggle.value = _selectedPresetData.RainData.areRipplesActive;
            rainRipplesToggle.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                _selectedPresetData.RainData.areRipplesActive = evt.newValue;
                _weatherManager.WeatherEffectsController.RainController.ApplyData(_selectedPresetData.RainData);
            });

            var rainNormalEvalDist = rootVisualElement.Q<Slider>("RipplesNormalIntensity");
            rainNormalEvalDist.value = _selectedPresetData.RainData.normalEvaluationDistance;
            rainNormalEvalDist.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.RainData.normalEvaluationDistance = evt.newValue;
                _weatherManager.WeatherEffectsController.RainController.ApplyData(_selectedPresetData.RainData);
            });

            var ripplesLayer = rootVisualElement.Q<LayerMaskField>("Layer");
            ripplesLayer.value = _selectedPresetData.RainData.layers;
            ripplesLayer.RegisterCallback<ChangeEvent<int>>((evt) =>
            {
                _selectedPresetData.RainData.layers = evt.newValue;
                _weatherManager.WeatherEffectsController.RainController.ApplyData(_selectedPresetData.RainData);
            });

            var rainLifetime = rootVisualElement.Q<FloatField>("Lifetime");
            rainLifetime.value = _selectedPresetData.RainData.particleLifetime;
            rainLifetime.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.RainData.particleLifetime = evt.newValue;
                _weatherManager.WeatherEffectsController.RainController.ApplyData(_selectedPresetData.RainData);
            });

            var gravity = rootVisualElement.Q<Slider>("Gravity");
            gravity.value = _selectedPresetData.RainData.gravity;
            gravity.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.RainData.gravity = evt.newValue;
                _weatherManager.WeatherEffectsController.RainController.ApplyData(_selectedPresetData.RainData);
            });

            var lifetime = rootVisualElement.Q<FloatField>("Lifetime");
            lifetime.value = _selectedPresetData.RainData.lifetime;
            lifetime.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.RainData.lifetime = evt.newValue;
                _weatherManager.WeatherEffectsController.RainController.ApplyData(_selectedPresetData.RainData);
            });

            _cameraEffect.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                _selectedPresetData.RainData.isFullscreenEffectEnabled = evt.newValue;
                _weatherManager.FullscreenEffectController.ChangeRainEffectState(evt.newValue);
            });

            _effectIntensity.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.RainData.fullscreenEffectIntensity = evt.newValue;
                _weatherManager.FullscreenEffectController.ApplyDataToRain(_selectedPresetData.RainData);
            });

            _effectDripSize.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.RainData.fullscreenEffectDripSize = evt.newValue;
                _weatherManager.FullscreenEffectController.ApplyDataToRain(_selectedPresetData.RainData);
            });

            _effectDripTiling.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.RainData.fulscreenEffectDropTiling = evt.newValue;
                _weatherManager.FullscreenEffectController.ApplyDataToRain(_selectedPresetData.RainData);
            });

            _ripplesCoverage.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.RainData.ripplesCoverage = evt.newValue;
                _weatherManager.WeatherEffectsController.RainController.ApplyData(_selectedPresetData.RainData);
            });

            _ripplesTiling.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.RainData.ripplesTiling = evt.newValue;
                _weatherManager.WeatherEffectsController.RainController.ApplyData(_selectedPresetData.RainData);
            });

            _ripplesSpeed.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.RainData.ripplesSpeed = evt.newValue;
                _weatherManager.WeatherEffectsController.RainController.ApplyData(_selectedPresetData.RainData);
            });

            _ripplesScale.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.RainData.ripplesScale = evt.newValue;
                _weatherManager.WeatherEffectsController.RainController.ApplyData(_selectedPresetData.RainData);
            });

            _ripplesNormalIntensity.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.RainData.ripplesNormalIntensity = evt.newValue;
                _weatherManager.WeatherEffectsController.RainController.ApplyData(_selectedPresetData.RainData);
            });

            _effectSpeed.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.RainData.fullscreenEffectSpeed = evt.newValue;
                _weatherManager.FullscreenEffectController.ApplyDataToRain(_selectedPresetData.RainData);
            });

            _lightning.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                _selectedPresetData.RainData.areLightningsEnabled = evt.newValue;
                _weatherManager.WeatherEffectsController.LightningsController.ApplyData(_selectedPresetData.RainData.areLightningsEnabled, _selectedPresetData.RainData.lightningsFrequency);
            });

            _lightningFreq.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.RainData.lightningsFrequency = evt.newValue;
                _weatherManager.WeatherEffectsController.LightningsController.ApplyData(_selectedPresetData.RainData.areLightningsEnabled, _selectedPresetData.RainData.lightningsFrequency);
            });
        }

        private void SetParticleData()
        {
            _selectedPresetData.RainData.isActive = _particleEnabled.value;
            _selectedPresetData.RainData.intensity = _intensity.value;
            _selectedPresetData.RainData.spawnboxCenter = _areaCenter.value;
            _selectedPresetData.RainData.spawnBoxSize = _areaSize.value;
            _selectedPresetData.RainData.minParticleSize = _minParticleSize.value;
            _selectedPresetData.RainData.maxParticleSize = _maxParticleSize.value;
            _selectedPresetData.RainData.particleTexture = (Texture2D)_particleTexture.value;
            _selectedPresetData.RainData.particleColor = _particleColor.value;
            _selectedPresetData.RainData.colorBlend = _particleColorBlend.value;
            _weatherManager.WeatherEffectsController.RainController.ApplyData(_selectedPresetData.RainData);
            _weatherManager.WeatherEffectsController.LightningsController.ApplyData(_selectedPresetData.RainData.areLightningsEnabled, _selectedPresetData.RainData.lightningsFrequency);
            _weatherManager.FireDataUpdated();
        }
        #endregion
    }
}