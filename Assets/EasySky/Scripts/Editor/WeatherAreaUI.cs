//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System.Collections.Generic;
using System.Linq;
using EasySky.Clouds;
using EasySky.Particles;
using EasySky.WeatherArea;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasySky.Editor
{
    public class WeatherAreaUI
    {
        #region Private Variables
        private VisualElement _root;
        private EasySkyWeatherManager _weatherManager;
        private int _selectedAreaIndex = -1;
        private int _selectedPresetIndex = -1;
        private WeatherAreaData _selectedAreaData;
        private WeatherPresetData _selectedPresetData;
        private Button _previousSelectedButton;
        private Button _previousSelectedPresetButton;
        private List<Button> _areaButtons = new List<Button>();
        private List<Button> _presetsButtons = new List<Button>();
        private Color _selectedButtonColor = new Color(0.2627f, 0.2627f, 0.2627f);
        private Color _unselectedButtonColor = new Color(0.4392f, 0.4392f, 0.4392f);
        private Button _standardParticleButon;
        private Button _vfxParticleButton;
        private int _selectedParticle;
        private int _selectedStandardParticle;
        private Toggle _particleEnabled;
        private Slider _particleIntensity;
        private ColorField _particleColor;
        private Toggle _particleCameraEffects;
        private Toggle _particleWindInteraction;
        private VisualElement _particleParent;
        private VisualElement _fogParent;
        private VisualElement _duststormParent;
        private Toggle _fogEnable;
        private ColorField _fogColor;
        private FloatField _fogMaxDistance;
        private FloatField _fogAttenuation;
        private Toggle _duststormEnable;
        private ColorField _duststormColor;
        private Slider _duststormSpawnRate;
        private Slider _duststormParticleSize;
        private VisualElement _vfxParticleParent;
        private VisualElement _standarParticleParent;
        private Toggle _standarParticleEnabled;
        private Slider _standardParticleIntensity;
        private ColorField _standardParticleColor;
        private Toggle _standarParticleWindInteraction;
        private Slider _coverage;
        private Slider _altitude;
        private Slider _thicknes;
        private ColorField _cloudColor;
        private ObjectField _cloudPreset;
        private Slider _coverage2DClouds;
        private ColorField _cloudColor2DClouds;
        private ObjectField _cloudPreset2DClouds;
        private Button _layerCloudButton;
        private Button _volumetricCloudButton;
        private Toggle _volumetricCloudWind;
        private Toggle _layerCloudWind;
        private FloatField _fogBaseheight;
        private FloatField _fogMaxHeight;
        private Slider _layerCloudAltitude;
        private Toggle _layerCloudShadows;
        private Toggle _volumetricCastShadows;
        private ColorField _weatherPresetColor;
        private TextField _weatherPresetName;
        private int _previousSelectedPresetImageIndex;
        private Button _previousSelectedPresetImage;
        private ColorField _areaColor;
        private TextField _areaName;
        private int _previousSelectedAreaImageIndex = -1;
        private VisualElement _previousSelectedAreaImage;
        private VisualElement _areaInputParent;
        private VisualElement _presetInputParent;
        private Button _advancedParticleButton;
        private TextField _volumetricPresetName;
        private TextField _layerPresetName;
        #endregion

        #region Constructors
        public WeatherAreaUI(VisualElement root, EasySkyWeatherManager weatherManager)
        {
            _root = root;
            _weatherManager = weatherManager;
        }
        #endregion

        #region Public Methods
        public void PopulateWeatherAreaUI()
        {
            _weatherManager.OnWeatherDataUpdated += OnDataUpdated;
            _weatherManager.OnPresetUpdated += OnPresetUpdated;

            _advancedParticleButton = _root.Q<Button>("ParticlesAdvancedSettingsButton");
            _advancedParticleButton.RegisterCallback<MouseUpEvent>((evt) => OpenParticleEditor());

            var advancedStandardParticleButton = _root.Q<Button>("StandardParticlesAdvancedSettingsButton");
            advancedStandardParticleButton.RegisterCallback<MouseUpEvent>((evt) => OpenStandardParticleEditor());

            PopulateParticleEditor();
            PopulateWeatherPresets();
            PopulateWeatherAreas();
            PopulateVolumetricCloudEditor();
            PopulateLayerCloudEditor();
        }
        #endregion

        #region Private Methods
        private void OpenParticleEditor()
        {
            switch (_selectedParticle)
            {
                case 0: //rain
                    RainAdvancedSettings.Instance.InitializeForm(_weatherManager, _selectedPresetData, EnableRootInteraction);
                    break;
                case 1: //snow
                    SnowAdvancedSettings.Instance.InitializeForm(_weatherManager, _selectedPresetData, EnableRootInteraction);
                    break;
                case 2: //hail
                    HailAdvancedSettings.Instance.InitializeForm(_weatherManager, _selectedPresetData, EnableRootInteraction);
                    break;
                case 4: //duststorm
                    DuststormAdvancedSettings.Instance.InitializeForm(_weatherManager, _selectedPresetData, EnableRootInteraction);
                    break;
                default:
                    return;
            }
            _root.SetEnabled(false);
        }

        private void EnableRootInteraction()
        {
            _root.SetEnabled(true);
        }

        private void OpenStandardParticleEditor()
        {
            StandardParticleAdvancedEditor.Instance.InitializeForm(_weatherManager, _selectedPresetData, _selectedStandardParticle, EnableRootInteraction);
        }

        private void OnDataUpdated()
        {
            PopulateStandardParticleData();
            PopulateParticleData();
            PopulateFogData();
            PopulateDuststormData();
            UpdateCloudData();
        }

        private void OnPresetUpdated(int index)
        {
            ChangePresetButtonState(_presetsButtons[index]);
        }

        private void PopulateParticleEditor()
        {
            _vfxParticleParent = _root.Q<VisualElement>("VfxParticleParent");
            _standarParticleParent = _root.Q<VisualElement>("StandardParticleParent");

            _particleParent = _root.Q<VisualElement>("ParticleParent");
            _fogParent = _root.Q<VisualElement>("FogParent");
            _duststormParent = _root.Q<VisualElement>("DuststormParent");

            _standardParticleButon = _root.Q<Button>("StandarParticleButton");
            _vfxParticleButton = _root.Q<Button>("VfxParticleButton");

            _standardParticleButon.RegisterCallback<MouseUpEvent>((evt) =>
            {
                _standardParticleButon.style.backgroundColor = _selectedButtonColor;
                _vfxParticleButton.style.backgroundColor = _unselectedButtonColor;
                _standarParticleParent.style.display = DisplayStyle.Flex;
                _vfxParticleParent.style.display = DisplayStyle.None;
                PopulateStandardParticleData();
            });

            _vfxParticleButton.RegisterCallback<MouseUpEvent>(evt =>
            {
                _vfxParticleButton.style.backgroundColor = _selectedButtonColor;
                _standardParticleButon.style.backgroundColor = _unselectedButtonColor;
                _standarParticleParent.style.display = DisplayStyle.None;
                _vfxParticleParent.style.display = DisplayStyle.Flex;
                PopulateParticleData();
            });

            RegisterVfxParticles();
            RegisterStandardParticle();
        }

        private void RegisterStandardParticle()
        {
            _standarParticleEnabled = _root.Q<Toggle>("StandardParticleEnabled");
            _standarParticleEnabled.RegisterCallback<ChangeEvent<bool>>((evt) => ChangeStandardParticleState());

            _standardParticleIntensity = _root.Q<Slider>("StandardParticleIntensity");
            _standardParticleIntensity.RegisterCallback<ChangeEvent<float>>((evt) => ChangeStandardParticleState());

            _standardParticleColor = _root.Q<ColorField>("StandardParticleColor");
            _standardParticleColor.RegisterCallback<ChangeEvent<Color>>((evt) => ChangeStandardParticleState());

            _standarParticleWindInteraction = _root.Q<Toggle>("StandardParticleWindInteraction");
            _standarParticleWindInteraction.RegisterCallback<ChangeEvent<bool>>((evt) => ChangeStandardParticleState());

            var particleDropdown = _root.Q<DropdownField>("StandardParticleDropdown");

            particleDropdown.RegisterCallback<ChangeEvent<string>>((evt =>
            {
                _selectedStandardParticle = particleDropdown.index;
                PopulateStandardParticleData();
            }));
        }

        private void PopulateStandardParticleData()
        {
            if (_selectedPresetData == null)
            {
                return;
            }

            switch (_selectedStandardParticle)
            {
                case 0: //rain
                    PopulateStandardParticle(_selectedPresetData.StandarRainData);
                    break;
                case 1: //snow
                    PopulateStandardParticle(_selectedPresetData.StandarSnowData);
                    break;
                case 2: //hail
                    PopulateStandardParticle(_selectedPresetData.StandarHailData);
                    break;
                case 3: //duststorm
                    PopulateStandardParticle(_selectedPresetData.StandardDuststormData);
                    break;
            }

            EditorUtility.SetDirty(_selectedPresetData);
        }

        private void PopulateStandardParticle(StandardParticleData particleData)
        {
            _standarParticleEnabled.SetValueWithoutNotify(particleData.isActive);
            _standardParticleIntensity.SetValueWithoutNotify(particleData.intensity);
            _standardParticleColor.SetValueWithoutNotify(particleData.particleColor);
            _standarParticleWindInteraction.SetValueWithoutNotify(particleData.isWindInteractionActive);
        }

        private void ChangeStandardParticleState()
        {
            switch (_selectedStandardParticle)
            {
                case 0: //rain
                    SetStandarParticleData(_selectedPresetData.StandarRainData);
                    _weatherManager.WeatherEffectsController.RainPsController.ApplyData(_selectedPresetData.StandarRainData);
                    break;
                case 1: //snow
                    SetStandarParticleData(_selectedPresetData.StandarSnowData);
                    _weatherManager.WeatherEffectsController.SnowPsController.ApplyData(_selectedPresetData.StandarSnowData);
                    break;
                case 2: //hail
                    SetStandarParticleData(_selectedPresetData.StandarHailData);
                    _weatherManager.WeatherEffectsController.HailPsController.ApplyData(_selectedPresetData.StandarHailData);
                    break;
                case 3: //duststorm
                    SetStandarParticleData(_selectedPresetData.StandardDuststormData);
                    _weatherManager.WeatherEffectsController.DuststormPsController.ApplyData(_selectedPresetData.StandardDuststormData);
                    break;
            }

            EditorUtility.SetDirty(_selectedPresetData);
        }

        private void SetStandarParticleData(StandardParticleData particleData)
        {
            particleData.isActive = _standarParticleEnabled.value;
            particleData.intensity = _standardParticleIntensity.value;
            particleData.particleColor = _standardParticleColor.value;
            particleData.isWindInteractionActive = _standarParticleWindInteraction.value;
        }

        private void RegisterVfxParticles()
        {
            _particleEnabled = _root.Q<Toggle>("ParticleEnabled");
            _particleEnabled.RegisterCallback<ChangeEvent<bool>>((evt) => ChangeParticleState());

            _particleIntensity = _root.Q<Slider>("ParticleIntensity");
            _particleIntensity.RegisterCallback<ChangeEvent<float>>((evt) => ChangeParticleState());

            _particleColor = _root.Q<ColorField>("ParticleColor");
            _particleColor.RegisterCallback<ChangeEvent<Color>>((evt) => ChangeParticleState());

            _particleCameraEffects = _root.Q<Toggle>("ParticleCameraEffects");
            _particleCameraEffects.RegisterCallback<ChangeEvent<bool>>((evt) => ChangeParticleState());

            _particleWindInteraction = _root.Q<Toggle>("ParticleWindInteraction");
            _particleWindInteraction.RegisterCallback<ChangeEvent<bool>>((evt) => ChangeParticleState());

            var particleDropdown = _root.Q<DropdownField>("ParticleDropdown");

            particleDropdown.RegisterCallback<ChangeEvent<string>>((evt =>
            {
                _selectedParticle = particleDropdown.index;
                PopulateParticleData();
            }));

            _selectedParticle = particleDropdown.index;
            PopulateParticleData();

            _fogEnable = _root.Q<Toggle>("FogEnabled");
            _fogEnable.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                if (_selectedPresetData == null) return;
                _selectedPresetData.FogData.isEnabled = evt.newValue;
                _weatherManager.WeatherEffectsController.FogController.ApplyData(_selectedPresetData.FogData);
            });

            _fogColor = _root.Q<ColorField>("FogColor");
            _fogColor.RegisterCallback<ChangeEvent<Color>>((evt) =>
            {
                if (_selectedPresetData == null) return;
                _selectedPresetData.FogData.color = evt.newValue;
                _weatherManager.WeatherEffectsController.FogController.ApplyData(_selectedPresetData.FogData);
            });

            _fogMaxDistance = _root.Q<FloatField>("MaxFogDistance");
            _fogMaxDistance.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                if (_selectedPresetData == null) return;
                _selectedPresetData.FogData.maxFogDistance = evt.newValue;
                _weatherManager.WeatherEffectsController.FogController.ApplyData(_selectedPresetData.FogData);
            });

            _fogAttenuation = _root.Q<FloatField>("FogAttenuationDistance");
            _fogAttenuation.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                if (_selectedPresetData == null) return;
                _selectedPresetData.FogData.attenuationDistance = evt.newValue;
                _weatherManager.WeatherEffectsController.FogController.ApplyData(_selectedPresetData.FogData);
            });

            _fogBaseheight = _root.Q<FloatField>("FogBaseHeight");
            _fogBaseheight.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                if (_selectedPresetData == null) return;
                _selectedPresetData.FogData.baseHeight = evt.newValue;
                _weatherManager.WeatherEffectsController.FogController.ApplyData(_selectedPresetData.FogData);
            });

            _fogMaxHeight = _root.Q<FloatField>("FogMaxHeight");
            _fogMaxHeight.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                if (_selectedPresetData == null) return;
                _selectedPresetData.FogData.maxHeight = evt.newValue;
                _weatherManager.WeatherEffectsController.FogController.ApplyData(_selectedPresetData.FogData);
            });

            PopulateFogData();

            _duststormEnable = _root.Q<Toggle>("DuststormEnabled");
            _duststormEnable.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                if (_selectedPresetData == null) return;
                _selectedPresetData.DuststormData.isActive = evt.newValue;
                _weatherManager.WeatherEffectsController.DuststormController.ApplyData(_selectedPresetData.DuststormData);
            });

            _duststormColor = _root.Q<ColorField>("DuststormColor");
            _duststormColor.RegisterCallback<ChangeEvent<Color>>((evt) =>
            {
                if (_selectedPresetData == null) return;
                _selectedPresetData.DuststormData.particleColor = evt.newValue;
                _weatherManager.WeatherEffectsController.DuststormController.ApplyData(_selectedPresetData.DuststormData);
            });

            _duststormSpawnRate = _root.Q<Slider>("DuststormSpawnrate");
            _duststormSpawnRate.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                if (_selectedPresetData == null) return;
                _selectedPresetData.DuststormData.intensity = evt.newValue;
                _weatherManager.WeatherEffectsController.DuststormController.ApplyData(_selectedPresetData.DuststormData);
            });

            _duststormParticleSize = _root.Q<Slider>("DuststormParticleSize");
            _duststormParticleSize.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                if (_selectedPresetData == null) return;
                _selectedPresetData.DuststormData.particleSize = evt.newValue;
                _weatherManager.WeatherEffectsController.DuststormController.ApplyData(_selectedPresetData.DuststormData);
            });

            PopulateDuststormData();
        }

        private void PopulateFogData()
        {
            if (_selectedPresetData == null) return;

            _fogEnable.SetValueWithoutNotify(_selectedPresetData.FogData.isEnabled);
            _fogColor.SetValueWithoutNotify(_selectedPresetData.FogData.color);
            _fogMaxDistance.SetValueWithoutNotify(_selectedPresetData.FogData.maxFogDistance);
            _fogAttenuation.SetValueWithoutNotify(_selectedPresetData.FogData.attenuationDistance);
            _fogBaseheight.SetValueWithoutNotify(_selectedPresetData.FogData.baseHeight);
            _fogMaxHeight.SetValueWithoutNotify(_selectedPresetData.FogData.maxHeight);
            _weatherManager.WeatherEffectsController.FogController.ApplyData(_selectedPresetData.FogData);
        }

        private void PopulateDuststormData()
        {
            if (_selectedPresetData == null) return;

            _duststormEnable.SetValueWithoutNotify(_selectedPresetData.DuststormData.isActive);
            _duststormColor.SetValueWithoutNotify(_selectedPresetData.DuststormData.particleColor);
            _duststormSpawnRate.SetValueWithoutNotify(_selectedPresetData.DuststormData.intensity);
            _duststormParticleSize.SetValueWithoutNotify(_selectedPresetData.DuststormData.particleSize);
        }

        private void PopulateParticleData()
        {
            if (_selectedPresetData == null) return;

            switch (_selectedParticle)
            {
                case 0: //rain
                    PopulateParticleInput(_selectedPresetData.RainData);
                    _particleCameraEffects.style.display = DisplayStyle.Flex;
                    _particleCameraEffects.value = _selectedPresetData.RainData.isFullscreenEffectEnabled;
                    break;
                case 1: //snow
                    PopulateParticleInput(_selectedPresetData.SnowData);
                    _particleCameraEffects.style.display = DisplayStyle.Flex;
                    _particleCameraEffects.value = _selectedPresetData.SnowData.isFullscreenEffectEnabled;
                    break;
                case 2: //hail
                    PopulateParticleInput(_selectedPresetData.HailData);
                    _particleCameraEffects.style.display = DisplayStyle.None;
                    break;
                case 3: //fog
                    _particleParent.style.display = DisplayStyle.None;
                    _fogParent.style.display = DisplayStyle.Flex;
                    _duststormParent.style.display = DisplayStyle.None;
                    _advancedParticleButton.style.display = DisplayStyle.None;
                    break;
                case 4: //duststorm
                    _particleParent.style.display = DisplayStyle.None;
                    _fogParent.style.display = DisplayStyle.None;
                    _duststormParent.style.display = DisplayStyle.Flex;
                    _advancedParticleButton.style.display = DisplayStyle.Flex;
                    break;
            }
        }

        private void PopulateParticleInput(WeatherEffectData data)
        {
            _particleEnabled.SetValueWithoutNotify(data.isActive);
            _particleIntensity.SetValueWithoutNotify(data.intensity);
            _particleColor.SetValueWithoutNotify(data.particleColor);
            _particleWindInteraction.SetValueWithoutNotify(data.isWindInteractionActive);
            _particleParent.style.display = DisplayStyle.Flex;
            _fogParent.style.display = DisplayStyle.None;
            _duststormParent.style.display = DisplayStyle.None;
            _advancedParticleButton.style.display = DisplayStyle.Flex;
        }

        private void ChangeParticleState()
        {
            if (_selectedPresetData == null) return;

            switch (_selectedParticle)
            {
                case 0: //rain
                    SetParticleData(_selectedPresetData.RainData);
                    _selectedPresetData.RainData.isFullscreenEffectEnabled = _particleCameraEffects.value;
                    _weatherManager.WeatherEffectsController.RainController.ApplyData(_selectedPresetData.RainData);
                    _weatherManager.FullscreenEffectController.ChangeRainEffectState(_particleCameraEffects.value);
                    break;
                case 1: //snow
                    SetParticleData(_selectedPresetData.SnowData);
                    _selectedPresetData.SnowData.isFullscreenEffectEnabled = _particleCameraEffects.value;
                    _weatherManager.WeatherEffectsController.SnowController.ApplyData(_selectedPresetData.SnowData);
                    _weatherManager.FullscreenEffectController.ChangeIceEffectState(_particleCameraEffects.value);
                    break;
                case 2: //hail
                    SetParticleData(_selectedPresetData.HailData);
                    _weatherManager.WeatherEffectsController.HailController.ApplyData(_selectedPresetData.HailData);
                    break;
            }

            EditorUtility.SetDirty(_selectedPresetData);
        }

        private void SetParticleData(WeatherEffectData data)
        {
            data.isActive = _particleEnabled.value;
            data.intensity = _particleIntensity.value;
            data.particleColor = _particleColor.value;
            data.isWindInteractionActive = _particleWindInteraction.value;
            EditorUtility.SetDirty(_selectedPresetData);
        }

        private void ApplyParticleData()
        {
            _weatherManager.WeatherEffectsController.RainController.ApplyData(_selectedPresetData.RainData);
            _weatherManager.FullscreenEffectController.ApplyDataToRain(_selectedPresetData.RainData);
            _weatherManager.WeatherEffectsController.SnowController.ApplyData(_selectedPresetData.SnowData);
            _weatherManager.FullscreenEffectController.ApplyDataToSnow(_selectedPresetData.SnowData);
            _weatherManager.FullscreenEffectController.ApplyDataToSnowLayer(_selectedPresetData.SnowData);
            _weatherManager.WeatherEffectsController.HailController.ApplyData(_selectedPresetData.HailData);
            _weatherManager.WeatherEffectsController.RainPsController.ApplyData(_selectedPresetData.StandarRainData);
            _weatherManager.WeatherEffectsController.SnowPsController.ApplyData(_selectedPresetData.StandarSnowData);
            _weatherManager.WeatherEffectsController.HailPsController.ApplyData(_selectedPresetData.StandarHailData);
            _weatherManager.WeatherEffectsController.DuststormPsController.ApplyData(_selectedPresetData.StandardDuststormData);
        }

        private void PopulateVolumetricCloudEditor()
        {
            var advanceEditCloudButton = _root.Q<Button>("AdvancedEditVolumetricPreset");

            advanceEditCloudButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                _root.SetEnabled(false);
                VolumetricCloudsAdvanceEditor.Instance.InitializeForm(_weatherManager, _selectedPresetData, EnableRootInteraction);
            });

            var newCloudPresetButton = _root.Q<Button>("CreateNewVolumetricPreset");

            newCloudPresetButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                if (_cloudPreset == null) return;
                _weatherManager.WeatherPresetsController.AddNewCloudPreset<VolumetricCloudPresetData, VolumetricCloudData>(_weatherManager.WeatherPresetsController.VolumetricCloudData, "Volumetric");
                _cloudPreset.value = _weatherManager.WeatherPresetsController.VolumetricCloudData[_weatherManager.WeatherPresetsController.VolumetricCloudData.Count - 1];
            });

            _volumetricPresetName = _root.Q<TextField>("VolumetricCloudPresetName");
            _volumetricPresetName.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.name);

            _volumetricPresetName.RegisterCallback<ChangeEvent<string>>((evt) =>
            {
                var asset = AssetDatabase.FindAssets($"{_selectedPresetData.VolumetricCloudPresetData.name} t:{typeof(VolumetricCloudPresetData)}");
                var path = AssetDatabase.GUIDToAssetPath(asset[0]);
                AssetDatabase.RenameAsset(path, evt.newValue);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            });

            _layerCloudButton = _root.Q<Button>("LayerCloudsButton");
            _volumetricCloudButton = _root.Q<Button>("VolumetricCloudButton");

            _layerCloudButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                _root.Q<VisualElement>("VolumetricParent").style.display = DisplayStyle.None;
                _root.Q<VisualElement>("LayerParent").style.display = DisplayStyle.Flex;
                _layerCloudButton.style.backgroundColor = _selectedButtonColor;
                _volumetricCloudButton.style.backgroundColor = _unselectedButtonColor;
            });

            _volumetricCloudButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                _root.Q<VisualElement>("VolumetricParent").style.display = DisplayStyle.Flex;
                _root.Q<VisualElement>("LayerParent").style.display = DisplayStyle.None;
                _layerCloudButton.style.backgroundColor = _unselectedButtonColor;
                _volumetricCloudButton.style.backgroundColor = _selectedButtonColor;
            });

            _cloudPreset = _root.Q<ObjectField>("CloudPresetCeva");
            _cloudPreset.objectType = typeof(VolumetricCloudPresetData);
            _cloudPreset.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData);

            _coverage = _root.Q<Slider>("Coverage3D");
            _coverage.value = _selectedPresetData.VolumetricCloudPresetData.cloudData.cloudDensityMultiplier;

            _coverage.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData.cloudData.cloudDensityMultiplier = evt.newValue;
                _weatherManager.CloudsController.VolumetricCloudDensityMultyplier = evt.newValue;
                EditorUtility.SetDirty(_selectedPresetData);
            });

            _altitude = _root.Q<Slider>("Altitude3D");
            _altitude.value = _selectedPresetData.VolumetricCloudPresetData.cloudData.cloudAltitude;

            _altitude.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData.cloudData.cloudAltitude = evt.newValue;
                _weatherManager.CloudsController.CloudAltitude = evt.newValue;
                EditorUtility.SetDirty(_selectedPresetData);
            });

            _thicknes = _root.Q<Slider>("Thickness");
            _thicknes.value = _selectedPresetData.VolumetricCloudPresetData.cloudData.cloudThickness;

            _thicknes.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData.cloudData.cloudThickness = evt.newValue;
                _weatherManager.CloudsController.CloudThickness = evt.newValue;
                EditorUtility.SetDirty(_selectedPresetData);
            });

            _cloudColor = _root.Q<ColorField>("CloudColor3D");
            _cloudColor.value = _selectedPresetData.VolumetricCloudPresetData.cloudData.cloudScatteringTint;

            _cloudColor.RegisterCallback<ChangeEvent<Color>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData.cloudData.cloudScatteringTint = evt.newValue;
                _weatherManager.CloudsController.VolumetricScatteringTint = evt.newValue;
                EditorUtility.SetDirty(_selectedPresetData);
            });

            _volumetricCloudWind = _root.Q<Toggle>("VolumetricCloudWind");
            _volumetricCloudWind.value = _selectedPresetData.VolumetricCloudPresetData.cloudData.isWindInteractionActive;
            _volumetricCloudWind.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData.cloudData.isWindInteractionActive = evt.newValue;
                _weatherManager.CloudsController.EnableVolumetricCloudWind(evt.newValue);
                EditorUtility.SetDirty(_selectedPresetData);
            });

            _layerCloudWind = _root.Q<Toggle>("LayerCloudWind");
            _layerCloudWind.value = _selectedPresetData.LayerCloudPresetData.cloudData.isWindInteractionActive;
            _layerCloudWind.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                _selectedPresetData.LayerCloudPresetData.cloudData.isWindInteractionActive = evt.newValue;
                _weatherManager.CloudsController.EnableLayerCloudWind(evt.newValue);
                EditorUtility.SetDirty(_selectedPresetData);
            });

            _volumetricCastShadows = _root.Q<Toggle>("VolumetricCloudsShadows");
            _volumetricCastShadows.value = _selectedPresetData.VolumetricCloudPresetData.cloudData.areShadowsEnabled;
            _volumetricCastShadows.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                _selectedPresetData.VolumetricCloudPresetData.cloudData.areShadowsEnabled = evt.newValue;
                _weatherManager.CloudsController.EnableVolumetricCloudsShadows = evt.newValue;
                EditorUtility.SetDirty(_selectedPresetData);
            });

            _cloudPreset.RegisterCallback<ChangeEvent<UnityEngine.Object>>((evt) =>
            {
                if (_cloudPreset.value == null) return;

                var preset = _cloudPreset.value as VolumetricCloudPresetData;

                if (preset == null)
                {
                    _cloudPreset.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData);
                    return;
                }
                _selectedPresetData.VolumetricCloudPresetData = preset;
                _selectedAreaData.presetData = _selectedPresetData;
                _coverage.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.cloudDensityMultiplier);
                _altitude.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.cloudAltitude);
                _thicknes.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.cloudThickness);
                _cloudColor.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.cloudScatteringTint);
                _volumetricCloudWind.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.isWindInteractionActive);
                _layerCloudWind.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.isWindInteractionActive);
                _volumetricCastShadows.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.areShadowsEnabled);
                _weatherManager.CloudsController.SetVolumetricCloudPreset(_selectedPresetData.VolumetricCloudPresetData);
                _volumetricPresetName.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.name);
            });
        }

        private void PopulateLayerCloudEditor()
        {
            var advanceEditLayerClouds = _root.Q<Button>("AdvancedEditLayerPreset");

            advanceEditLayerClouds.RegisterCallback<MouseUpEvent>((evt) =>
            {
                _root.SetEnabled(false);
                LayerCloudsAdvanceEditor.Instance.InitializeForm(_weatherManager, _selectedPresetData, EnableRootInteraction);
            });

            var newCloudPresetButton = _root.Q<Button>("CreateNewLayerPreset");

            newCloudPresetButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                _weatherManager.WeatherPresetsController.AddNewCloudPreset<LayerCloudPresetData, LayerCloudData>(_weatherManager.WeatherPresetsController.LayerCloudData, "Layer");
                _cloudPreset2DClouds.value = _weatherManager.WeatherPresetsController.LayerCloudData[_weatherManager.WeatherPresetsController.LayerCloudData.Count - 1];
            });

            _layerPresetName = _root.Q<TextField>("LayerPresetName");
            _layerPresetName.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.name);
            _layerPresetName.RegisterCallback<ChangeEvent<string>>((evt) =>
            {
                var asset = AssetDatabase.FindAssets($"{_selectedPresetData.LayerCloudPresetData.name} t:{typeof(LayerCloudPresetData)}");
                var path = AssetDatabase.GUIDToAssetPath(asset[0]);
                AssetDatabase.RenameAsset(path, evt.newValue);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            });

            _cloudPreset2DClouds = _root.Q<ObjectField>("LayerCloudPreset");
            _cloudPreset2DClouds.objectType = typeof(LayerCloudPresetData);
            _cloudPreset2DClouds.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData);

            _coverage2DClouds = _root.Q<Slider>("CoverageLayer");
            _coverage2DClouds.value = _selectedPresetData.LayerCloudPresetData.cloudData.cloudLayerCoverage;

            _coverage2DClouds.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.LayerCloudPresetData.cloudData.cloudLayerCoverage = evt.newValue;
                _weatherManager.CloudsController.LayerCloudsOpacity = evt.newValue;
                EditorUtility.SetDirty(_selectedPresetData.LayerCloudPresetData);
            });

            _cloudColor2DClouds = _root.Q<ColorField>("CloudColorLayer");
            _cloudColor2DClouds.value = _selectedPresetData.LayerCloudPresetData.cloudData.tintLayer2D;

            _cloudColor2DClouds.RegisterCallback<ChangeEvent<Color>>((evt) =>
            {
                _selectedPresetData.LayerCloudPresetData.cloudData.tintLayer2D = evt.newValue;
                _weatherManager.CloudsController.TintLayer2D = evt.newValue;
                EditorUtility.SetDirty(_selectedPresetData.LayerCloudPresetData);
            });

            _layerCloudAltitude = _root.Q<Slider>("AltitudeLayer");
            _layerCloudAltitude.value = _selectedPresetData.LayerCloudPresetData.cloudData.cloudAltitude;
            _layerCloudAltitude.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _selectedPresetData.LayerCloudPresetData.cloudData.cloudAltitude = evt.newValue;
                _weatherManager.CloudsController.CloudLayerAltitude = evt.newValue;
                EditorUtility.SetDirty(_selectedPresetData.LayerCloudPresetData);
            });

            _layerCloudShadows = _root.Q<Toggle>("LayerCloudsShadows");
            _layerCloudShadows.value = _selectedPresetData.LayerCloudPresetData.cloudData.areShadowsEnabled;
            _layerCloudShadows.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                _selectedPresetData.LayerCloudPresetData.cloudData.areShadowsEnabled = evt.newValue;
                _weatherManager.CloudsController.CloudLayerCastShadows = evt.newValue;
                EditorUtility.SetDirty(_selectedPresetData.LayerCloudPresetData);
            });

            _layerCloudWind = _root.Q<Toggle>("LayerCloudWind");
            _layerCloudWind.value = _selectedPresetData.LayerCloudPresetData.cloudData.isWindInteractionActive;
            _layerCloudWind.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                _selectedPresetData.LayerCloudPresetData.cloudData.isWindInteractionActive = evt.newValue;
                _weatherManager.CloudsController.EnableLayerCloudWind(false);
                EditorUtility.SetDirty(_selectedPresetData.LayerCloudPresetData);
            });

            _cloudPreset2DClouds.RegisterCallback<ChangeEvent<UnityEngine.Object>>((evt) =>
            {
                if (_cloudPreset2DClouds.value == null) return;

                var cloudPreset = _cloudPreset2DClouds.value as LayerCloudPresetData;
                if (cloudPreset == null)
                {
                    _cloudPreset2DClouds.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData);
                    return;
                }
                _selectedPresetData.LayerCloudPresetData = cloudPreset;
                _coverage2DClouds.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.cloudData.cloudLayerCoverage);
                _cloudColor2DClouds.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.cloudData.tintLayer2D);
                _layerCloudAltitude.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.cloudData.cloudAltitude);
                _layerPresetName.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.name);
            });
        }

        private void UpdateCloudData()
        {
            if (_coverage == null) return;

            _altitude.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.cloudAltitude);
            _thicknes.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.cloudThickness);
            _cloudColor.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.cloudScatteringTint);
            _coverage2DClouds.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.cloudData.cloudLayerCoverage);
            _cloudColor2DClouds.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.cloudData.tintLayer2D);
            _cloudPreset.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData);
            _cloudPreset2DClouds.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData);
            _volumetricCastShadows.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.areShadowsEnabled);
            _volumetricCloudWind.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.isWindInteractionActive);
            _layerCloudAltitude.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.cloudData.cloudAltitude);
            _layerCloudShadows.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.cloudData.areShadowsEnabled);
            _layerCloudWind.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.cloudData.isWindInteractionActive);
            _weatherManager.CloudsController.SetVolumetricCloudPreset(_selectedPresetData.VolumetricCloudPresetData);
            _weatherManager.CloudsController.SetLayerCloudPreset(_selectedPresetData.LayerCloudPresetData);
            _coverage.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.cloudData.cloudDensityMultiplier);
            _volumetricPresetName.SetValueWithoutNotify(_selectedPresetData.VolumetricCloudPresetData.name);
            _layerPresetName.SetValueWithoutNotify(_selectedPresetData.LayerCloudPresetData.name);
        }

        private void PopulateWeatherPresets()
        {
            _weatherManager.WeatherPresetsController.LoadAllWeatherDatas();
            _weatherManager.WeatherPresetsController.WeatherPresetsData = _weatherManager.WeatherPresetsController.WeatherPresetsData.OrderBy(x => x.PresetName).ToList();
            var newWeatherPreset = _root.Q<Button>("CreateNewWeatherPreset");
            _presetInputParent = _root.Q<VisualElement>("PresetInputParent");
            _presetInputParent.SetEnabled(false);

            var imagesParent = _root.Q<VisualElement>("PresetsImageParent");

            for (int i = 0; i < _weatherManager.PresetsImages.WeatherPresetsImages.Count; i++)
            {
                var imageHolder = _weatherManager.PresetsImages.ImageLable.Instantiate();
                imagesParent.Add(imageHolder);

                var button = imageHolder.Q<Button>("ImageSelecorButton");
                imageHolder.Q<IntegerField>(GlobalConstantData.IndexString).value = i;
                button.Q<VisualElement>("ImageHolder").style.backgroundImage = _weatherManager.PresetsImages.WeatherPresetsImages[i];
                button.RegisterCallback<MouseUpEvent>((evt) =>
                {
                    ChangeWeatherPresetImageStyle(button);
                });
            }

            _weatherPresetColor = _root.Q<ColorField>("PresetColor");
            _weatherPresetName = _root.Q<TextField>("PresetName");

            _weatherPresetColor.RegisterCallback<ChangeEvent<Color>>((evt) =>
            {
                _selectedPresetData.PresetColor = evt.newValue;
                _previousSelectedPresetButton.parent.Q<VisualElement>("Color").style.backgroundColor = _selectedPresetData.PresetColor;
                EditorUtility.SetDirty(_selectedPresetData);
            });

            _weatherPresetName.RegisterCallback<ChangeEvent<string>>((evt) =>
            {
                _selectedPresetData.PresetName = evt.newValue;
                _previousSelectedPresetButton.parent.Q<Label>(GlobalConstantData.TextString).text = _selectedPresetData.PresetName;
                EditorUtility.SetDirty(_selectedPresetData);
            });

            newWeatherPreset.RegisterCallback<MouseUpEvent>((evt) =>
            {
                _weatherManager.WeatherPresetsController.AddNewWeatherPreset();

                DeleteAllWeatherPresets();
                SpawnAllWetherPresets();
            });

            var deletePresetButton = _root.Q<Button>("DeleteWeatherPreset");

            deletePresetButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                if (_previousSelectedPresetButton == null) return;
                if (_presetsButtons.Count == 1) return;

                _previousSelectedPresetButton.parent.style.display = DisplayStyle.None;
                _previousSelectedPresetButton.parent.Clear();
                _presetsButtons.Remove(_previousSelectedPresetButton);

                for (int i = _selectedPresetIndex; i < _presetsButtons.Count; i++)
                {
                    _presetsButtons[i].Q<IntegerField>(GlobalConstantData.IndexString).value -= 1;
                }

                _previousSelectedPresetButton = null;
                _weatherManager.WeatherPresetsController.RemoveWeatherPreset(_selectedPresetIndex);
                _selectedPresetIndex = -1;

                if (_presetsButtons.Count > 0)
                {
                    ChangePresetButtonState(_presetsButtons[0]);
                }
                else
                {
                    _presetInputParent.SetEnabled(false);
                }
            });

            SpawnAllWetherPresets();
        }

        private void DeleteAllWeatherPresets()
        {
            for (int i = 0; i < _presetsButtons.Count; i++)
            {
                _presetsButtons[i].parent.style.display = DisplayStyle.None;
                _presetsButtons[i].parent.Clear();
            }

            _presetsButtons.Clear();
            _previousSelectedPresetButton = null;
        }

        private void SpawnAllWetherPresets()
        {
            var parent = _root.Q<VisualElement>("WeatherPresetsParent");
            for (int i = 0; i < _weatherManager.WeatherPresetsController.WeatherPresetsData.Count; i++)
            {
                var preset = _weatherManager.WeatherPresetsController.WeatherPresetLabel.Instantiate();
                var button = preset.Q<Button>("LableButton");

                button.Q<IntegerField>(GlobalConstantData.IndexString).value = i;
                button.RegisterCallback<MouseUpEvent>((evt) => { ChangePresetButtonState(button); });
                parent.Add(preset);
                _presetsButtons.Add(button);

                if (i == _weatherManager.SelectedWeatherPreset)
                {
                    ChangePresetButtonState(button);
                    _presetInputParent.SetEnabled(true);
                }

                button.parent.Q<Label>(GlobalConstantData.TextString).text = _weatherManager.WeatherPresetsController.WeatherPresetsData[i].PresetName;
                button.parent.Q<VisualElement>("Image").style.backgroundImage = _weatherManager.WeatherPresetsController.WeatherPresetsData[i].Icon;
                button.parent.Q<VisualElement>("Color").style.backgroundColor = _weatherManager.WeatherPresetsController.WeatherPresetsData[i].PresetColor;
            }
        }

        private void ChangeWeatherPresetImageStyle(Button button)
        {
            var index = button.parent.Q<IntegerField>(GlobalConstantData.IndexString).value;
            if (index == _previousSelectedPresetImageIndex) return;

            _previousSelectedPresetImageIndex = index;
            _selectedPresetData.Icon = _weatherManager.PresetsImages.WeatherPresetsImages[index];
            _previousSelectedPresetButton.parent.Q<VisualElement>("Image").style.backgroundImage = _selectedPresetData.Icon;
            ChangeImageSelectorState(button, true);

            if (_previousSelectedPresetImage != null)
            {
                ChangeImageSelectorState(_previousSelectedPresetImage, false);
            }
            EditorUtility.SetDirty(_selectedPresetData);
            _previousSelectedPresetImage = button;
        }

        private void PopulateWeatherAreas()
        {
            var newArea = _root.Q<Button>("CreateNewWeatherAreaButton");
            _areaColor = _root.Q<ColorField>("AreaColor");
            _areaName = _root.Q<TextField>("AreaName");
            _areaInputParent = _root.Q<VisualElement>("AreaInputParent");
            _areaInputParent.SetEnabled(false);

            var imagesParent = _root.Q<VisualElement>("ImageSelectorParent");

            for (int i = 0; i < _weatherManager.PresetsImages.AreaPresetsImages.Count; i++)
            {
                var imageHolder = _weatherManager.PresetsImages.ImageLable.Instantiate();
                imagesParent.Add(imageHolder);

                var button = imageHolder.Q<Button>("ImageSelecorButton");
                imageHolder.Q<IntegerField>(GlobalConstantData.IndexString).value = i;
                button.Q<VisualElement>("ImageHolder").style.backgroundImage = _weatherManager.PresetsImages.AreaPresetsImages[i];
                button.RegisterCallback<MouseUpEvent>((evt) =>
                {
                    ChangeAreaImageStyle(button);
                    EditorUtility.SetDirty(_weatherManager.WeatherAreaController.Areas[_selectedAreaIndex]);
                });
            }

            _areaColor.RegisterCallback<ChangeEvent<Color>>((evt) =>
            {
                _selectedAreaData.areaColor = evt.newValue;
                _previousSelectedButton.parent.Q<VisualElement>("Color").style.backgroundColor = _selectedAreaData.areaColor;
                _weatherManager.WeatherAreaController.Areas[_selectedAreaIndex].MeshRenderer.sharedMaterial.color = new Color(_selectedAreaData.areaColor.r, _selectedAreaData.areaColor.g, _selectedAreaData.areaColor.b, 0.5f);
                EditorUtility.SetDirty(_weatherManager.WeatherAreaController.Areas[_selectedAreaIndex]);
            });

            _areaName.RegisterCallback<ChangeEvent<string>>((evt) =>
            {
                _selectedAreaData.areaName = evt.newValue;
                _previousSelectedButton.parent.Q<Label>(GlobalConstantData.TextString).text = _selectedAreaData.areaName;
                EditorUtility.SetDirty(_weatherManager.WeatherAreaController.Areas[_selectedAreaIndex]);
            });

            newArea.RegisterCallback<MouseUpEvent>((evt) =>
            {
                var area = _weatherManager.WeatherAreaController.CreateNewWeatherArea();
                var parent = _root.Q<VisualElement>("AreasParent");
                var areaItem = _weatherManager.WeatherAreaController.WeatherAreaLabel.Instantiate();
                areaItem.Q<Label>(GlobalConstantData.TextString).text = _weatherManager.WeatherAreaController.Areas[_weatherManager.WeatherAreaController.Areas.Count - 1].WeatherAreaData.areaName;
                parent.Add(areaItem);
                var button = areaItem.Q<Button>("LableButton");

                button.Q<IntegerField>(GlobalConstantData.IndexString).value = _weatherManager.WeatherAreaController.Areas.Count - 1;
                button.RegisterCallback<MouseUpEvent>((evt) => { ChangeAreaButtonState(button); });
                button.parent.Q<VisualElement>("Image").style.backgroundImage = area.WeatherAreaData.areaIcon;
                button.parent.Q<VisualElement>("Color").style.backgroundColor = area.WeatherAreaData.areaColor;
                button.parent.Q<Label>(GlobalConstantData.TextString).text = area.WeatherAreaData.areaName;
                _areaButtons.Add(button);

                ChangeAreaButtonState(_areaButtons[_weatherManager.WeatherAreaController.Areas.Count - 1]);
                var tempMaterial = new Material(area.MeshRenderer.sharedMaterial);
                area.MeshRenderer.sharedMaterial = tempMaterial;
                area.MeshRenderer.sharedMaterial.color = new Color(area.WeatherAreaData.areaColor.r, area.WeatherAreaData.areaColor.g, area.WeatherAreaData.areaColor.b, 0.5f);
                _areaInputParent.SetEnabled(true);
            });

            var deleteAreaButton = _root.Q<Button>("DeleteWeatherAreaButton");

            deleteAreaButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                _previousSelectedButton.parent.style.display = DisplayStyle.None;
                _previousSelectedButton.parent.Clear();
                _areaButtons.Remove(_previousSelectedButton);

                for (int i = _selectedAreaIndex; i < _areaButtons.Count; i++)
                {
                    _areaButtons[i].Q<IntegerField>(GlobalConstantData.IndexString).value -= 1;
                }

                _weatherManager.WeatherAreaController.DeleteArea(_selectedAreaIndex);
                _selectedAreaIndex = -1;
                _previousSelectedButton = null;

                if (_weatherManager.WeatherAreaController.Areas.Count == 0)
                {
                    _selectedAreaData = _weatherManager.WeatherAreaController.GlobalPresetData;
                    _selectedPresetData = _selectedAreaData.presetData;
                }
                else
                {
                    ChangeAreaButtonState(_areaButtons[0]);
                }

                if (_areaButtons.Count == 0)
                {
                    _areaInputParent.SetEnabled(false);
                }
            });

            var editButton = _root.Q<Button>("EditWeatherAreaButton");

            editButton.RegisterCallback<MouseUpEvent>(evt => { AssetDatabase.OpenAsset(_weatherManager.WeatherAreaController.Areas[_selectedAreaIndex]); });

            var parent = _root.Q<VisualElement>("AreasParent");

            for (int i = 0; i < _weatherManager.WeatherAreaController.Areas.Count; i++)
            {
                var areaItem = _weatherManager.WeatherAreaController.WeatherAreaLabel.Instantiate();
                areaItem.Q<Label>(GlobalConstantData.TextString).text = _weatherManager.WeatherAreaController.Areas[i].WeatherAreaData.areaName;
                parent.Add(areaItem);

                var button = areaItem.Q<Button>("LableButton");

                button.Q<IntegerField>(GlobalConstantData.IndexString).value = i;
                button.RegisterCallback<MouseUpEvent>((evt) => { ChangeAreaButtonState(button); });
                _areaButtons.Add(button);
                button.parent.Q<VisualElement>("Image").style.backgroundImage = _weatherManager.WeatherAreaController.Areas[i].WeatherAreaData.areaIcon;
                button.parent.Q<VisualElement>("Color").style.backgroundColor = _weatherManager.WeatherAreaController.Areas[i].WeatherAreaData.areaColor;
                button.parent.Q<Label>(GlobalConstantData.TextString).text = _weatherManager.WeatherAreaController.Areas[i].WeatherAreaData.areaName;

                if (i == 0)
                {
                    ChangeAreaButtonState(button);
                    _areaInputParent.SetEnabled(true);
                }
            }

            if (_weatherManager.WeatherAreaController.Areas.Count == 0)
            {
                _selectedAreaData = _weatherManager.WeatherAreaController.GlobalPresetData;
                _selectedPresetData = _selectedAreaData.presetData;
            }
        }

        private void ChangeAreaImageStyle(Button button)
        {
            var index = button.parent.Q<IntegerField>(GlobalConstantData.IndexString).value;
            if (index == _previousSelectedAreaImageIndex) return;

            _previousSelectedAreaImageIndex = index;
            _selectedAreaData.areaIcon = _weatherManager.PresetsImages.AreaPresetsImages[index];
            _previousSelectedButton.parent.Q<VisualElement>("Image").style.backgroundImage = _selectedAreaData.areaIcon;
            ChangeImageSelectorState(button, true);

            if (_previousSelectedAreaImage != null)
            {
                ChangeImageSelectorState(_previousSelectedAreaImage, false);
            }

            _previousSelectedAreaImage = button;
        }

        private void ChangeImageSelectorState(VisualElement border, bool state)
        {
            var value = state ? 1 : 0;
            border.style.borderRightWidth = value;
            border.style.borderLeftWidth = value;
            border.style.borderBottomWidth = value;
            border.style.borderTopWidth = value;

            var color = new Color(GlobalConstantData.BorderColor.r, GlobalConstantData.BorderColor.g, GlobalConstantData.BorderColor.b, value);

            border.style.borderRightColor = color;
            border.style.borderLeftColor = color;
            border.style.borderTopColor = color;
            border.style.borderBottomColor = color;
        }

        private void ChangeAreaButtonState(Button button)
        {
            var index = button.Q<IntegerField>(GlobalConstantData.IndexString).value;
            if (index == _selectedAreaIndex) return;

            _selectedAreaIndex = index;
            if (_previousSelectedButton != null)
            {
                ChangeButtonStyle(_previousSelectedButton, false);
            }

            _selectedAreaData = _weatherManager.WeatherAreaController.Areas[_selectedAreaIndex].WeatherAreaData;
            _areaColor.SetValueWithoutNotify(_selectedAreaData.areaColor);
            _areaName.SetValueWithoutNotify(_selectedAreaData.areaName);
            _previousSelectedButton = button;
            ChangeButtonStyle(button, true);
            _selectedPresetData = _selectedAreaData.presetData;
            PopulateDuststormData();
            PopulateFogData();
            PopulateParticleData();
            UpdateCloudData();

            for (var i = 0; i <= _weatherManager.WeatherPresetsController.WeatherPresetsData.Count; i++)
            {
                if (_selectedAreaData.presetData == _weatherManager.WeatherPresetsController.WeatherPresetsData[i])
                {
                    ChangePresetButtonState(_presetsButtons[i]);
                    return;
                }
            }
            EditorUtility.SetDirty(_selectedPresetData);
        }

        private void ChangePresetButtonState(Button button)
        {
            var index = button.Q<IntegerField>(GlobalConstantData.IndexString).value;
            if (index == _selectedPresetIndex) return;

            _selectedPresetIndex = index;
            _weatherManager.SelectedWeatherPreset = index;
            if (_previousSelectedPresetButton != null)
            {
                ChangeButtonStyle(_previousSelectedPresetButton, false);
            }

            _selectedPresetData = _weatherManager.WeatherPresetsController.WeatherPresetsData[_selectedPresetIndex];
            _previousSelectedPresetButton = button;
            _weatherPresetColor.SetValueWithoutNotify(_selectedPresetData.PresetColor);
            _weatherPresetName.SetValueWithoutNotify(_selectedPresetData.PresetName);
            ChangeButtonStyle(button, true);
            PopulateParticleData();
            PopulateStandardParticleData();
            PopulateFogData();
            PopulateDuststormData();
            UpdateCloudData();
            ApplyParticleData();
            if (_selectedAreaData != null)
            {
                _selectedAreaData.presetData = _selectedPresetData;
            }

            EditorUtility.SetDirty(_selectedPresetData);
            EditorUtility.SetDirty(_weatherManager.WeatherAreaController);
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

            var color = new Color(GlobalConstantData.BorderColor.r, GlobalConstantData.BorderColor.g, GlobalConstantData.BorderColor.b, value);

            border.style.borderRightColor = color;
            border.style.borderLeftColor = color;
            border.style.borderTopColor = color;
            border.style.borderBottomColor = color;

            var selected = parent.Q<VisualElement>("SelectedImage");

            selected.style.visibility = state ? Visibility.Visible : Visibility.Hidden;
        }
        #endregion
    }
}