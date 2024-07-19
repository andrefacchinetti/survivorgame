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
        [HideInInspector] public GlobalData _globalData;
        [HideInInspector] public GameController gameController;

        private void Start()
        {
           UpdateTime();
        }

        int dayOld = 0;
        bool spawnouLobisomens = false, spawnouAnimais = false;
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

                if (dayOld != _globalData.days)
                {
                    gameController.SpawnarLootsPorDia();
                    dayOld = _globalData.days;
                    spawnouLobisomens = false;
                    spawnouAnimais = false;
                }
                if (!spawnouLobisomens && (_globalData.hours >= 19 && _globalData.hours < 24))
                {
                    gameController.SpawnarLobisomensPorDia();
                    spawnouLobisomens = true;
                    Debug.Log("spawnou lobisomens");
                }
                if (!spawnouAnimais && (_globalData.hours >= 5 && _globalData.hours < 17))
                {
                    gameController.SpawnarAnimaisPorDia();
                    spawnouAnimais = true;
                    Debug.Log("spawnou animais");
                }
            }
        }

        public void UpdateTime()
        {
            _globalData = EasySkyWeatherManager.Instance.GlobalData;
            _globalData.globalTime = new DateTime(_globalData.years, _globalData.months, _globalData.days, _globalData.hours, _globalData.minutes, _globalData.seconds);
        }
    }
}