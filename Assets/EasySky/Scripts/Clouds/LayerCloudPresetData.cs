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
    [CreateAssetMenu(fileName = "LayerCloudPresetData", menuName = "Assets/LayerCloudPresetData")]
    public class LayerCloudPresetData : CloudPresetData<LayerCloudData>
    {
        public LayerCloudData cloudData;

        public override void CopyData(LayerCloudData cloudPresetData)
        {
            cloudData = new LayerCloudData(cloudPresetData);
        }
    }
}