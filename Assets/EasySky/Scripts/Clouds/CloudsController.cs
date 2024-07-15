//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System;
using System.Threading;
using System.Threading.Tasks;
using EasySky.WeatherArea;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using static UnityEngine.Rendering.HighDefinition.VolumetricClouds;

namespace EasySky.Clouds
{
    /// <summary>
    /// This class controls the clouds parameters like coverage, shape, and more. Also controls the transition between cloud types.
    /// </summary>
    /// 
    public class CloudsController : MonoBehaviour
    {
        #region Constants
        private const int EDITOR_TEXTURE_RESOLUTION = 512;
        private const int BUILD_TEXTURE_RESOLUTION = 256;
#endregion

        #region Private Variable
        [SerializeField] private Volume _volume;

        private VolumetricClouds _volumetricClouds;
        private CloudLayer _cloudLayer;
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;
        private VolumetricCloudPresetData _volumetricCloudData;
        private LayerCloudPresetData _layerCloudData;
        private ComputeShader _lerpShader;
        private RenderTexture _lerpRenderTexture;
        private int textureSize = 256;
        private WeatherPresetData _currentData;
        private Texture2D _texture;
        #endregion

        #region Properties
        public float CloudAltitude
        {
            get => _volumetricClouds.bottomAltitude.value;
            set => _volumetricClouds.bottomAltitude.value = value;
        }

        public CloudMapResolution CloudPerformance
        {
            get => _volumetricClouds.cloudMapResolution.value;
            set => _volumetricClouds.cloudMapResolution.value = value;
        }

        public WindSpeedParameter VolumetricCloudWindSpeed
        {
            get => _volumetricClouds.globalWindSpeed;

            set
            {
                _volumetricClouds.globalWindSpeed.value = (WindParameter.WindParamaterValue)value;
            }
        }

        public WindSpeedParameter LayerCloudWindSpeed
        {
            get => _cloudLayer.layerA.scrollSpeed;

            set
            {
                _cloudLayer.layerA.scrollSpeed.value = (WindParameter.WindParamaterValue)value;
            }
        }

        public WindOrientationParameter LayerWindDirection
        {
            get => _cloudLayer.layerA.scrollOrientation;

            set
            {
                _cloudLayer.layerA.scrollOrientation.value = (WindParameter.WindParamaterValue)value;
            }
        }

        public bool EnableVolumetricCloudsShadows
        {
            get => _volumetricClouds.shadows.value;

            set
            {
                _volumetricClouds.shadows.value = value;
                _volumetricClouds.shadows.overrideState = value;
            }
        }

        public float LayerCloudsOpacity
        {
            get => _cloudLayer.opacity.value;
            set => _cloudLayer.opacity.value = value;
        }

        public Texture FirstLayerTexture
        {
            get => _volumetricClouds.cumulusMap.value;
            set => _volumetricClouds.cumulusMap.value = value;
        }

        public float VolumetricCloudDensityMultyplier
        {
            get => _volumetricClouds.densityMultiplier.value;
            set => _volumetricClouds.densityMultiplier.value = value;
        }

        public float VolumetricCloudShapeFactor
        {
            get => _volumetricClouds.shapeFactor.value;
            set => _volumetricClouds.shapeFactor.value = value;
        }

        public float VolumetricCloudShapeScale
        {
            get => _volumetricClouds.shapeScale.value;
            set => _volumetricClouds.shapeScale.value = value;
        }

        public float VolumetricCloudErosionFactor
        {
            get => _volumetricClouds.erosionFactor.value;
            set => _volumetricClouds.erosionFactor.value = value;
        }

        public float VolumetricCloudErosionScale
        {
            get => _volumetricClouds.erosionScale.value;
            set => _volumetricClouds.erosionScale.value = value;
        }

        public float CloudThickness
        {
            get => _volumetricClouds.altitudeRange.value;
            set => _volumetricClouds.altitudeRange.value = value;
        }

        public WindOrientationParameter WindDirection
        {
            get => _volumetricClouds.orientation;
            set => _volumetricClouds.orientation = value;
        }

        public float ExposureLayer2D
        {
            get => _cloudLayer.layerA.exposure.value;
            set => _cloudLayer.layerA.exposure.value = value;
        }

