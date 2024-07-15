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
    /// <summary>
    /// This class handles the input from the volumetric clouds editor
    /// </summary>
    public class VolumetricCloudsAdvanceEditor : SingletonEditorWindow<VolumetricCloudsAdvanceEditor>
    {
        #region Private Variables
        private ObjectField _cloudPreset;
        private Slider _volCloudAltitude;
        private Slider _volCloudThickness;
        private ColorField _volCloudColor;
        private Toggle _volCastShadows;
        private Toggle _colWindInteraction;
        private Slider _volCloudDensity;
        private Slider _volShapeFactor;
        private Slider _volErosionFactor;
        private Slider _volEarthCurve;
        private FloatField _volErosionScale;
        private FloatField _volShapeScale;
        private Slider _volLightDimmer;
        private CurveField _densityCurve;
        private CurveField _erosionCurve;
        private CurveField _ambientOcclusinCurve;
        #endregion

        #region Protected Variables
        protected override string _ussPath { get; set; } = "/Editor/UI/CloudsAdvanceEditor.uxml";
        protected override string _windowName { get; set; } = "Volumetric Clouds Advanced Editor";
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
            _cloudPreset = rootVisualElement.Q<ObjectField>("VolCloudPreset");
            _volCloudAltitude = rootVisualElement.Q<Slider>("VolAltitude");
            _volCloudThickness = rootVisualElement.Q<Slider>("VolThickness");
            _volCloudColor = rootVisualElement.Q<ColorField>("VolColor");
            _volCastShadows = rootVisualElement.Q<Toggle>("VolCastShadows");
            _colWindInteraction = rootVisualElement.Q<Toggle>("VolWindInteraction");
            _volCloudDensity = rootVisualElement.Q<Slider>("VolDensity");
            _volShapeFactor = rootVisualElement.Q<Slider>("VolShapeFactor");
            _volErosionFactor = rootVisualElement.Q<Slider>("VolErosionFactor");
            _volEarthCurve = rootVisualElement.Q<Slider>("VolEarthCurvature");
            _volErosionScale = rootVisualElement.Q<FloatField>("VolErosionScale");
            _volShapeScale = rootVisualElement.Q<FloatField>("VolShapeScale");
            _volLightDimmer = rootVisualElement.Q<Slider>("ProbeLightDimmer");
            _densityCurve = rootVisualElement.Q<CurveField>("DensityCurve");
            _erosionCurve = rootVisualElement.Q<CurveField>("ErosionCurve");
            _ambientOcclusinCurve = rootVisualElement.Q<CurveField>("AmbientOclusionCurve");
        }

        private void RegisterCloudInput()
        {
            _cloudPreset.RegisterCallback<ChangeEvent<UnityEngine.Object>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData = (VolumetricCloudPresetData)_cloudPreset.value;
                _weatherManager.FireDataUpdated();
            });

            _volCloudAltitude.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData.cloudData.cloudAltitude = _volCloudAltitude.value;
                _weatherManager.FireDataUpdated();
            });

            _volCloudThickness.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData.cloudData.cloudThickness = _volCloudThickness.value;
                _weatherManager.FireDataUpdated();
            });

            _volCloudColor.RegisterCallback<ChangeEvent<Color>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData.cloudData.cloudScatteringTint = _volCloudColor.value;
                _weatherManager.FireDataUpdated();
            });

            _volCastShadows.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData.cloudData.areShadowsEnabled = _volCastShadows.value;
                _weatherManager.FireDataUpdated();
            });

            _colWindInteraction.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData.cloudData.isWindInteractionActive = evt.newValue;
                _weatherManager.FireDataUpdated();
            });

            _volCloudDensity.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData.cloudData.cloudDensityMultiplier = _volCloudDensity.value;
                _weatherManager.FireDataUpdated();
            });

            _volShapeFactor.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData.cloudData.cloudShapeFactor = _volShapeFactor.value;
                _weatherManager.FireDataUpdated();
            });

            _volErosionFactor.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData.cloudData.cloudErosionFactor = _volErosionFactor.value;
                _weatherManager.FireDataUpdated();
            });

            _volErosionScale.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData.cloudData.erosionScale = _volErosionScale.value;
                _weatherManager.FireDataUpdated();
            });

            _volShapeScale.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData.cloudData.shapeScale = _volShapeScale.value;
                _weatherManager.FireDataUpdated();
            });

            _volLightDimmer.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData.cloudData.lightProbeDimmer = evt.newValue;
                _weatherManager.FireDataUpdated();
            });

            _volEarthCurve.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData.cloudData.earthCurvature = evt.newValue;
                _weatherManager.FireDataUpdated();
            });

            _densityCurve.RegisterCallback<ChangeEvent<AnimationCurve>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData.cloudData.densityCurve = evt.newValue;
                _weatherManager.FireDataUpdated();
            });

            _erosionCurve.RegisterCallback<ChangeEvent<AnimationCurve>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData.cloudData.erosionCurve = evt.newValue;
                _weatherManager.FireDataUpdated();
            });

            _ambientOcclusinCurve.RegisterCallback<ChangeEvent<AnimationCurve>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData.cloudData.ambientOcclusionCurve = evt.newValue;
                _weatherManager.FireDataUpdated();
            });
        }

        private void SetCloudInputData()
        {
            _cloudPreset.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData);
            _volCloudAltitude.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.cloudAltitude);
            _volCloudThickness.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.cloudThickness);
            _volCloudColor.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.cloudScatteringTint);
            _volCastShadows.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.areShadowsEnabled);
            _volCloudDensity.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.cloudDensityMultiplier);
            _volShapeFactor.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.cloudShapeFactor);
            _volErosionFactor.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.cloudErosionFactor);
            _volErosionScale.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.erosionScale);
            _volShapeScale.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.shapeScale);
            _colWindInteraction.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.isWindInteractionActive);
            _volLightDimmer.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.lightProbeDimmer);
            _volEarthCurve.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.earthCurvature);
            _densityCurve.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.densityCurve);
            _erosionCurve.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.erosionCurve);
            _ambientOcclusinCurve.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.ambientOcclusionCurve);
        }
        #endregion
    }
}