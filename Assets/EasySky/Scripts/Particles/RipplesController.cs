//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace EasySky.Particles
{
    public class RipplesController : MonoBehaviour
    {
        [SerializeField] private CustomPassVolume _ripples;

        public void ApplyDataToRipples(RainData data)
        {
            var fullscreenPass = _ripples.customPasses[0] as DrawRenderersCustomPass;
            if (fullscreenPass == null) return;

            fullscreenPass.overrideMaterial.SetFloat("_Coverage", data.ripplesCoverage);
            fullscreenPass.overrideMaterial.SetFloat("_Tiling", data.ripplesTiling);
            fullscreenPass.overrideMaterial.SetFloat("_Speed", data.ripplesSpeed);
            fullscreenPass.overrideMaterial.SetFloat("_Ripple_Scale", data.ripplesScale);
            fullscreenPass.overrideMaterial.SetFloat("_Normal_Intesity", data.ripplesNormalIntensity);
            fullscreenPass.layerMask = data.layers;
            ChangeRipplesState(data.areRipplesActive);
        }

        public void ChangeRipplesState(bool state)
        {
            _ripples.enabled = state;
        }
    }
}