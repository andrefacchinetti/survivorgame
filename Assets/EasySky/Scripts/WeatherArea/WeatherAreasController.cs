//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasySky.WeatherArea
{
    public class WeatherAreasController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private WeatherArea _weatherArea;
        [SerializeField] private VisualTreeAsset _weatherAreaLabel;
        [SerializeField] private List<WeatherArea> _areas;
        [SerializeField] private WeatherPresetData _defaultPreset;
        [SerializeField] private Texture2D _defaultIcon;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private WeatherAreaData _globalPresetData;

        private WeatherArea _currentWeatherArea;
        private Camera _camera;
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;
        #endregion

        #region Properties
        public List<WeatherArea> Areas { get => _areas; }
        public VisualTreeAsset WeatherAreaLabel { get => _weatherAreaLabel; }
        public WeatherAreaData GlobalPresetData { get => _globalPresetData; }
        #endregion

        #region Unity Methods
        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
            if (_currentWeatherArea == null)
            {
                if (_areas.Count == 0)
                {
                    var weatherManager = EasySkyWeatherManager.Instance;
                    weatherManager.WeatherEffectsController.ApplyData(_globalPresetData);
                    weatherManager.FullscreenEffectController.ApplyData(_globalPresetData);
                    weatherManager.CloudsController.SetData(_globalPresetData.presetData);
                    weatherManager.FullscreenEffectController.ApplyDataToSnow(_globalPresetData.presetData.SnowData);
                    weatherManager.FullscreenEffectController.ApplyDataToRain(_globalPresetData.presetData.RainData);
                    weatherManager.FullscreenEffectController.ApplyDataToSnowLayer(_globalPresetData.presetData.SnowData);
                }

                CheckArea();
            }

            if (_currentWeatherArea == null)
            {
                ApplyDataFromNearastArea();
            }
        }

        private void Update()
        {
            CheckArea();
        }
        #endregion

        #region Public Methods
#if UNITY_EDITOR
        public WeatherArea CreateNewWeatherArea()
        {
            var area = Instantiate(_weatherArea, transform);
            area.WeatherAreaData.presetData = _defaultPreset;
            area.WeatherAreaData.areaIcon = _defaultIcon;
            area.WeatherAreaData.areaColor = _defaultColor;
            area.WeatherAreaData.areaName = "Area";
            _areas.Add(area);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();

            return area;
        }

        public void DeleteArea(int index)
        {
            DestroyImmediate(_areas[index].gameObject);
            EditorUtility.SetDirty(this);
            _areas.RemoveAt(index);
        }
#endif
        #endregion

        #region Private Methods
        private void CheckArea()
        {
            var weatherManager = EasySkyWeatherManager.Instance;
            foreach (var area in _areas)
            {
                Vector3 halfExtents = area.transform.localScale * 0.5f;
                Vector3 minBounds = area.transform.position - halfExtents;
                Vector3 maxBounds = area.transform.position + halfExtents;

                if (_camera.transform.position.x >= minBounds.x && _camera.transform.position.x <= maxBounds.x &&
        _camera.transform.position.z >= minBounds.z && _camera.transform.position.z <= maxBounds.z)
                {
                    var closestArea = GetNearestWeatherArea();
                    minBounds = closestArea.transform.position - halfExtents;
                    maxBounds = closestArea.transform.position + halfExtents;
                    if(_camera.transform.position.x >= minBounds.x && _camera.transform.position.x <= maxBounds.x &&
        _camera.transform.position.z >= minBounds.z && _camera.transform.position.z <= maxBounds.z)
                    {
                        closestArea = area;
                    }

                    if (area != _currentWeatherArea && area == closestArea)
                    {
                        if (_cancellationTokenSource != null)
                        {
                            _cancellationTokenSource.Cancel();
                            _cancellationTokenSource.Dispose();
                            _cancellationTokenSource = null;
                        }
                        _cancellationTokenSource = new CancellationTokenSource();
                        _cancellationToken = _cancellationTokenSource.Token;

                        if (_currentWeatherArea == null)
                        {
                            _currentWeatherArea = area;
                            weatherManager.WeatherEffectsController.ApplyData(_currentWeatherArea.WeatherAreaData);
                            weatherManager.FullscreenEffectController.ApplyData(_currentWeatherArea.WeatherAreaData);
                            weatherManager.CloudsController.SetData(_currentWeatherArea.WeatherAreaData.presetData);
                            weatherManager.FullscreenEffectController.ApplyDataToSnow(_currentWeatherArea.WeatherAreaData.presetData.SnowData);
                            weatherManager.FullscreenEffectController.ApplyDataToRain(_currentWeatherArea.WeatherAreaData.presetData.RainData);
                            weatherManager.FullscreenEffectController.ApplyDataToSnowLayer(_currentWeatherArea.WeatherAreaData.presetData.SnowData);
                        }
                        else
                        {
                            _currentWeatherArea = area;
                            weatherManager.CloudsController.InterpolateBetweenCloudPresets(_currentWeatherArea.WeatherAreaData.presetData);
                            weatherManager.WeatherEffectsController.InterpolateEffects(area.WeatherAreaData.presetData, _cancellationToken);
                            weatherManager.FullscreenEffectController.InterpolateEffects(_currentWeatherArea.WeatherAreaData.presetData.RainData, _currentWeatherArea.WeatherAreaData.presetData.SnowData, _cancellationToken);
                        }
                    }
                }
            }
        }

        private void ApplyDataFromNearastArea()
        {
            _currentWeatherArea = GetNearestWeatherArea();

            if (_currentWeatherArea == null) return;

            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;

            var weatherManager = EasySkyWeatherManager.Instance;
            weatherManager.WeatherEffectsController.ApplyData(_currentWeatherArea.WeatherAreaData);
            weatherManager.FullscreenEffectController.ApplyData(_currentWeatherArea.WeatherAreaData);
            weatherManager.CloudsController.SetData(_currentWeatherArea.WeatherAreaData.presetData);
            weatherManager.FullscreenEffectController.ApplyDataToSnow(_currentWeatherArea.WeatherAreaData.presetData.SnowData);
            weatherManager.FullscreenEffectController.ApplyDataToRain(_currentWeatherArea.WeatherAreaData.presetData.RainData);
            weatherManager.FullscreenEffectController.ApplyDataToSnowLayer(_currentWeatherArea.WeatherAreaData.presetData.SnowData);
        }

        private WeatherArea GetNearestWeatherArea()
        {
            var maxDistance = float.MaxValue;

            if (_areas.Count <= 0)
            {
                return null;
            }

            var closestArea = _areas[0];
            var cameraPosition = new Vector3(_camera.transform.position.x, 0, _camera.transform.position.z);
            foreach (var area in _areas)
            {
                var areaPosition = new Vector3(area.transform.position.x, 0, area.transform.position.z);
                var distance = Vector3.Distance(cameraPosition, areaPosition);
                if (distance < maxDistance)
                {
                    closestArea = area;
                    maxDistance = distance;
                }
            }

            return closestArea;
        }
        #endregion
    }
}