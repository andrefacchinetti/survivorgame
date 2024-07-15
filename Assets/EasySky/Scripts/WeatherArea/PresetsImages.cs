//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasySky.WeatherArea
{
    [CreateAssetMenu(fileName = "PresetsImages", menuName = "Assets/PresetsImages")]
    public class PresetsImages : ScriptableObject
    {
        [SerializeField] private VisualTreeAsset _imageLable;
        [SerializeField] private List<Texture2D> _areaPresetsImages;
        [SerializeField] private List<Texture2D> _weatherPresetsImages;
        [SerializeField] private List<Texture2D> _starPresetIcons;
        [SerializeField] private List<Texture2D> _planetPresetIcons;

        public VisualTreeAsset ImageLable { get => _imageLable; }
        public List<Texture2D> AreaPresetsImages { get => _areaPresetsImages; }
        public List<Texture2D> WeatherPresetsImages { get => _weatherPresetsImages; }
        public List<Texture2D> StarPresetIcons { get => _starPresetIcons; }
        public List<Texture2D> PlanetPresetIcons { get => _planetPresetIcons; }
    }
}