        public Color TintLayer2D
        {
            get => _cloudLayer.layerA.tint.value;
            set => _cloudLayer.layerA.tint.value = value;
        }

        public Color VolumetricScatteringTint
        {
            get => _volumetricClouds.scatteringTint.value;
            set => _volumetricClouds.scatteringTint.value = value;
        }

        public float Layer2DCoverage
        {
            get => _cloudLayer.opacity.value;
            set => _cloudLayer.opacity.value = value;
        }

        public float Layer2DOpacityA
        {
            set => _cloudLayer.layerA.opacityA.value = value;
        }

        public float Layer2DOpacityB
        {
            set => _cloudLayer.layerA.opacityB.value = value;
        }

        public float Layer2DOpacityR
        {
            set => _cloudLayer.layerA.opacityR.value = value;
        }

        public float Layer2DOpacityG
        {
            set => _cloudLayer.layerA.opacityG.value = value;
        }

        public float CloudLayerAltitude
        {
            set => _cloudLayer.layerA.altitude.value = value;
            get => _cloudLayer.layerA.altitude.value;
        }

        public float CloudLayerRotation
        {
            set => _cloudLayer.layerA.rotation.value = value;
            get => _cloudLayer.layerA.rotation.value;
        }

        public float AmbientProbeLightDimmer
        {
            set => _volumetricClouds.ambientLightProbeDimmer.value = value;
            get => _volumetricClouds.ambientLightProbeDimmer.value;
        }

        public bool CloudLayerCastShadows
        {
            set => _cloudLayer.layerA.castShadows.value = value;
        }

        public AnimationCurve DensityCurve
        {
            set => _volumetricClouds.densityCurve.value = value;
        }

        public AnimationCurve ErosionCurve
        {
            set => _volumetricClouds.erosionCurve.value = value;
        }

        public AnimationCurve AmbientOcclusionCurve
        {
            set => _volumetricClouds.ambientOcclusionCurve.value = value;
        }
      
        #endregion

        #region Unity Methods
        private void Awake()
        {
            SetupClouds();
            _lerpShader = Resources.Load<ComputeShader>("LerpShader");

#if UNITY_EDITOR
            textureSize = EDITOR_TEXTURE_RESOLUTION;
#else
            textureSize = BUILD_TEXTURE_RESOLUTION;
#endif
            _lerpRenderTexture = new RenderTexture(textureSize, textureSize, 0, RenderTextureFormat.ARGB64);
            _lerpRenderTexture.enableRandomWrite = true;
            _lerpRenderTexture.Create();

            _texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA64, false);
        }

        private void Start()
        {
            EasySkyWeatherManager.Instance.OnWindUpdated += OnWindUpdate;
        }

        private void OnDestroy()
        {
            EasySkyWeatherManager.Instance.OnWindUpdated -= OnWindUpdate;
        }
        #endregion

        #region Public Methods
        public void EnableVolumetricCloudWind(bool value)
        {
            VolumetricCloudWindSpeed = new WindSpeedParameter(value ? EasySkyWeatherManager.Instance.GlobalData.windSpeed : 0, WindParameter.WindOverrideMode.Multiply);
        }

        public void EnableLayerCloudWind(bool value)
        {
            LayerCloudWindSpeed = new WindSpeedParameter(value ? EasySkyWeatherManager.Instance.GlobalData.windSpeed : 0, WindParameter.WindOverrideMode.Multiply);
        }

        public void SetupClouds()
        {
            _volume.sharedProfile.TryGet(out _volumetricClouds);
            _volume.sharedProfile.TryGet(out _cloudLayer);
        }

        public void SetData(WeatherPresetData data)
        {
            SetVolumetricCloudPreset(data.VolumetricCloudPresetData);
            SetLayerCloudPreset(data.LayerCloudPresetData);
            _currentData = data;
        }

