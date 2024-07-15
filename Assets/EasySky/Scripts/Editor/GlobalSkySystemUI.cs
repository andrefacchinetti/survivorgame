//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System.Collections.Generic;
using System.Linq;
using EasySky.Skybox;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static EasySky.GlobalConstantData;

namespace EasySky.Editor
{
    public class GlobalSkySystemUI
    {
        #region Private Variables
        private VisualElement _root;
        private CurveField _curveField;
        private EasySkyWeatherManager _weatherManager;
        private VisualElement _skyboxParent;
        private VisualElement _starsParent;
        private VisualElement _planetsParent;
        private VisualElement _starInputsParent;
        private VisualElement _planetsInputsParent;
        private List<string> _guidList = new List<string>();
        private int _selectedStarIndex = -1;
        private int _selectedPlanetIndex = -1;
        private Button _previousStarButton;
        private Button _previousStarImageButton;
        private Button _previousPlanetImageButton;
        private Button _previousPlanetButton;
        private CelestialObjectPresetData _selectedStarData;
        private CelestialObjectPresetData _selectedPlanetData;
        private List<Button> _starButtons;
        private List<Button> _planetsButtons;
        private int _previousSelectedSunImageIndex;
        private int _previousSelectedPlanetImageIndex;
        #endregion

        #region Constructors
        public GlobalSkySystemUI(VisualElement root)
        {
            _root = root;
        }
        #endregion

        #region Public Methods
        public void PopulateGlobalSkySystemUI(EasySkyWeatherManager weatherManager)
        {
            _weatherManager = weatherManager;

            _skyboxParent = _root.Q<VisualElement>(SkyboxParent);
            _starsParent = _root.Q<VisualElement>(StarsParent);
            _planetsParent = _root.Q<VisualElement>(PlanetsParent);
            var dropdown = _root.Q<DropdownField>(GlobalSkyDropdown);
            PopulateSkybox();
            dropdown.RegisterCallback<ChangeEvent<string>>((evt) => UIOptionSelectUpdate(dropdown.index));
            PopulateSunEditor();
            PopulatePlanetsEditor();

            _weatherManager.OnWeatherDataUpdated += OnDataUpdated;
        }

        public void PopulateSkybox()
        {
            _skyboxParent.style.display = DisplayStyle.Flex;
            var textures = Resources.LoadAll(GlobalConstantData.SkyboxString, typeof(Cubemap));

            var skyboxParent = _root.Q<RadioButtonGroup>(SkyboxParent);
            foreach (var item in textures)
            {
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(item, out var guid, out long file))
                {
                    if (_weatherManager.SkyboxController.SkyboxData.data.FirstOrDefault(x => x.guid == guid).animationCurve == null)
                    {
                        _weatherManager.SkyboxController.SkyboxData.data.Add(new SkyData(guid, new AnimationCurve(), (Cubemap)item));
                    }

                    if (!_guidList.Contains(guid))
                    {
                        _guidList.Add(guid);
                        var skyboxLabel = _weatherManager.SkyboxController.SkyboxData.SkyboxLabel.Instantiate();
                        skyboxLabel.Q<RadioButton>(GlobalConstantData.ButtonString).label = item.name;
                        skyboxParent.Add(skyboxLabel);
                    }
                }
            }

            var toBeRemoved = new List<string>();
            foreach (var guid in _weatherManager.SkyboxController.SkyboxData.data)
            {
                if (!_guidList.Contains(guid.guid))
                {
                    toBeRemoved.Add(guid.guid);
                }
            }

