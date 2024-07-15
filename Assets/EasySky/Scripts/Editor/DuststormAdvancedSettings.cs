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
    public class DuststormAdvancedSettings : SingletonEditorWindow<DuststormAdvancedSettings>
    {
        #region Protected Variables
        protected override string _ussPath { get; set; } = "/Editor/UI/DuststormAdvanceEditor.uxml";
        protected override string _windowName { get; set; } = "Duststorm Advanced Settings";
        #endregion

        #region Public Methods
        public override void InitializeForm(EasySkyWeatherManager weatherManager, WeatherPresetData presetData, Action onCloseWindowAction)
        {
            base.InitializeForm(weatherManager, presetData, onCloseWindowAction);

            RegisterDuststormChanges();
            SetDuststormData();
        }
        #endregion

        #region Private Methods
        private void RegisterDuststormChanges()
        {
            var duststormEnable = rootVisualElement.Q<Toggle>("DuststormEnabled");
            duststormEnable.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                _selectedPresetData.DuststormData.isActive = evt.newValue;
                _weatherManager.WeatherEffectsController.DuststormController.ApplyData(_selectedPresetData.DuststormData);
            });

            var duststormColor = rootVisualElement.Q<ColorField>("DuststormColor");
            duststormColor.RegisterCallback<ChangeEvent<Color>>((evt) =>
            {
                _selectedPresetData.DuststormData.particleColor = evt.newValue;
                _weatherManager.WeatherEffectsController.DuststormController.ApplyData(_selectedPresetData.DuststormData);
            });

            var duststormSpawnRate = rootVisualElement.Q<Slider>("DuststormSpawnrate");
            duststormSpawnRate.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.DuststormData.intensity = evt.newValue;
                _weatherManager.WeatherEffectsController.DuststormController.ApplyData(_selectedPresetData.DuststormData);
            });

            var duststormParticleSize = rootVisualElement.Q<Slider>("DuststormParticleSize");
            duststormParticleSize.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.DuststormData.particleSize = evt.newValue;
                _weatherManager.WeatherEffectsController.DuststormController.ApplyData(_selectedPresetData.DuststormData);
            });

            var duststormColorblend = rootVisualElement.Q<Slider>("DuststormColorBlend");
            duststormColorblend.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.DuststormData.colorBlend = evt.newValue;
                _weatherManager.WeatherEffectsController.DuststormController.ApplyData(_selectedPresetData.DuststormData);
            });

            var duststormSpawnSize = rootVisualElement.Q<Vector3Field>("SpawnSize");
            duststormSpawnSize.RegisterCallback<ChangeEvent<Vector3>>((evt) =>
            {
                _selectedPresetData.DuststormData.spawnBoxSize = evt.newValue;
                _weatherManager.WeatherEffectsController.DuststormController.ApplyData(_selectedPresetData.DuststormData);
            });

            var duststormSpawnCenter = rootVisualElement.Q<Vector3Field>("SpawnPosition");
            duststormSpawnCenter.RegisterCallback<ChangeEvent<Vector3>>((evt) =>
            {
                _selectedPresetData.DuststormData.spawnboxCenter = evt.newValue;
                _weatherManager.WeatherEffectsController.DuststormController.ApplyData(_selectedPresetData.DuststormData);
            });

            var duststormTexture = rootVisualElement.Q<ObjectField>("Texture");
            duststormTexture.RegisterCallback<ChangeEvent<UnityEngine.Object>>((evt) =>
            {
                _selectedPresetData.DuststormData.particleTexture = (Texture2D) evt.newValue;
                _weatherManager.WeatherEffectsController.DuststormController.ApplyData(_selectedPresetData.DuststormData);
            });

            var flipbookSize = rootVisualElement.Q<Vector2Field>("FlipbookSize");
            flipbookSize.RegisterCallback<ChangeEvent<Vector2>>((evt) =>
            {
                _selectedPresetData.DuststormData.flipBookSize = evt.newValue;
                _weatherManager.WeatherEffectsController.DuststormController.ApplyData(_selectedPresetData.DuststormData);
            });
        }

        private void SetDuststormData()
        {
            var duststormEnable = rootVisualElement.Q<Toggle>("DuststormEnabled");
            duststormEnable.SetValueWithoutNotify(_selectedPresetData.DuststormData.isActive);

            var duststormColor = rootVisualElement.Q<ColorField>("DuststormColor");
            duststormColor.SetValueWithoutNotify(_selectedPresetData.DuststormData.particleColor);

            var duststormSpawnRate = rootVisualElement.Q<Slider>("DuststormSpawnrate");
            duststormSpawnRate.SetValueWithoutNotify(_selectedPresetData.DuststormData.intensity);

            var duststormParticleSize = rootVisualElement.Q<Slider>("DuststormParticleSize");
            duststormParticleSize.SetValueWithoutNotify(_selectedPresetData.DuststormData.particleSize);

            var duststormColorblend = rootVisualElement.Q<Slider>("DuststormColorBlend");
            duststormColorblend.SetValueWithoutNotify(_selectedPresetData.DuststormData.colorBlend);

            var duststormSpawnSize = rootVisualElement.Q<Vector3Field>("SpawnSize");
            duststormSpawnSize.SetValueWithoutNotify(_selectedPresetData.DuststormData.spawnBoxSize);

            var duststormSpawnCenter = rootVisualElement.Q<Vector3Field>("SpawnPosition");
            duststormSpawnCenter.SetValueWithoutNotify(_selectedPresetData.DuststormData.spawnboxCenter);

            var duststormTexture = rootVisualElement.Q<ObjectField>("Texture");
            duststormTexture.SetValueWithoutNotify(_selectedPresetData.DuststormData.particleTexture);

            var flipbookSize = rootVisualElement.Q<Vector2Field>("FlipbookSize");
            flipbookSize.SetValueWithoutNotify(_selectedPresetData.DuststormData.flipBookSize);
        }
        #endregion
    }
}