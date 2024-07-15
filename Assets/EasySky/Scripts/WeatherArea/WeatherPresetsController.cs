//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using EasySky.Clouds;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasySky.WeatherArea
{
    public class WeatherPresetsController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private List<WeatherPresetData> _weatherPresetData;
        [SerializeField] private List<VolumetricCloudPresetData> _volumetricCloudData;
        [SerializeField] private List<LayerCloudPresetData> _layerCloudData;
        [SerializeField] private VisualTreeAsset _weatherPresetLable;
        [SerializeField] private WeatherPresetData _defaultData;
        [SerializeField] private VolumetricCloudPresetData _defaultVolumetricCloudData;
        [SerializeField] private LayerCloudPresetData _defaultLayerCloudData;
        #endregion

        #region Properties
        public VisualTreeAsset WeatherPresetLabel { get => _weatherPresetLable; set => _weatherPresetLable = value; }
        public List<WeatherPresetData> WeatherPresetsData { get => _weatherPresetData; set => _weatherPresetData = value; }
        public List<VolumetricCloudPresetData> VolumetricCloudData { get => _volumetricCloudData; }
        public List<LayerCloudPresetData> LayerCloudData { get => _layerCloudData; }
        #endregion

#if UNITY_EDITOR
        #region Public Methods
        public void LoadAllWeatherDatas()
        {
            _weatherPresetData = FindAllWeatherPresets();
            _volumetricCloudData = FindAllCloudPresets<VolumetricCloudPresetData>("Volumetric");
            _layerCloudData = FindAllCloudPresets<LayerCloudPresetData>("Layer");
        }

        public void AddNewWeatherPreset()
        {
            var asset = (WeatherPresetData)ScriptableObject.CreateInstance(typeof(WeatherPresetData));
            asset.SetToDefault(_defaultData);

            var simulationName = _defaultData.PresetName;

            var index = 0;
            while (AssetDatabase.LoadAssetAtPath($"{EasySkyWeatherManager.Instance.RelativePath}/Resources/WeatherPresets/{simulationName}.asset", typeof(WeatherPresetData)) != null)
            {
                index++;
                simulationName = _defaultData.PresetName + index;
            }

            AssetDatabase.CreateAsset(asset, $"{EasySkyWeatherManager.Instance.RelativePath}/Resources/WeatherPresets/{simulationName}.asset");
            AssetDatabase.SaveAssets();
            _weatherPresetData.Add(asset);
            asset.SetToDefault(_defaultData);
            _weatherPresetData = _weatherPresetData.OrderBy(x => x.PresetName).ToList();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public void RemoveWeatherPreset(int index)
        {
            var asset = _weatherPresetData[index];
            _weatherPresetData.RemoveAt(index);
            AssetDatabase.Refresh();
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(asset));
        }

        public void AddNewCloudPreset<T0, T1>(List<T0> clouds, string type) where T0 : CloudPresetData<T1> where T1 : class
        {
            var asset = (T0)ScriptableObject.CreateInstance(typeof(T0));

            var simulationName = $"{type}CloudPresetData";

            var index = 0;
            while (AssetDatabase.LoadAssetAtPath($"{EasySkyWeatherManager.Instance.RelativePath}/Resources/CloudPresets/{type}/{simulationName}.asset", typeof(T0)) != null)
            {
                index++;
                simulationName = $"{type}CloudPresetData" + index;
            }

            AssetDatabase.CreateAsset(asset, $"{EasySkyWeatherManager.Instance.RelativePath}/Resources/CloudPresets/{type}/{simulationName}.asset");
            AssetDatabase.SaveAssets();
            clouds.Add(asset);

            switch (type)
            {
                case "Layer":
                    var layerCloudPresetData = asset as LayerCloudPresetData;
                    layerCloudPresetData.CopyData(_defaultLayerCloudData.cloudData);
                    break;
                case "Volumetric":
                    var volumetricCloudPresetData = asset as VolumetricCloudPresetData;
                    volumetricCloudPresetData.CopyData(_defaultVolumetricCloudData.cloudData);
                    break;
            }

            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(this);
        }

        public void RemoveCloudPreset<T>(List<T> cloudPresets, T toBeRemoved) where T : ScriptableObject
        {
            cloudPresets.Remove(toBeRemoved);
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(toBeRemoved));
        }
        #endregion

        #region Private Methods
        private List<WeatherPresetData> FindAllWeatherPresets()
        {
            if (!UnityEngine.Windows.Directory.Exists($"{EasySkyWeatherManager.Instance.RelativePath}/Resources/WeatherPresets"))
            {
                return new List<WeatherPresetData>();
            }
            string[] assets = AssetDatabase.FindAssets("t: WeatherPresetData", new[] { $"{EasySkyWeatherManager.Instance.RelativePath}/Resources/WeatherPresets" });
            List<WeatherPresetData> weatherPresets = new List<WeatherPresetData>();

            foreach (string asset in assets)
            {
                var path = AssetDatabase.GUIDToAssetPath(asset);
                WeatherPresetData profile = (WeatherPresetData)AssetDatabase.LoadAssetAtPath(path, typeof(WeatherPresetData));
                weatherPresets.Add(profile);
            }

            return weatherPresets;
        }

        private List<T> FindAllCloudPresets<T>(string type) where T : ScriptableObject
        {
            if (!UnityEngine.Windows.Directory.Exists($"{EasySkyWeatherManager.Instance.RelativePath}/Resources/CloudPresets/{type}"))
            {
                return new List<T>();
            }
            string[] assets = AssetDatabase.FindAssets($"t: {type}CloudPresetData", new[] { $"{EasySkyWeatherManager.Instance.RelativePath}/Resources/CloudPresets/{type}" });
            var weatherPresets = new List<T>();

            foreach (string asset in assets)
            {
                var path = AssetDatabase.GUIDToAssetPath(asset);
                var profile = (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));
                weatherPresets.Add(profile);
            }

            return weatherPresets;
        }
        #endregion
#endif
    }
}