            foreach (var guid in toBeRemoved)
            {
                for (int i = 0; i < _weatherManager.SkyboxController.SkyboxData.data.Count;)
                {
                    SkyData item = _weatherManager.SkyboxController.SkyboxData.data[i];
                    if (item.guid == guid)
                    {
                        _weatherManager.SkyboxController.SkyboxData.data.Remove(item);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            _curveField = _root.Q<CurveField>("IntensityCurve");
            _curveField.RegisterCallback<ChangeEvent<AnimationCurve>>((evt) =>
            {
                _weatherManager.SkyboxController.SkyboxData.data[skyboxParent.value] = new SkyData(_weatherManager.SkyboxController.SkyboxData.data[skyboxParent.value].guid, new AnimationCurve(evt.newValue.keys), _weatherManager.SkyboxController.SkyboxData.data[skyboxParent.value].cubemap);
                EditorUtility.SetDirty(_weatherManager.SkyboxController.SkyboxData);
                AssetDatabase.SaveAssets();
            });

            skyboxParent.RegisterCallback<ChangeEvent<int>>((evt) =>
            {
                AssetDatabase.SaveAssets();
                _curveField.SetValueWithoutNotify(_weatherManager.SkyboxController.SkyboxData.data.ElementAt(evt.newValue).animationCurve);
                _weatherManager.SkyboxController.SkyboxData.selectedData = evt.newValue;
                _weatherManager.SkyboxController.SetSky(evt.newValue);
            });

            AssetDatabase.SaveAssets();
            _curveField.SetValueWithoutNotify(_weatherManager.SkyboxController.SkyboxData.data.ElementAt(_weatherManager.SkyboxController.SkyboxData.selectedData).animationCurve);
            _weatherManager.SkyboxController.SetSky(_weatherManager.SkyboxController.SkyboxData.selectedData);
            skyboxParent.SetValueWithoutNotify(_weatherManager.SkyboxController.SkyboxData.selectedData);
        }
        #endregion

        #region Private Methods
        private void OnDataUpdated()
        {
            PopulateStarPresetInput();
        }

        private void UIOptionSelectUpdate(int index)
        {
            _skyboxParent.style.display = DisplayStyle.None;
            _starsParent.style.display = DisplayStyle.None;
            _planetsParent.style.display = DisplayStyle.None;

            switch (index)
            {
                case 0:
                    PopulateSkybox();
                    break;
                case 1:
                    PopulateStarsUI();
                    break;
                case 2:
                    PopulatePlanetsUI();
                    break;
                default:
                    break;
            }
        }

        private void PopulateStarsUI()
        {
            _starsParent.style.display = DisplayStyle.Flex;
        }

        private void PopulatePlanetsUI()
        {
            _planetsParent.style.display = DisplayStyle.Flex;
        }

        private void PopulateSunEditor()
        {
            _starInputsParent = _root.Q<VisualElement>("StarsInputFields");

            var imagesParent = _root.Q<VisualElement>("SunPresetsImageParent");

            for (int i = 0; i < _weatherManager.PresetsImages.StarPresetIcons.Count; i++)
            {
                var imageHolder = _weatherManager.PresetsImages.ImageLable.Instantiate();
                imagesParent.Add(imageHolder);

                var button = imageHolder.Q<Button>("ImageSelecorButton");
                imageHolder.Q<IntegerField>(IndexString).value = i;
                button.Q<VisualElement>("ImageHolder").style.backgroundImage = _weatherManager.PresetsImages.StarPresetIcons[i];
                button.RegisterCallback<MouseUpEvent>((evt) =>
                {
                    ChangeSunImageStyle(button);
                });
            }

            _starButtons = new List<Button>();
            var starsHolder = _root.Q<ScrollView>("StarsHolder");
            var starsParent = _root.Q<VisualElement>("StarsPresetsParent");

            for (int i = 0; i < _weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets.Count; i++)
            {
                var starLable = _weatherManager.StarsDataContainer.starLabel.Instantiate();

                var button = starLable.Q<Button>("LableButton");

                button.Q<IntegerField>(IndexString).value = i;
                button.RegisterCallback<MouseUpEvent>((evt) => { ChangeStarButtonState(button); });

                starLable.Q<Label>(TextString).text = _weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets[i].presetName;
                starLable.Q<VisualElement>("Color").style.backgroundColor = _weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets[i].presetColor;
                starLable.Q<VisualElement>("Image").style.backgroundImage = _weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets[i].starIcon;

                starsParent.Add(starLable);

                if (i == _weatherManager.SelectedStarPreset)
                {
                    _selectedStarData = _weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets[i];
                    ChangeStarButtonState(button);
                }
                _starButtons.Add(button);
            }

            var newStarButton = _root.Q<Button>("NewStarButton");

            newStarButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                _weatherManager.CelestialObjectsController.StarsDataContainer.AddNewStarPreset();
                var i = _weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets.Count - 1;
                var starLable = _weatherManager.StarsDataContainer.starLabel.Instantiate();

                var button = starLable.Q<Button>("LableButton");

                button.Q<IntegerField>(IndexString).value = i;
                button.RegisterCallback<MouseUpEvent>((evt) => { ChangeStarButtonState(button); });

                starLable.Q<Label>(TextString).text = _weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets[i].presetName;
                starLable.Q<VisualElement>("Color").style.backgroundColor = _weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets[i].presetColor;
                starLable.Q<VisualElement>("Image").style.backgroundImage = _weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets[i].starIcon;

                starsParent.Add(starLable);
                _starButtons.Add(button);
            });

            var deleteStarButton = _root.Q<Button>("StarDeleteButton");

            deleteStarButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                if (_previousStarButton == null)
                {
                    return;
                }

                _previousStarButton.parent.style.display = DisplayStyle.None;
                _previousStarButton.parent.Clear();
                _weatherManager.CelestialObjectsController.DestroyStar(_weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets[_selectedStarIndex]);
                _weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets.RemoveAt(_selectedStarIndex);
                _starButtons.Remove(_previousStarButton);

                for (int i = _selectedStarIndex; i < _starButtons.Count; i++)
                {
                    _starButtons[i].Q<IntegerField>(IndexString).value -= 1;
                }

                _selectedStarIndex = -1;
                _weatherManager.SelectedStarPreset = -1;
                _previousStarButton = null;

                _selectedStarData = null;
                _starInputsParent.SetEnabled(false);
            });

