//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using EasySky.WeatherArea;

namespace EasySky.Utils
{
    public class SingletonEditorWindow<T> : EditorWindow where T : EditorWindow
    {
        #region Protected Variables
        protected EasySkyWeatherManager _weatherManager;
        protected WeatherPresetData _selectedPresetData;
        protected VisualTreeAsset _treeAsset;
        protected Action OnWindowClosed;
        protected static T _instance;
        #endregion

        #region Properties
        protected virtual string _windowName { get; set; }
        protected virtual string _ussPath { get; set; }
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    return (T)CreateInstance(typeof(T));
                }
                else
                { 
                    return _instance;
                }
            }
        }
        #endregion

        #region Unity Methods
        private void CreateGUI()
        {
            _treeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(EasySkyWeatherManager.Instance.RelativePath + _ussPath);
            _treeAsset.CloneTree(rootVisualElement);
        }

        private void OnDestroy()
        {
            OnWindowClosed?.Invoke();
            _weatherManager.FireDataUpdated();

            if (_selectedPresetData != null)
            {
                EditorUtility.SetDirty(_selectedPresetData);
                EditorUtility.SetDirty(_selectedPresetData.VolumetricCloudPresetData);
                EditorUtility.SetDirty(_selectedPresetData.LayerCloudPresetData);
            }

            AssetDatabase.SaveAssets();
        }
        #endregion

        #region Public Methods
        public virtual void InitializeForm(EasySkyWeatherManager weatherManager, WeatherPresetData presetData, Action onCloseWindowAction)
        {
            OnWindowClosed = onCloseWindowAction;
            EditorWindow wnd = GetWindow<T>();
            wnd.titleContent = new GUIContent(_windowName);
            _weatherManager = weatherManager;
            _selectedPresetData = presetData;
        }
        #endregion
    }
}
#endif