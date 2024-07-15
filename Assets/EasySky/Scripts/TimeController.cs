//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using EasySky.Data;
using System;
using UnityEngine;

namespace EasySky
{
    public class TimeController : MonoBehaviour
    {
        private GlobalData _globalData;

        private void Start()
        {
           UpdateTime();
        }

        private void Update()
        {
            if (_globalData != null && _globalData.shouldUpdateTime)
            {
                _globalData.globalTime = _globalData.globalTime.AddSeconds(_globalData.timeSpeed * Time.deltaTime);
                _globalData.seconds = _globalData.globalTime.Second;
                _globalData.minutes = _globalData.globalTime.Minute;
                _globalData.hours = _globalData.globalTime.Hour;
                _globalData.days = _globalData.globalTime.Day;
                _globalData.months = _globalData.globalTime.Month;
                _globalData.years = _globalData.globalTime.Year;
            }
        }

        public void UpdateTime()
        {
            _globalData = EasySkyWeatherManager.Instance.GlobalData;
            _globalData.globalTime = new DateTime(_globalData.years, _globalData.months, _globalData.days, _globalData.hours, _globalData.minutes, _globalData.seconds);
        }
    }
}