            var advanceEditButton = _root.Q<Button>("AdvanceStarButton");

            advanceEditButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                StarsAdvancedEditWindow.Instance.InitializeForm(_weatherManager, AddNewStarPreset, DeleteStar, EnableRootInteraction);
                _root.SetEnabled(false);
            });

            PopulateStarPresetInput();
            RegisterStarPresetInput();

            if (_selectedStarData == null)
            {
                _starInputsParent.SetEnabled(false);
            }
        }

        private void EnableRootInteraction()
        {
            _root.SetEnabled(true);
        }

        private void AddNewStarPreset(int index)
        {
            var starLable = _weatherManager.StarsDataContainer.starLabel.Instantiate();
            var starsHolder = _root.Q<ScrollView>("StarsHolder");
            var button = starLable.Q<Button>("LableButton");

            button.Q<IntegerField>(IndexString).value = index;
            button.RegisterCallback<MouseUpEvent>((evt) => { ChangeStarButtonState(button); });

            starLable.Q<Label>(TextString).text = _weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets[index].presetName;
            starLable.Q<VisualElement>("Color").style.backgroundColor = _weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets[index].presetColor;
            starLable.Q<VisualElement>("Image").style.backgroundImage = _weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets[index].starIcon;

            starsHolder.Add(starLable);
            _starButtons.Add(button);
        }

        private void DeleteStar(int index)
        {
            var button = _starButtons[index];
            button.parent.style.display = DisplayStyle.None;
            button.parent.Clear();
            _starButtons.Remove(button);

            for (int i = index; i < _starButtons.Count; i++)
            {
                _starButtons[i].Q<IntegerField>(IndexString).value -= 1;
            }

            if (index == _selectedStarIndex)
            {
                _selectedStarIndex = -1;
                _weatherManager.SelectedStarPreset = -1;
                _previousStarButton = null;
                _selectedStarData = null;
                if (_starButtons.Count > 0)
                {
                    ChangeStarButtonState(_starButtons[0]);
                }
            }
        }

        private void ChangeSunImageStyle(Button button)
        {
            var index = button.parent.Q<IntegerField>(IndexString).value;
            if (index == _previousSelectedSunImageIndex) return;

            _previousSelectedSunImageIndex = index;
            _selectedStarData.starIcon = _weatherManager.PresetsImages.StarPresetIcons[index];
            _previousStarButton.parent.Q<VisualElement>("Image").style.backgroundImage = _selectedStarData.starIcon;
            ChangeImageSelectorState(button, true);

            if (_previousStarImageButton != null)
            {
                ChangeImageSelectorState(_previousStarImageButton, false);
            }

            _previousStarImageButton = button;
        }

        private void ChangePlanetImageStyle(Button button)
        {
            var index = button.parent.Q<IntegerField>(IndexString).value;
            if (index == _previousSelectedPlanetImageIndex) return;

            _previousSelectedPlanetImageIndex = index;
            _selectedPlanetData.starIcon = _weatherManager.PresetsImages.PlanetPresetIcons[index];
            _previousPlanetButton.parent.Q<VisualElement>("Image").style.backgroundImage = _selectedPlanetData.starIcon;
            ChangeImageSelectorState(button, true);

            if (_previousPlanetImageButton != null)
            {
                ChangeImageSelectorState(_previousPlanetImageButton, false);
            }

            _previousPlanetImageButton = button;
        }

        private void ChangeImageSelectorState(VisualElement border, bool state)
        {
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
        }

        private void PopulatePlanetsEditor()
        {
            _planetsInputsParent = _root.Q<VisualElement>("PlanetsInputFields");

            _planetsButtons = new List<Button>();
            var planetHolder = _root.Q<ScrollView>("PlanetsHolder");
            var planetsPresetsParent = _root.Q<VisualElement>("PlanetsPresetsParent");

            for (int i = 0; i < _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets.Count; i++)
            {
                var starLable = _weatherManager.PlanetsDataContainer.starLabel.Instantiate();

                var button = starLable.Q<Button>("LableButton");

                button.Q<IntegerField>(IndexString).value = i;
                button.RegisterCallback<MouseUpEvent>((evt) => { ChangePlanetButtonState(button); });

                starLable.Q<Label>(TextString).text = _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[i].presetName;
                starLable.Q<VisualElement>("Color").style.backgroundColor = _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[i].presetColor;
                starLable.Q<VisualElement>("Image").style.backgroundImage = _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[i].starIcon;

                planetsPresetsParent.Add(starLable);

                if (i == _weatherManager.SelectedPlanetPreset)
                {
                    _selectedPlanetData = _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[i];
                    ChangePlanetButtonState(button);
                }
                _planetsButtons.Add(button);
            }

            var imagesParent = _root.Q<VisualElement>("PlanetsPresetsImageParent");

            for (int i = 0; i < _weatherManager.PresetsImages.PlanetPresetIcons.Count; i++)
            {
                var imageHolder = _weatherManager.PresetsImages.ImageLable.Instantiate();
                imagesParent.Add(imageHolder);

                var button = imageHolder.Q<Button>("ImageSelecorButton");
                imageHolder.Q<IntegerField>(IndexString).value = i;
                button.Q<VisualElement>("ImageHolder").style.backgroundImage = _weatherManager.PresetsImages.PlanetPresetIcons[i];
                button.RegisterCallback<MouseUpEvent>((evt) =>
                {
                    ChangePlanetImageStyle(button);
                });
            }

            var newPlanetButton = _root.Q<Button>("NewPlanetButton");

            newPlanetButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                _weatherManager.CelestialObjectsController.AddNewPlanetData();
                var i = _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets.Count - 1;
                var planetLabel = _weatherManager.PlanetsDataContainer.starLabel.Instantiate();

                var button = planetLabel.Q<Button>("LableButton");

                button.Q<IntegerField>(IndexString).value = i;
                button.RegisterCallback<MouseUpEvent>((evt) => { ChangePlanetButtonState(button); });

                planetLabel.Q<Label>(TextString).text = _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[i].presetName;
                planetLabel.Q<VisualElement>("Color").style.backgroundColor = _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[i].presetColor;
                planetLabel.Q<VisualElement>("Image").style.backgroundImage = _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[i].starIcon;

                planetsPresetsParent.Add(planetLabel);
                _planetsButtons.Add(button);
            });

            var deleteStarButton = _root.Q<Button>("PlanetDeleteButton");

            deleteStarButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                _previousPlanetButton.parent.style.display = DisplayStyle.None;
                _previousPlanetButton.parent.Clear();
                _weatherManager.CelestialObjectsController.DestroyPlanet(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedPlanetIndex]);
                _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets.RemoveAt(_selectedPlanetIndex);
                _planetsButtons.Remove(_previousPlanetButton);

                for (int i = _selectedPlanetIndex; i < _planetsButtons.Count; i++)
                {
                    _planetsButtons[i].Q<IntegerField>(IndexString).value -= 1;
                }

                _selectedPlanetIndex = -1;
                _weatherManager.SelectedPlanetPreset = -1;
                _previousPlanetButton = null;
                _selectedPlanetData = null;
                _planetsInputsParent.SetEnabled(false);
            });

            var advanceEditButton = _root.Q<Button>("AdvancePlanetButton");

            advanceEditButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                PlanetsAdvanceEditWindow.Instance.InitializeForm(_weatherManager, AddNewPlanetPreset, DeletePlanet, EnableRootInteraction);
                _root.SetEnabled(false);
            });
            RegisterPlanetPresetInput();

            if(_selectedPlanetData == null)
            {
                _planetsInputsParent.SetEnabled(false);
            }
        }

        private void AddNewPlanetPreset(int index)
        {
            var planetLable = _weatherManager.PlanetsDataContainer.starLabel.Instantiate();
            var planetHolder = _root.Q<ScrollView>("PlanetsHolder");
            var button = planetLable.Q<Button>("LableButton");

            button.Q<IntegerField>(IndexString).value = index;
            button.RegisterCallback<MouseUpEvent>((evt) => { ChangePlanetButtonState(button); });

            planetLable.Q<Label>(TextString).text = _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[index].presetName;
            planetLable.Q<VisualElement>("Color").style.backgroundColor = _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[index].presetColor;
            planetLable.Q<VisualElement>("Image").style.backgroundImage = _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[index].starIcon;

            planetHolder.Add(planetLable);
            _planetsButtons.Add(button);
        }

        private void DeletePlanet(int index)
        {
            var button = _planetsButtons[index];
            button.parent.style.display = DisplayStyle.None;
            button.parent.Clear();
            _planetsButtons.Remove(button);

            for (int i = index; i < _planetsButtons.Count; i++)
            {
                _planetsButtons[i].Q<IntegerField>(IndexString).value -= 1;
            }

            if (index == _selectedPlanetIndex)
            {
                _selectedPlanetIndex = -1;
                _weatherManager.SelectedPlanetPreset = -1;
                _previousPlanetButton = null;
                if (_planetsButtons.Count > 0)
                {
                    ChangePlanetButtonState(_planetsButtons[0]);
                }
            }
        }

        private void ChangeStarButtonState(Button button)
        {
            var index = button.Q<IntegerField>(IndexString).value;
            if (index == _selectedStarIndex) return;

            _selectedStarIndex = index;
            _weatherManager.SelectedStarPreset = index;
            if (_previousStarButton != null)
            {
                ChangeButtonStyle(_previousStarButton, false);
            }

            _selectedStarData = _weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets[_selectedStarIndex];
            _previousStarButton = button;
            PopulateStarPresetInput();
            ChangeButtonStyle(button, true);
            _starInputsParent.SetEnabled(true);
        }

        private void ChangePlanetButtonState(Button button)
        {
            var index = button.Q<IntegerField>(IndexString).value;
            if (index == _selectedPlanetIndex) return;

            _selectedPlanetIndex = index;
            _weatherManager.SelectedPlanetPreset = index;
            if (_previousPlanetButton != null)
            {
                ChangeButtonStyle(_previousPlanetButton, false);
            }

            _selectedPlanetData = _weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedPlanetIndex];
            _previousPlanetButton = button;
            PopulatePlanetPresetInput();
            ChangeButtonStyle(button, true);
            _planetsInputsParent.SetEnabled(true);
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

        private void RegisterStarPresetInput()
        {
            var presetName = _root.Q<TextField>("SunPresetName");
            presetName.RegisterCallback<ChangeEvent<string>>((evt) =>
            {
                if (_selectedStarData == null) return;

                _selectedStarData.presetName = evt.newValue;
                _previousStarButton.parent.Q<Label>(TextString).text = _selectedStarData.presetName;
                _weatherManager.CelestialObjectsController.UpdateStar(_weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets[_selectedStarIndex]);
                _weatherManager.CelestialObjectsController.SetDirty();
            });

            var presetColor = _root.Q<ColorField>("SunPresetColor");
            presetColor.RegisterCallback<ChangeEvent<Color>>((evt) =>
            {
                if (_selectedStarData == null) return;

                _selectedStarData.presetColor = evt.newValue;
                _previousStarButton.parent.Q<VisualElement>("Color").style.backgroundColor = _selectedStarData.presetColor;
                _weatherManager.CelestialObjectsController.SetDirty();
            });

            var presetTexture = _root.Q<ObjectField>("SunPresetTexture");
            presetTexture.objectType = typeof(Texture);

            presetTexture.RegisterCallback<ChangeEvent<Object>>((evt) =>
            {
                if (_selectedStarData == null) return;

                _selectedStarData.starTexture = (Texture2D)evt.newValue;
                _weatherManager.CelestialObjectsController.UpdateStar(_weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets[_selectedStarIndex]);
                _weatherManager.CelestialObjectsController.SetDirty();
            });

            var lightIntensity = _root.Q<FloatField>("LigthIntensity");
            lightIntensity.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                if (_selectedStarData == null) return;

                _selectedStarData.lightIntensity = evt.newValue;
                _weatherManager.CelestialObjectsController.UpdateStar(_weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets[_selectedStarIndex]);
                _weatherManager.CelestialObjectsController.SetDirty();
            });

            var starScale = _root.Q<FloatField>("SunPresetScale");
            starScale.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                if (_selectedStarData == null) return;

                _selectedStarData.scale = evt.newValue;
                _weatherManager.CelestialObjectsController.UpdateStar(_weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets[_selectedStarIndex]);
                _weatherManager.CelestialObjectsController.SetDirty();
            });

            var starOffset = _root.Q<Vector2Field>("SunPresetOffset");
            starOffset.RegisterCallback<ChangeEvent<Vector2>>((evt) =>
            {
                if (_selectedStarData == null) return;

                _selectedStarData.offset = evt.newValue;
                _weatherManager.CelestialObjectsController.UpdateStarsPositions();
                _weatherManager.CelestialObjectsController.SetDirty();
            });

            var starCastShadows = _root.Q<Toggle>("SunCastShadows");
            starCastShadows.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                if (_selectedStarData == null) return;

                _selectedStarData.castShadows = evt.newValue;
                _weatherManager.CelestialObjectsController.UpdateStar(_weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets[_selectedStarIndex]);
                _weatherManager.CelestialObjectsController.SetDirty();
            });

            var starEnabled = _root.Q<Toggle>("SunEnabled");
            starEnabled.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                if (_selectedStarData == null) return;
                if (starEnabled.value == true)
                {
                    if (!_weatherManager.CelestialObjectsController.IsStarInScene(_weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets[_selectedStarIndex]))
                    {
                        _weatherManager.CelestialObjectsController.AddStar(_weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets[_selectedStarIndex]);
                    }
                }
                else
                {
                    if (_weatherManager.CelestialObjectsController.IsStarInScene(_weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets[_selectedStarIndex]))
                    {
                        _weatherManager.CelestialObjectsController.DestroyStar(_weatherManager.CelestialObjectsController.StarsDataContainer.StarsPresets[_selectedStarIndex]);
                    }
                }
            });
        }

        private void PopulateStarPresetInput()
        {
            if (_selectedStarData == null) return;

            var presetName = _root.Q<TextField>("SunPresetName");
            presetName.value = _selectedStarData.presetName;

            var presetColor = _root.Q<ColorField>("SunPresetColor");
            presetColor.value = _selectedStarData.presetColor;

            var presetTexture = _root.Q<ObjectField>("SunPresetTexture");
            presetTexture.value = _selectedStarData.starTexture;

            var starScale = _root.Q<FloatField>("SunPresetScale");
            starScale.value = _selectedStarData.scale;

            var starOffset = _root.Q<Vector2Field>("SunPresetOffset");
            starOffset.value = _selectedStarData.offset;

            var lightIntensity = _root.Q<FloatField>("LigthIntensity");
            lightIntensity.value = _selectedStarData.lightIntensity;

            var starCastShadows = _root.Q<Toggle>("SunCastShadows");
            starCastShadows.value = _selectedStarData.castShadows;

            var starEnabled = _root.Q<Toggle>("SunEnabled");
            starEnabled.SetValueWithoutNotify(_weatherManager.CelestialObjectsController.IsStarInScene(_selectedStarData));

            _previousSelectedSunImageIndex = -1;
            if (_previousStarImageButton != null)
            {
                ChangeImageSelectorState(_previousStarImageButton, false);
                _previousStarImageButton = null;
            }
        }

        private void PopulatePlanetPresetInput()
        {
            var presetName = _root.Q<TextField>("PlanetPresetName");
            presetName.value = _selectedPlanetData.presetName;

            var presetColor = _root.Q<ColorField>("PlanetPresetColor");
            presetColor.value = _selectedPlanetData.presetColor;

            var presetTexture = _root.Q<ObjectField>("PlanetPresetTexture");
            presetTexture.value = _selectedPlanetData.starTexture;

            var planetScale = _root.Q<FloatField>("PlanetPresetScale");
            planetScale.value = _selectedPlanetData.scale;

            var starOffset = _root.Q<Vector2Field>("PlanetPresetOffset");
            starOffset.value = _selectedPlanetData.offset;

            var lightIntensity = _root.Q<FloatField>("PlanetLigthIntensity");
            lightIntensity.value = _selectedPlanetData.lightIntensity;

            var nightLightIntensity = _root.Q<FloatField>("PlanetNightLigthIntensity");
            nightLightIntensity.SetValueWithoutNotify(_selectedPlanetData.nightLightIntensity);

            var planetCastShadows = _root.Q<Toggle>("PlanetsCastShadows");
            planetCastShadows.value = _selectedPlanetData.castShadows;

            var planetEnabled = _root.Q<Toggle>("PlanetsEnabled");
            planetEnabled.SetValueWithoutNotify(_weatherManager.CelestialObjectsController.IsPlanetInScene(_selectedPlanetData));
        }

        private void RegisterPlanetPresetInput()
        {
            var presetName = _root.Q<TextField>("PlanetPresetName");
            presetName.RegisterCallback<ChangeEvent<string>>((evt) =>
            {
                if (_selectedPlanetData == null) return;

                _selectedPlanetData.presetName = evt.newValue;
                _previousPlanetButton.parent.Q<Label>(TextString).text = _selectedPlanetData.presetName;
                _weatherManager.CelestialObjectsController.UpdatePlanet(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedPlanetIndex]);
                _weatherManager.CelestialObjectsController.SetDirty();
            });

            var presetColor = _root.Q<ColorField>("PlanetPresetColor");
            presetColor.RegisterCallback<ChangeEvent<Color>>((evt) =>
            {
                if (_selectedPlanetData == null) return;

                _selectedPlanetData.presetColor = evt.newValue;
                _previousPlanetButton.parent.Q<VisualElement>("Color").style.backgroundColor = _selectedPlanetData.presetColor;
                _weatherManager.CelestialObjectsController.UpdatePlanet(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedPlanetIndex]);
                _weatherManager.CelestialObjectsController.SetDirty();
            });

            var presetTexture = _root.Q<ObjectField>("PlanetPresetTexture");
            presetTexture.objectType = typeof(Texture);

            presetTexture.RegisterCallback<ChangeEvent<Object>>((evt) =>
            {
                if (_selectedPlanetData == null) return;

                _selectedPlanetData.starTexture = (Texture2D)evt.newValue;
                _weatherManager.CelestialObjectsController.UpdatePlanet(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedPlanetIndex]);
                _weatherManager.CelestialObjectsController.SetDirty();
            });

            var lightIntensity = _root.Q<FloatField>("PlanetLigthIntensity");
            lightIntensity.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                if (_selectedPlanetData == null) return;

                _selectedPlanetData.lightIntensity = evt.newValue;
                _weatherManager.CelestialObjectsController.UpdatePlanet(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedPlanetIndex]);
                _weatherManager.CelestialObjectsController.SetDirty();
            });

            var nightLightIntensity = _root.Q<FloatField>("PlanetNightLigthIntensity");
            nightLightIntensity.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                if (_selectedPlanetData == null) return;

                _selectedPlanetData.nightLightIntensity = evt.newValue;
                _weatherManager.CelestialObjectsController.UpdatePlanet(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedPlanetIndex]);
                _weatherManager.CelestialObjectsController.SetDirty();
            });

            var starScale = _root.Q<FloatField>("PlanetPresetScale");
            starScale.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                if (_selectedPlanetData == null) return;

                _selectedPlanetData.scale = evt.newValue;
                _weatherManager.CelestialObjectsController.UpdatePlanet(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedPlanetIndex]);
                _weatherManager.CelestialObjectsController.SetDirty();
            });

            var starOffset = _root.Q<Vector2Field>("PlanetPresetOffset");
            starOffset.RegisterCallback<ChangeEvent<Vector2>>((evt) =>
            {
                if (_selectedPlanetData == null) return;

                _selectedPlanetData.offset = evt.newValue;
                _weatherManager.CelestialObjectsController.UpdatePlanetPosition();
                _weatherManager.CelestialObjectsController.SetDirty();
            });

            var planetCastShadows = _root.Q<Toggle>("PlanetsCastShadows");
            planetCastShadows.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                if (_selectedPlanetData == null) return;

                _selectedPlanetData.castShadows = evt.newValue;
                _weatherManager.CelestialObjectsController.UpdatePlanet(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedPlanetIndex]);
                _weatherManager.CelestialObjectsController.SetDirty();
            });

            var planetEnabled = _root.Q<Toggle>("PlanetsEnabled");
            planetEnabled.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                if (_selectedPlanetData == null) return;
                if (planetEnabled.value == true)
                {
                    if (!_weatherManager.CelestialObjectsController.IsPlanetInScene(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedPlanetIndex]))
                    {
                        _weatherManager.CelestialObjectsController.AddPlanet(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedPlanetIndex]);
                    }
                }
                else
                {
                    if (_weatherManager.CelestialObjectsController.IsPlanetInScene(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedPlanetIndex]))
                    {
                        _weatherManager.CelestialObjectsController.DestroyPlanet(_weatherManager.CelestialObjectsController.PlanetsDataContainer.PlanetsPresets[_selectedPlanetIndex]);
                    }
                }
            });
        }
        #endregion
    }
}