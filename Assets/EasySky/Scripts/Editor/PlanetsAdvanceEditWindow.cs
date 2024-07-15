//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System;
using System.Collections.Generic;
using EasySky.Skybox;
using EasySky.Utils;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static EasySky.GlobalConstantData;

namespace EasySky.Editor
{
    public class PlanetsAdvanceEditWindow : SingletonEditorWindow<PlanetsAdvanceEditWindow>
    {
        #region Private Variables
        private List<Button> _planetsButtons = new List<Button>();
        private int _selectedIndex = -1;
        private Button _previousButton;
        private CelestialObjectPresetData _data;
        #endregion

        #region Protected Variables
        protected override string _ussPath { get; set; } = "/Editor/UI/StarsAdvanceEditor.uxml";
        protected override string _windowName { get; set; } = "Planets Advance Edit Window";
        #endregion

        #region Public Methods
        public void InitializeForm(EasySkyWeatherManager weatherManager, Action<int> addNewPlanet, Action<int> deletePlanet, Action onCloseWindowAction)
        {
            base.InitializeForm(weatherManager, null, onCloseWindowAction);
            PopulateScrollview();
            var planetsHolder = rootVisualElement.Q<ScrollView>("PresetsHolder");
            var newStarButton = rootVisualElement.Q<Button>("NewStarButton");

            newStarButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                _weatherManager.CelestialObjectsController.AddNewPlanetData();
                var i = _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets.Count - 1;
                var planetLabel = _weatherManager.PlanetsDataContainer.starLabel.Instantiate();

                var button = planetLabel.Q<Button>("LableButton");

                button.Q<IntegerField>(GlobalConstantData.IndexString).value = i;
                button.RegisterCallback<MouseUpEvent>((evt) => { ChangeButtonState(button); });

                planetLabel.Q<Label>(TextString).text = _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[i].presetName;
                planetLabel.Q<VisualElement>("Color").style.backgroundColor = _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[i].presetColor;
                planetLabel.Q<VisualElement>("Image").style.backgroundImage = _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[i].starIcon;

                planetsHolder.Add(planetLabel);
                _planetsButtons.Add(button);
                addNewPlanet?.Invoke(i);
            });

            var deleteStarButton = rootVisualElement.Q<Button>("DeleteButton");

            deleteStarButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                _previousButton.parent.style.display = DisplayStyle.None;
                _previousButton.parent.Clear();
                _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets.RemoveAt(_selectedIndex);
                _planetsButtons.Remove(_previousButton);

                for (int i = _selectedIndex; i < _planetsButtons.Count; i++)
                {
                    _planetsButtons[i].Q<IntegerField>(GlobalConstantData.IndexString).value -= 1;
                }

