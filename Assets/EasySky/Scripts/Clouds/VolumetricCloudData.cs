//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using static UnityEngine.Rendering.HighDefinition.VolumetricClouds;
using UnityEngine;
using System;

namespace EasySky.Clouds
{
    [Serializable]
    public class VolumetricCloudData
    {
        public CloudMapResolution cloudResolution;
        [Range(0f, 1f)]
        public float cloudDensityMultiplier;
        [Range(0f, 1f)]
        public float cloudShapeFactor;
        [Range(0f, 1f)]
        public float cloudErosionFactor;
        public float erosionScale;
        public float shapeScale;
        public float cloudAltitude;
        public float cloudThickness;
        public Color cloudScatteringTint;
        public bool areShadowsEnabled;
        public bool isWindInteractionActive;
        [Range(0f, 1f)]
        public float lightProbeDimmer;
        public float earthCurvature;
        public AnimationCurve densityCurve;
        public AnimationCurve erosionCurve;
        public AnimationCurve ambientOcclusionCurve;

        public VolumetricCloudData(VolumetricCloudData volumetricCloudData)
        {
            cloudResolution = volumetricCloudData.cloudResolution;
            cloudDensityMultiplier = volumetricCloudData.cloudDensityMultiplier;
            cloudShapeFactor = volumetricCloudData.cloudShapeFactor;
            cloudErosionFactor = volumetricCloudData.cloudErosionFactor;
            erosionScale = volumetricCloudData.erosionScale;
            shapeScale = volumetricCloudData.shapeScale;
            cloudAltitude = volumetricCloudData.cloudAltitude;
            cloudThickness = volumetricCloudData.cloudThickness;
            cloudScatteringTint = volumetricCloudData.cloudScatteringTint;
            areShadowsEnabled = volumetricCloudData.areShadowsEnabled;
            isWindInteractionActive = volumetricCloudData.isWindInteractionActive;
            lightProbeDimmer = volumetricCloudData.lightProbeDimmer;
            densityCurve = volumetricCloudData.densityCurve;
            erosionCurve = volumetricCloudData.erosionCurve;
            ambientOcclusionCurve = volumetricCloudData.ambientOcclusionCurve;
        }
    }

    [Serializable]
    public class LayerCloudData
    {
        public Color tintLayer2D;
        [Range(0f, 1f)]
        public float cloudLayerCoverage;
        public float cloudAltitude;
        public float exposureLayer2D;
        public bool areShadowsEnabled;
        public bool isWindInteractionActive;
        public float rotation;
        public float opacityLayer1;
        public float opacityLayer2;
        public float opacityLayer3;
        public float opacityLayer4;

        public LayerCloudData(LayerCloudData layerCloudData)
        {
            tintLayer2D = layerCloudData.tintLayer2D;
            cloudLayerCoverage = layerCloudData.cloudLayerCoverage;
            cloudAltitude = layerCloudData.cloudAltitude;
            exposureLayer2D = layerCloudData.exposureLayer2D;
            areShadowsEnabled = layerCloudData.areShadowsEnabled;
            isWindInteractionActive = layerCloudData.isWindInteractionActive;
            rotation = layerCloudData.rotation;
            opacityLayer1 = layerCloudData.opacityLayer1;
            opacityLayer2 = layerCloudData.opacityLayer2;
            opacityLayer3 = layerCloudData.opacityLayer3;
            opacityLayer4 = layerCloudData.opacityLayer4;
        }
    }
}