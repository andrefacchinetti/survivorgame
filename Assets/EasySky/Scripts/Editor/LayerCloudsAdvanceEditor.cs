//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System;
using EasySky.Clouds;
using EasySky.Utils;
using EasySky.WeatherArea;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasySky.Editor
{
    public class LayerCloudsAdvanceEditor : SingletonEditorWindow<LayerCloudsAdvanceEditor>
    {
        #region Private Variables
        private ObjectField _2dCloudPreset;
        private Slider _2dCoverage;
        private Slider _2dOpacityLayer1;
        private Slider _2dOpacityLayer2;
        private Slider _2dOpacityLayer3;
        private Slider _2dOpacityLayer4;
        private ColorField _2dColor;
        private Toggle _2dShadows;
        private Toggle _2dWindInteraction;
        private Slider _2dAltitude;
        private FloatField _2dExposure;
        private Slider _2dRotation;
        #endregion

        #region Protected Variables
        protected override string _ussPath { get; set; } = "/Editor/UI/LayerCloudsAdvanceEditor.uxml";
        protected override string _windowName { get; set; } = "2D Clouds Advance Editor";
        #endregion

        #region Public Methods
        public override void InitializeForm(EasySkyWeatherManager weatherManager, WeatherPresetData presetData, Action onCloseWindowAction)
        {
            base.InitializeForm(weatherManager, presetData, onCloseWindowAction);
            PopulateCloudPresetInput();
            SetCloudInputData();
            RegisterCloudInput();
        }
        #endregion

        #region Private Methods
        private void PopulateCloudPresetInput()
        {
            _2dCloudPreset = rootVisualElement.Q<ObjectField>("2DCloudPreset");
            _2dCoverage = rootVisualElement.Q<Slider>("2DCoverage");
            _2dColor = rootVisualElement.Q<ColorField>("2DColor");
            _2dShadows = rootVisualElement.Q<Toggle>("2DCastShadows");
            _2dWindInteraction = rootVisualElement.Q<Toggle>("2DWindInteraction");
            _2dAltitude = rootVisualElement.Q<Slider>("2DAltitude");
            _2dExposure = rootVisualElement.Q<FloatField>("2DExposureComp");
            _2dRotation = rootVisualElement.Q<Slider>("2DRotation");
            _2dOpacityLayer1 = rootVisualElement.Q<Slider>("2DOpacity1");
            _2dOpacityLayer2 = rootVisualElement.Q<Slider>("2DOpacity2");
            _2dOpacityLayer3 = rootVisualElement.Q<Slider>("2DOpacity3");
            _2dOpacityLayer4 = rootVisualElement.Q<Slider>("2DOpacity4");
        }

        private void RegisterCloudInput()
        {
            _2dCloudPreset.RegisterCallback<ChangeEvent<UnityEngine.Object>>((evt) =>
            {
                _selectedPresetData.LayerCloudPresetData = (LayerCloudPresetData)evt.newValue;
                _weatherManager.FireDataUpdated();
            });

            _2dCoverage.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.LayerCloudPresetData.cloudData.cloudLayerCoverage = evt.newValue;
                _weatherManager.FireDataUpdated();
            });

            _2dColor.RegisterCallback<ChangeEvent<Color>>((evt) =>
            {
                _selectedPresetData.LayerCloudPresetData.cloudData.tintLayer2D = evt.newValue;
                _weatherManager.FireDataUpdated();
            });

            _2dShadows.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                _selectedPresetData.LayerCloudPresetData.cloudData.areShadowsEnabled = evt.newValue;
                _weatherManager.FireDataUpdated();
            });

            _2dAltitude.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.LayerCloudPresetData.cloudData.cloudAltitude = evt.newValue;
                _weatherManager.FireDataUpdated();
            });

            _2dExposure.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.LayerCloudPresetData.cloudData.exposureLayer2D = evt.newValue;
                _weatherManager.FireDataUpdated();
            });

            _2dRotation.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.LayerCloudPresetData.cloudData.rotation = evt.newValue;
                _weatherManager.FireDataUpdated();
            });

            _2dWindInteraction.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                _selectedPresetData.LayerCloudPresetData.cloudData.isWindInteractionActive = evt.newValue;
                _weatherManager.FireDataUpdated();
            });

            _2dOpacityLayer1.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.LayerCloudPresetData.cloudData.opacityLayer1 = evt.newValue;
                _weatherManager.FireDataUpdated();
            });

            _2dOpacityLayer2.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.LayerCloudPresetData.cloudData.opacityLayer2 = evt.newValue;
                _weatherManager.FireDataUpdated();
            });

            _2dOpacityLayer3.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.LayerCloudPresetData.cloudData.opacityLayer3 = evt.newValue;
                _weatherManager.FireDataUpdated();
            });

            _2dOpacityLayer4.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.LayerCloudPresetData.cloudData.opacityLayer4 = evt.newValue;
                _weatherManager.FireDataUpdated();
            });
        }

        private void SetCloudInputData()
        {
            _2dCloudPreset.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData);
            _2dCoverage.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.cloudData.cloudLayerCoverage);
            _2dColor.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.cloudData.tintLayer2D);
            _2dShadows.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.cloudData.areShadowsEnabled);
            _2dAltitude.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.cloudData.cloudAltitude);
            _2dExposure.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.cloudData.exposureLayer2D);
            _2dWindInteraction.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.cloudData.isWindInteractionActive);
            _2dRotation.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.cloudData.rotation);
            _2dOpacityLayer1.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.cloudData.opacityLayer1);
            _2dOpacityLayer2.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.cloudData.opacityLayer2);
            _2dOpacityLayer3.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.cloudData.opacityLayer3);
            _2dOpacityLayer4.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.cloudData.opacityLayer4);
        }
        #endregion
    }
}