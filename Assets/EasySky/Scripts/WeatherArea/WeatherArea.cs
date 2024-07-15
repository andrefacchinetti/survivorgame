//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using UnityEngine;

namespace EasySky.WeatherArea
{
    public class WeatherArea : MonoBehaviour
    {
        [SerializeField] private WeatherAreaData _weatherAreaData;
        [SerializeField] private MeshRenderer _meshRenderer;

        public WeatherAreaData WeatherAreaData { get => _weatherAreaData; }
        public MeshRenderer MeshRenderer { get => _meshRenderer; }

        private void Start()
        {
            gameObject.SetActive(false);
        }
    }
}