        public void SetVolumetricCloudPreset(VolumetricCloudPresetData cloudPresetData)
        {
            CloudPerformance = cloudPresetData.cloudData.cloudResolution;
            VolumetricCloudDensityMultyplier = cloudPresetData.cloudData.cloudDensityMultiplier;
            VolumetricCloudShapeFactor = cloudPresetData.cloudData.cloudShapeFactor;
            VolumetricCloudShapeScale = cloudPresetData.cloudData.shapeScale;
            VolumetricCloudErosionFactor = cloudPresetData.cloudData.cloudErosionFactor;
            VolumetricCloudErosionScale = cloudPresetData.cloudData.erosionScale;
            CloudAltitude = cloudPresetData.cloudData.cloudAltitude;
            CloudThickness = cloudPresetData.cloudData.cloudThickness;
            EnableVolumetricCloudsShadows = cloudPresetData.cloudData.areShadowsEnabled;
            AmbientProbeLightDimmer = cloudPresetData.cloudData.lightProbeDimmer;
            ErosionCurve = cloudPresetData.cloudData.erosionCurve;
            DensityCurve = cloudPresetData.cloudData.densityCurve;
            AmbientOcclusionCurve = cloudPresetData.cloudData.ambientOcclusionCurve;
            _volumetricCloudData = cloudPresetData;
            VolumetricScatteringTint = cloudPresetData.cloudData.cloudScatteringTint;
            EnableVolumetricCloudWind(cloudPresetData.cloudData.isWindInteractionActive);
        }

        public void SetLayerCloudPreset(LayerCloudPresetData cloudPresetData)
        {
            if (_cloudLayer == null)
            {
                SetupClouds();
            }
            LayerCloudsOpacity = cloudPresetData.cloudData.cloudLayerCoverage;
            ExposureLayer2D = cloudPresetData.cloudData.exposureLayer2D;
            TintLayer2D = cloudPresetData.cloudData.tintLayer2D;
            CloudLayerAltitude = cloudPresetData.cloudData.cloudAltitude;
            CloudLayerRotation = cloudPresetData.cloudData.rotation;
            _layerCloudData = cloudPresetData;
            Layer2DOpacityR = cloudPresetData.cloudData.opacityLayer1;
            Layer2DOpacityG = cloudPresetData.cloudData.opacityLayer2;
            Layer2DOpacityB = cloudPresetData.cloudData.opacityLayer3;
            Layer2DOpacityA = cloudPresetData.cloudData.opacityLayer4;
            CloudLayerCastShadows = cloudPresetData.cloudData.areShadowsEnabled;
            EnableLayerCloudWind(cloudPresetData.cloudData.isWindInteractionActive);
        }

