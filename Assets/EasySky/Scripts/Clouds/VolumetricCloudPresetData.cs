//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using UnityEngine;

namespace EasySky.Clouds
{
    [CreateAssetMenu(fileName = "VolumetricCloudPresetData", menuName = "Assets/VolumetricCloudPresetData")]
    public class VolumetricCloudPresetData : CloudPresetData<VolumetricCloudData>
    {
        public VolumetricCloudData cloudData;

        public override void CopyData(VolumetricCloudData cloudPresetData)
        {
            cloudData = new VolumetricCloudData(cloudPresetData);
        }
    }
}