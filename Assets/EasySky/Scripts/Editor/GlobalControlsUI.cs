//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using EasySky.Data;
using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace EasySky.Editor
{
    /// <summary>
    /// The main UI class for modifying the Easy Sky global controls
    /// </summary>
    public class GlobalControlsUI
    {
        #region Private Variables
        private VisualElement _root;
        private SliderInt _seconds;
        private SliderInt _minutes;
        private SliderInt _hours;
        private SliderInt _days;
        private SliderInt _months;
        private SliderInt _years;
        private EasySkyWeatherManager _weatherManager;
        private GlobalData _globalData;
        private Toggle _shouldSimulate;
        private FloatField _cycleLength;
        private Slider _longitude;
        private Slider _latitude;
        private FloatField _windSpeed;
        private FloatField _windDirection;
        #endregion

        #region Constructors
        public GlobalControlsUI(VisualElement root, EasySkyWeatherManager weatherManager)
        {
            _root = root;
            _weatherManager = weatherManager;
            _globalData = _weatherManager.GlobalData;
        }
        #endregion

        #region Public Methods
        public void UpdateSliders()
        {
            _seconds.SetValueWithoutNotify(_globalData.seconds);
            _minutes.SetValueWithoutNotify(_globalData.minutes);
            _hours.SetValueWithoutNotify(_globalData.hours);
            _days.SetValueWithoutNotify(_globalData.days);
            _months.SetValueWithoutNotify(_globalData.months);
            _years.SetValueWithoutNotify(_globalData.years);
            _shouldSimulate.SetValueWithoutNotify(_globalData.shouldUpdateTime);
            _latitude.SetValueWithoutNotify(_globalData.latitude);
            _longitude.SetValueWithoutNotify(_globalData.longitude);
            _cycleLength.SetValueWithoutNotify(_globalData.timeSpeedMultiplier);
            _windDirection.SetValueWithoutNotify(_globalData.windDirection);
            _windSpeed.SetValueWithoutNotify(_globalData.windSpeed);
            UpdateDaysMaxValue();
        }

        public void PopulateGlobalData()
        {
            _seconds = _root.Q<SliderInt>("TimeSeconds");

            _seconds.RegisterCallback<ChangeEvent<int>>((evt) =>
            {
                var date = _globalData.globalTime;
                _globalData.globalTime = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, evt.newValue);
                _weatherManager.CelestialObjectsController.UpdatePlanetPosition();
                _weatherManager.CelestialObjectsController.UpdateStarsPositions();
                _globalData.seconds = evt.newValue;
                EditorUtility.SetDirty(_globalData);
            });

            _minutes = _root.Q<SliderInt>("TimeMinutes");

            _minutes.RegisterCallback<ChangeEvent<int>>((evt) =>
            {
                var date = _globalData.globalTime;
                _globalData.globalTime = new DateTime(date.Year, date.Month, date.Day, date.Hour, evt.newValue, date.Second);
                _weatherManager.CelestialObjectsController.UpdatePlanetPosition();
                _weatherManager.CelestialObjectsController.UpdateStarsPositions();
                _globalData.minutes = evt.newValue;
                EditorUtility.SetDirty(_globalData);

            });

            _hours = _root.Q<SliderInt>("TimeHours");

            _hours.RegisterCallback<ChangeEvent<int>>((evt) =>
            {
                var date = _globalData.globalTime;
                _globalData.globalTime = new DateTime(date.Year, date.Month, date.Day, evt.newValue, date.Minute, date.Second);
                _weatherManager.CelestialObjectsController.UpdatePlanetPosition();
                _weatherManager.CelestialObjectsController.UpdateStarsPositions();
                _globalData.hours = evt.newValue;
                EditorUtility.SetDirty(_globalData);
            });

            _days = _root.Q<SliderInt>("DateDay");

            _days.RegisterCallback<ChangeEvent<int>>((evt) =>
            {
                var date = _globalData.globalTime;
                _globalData.globalTime = new DateTime(date.Year, date.Month, evt.newValue, date.Hour, date.Minute, date.Second);
                _weatherManager.CelestialObjectsController.UpdatePlanetPosition();
                _weatherManager.CelestialObjectsController.UpdateStarsPositions();
                _globalData.days = evt.newValue;
                EditorUtility.SetDirty(_globalData);
            });

            _months = _root.Q<SliderInt>("DateMonth");

            _months.RegisterCallback<ChangeEvent<int>>((evt) =>
            {
                var date = _globalData.globalTime;
                _globalData.globalTime = new DateTime(date.Year, evt.newValue, date.Day, date.Hour, date.Minute, date.Second);
                _weatherManager.CelestialObjectsController.UpdatePlanetPosition();
                _weatherManager.CelestialObjectsController.UpdateStarsPositions();
                _globalData.months = evt.newValue;
                UpdateDaysMaxValue();
                EditorUtility.SetDirty(_globalData);
            });

            _years = _root.Q<SliderInt>("DateYear");

            _years.RegisterCallback<ChangeEvent<int>>((evt) =>
            {
                var date = _globalData.globalTime;
                _globalData.globalTime = new DateTime(evt.newValue, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                _weatherManager.CelestialObjectsController.UpdatePlanetPosition();
                _weatherManager.CelestialObjectsController.UpdateStarsPositions();
                _globalData.years = evt.newValue;
                UpdateDaysMaxValue();
                EditorUtility.SetDirty(_globalData);
            });

            _globalData.globalTime = new DateTime(_globalData.years, _globalData.months, _globalData.days, _globalData.hours, _globalData.minutes, _globalData.seconds);

            _shouldSimulate = _root.Q<Toggle>("TimeSimulate");

            _shouldSimulate.RegisterCallback<ChangeEvent<bool>>((evt) => { _globalData.shouldUpdateTime = evt.newValue; _shouldSimulate.value = evt.newValue; });

            _cycleLength = _root.Q<FloatField>("CycleLength");

            _cycleLength.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _globalData.timeSpeed = 1440f / evt.newValue;
                _globalData.timeSpeedMultiplier = evt.newValue;
                _cycleLength.value = evt.newValue;
                EditorUtility.SetDirty(_globalData);
            });

            _longitude = _root.Q<Slider>("Longitude");

            _longitude.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _globalData.longitude = evt.newValue;
                _weatherManager.CelestialObjectsController.UpdatePlanetPosition();
                _weatherManager.CelestialObjectsController.UpdateStarsPositions();
                EditorUtility.SetDirty(_globalData);
            });

            _latitude = _root.Q<Slider>("Latitude");

            _latitude.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _globalData.latitude = evt.newValue;
                _weatherManager.CelestialObjectsController.UpdatePlanetPosition();
                _weatherManager.CelestialObjectsController.UpdateStarsPositions();
                EditorUtility.SetDirty(_globalData);
            });

            _windSpeed = _root.Q<FloatField>("WindSpeed");
            _windSpeed.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _globalData.windSpeed = evt.newValue;
                _weatherManager.FireUpdateWind();
                EditorUtility.SetDirty(_globalData);
            });

            _windDirection = _root.Q<FloatField>("WindDirection");
            _windDirection.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                _globalData.windDirection = evt.newValue;
                _weatherManager.FireUpdateWind();
                EditorUtility.SetDirty(_globalData);
            });

            UpdateSliders();
            _weatherManager.CelestialObjectsController.UpdateStarsPositions();
            _weatherManager.CelestialObjectsController.UpdatePlanetPosition();
        }
        #endregion

        #region Private Methods
        private void UpdateDaysMaxValue()
        {
            _days.highValue = DateTime.DaysInMonth(_years.value, _months.value);
            if (_days.value > _days.highValue)
            {
                _days.value = _days.highValue;
            }
        }
        #endregion
    }
}