        public async void InterpolateBetweenCloudPresets(WeatherPresetData data)
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
            }
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;

            SetupClouds();

            try
            {
                var progress = 0f;

                while (progress <= 1)
                {
                    if(_cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    progress += Time.deltaTime;
                    LerpVolCloudPreset(_currentData.VolumetricCloudPresetData, data.VolumetricCloudPresetData, progress);
                    LerpLayerCloudPreset(_currentData.LayerCloudPresetData, data.LayerCloudPresetData, progress);

                    await Task.Yield();
                }

                SetVolumetricCloudPreset(data.VolumetricCloudPresetData);
                SetLayerCloudPreset(data.LayerCloudPresetData);
            }
            catch (OperationCanceledException e)
            {
                Debug.LogWarning(e);
            }

            _currentData = data;
        }

        public async Task LerpTextures(Texture2D startTexture, Texture2D endTexture, float ammount)
        {
            int kernelIndex = _lerpShader.FindKernel("SampleCode");

            _lerpShader.SetInt("textureWidth", startTexture.width);
            _lerpShader.SetInt("textureHeight", startTexture.height);
            _lerpShader.SetTexture(kernelIndex, "startTexture", startTexture);
            _lerpShader.SetTexture(kernelIndex, "endTexture", endTexture);
            _lerpShader.SetTexture(kernelIndex, "output", _lerpRenderTexture);

            _lerpShader.SetFloat("ammount", ammount);
            _lerpShader.Dispatch(kernelIndex, startTexture.width, startTexture.height, 1);

            AsyncGPUReadbackRequest request = AsyncGPUReadback.Request(_lerpRenderTexture, 0);
            while (!request.done)
            {
                await Task.Yield();
            }

            var response = request.GetData<byte>();

            if (response.Length == textureSize * textureSize * 8)
            {
                Texture.DestroyImmediate(_texture);
                _texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA64, false);
                _texture.LoadRawTextureData<byte>(response);
                _texture.Apply();

                FirstLayerTexture = _texture;
            }

            _lerpRenderTexture.Release();
        }

        public void LerpVolCloudPreset(VolumetricCloudPresetData curentVolCloudData, VolumetricCloudPresetData targetVolCloudData, float ammount)
        {
            _volumetricCloudData = curentVolCloudData;
            VolumetricCloudDensityMultyplier = Mathf.Lerp(curentVolCloudData.cloudData.cloudDensityMultiplier, targetVolCloudData.cloudData.cloudDensityMultiplier, ammount);
            VolumetricCloudErosionFactor = Mathf.Lerp(curentVolCloudData.cloudData.cloudErosionFactor, targetVolCloudData.cloudData.cloudErosionFactor, ammount);
            VolumetricCloudErosionScale = Mathf.Lerp(curentVolCloudData.cloudData.erosionScale, targetVolCloudData.cloudData.erosionScale, ammount);
            CloudAltitude = Mathf.Lerp(curentVolCloudData.cloudData.cloudAltitude, targetVolCloudData.cloudData.cloudAltitude, ammount);
            CloudThickness = Mathf.Lerp(curentVolCloudData.cloudData.cloudThickness, targetVolCloudData.cloudData.cloudThickness, ammount);
            VolumetricCloudShapeFactor = Mathf.Lerp(curentVolCloudData.cloudData.cloudShapeFactor, targetVolCloudData.cloudData.cloudShapeFactor, ammount);
            VolumetricCloudShapeScale = Mathf.Lerp(curentVolCloudData.cloudData.shapeScale, targetVolCloudData.cloudData.shapeScale, ammount);
            VolumetricScatteringTint = Color.Lerp(curentVolCloudData.cloudData.cloudScatteringTint, targetVolCloudData.cloudData.cloudScatteringTint, ammount);
            AmbientProbeLightDimmer = Mathf.Lerp(curentVolCloudData.cloudData.lightProbeDimmer, targetVolCloudData.cloudData.lightProbeDimmer, ammount);
        }

        public void LerpLayerCloudPreset(LayerCloudPresetData curentCloudPresetData, LayerCloudPresetData targetCloudPresetData, float ammount)
        {
            _layerCloudData = curentCloudPresetData;
            Layer2DCoverage = Mathf.Lerp(curentCloudPresetData.cloudData.cloudLayerCoverage, targetCloudPresetData.cloudData.cloudLayerCoverage, ammount);
            ExposureLayer2D = Mathf.Lerp(curentCloudPresetData.cloudData.exposureLayer2D, targetCloudPresetData.cloudData.exposureLayer2D, ammount);
            TintLayer2D = Color.Lerp(curentCloudPresetData.cloudData.tintLayer2D, targetCloudPresetData.cloudData.tintLayer2D, ammount);
            CloudLayerAltitude = Mathf.Lerp(curentCloudPresetData.cloudData.cloudAltitude, targetCloudPresetData.cloudData.cloudAltitude, ammount);
            CloudLayerRotation = Mathf.Lerp(curentCloudPresetData.cloudData.rotation, targetCloudPresetData.cloudData.rotation, ammount);
        }
        #endregion

        #region Private Methods
        private void OnWindUpdate()
        {
            if (_volumetricCloudData.cloudData.isWindInteractionActive)
            {
                WindDirection = new WindOrientationParameter(EasySkyWeatherManager.Instance.GlobalData.windDirection, WindParameter.WindOverrideMode.Additive, true);
                VolumetricCloudWindSpeed = new WindSpeedParameter(EasySkyWeatherManager.Instance.GlobalData.windSpeed, WindParameter.WindOverrideMode.Multiply, true);
            }

            if (_layerCloudData.cloudData.isWindInteractionActive)
            {
                LayerWindDirection = new WindOrientationParameter(EasySkyWeatherManager.Instance.GlobalData.windDirection, WindParameter.WindOverrideMode.Additive, true);
                LayerCloudWindSpeed = new WindSpeedParameter(EasySkyWeatherManager.Instance.GlobalData.windSpeed, WindParameter.WindOverrideMode.Multiply, true);
            }
        }
        #endregion
    }
}