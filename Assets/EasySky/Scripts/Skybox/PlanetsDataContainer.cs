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
    [CreateAssetMenu(fileName = "PlanetsDataContainer", menuName = "Assets/PlanetsDataContainer")]
    public class PlanetsDataContainer : ScriptableObject
    {
        public VisualTreeAsset starLabel;
        public CelestialObjectPresetData defaultData;

        [SerializeField] private List<CelestialObjectPresetData> _planetsPresets;

        public List<CelestialObjectPresetData> PlanetsPresets { get => _planetsPresets; set => _planetsPresets = value; }

        public void AddNewPlanetPreset()
        {
#if UNITY_EDITOR
            var preset = new CelestialObjectPresetData(defaultData);
            preset.identifier = GUID.Generate().ToString();
            PlanetsPresets.Add(preset);
#endif
        }

        public CelestialObjectPresetData GetPlanetData(string id)
        {
            foreach (var item in PlanetsPresets)
            {
                if (item.identifier == id)
                {
                    return item;
                }
            }

            return null;
        }
    }
}