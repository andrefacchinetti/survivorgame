//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System;
using EasySky.Clouds;
using EasySky.Data;
using EasySky.Particles;
using EasySky.Skybox;
using EasySky.Utils;
using EasySky.WeatherArea;
using UnityEditor;
using UnityEngine;

namespace EasySky
{
    /// <summary>
    /// This class manages all controllers for clouds, weather effects, and celestial objects
    /// </summary>
    [ExecuteAlways]
    public class EasySkyWeatherManager : Singleton<EasySkyWeatherManager>
    {
        #region Constants
        private const string kMenuItemPath = "Easy Sky/Weather Manager";
        #endregion

        [SerializeField] private GameController gameController;

        #region Private Variables
        [SerializeField] private CloudsController _cloudsController;
        [SerializeField] private WeatherEffectsController _weatherEffectsController;
        [SerializeField] private StarsDataContainer _starsDataContainer;
        [SerializeField] private PlanetsDataContainer _planetsDataContainer;
        [SerializeField] private CelestialObjectsController _celestialObjectsController;
        [SerializeField] private WeatherAreasController _weatherAreaController;
        [SerializeField] private WeatherPresetsController _weatherController;
        [SerializeField] private FullscreenEffectController _fullscreenEffectController;
        [SerializeField] private SkyboxController _skyboxController;
        [SerializeField] private PresetsImages _presetsImages;
        [SerializeField] private GlobalData _globalData;
        [SerializeField] private WeatherPresets _defaultWeatherPresets;
        [SerializeField] private TimeController _timeController;
        [SerializeField] private int _selectedWeatherPreset = 0;
        [SerializeField] private int _selectedStarPreset = 0;
        [SerializeField] private int _selectedPlanetPreset = 0;
        #endregion

        #region Events
        public event Action OnWeatherDataUpdated;
        public event Action OnWindUpdated;
        public event Action<int> OnPresetUpdated;
        #endregion

        #region Properties
        public CloudsController CloudsController { get => _cloudsController; }
        public WeatherEffectsController WeatherEffectsController { get => _weatherEffectsController; }
        public StarsDataContainer StarsDataContainer { get => _starsDataContainer; }
        public CelestialObjectsController CelestialObjectsController { get => _celestialObjectsController; }
        public PlanetsDataContainer PlanetsDataContainer { get => _planetsDataContainer; }
        public WeatherAreasController WeatherAreaController { get => _weatherAreaController; }
        public WeatherPresetsController WeatherPresetsController { get => _weatherController; }
        public FullscreenEffectController FullscreenEffectController { get => _fullscreenEffectController; }
        public SkyboxController SkyboxController { get => _skyboxController; }
        public PresetsImages PresetsImages { get => _presetsImages; }
        public GlobalData GlobalData { get => _globalData; set => _globalData = value; }
        public WeatherPresets DefaultWeatherPresets { get => _defaultWeatherPresets; }
        public string RelativePath { get; set; }
        public int SelectedWeatherPreset { get => _selectedWeatherPreset; set => _selectedWeatherPreset = value; }
        public int SelectedStarPreset { get => _selectedStarPreset; set => _selectedStarPreset = value; }
        public int SelectedPlanetPreset { get => _selectedPlanetPreset; set => _selectedPlanetPreset = value; }
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
#if UNITY_EDITOR
            EasySkyRelativePath.UpdatePath();
#endif
        }

        protected override void Start()
        {
            if(WeatherAreaController.Areas.Count == 0)
            {
                SetWeatherPreset(WeatherAreaController.GlobalPresetData.presetData);
            }
            base.Start();

            _timeController.UpdateTime();
            CelestialObjectsController.UpdatePlanetPosition();
            CelestialObjectsController.UpdateStarsPositions();
        }
        #endregion

        #region Public Methods
        public void FireDataUpdated()
        {
            OnWeatherDataUpdated?.Invoke();
        }

        public void FireUpdateWind()
        {
            OnWindUpdated?.Invoke();
        }

        public void FirePresetUpdated(int value)
        {
            OnPresetUpdated?.Invoke(value);
        }

#if UNITY_EDITOR
        [MenuItem(kMenuItemPath, false, 1)]
        public static void CreateManagerInstance()
        {
            if (Instance == null)
            {
                var weatherManager = GetPrefab("EasySkyWeatherManager");

                if (weatherManager == null)
                {
                    Debug.LogWarning("EasySkyWeatherManager prefab not found in the project");
                    return;
                }

                var manager = Instantiate(weatherManager);
                _instance = manager.GetComponent<EasySkyWeatherManager>();
            }
        }

        public static GameObject GetPrefab(string prefabName)
        {
            string[] assets = AssetDatabase.FindAssets(prefabName);
            for (int idx = 0; idx < assets.Length; idx++)
            {
                string path = AssetDatabase.GUIDToAssetPath(assets[idx]);
                if (path.Contains(".prefab"))
                {
                    return AssetDatabase.LoadAssetAtPath<GameObject>(path);
                }
            }
            return null;
        }
#endif

        public void SetWeatherPreset(WeatherPresetData presetData)
        {
            CloudsController.SetLayerCloudPreset(presetData.LayerCloudPresetData);
            CloudsController.SetVolumetricCloudPreset(presetData.VolumetricCloudPresetData);
            WeatherEffectsController.RainController.ApplyData(presetData.RainData);
            WeatherEffectsController.SnowController.ApplyData(presetData.SnowData);
            WeatherEffectsController.HailController.ApplyData(presetData.HailData);
            WeatherEffectsController.DuststormController.ApplyData(presetData.DuststormData);
            WeatherEffectsController.RainPsController.ApplyData(presetData.StandarRainData);
            WeatherEffectsController.DuststormPsController.ApplyData(presetData.StandardDuststormData);
            WeatherEffectsController.SnowPsController.ApplyData(presetData.StandarSnowData);
            WeatherEffectsController.HailController.ApplyData(presetData.HailData);
        }
        #endregion
    }
}