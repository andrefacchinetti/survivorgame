//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace EasySky.Editor
{
    [CustomEditor(typeof(EasySkyWeatherManager))]
    public class WeatherManagerUI : UnityEditor.Editor
    {
        #region Private Variables
        private VisualElement _root;
        private VisualTreeAsset _treeAsset;
        private StyleSheet _styleSheet;
        private EasySkyWeatherManager _weatherManager;
        private GlobalControlsUI _globalControlsUI;
        private GlobalSkySystemUI _globalSkySystemUI;
        private WeatherAreaUI _weatherAreaUI;
        private DateTime _currentTime;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            _root = new VisualElement();

            if (EasySkyWeatherManager.Instance == null)
            {
                return;
            }

            _treeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(EasySkyWeatherManager.Instance.RelativePath + "/Editor/UI/WeatherManager.uxml");
            _styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(EasySkyWeatherManager.Instance.RelativePath + "/Editor/UI/WeatherManager.uss");

            _treeAsset.CloneTree(_root);
            _root.styleSheets.Add(_styleSheet);

            _weatherManager = this.target as EasySkyWeatherManager;
            _weatherManager.CloudsController.SetupClouds();
            _weatherManager.WeatherEffectsController.FogController.SetupFog();
            _weatherManager.SkyboxController.SetupSky();

            _globalControlsUI = new GlobalControlsUI(_root, _weatherManager);
            _globalSkySystemUI = new GlobalSkySystemUI(_root);
            _weatherAreaUI = new WeatherAreaUI(_root, _weatherManager);

            _globalControlsUI.PopulateGlobalData();
            _globalSkySystemUI.PopulateGlobalSkySystemUI(_weatherManager);
            _weatherAreaUI.PopulateWeatherAreaUI();

            EditorApplication.update += Update;
            _currentTime = _weatherManager.GlobalData.globalTime;
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (_weatherManager != null && (_weatherManager.GlobalData.shouldUpdateTime || _currentTime != _weatherManager.GlobalData.globalTime))
            {
                _globalControlsUI.UpdateSliders();
            }
#endif
        }

        private void OnDestroy()
        {
            EditorApplication.update -= Update;
        }
        #endregion

        #region Public Methods
        public override VisualElement CreateInspectorGUI()
        {
            return _root;
        }
        #endregion
    }
}