                _weatherManager.CelestialObjectsController.DestroyPlanet(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedIndex]);
                deletePlanet?.Invoke(_selectedIndex);
                _selectedIndex = -1;
                _previousButton = null;
                if (_planetsButtons.Count > 0)
                {
                    ChangeButtonState(_planetsButtons[0]);
                }
            });

            var presetsName = rootVisualElement.Q<Label>("PresetNames");
            presetsName.text = "Planets & Satellites\npresets";
        }
        #endregion

        #region Private Methods
        private void PopulateScrollview()
        {
            var presetsHolder = rootVisualElement.Q<ScrollView>("PresetsHolder");
            for (int i = 0; i < _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets.Count; i++)
            {
                var planetLabel = _weatherManager.PlanetsDataContainer.starLabel.Instantiate();

                var button = planetLabel.Q<Button>("LableButton");

                button.Q<IntegerField>(GlobalConstantData.IndexString).value = i;
                button.RegisterCallback<MouseUpEvent>((evt) => { ChangeButtonState(button); });

                planetLabel.Q<Label>(TextString).text = _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[i].presetName;
                planetLabel.Q<VisualElement>("Color").style.backgroundColor = _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[i].presetColor;
                planetLabel.Q<VisualElement>("Image").style.backgroundImage = _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[i].starIcon;
                _planetsButtons.Add(button);
                presetsHolder.Add(planetLabel);

                if (i == 0)
                {
                    _data = _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[0];
                    ChangeButtonState(button);
                }
            }
            RegisterPresetInput();
        }

        private void ChangeButtonState(Button button)
        {
            var index = button.Q<IntegerField>(GlobalConstantData.IndexString).value;
            if (index == _selectedIndex) return;

            _selectedIndex = index;
            if (_previousButton != null)
            {
                ChangeButtonStyle(_previousButton, false);
            }

            _data = _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedIndex];
            _previousButton = button;
            PopulatePresetInput();
            ChangeButtonStyle(button, true);
        }

        private void ChangeButtonStyle(Button button, bool state)
        {
            var parent = button.parent;

            var border = parent.Q<VisualElement>("LableParent");

            var value = state ? 1 : 0;
            border.style.borderRightWidth = value;
            border.style.borderLeftWidth = value;
            border.style.borderBottomWidth = value;
            border.style.borderTopWidth = value;

            var color = new Color(BorderColor.r, BorderColor.g, BorderColor.b, value);

            border.style.borderRightColor = color;
            border.style.borderLeftColor = color;
            border.style.borderTopColor = color;
            border.style.borderBottomColor = color;

            var selected = parent.Q<VisualElement>("SelectedImage");

            selected.style.visibility = state ? Visibility.Visible : Visibility.Hidden;
        }

        private void RegisterPresetInput()
        {
            var presetName = rootVisualElement.Q<TextField>("SunPresetName");
            presetName.RegisterCallback<ChangeEvent<string>>((evt) =>
            {
                _data.presetName = evt.newValue;
                _previousButton.parent.Q<Label>(TextString).text = _data.presetName;
            });

            var presetTexture = rootVisualElement.Q<ObjectField>("SunPresetTexture");
            presetTexture.objectType = typeof(Texture);

            presetTexture.RegisterCallback<ChangeEvent<UnityEngine.Object>>((evt) =>
            {
                _data.starTexture = (Texture2D)evt.newValue; 
                _weatherManager.CelestialObjectsController.UpdatePlanet(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedIndex]);
                _weatherManager.FireDataUpdated();
            });

            var presetColor = rootVisualElement.Q<ColorField>("SunPresetColor");
            presetColor.RegisterCallback<ChangeEvent<Color>>((evt) =>
            {
                _data.lightColor = evt.newValue; _previousButton.parent.Q<VisualElement>("Color").style.backgroundColor = _data.presetColor;
                _weatherManager.CelestialObjectsController.UpdatePlanet(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedIndex]);
            });

            var flareColor = rootVisualElement.Q<ColorField>("SunPresetFlare");
            flareColor.RegisterCallback<ChangeEvent<Color>>((evt) => { _data.flareTint = evt.newValue; _weatherManager.CelestialObjectsController.UpdatePlanet(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedIndex]); });

            var tintColor = rootVisualElement.Q<ColorField>("SunPresetTint");
            tintColor.RegisterCallback<ChangeEvent<Color>>((evt) => { _data.surfaceTint = evt.newValue; _weatherManager.CelestialObjectsController.UpdatePlanet(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedIndex]); });

            var starScale = rootVisualElement.Q<FloatField>("SunPresetScale");
            starScale.RegisterCallback<ChangeEvent<float>>((evt) => { _data.scale = evt.newValue; _weatherManager.CelestialObjectsController.UpdatePlanet(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedIndex]); });

            var starOffset = rootVisualElement.Q<Vector2Field>("SunPresetOffset");
            starOffset.RegisterCallback<ChangeEvent<Vector2>>((evt) => { _data.offset = evt.newValue; _weatherManager.CelestialObjectsController.UpdatePlanetPosition(); });

            var flareSize = rootVisualElement.Q<FloatField>("FlareSize");
            flareSize.RegisterCallback<ChangeEvent<float>>((evt) => { _data.flareSize = evt.newValue; _weatherManager.CelestialObjectsController.UpdatePlanet(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedIndex]); });

            var flareFalloff = rootVisualElement.Q<FloatField>("FlareFalloff");
            flareFalloff.RegisterCallback<ChangeEvent<float>>((evt) => { _data.flareFalloff = evt.newValue; _weatherManager.CelestialObjectsController.UpdatePlanet(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedIndex]); });

            var lightTemp = rootVisualElement.Q<IntegerField>("LightTemp");
            lightTemp.RegisterCallback<ChangeEvent<int>>((evt) => { _data.lightTemperature = evt.newValue; _weatherManager.CelestialObjectsController.UpdatePlanet(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedIndex]); });

            var shadowDimmer = rootVisualElement.Q<Slider>("ShadowDimmer");
            shadowDimmer.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _data.shadowDimmer = evt.newValue;
                _weatherManager.CelestialObjectsController.UpdatePlanet(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedIndex]);
                _weatherManager.FireDataUpdated();
            });

            var shadowResolution = rootVisualElement.Q<EnumField>("ShadowResolution");
            shadowResolution.RegisterCallback<ChangeEvent<Enum>>((evt) =>
            {
                _data.resolution = (Resolutions)evt.newValue;
                _weatherManager.CelestialObjectsController.UpdatePlanet(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedIndex]);
                _weatherManager.FireDataUpdated();
            });
        }

        private void PopulatePresetInput()
        {
            var presetName = rootVisualElement.Q<TextField>("SunPresetName");
            presetName.value = _data.presetName;

            var presetColor = rootVisualElement.Q<ColorField>("SunPresetColor");
            presetColor.value = _data.lightColor;

            var presetTexture = rootVisualElement.Q<ObjectField>("SunPresetTexture");
            presetTexture.value = _data.starTexture;

            var flareColor = rootVisualElement.Q<ColorField>("SunPresetFlare");
            flareColor.value = _data.flareTint;

            var tintColor = rootVisualElement.Q<ColorField>("SunPresetTint");
            tintColor.value = _data.surfaceTint;

            var starScale = rootVisualElement.Q<FloatField>("SunPresetScale");
            starScale.value = _data.scale;

            var starOffset = rootVisualElement.Q<Vector2Field>("SunPresetOffset");
            starOffset.value = _data.offset;

            var flareSize = rootVisualElement.Q<FloatField>("FlareSize");
            flareSize.value = _data.flareSize;

            var flareFalloff = rootVisualElement.Q<FloatField>("FlareFalloff");
            flareFalloff.value = _data.flareFalloff;

            var lightTemp = rootVisualElement.Q<IntegerField>("LightTemp");
            lightTemp.value = _data.lightTemperature;

            var shadowDimmer = rootVisualElement.Q<Slider>("ShadowDimmer");
            shadowDimmer.value = _data.shadowDimmer;

            var shadowResolution = rootVisualElement.Q<EnumField>("ShadowResolution");
            shadowResolution.value = _data.resolution;
        }
        #endregion
    }
}