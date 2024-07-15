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
using UnityEngine.UIElements;

namespace EasySky.Skybox
{
[CreateAssetMenu(fileName = "StarsDataContainer", menuName = "Assets/StarsDataContainer")]
    public class StarsDataContainer : ScriptableObject
    {
        public VisualTreeAsset starLabel;
        public CelestialObjectPresetData defaultData;
        [SerializeField] private List<CelestialObjectPresetData> _starsPresets;

        public List<CelestialObjectPresetData> StarsPresets { get => _starsPresets; set => _starsPresets = value; }

        public void AddNewStarPreset()
        {
#if UNITY_EDITOR
            var preset = new CelestialObjectPresetData(defaultData);
            preset.identifier = GUID.Generate().ToString();
            _starsPresets.Add(preset);
#endif
        }

        public CelestialObjectPresetData GetStarData(string id)
        {
            foreach (var item in _starsPresets) 
            {
                if(item.identifier == id)
                {
                    return item;
                }
            }

            return null;
        }
    }
}