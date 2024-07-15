//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EasySky.Skybox
{
    /// <summary>
    /// Controls all celestial objects
    /// </summary>
    public class CelestialObjectsController : MonoBehaviour
    {
        #region Constants
        private const float HOUR_IN_DAY = 24f;
        private const float CIRCLE_MAX_ANGLE = 360f;
        private const float SUN_NIGHT_LIGHT_INTENSITY = 10f;
        #endregion

        #region Private Variables
        [SerializeField] private StarsDataContainer _starsDataContainer;
        [SerializeField] private PlanetsDataContainer _planetsDataContainer;
        [SerializeField] private CelestialObjectController _defaultStar;
        [SerializeField] private List<CelestialObjectController> _stars;
        [SerializeField] private List<CelestialObjectController> _planets;
        [SerializeField] private List<string> _starsData;
        [SerializeField] private List<string> _planetsData;

        private SolarPositionCalculator _solarPositionCalculator = new SolarPositionCalculator();
        private MoonPositionCalculator _moonPositionCalculator = new MoonPositionCalculator();
        private bool _isNight;
        #endregion

        #region Properties
        public StarsDataContainer StarsDataContainer { get => _starsDataContainer; set => _starsDataContainer = value; }
        public PlanetsDataContainer PlanetsDataContainer { get => _planetsDataContainer; set => _planetsDataContainer = value; }
        #endregion

        #region Unity Methods
        private void Update()
        {
            if (EasySkyWeatherManager.Instance.GlobalData.shouldUpdateTime)
            {
                UpdateStarsPositions();
                UpdatePlanetPosition();
            }
        }
        #endregion

        #region Public Methods
        public void AddNewStarData()
        {
            StarsDataContainer.AddNewStarPreset();
        }

        public void AddNewPlanetData()
        {
            PlanetsDataContainer.AddNewPlanetPreset();
        }

        public void AddStar(CelestialObjectPresetData celestialObject)
        {
            if (_starsData.Contains(celestialObject.identifier))
            {
                return;
            }

            _starsData.Add(celestialObject.identifier);
            SpawnStar(celestialObject);
        }

        public void AddPlanet(CelestialObjectPresetData celestialObject)
        {
            if (_planetsData.Contains(celestialObject.identifier))
            {
                return;
            }

            _planetsData.Add(celestialObject.identifier);
            SpawnPlanet(celestialObject);
        }


        public bool IsStarInScene(CelestialObjectPresetData data)
        {
            return _starsData.Contains(data.identifier);
        }

        public bool IsPlanetInScene(CelestialObjectPresetData data)
        {
            return _planetsData.Contains(data.identifier);
        }

        public void SpawnStar(CelestialObjectPresetData data)
        {
            var globalData = EasySkyWeatherManager.Instance.GlobalData;
            var star = Instantiate(_defaultStar, transform);
            star.name = data.presetName;
            star.SetData(data);
            star.SetLightIntensity(data.lightIntensity);
            _stars.Add(star);
            data.isInstantiated = true;
            var offset = globalData.longitude * (HOUR_IN_DAY / CIRCLE_MAX_ANGLE);
            var sunPos = _solarPositionCalculator.GetSunPosition(globalData.globalTime, globalData.latitude, globalData.longitude, (float)offset, data.offset);
            star.transform.rotation = Quaternion.Euler(new Vector3((float)sunPos.altitude + data.offset.x, (float)sunPos.azimuth + data.offset.y));
            UpdateStarsPositions();
        }

        public void SpawnPlanet(CelestialObjectPresetData data)
        {
            var globalData = EasySkyWeatherManager.Instance.GlobalData;
            var planet = Instantiate(_defaultStar, transform);
            planet.name = data.presetName;
            planet.SetData(data);
            _planets.Add(planet);
            data.isInstantiated = true;
            var moonPos = _moonPositionCalculator.Coordinates(globalData.globalTime, globalData.latitude, globalData.longitude, data.offset);
            planet.transform.rotation = Quaternion.Euler(new Vector3((float)moonPos.altitude + data.offset.x, (float)moonPos.azimuth + data.offset.y));
            UpdatePlanetPosition();
            UpdatePlanetsLightIntensity();
        }

        public void UpdateStar(CelestialObjectPresetData data)
        {
            for (int i = 0; i < _starsData.Count; i++)
            {
                if (_starsData[i] == data.identifier)
                {
                    _stars[i].SetData(data);
                    _stars[i].SetLightIntensity(data.lightIntensity);
                }
            }

            UpdatePlanetsLightIntensity();
        }

        public void UpdatePlanet(CelestialObjectPresetData data)
        {
            for (int i = 0; i < _planetsData.Count; i++)
            {
                if (_planetsData[i] == data.identifier)
                {
                    _planets[i].SetData(data);
                }
            }

            UpdatePlanetsLightIntensity();
        }

        public void DestroyStar(CelestialObjectPresetData data)
        {
            for (int i = 0; i < _starsData.Count; i++)
            {
                if (_starsData[i] == data.identifier)
                {
                    DestroyImmediate(_stars[i].gameObject);
                    _starsData.RemoveAt(i);
                    _stars.RemoveAt(i);
                    data.isInstantiated = false;
                    break;
                }
            }

            UpdatePlanetsLightIntensity();
        }

        public void DestroyPlanet(CelestialObjectPresetData data)
        {
            for (int i = 0; i < _planetsData.Count; i++)
            {
                if (_planetsData[i] == data.identifier)
                {
                    DestroyImmediate(_planets[i].gameObject);
                    _planetsData.RemoveAt(i);
                    _planets.RemoveAt(i);
                    data.isInstantiated = false;
                    break;
                }
            }
        }

        public void UpdateStarsPositions()
        {
            var globalData = EasySkyWeatherManager.Instance.GlobalData;
            var utcOffset = globalData.longitude * (24 / 360f);

            for (int i = 0; i < _starsData.Count; i++)
            {
                var starData = StarsDataContainer.GetStarData(_starsData[i]);
                if(starData == null)
                {
                    return;
                }
                var sunPos = _solarPositionCalculator.GetSunPosition(globalData.globalTime, globalData.latitude, globalData.longitude, (float)utcOffset, starData.offset);
                _stars[i].transform.rotation = Quaternion.Euler(new Vector3((float)sunPos.altitude, (float)sunPos.azimuth));
                _stars[i].SetLightIntensity(sunPos.altitude < -3.5d ? SUN_NIGHT_LIGHT_INTENSITY : starData.lightIntensity);
            }

            UpdatePlanetsLightIntensity();
        }

        public void UpdatePlanetPosition()
        {
            var globalData = EasySkyWeatherManager.Instance.GlobalData;
            for (int i = 0; i < _planetsData.Count; i++)
            {
                var planetData = PlanetsDataContainer.GetPlanetData(_planetsData[i]);
                if (planetData == null)
                {
                    return;
                }
                var moonPos = _moonPositionCalculator.Coordinates(globalData.globalTime, globalData.latitude, globalData.longitude, planetData.offset);

                _planets[i].transform.eulerAngles = new Vector3((float)moonPos.altitude, (float)moonPos.azimuth, (float)moonPos.azimuth);
            }
        }

#if UNITY_EDITOR
        public void SetDirty()
        {
            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(StarsDataContainer);
            EditorUtility.SetDirty(PlanetsDataContainer);
        }
#endif
        #endregion

        #region Private Methods
        private void UpdatePlanetsLightIntensity()
        {
            var isCurrentlyNight = false;
            for (int i = 0; i < _stars.Count; i++)
            {
                if (_stars[i].transform.rotation.eulerAngles.x > 180 && _stars[i].transform.rotation.eulerAngles.x < 357)
                {
                    isCurrentlyNight = true;
                    break;
                }
            }

            _isNight = isCurrentlyNight;
            for (int i = 0; i < _planets.Count; i++)
            {
                var planetData = PlanetsDataContainer.GetPlanetData(_planetsData[i]);
                if (planetData == null)
                {
                    return;
                }
                _planets[i].SetLightIntensity(_isNight ? planetData.nightLightIntensity : planetData.lightIntensity);
            }
        }
        #endregion
    }
}