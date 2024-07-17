﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Rendering;
//using UnityEngine.Experimental.TerrainAPI;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;
using UnityEditor.Build;
#if NM_HDRP
using UnityEngine.Rendering.HighDefinition;
#endif

namespace WorldStreamer2
{
    /// <summary>
    /// Split terrain.
    /// </summary>
    public class TerrainManager : Editor
    {
        private string _currentScene;
        private TerrainManagerSettings _terrainManagerSettings;


        private Terrain _parentTerrain;
        private Scene _previewScene;
        private Scene _lastScene;
        private float _minHeightTerrain;
        private float _maxHeightTerrain;

        /// <summary>
        /// The scroll position.
        /// </summary>
        private Vector2 _scrollPos;

        private List<MeshFilter> _meshFiltersToFix = new List<MeshFilter>();


        public TerrainManagerSettings ManagerSettings
        {
            get => _terrainManagerSettings;
            set => _terrainManagerSettings = value;
        }

        public float MaxHeightTerrain
        {
            get => _maxHeightTerrain;
            set => _maxHeightTerrain = value;
        }

        public float MinHeightTerrain
        {
            get => _minHeightTerrain;
            set => _minHeightTerrain = value;
        }

        public void OnEnable()
        {
            if (RenderPipelineManager.currentPipeline == null)
            {
            }
            else
            {
                BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
                BuildTargetGroup targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
                NamedBuildTarget namedBuildTarget = UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup(targetGroup);

                string srpType = GraphicsSettings.defaultRenderPipeline.GetType().ToString();
                if (srpType.Contains("HDRenderPipelineAsset"))
                {
                    string defineHdrp = "NM_HDRP";
                    string define = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);
                    if (!define.Contains(defineHdrp))
                        PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, define + " " + defineHdrp);
                }
                else if (srpType.Contains("UniversalRenderPipelineAsset") ||
                         srpType.Contains("LightweightRenderPipelineAsset"))
                {
                    string defineUrp = "NM_URP";
                    string define = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);
                    if (!define.Contains(defineUrp))
                        PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, define + " " + defineUrp);
                }
            }
        }

        public void OnGUI()
        {
            if (_currentScene != EditorSceneManager.GetActiveScene().path || ManagerSettings == null)
            {
                SceneChanged();
            }

            if (ManagerSettings == null)
                return;

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            if (ManagerSettings.colorSpaceLast == ColorSpace.Uninitialized)
            {
                ManagerSettings.ambientLightColor = PlayerSettings.colorSpace == ColorSpace.Gamma ? new Color(1.23f, 1.23f, 1.23f) : new Color(1f, 1f, 1f);

                ManagerSettings.colorSpaceLast = PlayerSettings.colorSpace;
            }
            else if (ManagerSettings.colorSpaceLast != PlayerSettings.colorSpace)
            {
                if (ManagerSettings.colorSpaceLast == ColorSpace.Gamma)
                {
                    ManagerSettings.ambientLightColor.r /= 1.23f;
                    ManagerSettings.ambientLightColor.g /= 1.23f;
                    ManagerSettings.ambientLightColor.b /= 1.23f;
                }
                else
                {
                    ManagerSettings.ambientLightColor.r *= 1.23f;
                    ManagerSettings.ambientLightColor.g *= 1.23f;
                    ManagerSettings.ambientLightColor.b *= 1.23f;
                }

                ManagerSettings.colorSpaceLast = PlayerSettings.colorSpace;
            }


            Terrain[] terrains = Terrain.activeTerrains;


            SerializedObject serializedManagerSettings = new SerializedObject(ManagerSettings);

            SerializedProperty listTerrainTrees = serializedManagerSettings.FindProperty("terrainTrees");

            for (int i = 0; i < terrains.Length; i++)
            {
                if (terrains[i] != null && terrains[i].terrainData != null)
                {
                    TreePrototype[] prototypes = terrains[i].terrainData.treePrototypes;
                    foreach (var treePrototype in prototypes)
                    {
                        bool toAdd = true;
                        foreach (var terrainTree in ManagerSettings.terrainTrees)
                        {
                            if (treePrototype.prefab == terrainTree.tree)
                            {
                                toAdd = false;
                                break;
                            }
                        }

                        if (toAdd)
                        {
                            ManagerSettings.terrainTrees.Add(new TerrainTrees() {tree = treePrototype.prefab});
                        }
                    }
                }
            }

            EditorGUILayout.Space();
#if NM_HDRP
            GUILayout.Label("[HDRP]", EditorStyles.boldLabel);
            EditorGUILayout.Space();
#endif


#if NM_URP
            GUILayout.Label("[URP]", EditorStyles.boldLabel);
            EditorGUILayout.Space();


#endif
            ////Vertical
            GUILayout.Label("Terrains settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            TerrainSettingsUI.TerrainSettingsGUI(this);

            EditorGUILayout.Space();

            if (GUILayout.Button("Set settings for selected terrains"))
            {
                TerrainSettingsSetter.SetTerrainSettings(ManagerSettings);
            }

            if (GUILayout.Button("Set settings for all terrains"))
            {
                TerrainSettingsSetter.SetTerrainSettings(ManagerSettings, true);
            }

            EditorGUILayout.Space();


            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            ////Vertical
            GUILayout.Label("Terrain splitter", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.Space();
            ManagerSettings.terrainsDataPath =
                EditorGUILayout.TextField("Terrain create folder", ManagerSettings.terrainsDataPath);
            ManagerSettings.splitSize =
                Mathf.NextPowerOfTwo(EditorGUILayout.IntSlider("Split size", ManagerSettings.splitSize, 2, 32));
            ManagerSettings.addTerrainCulling =
                EditorGUILayout.Toggle("Add terrain culling", ManagerSettings.addTerrainCulling);

            if (GUILayout.Button("Split selected terrains"))
            {
                TerrainSplitter.SplitTerrain(ManagerSettings);
            }

            if (GUILayout.Button("Split all terrains"))
            {
                TerrainSplitter.SplitTerrain(ManagerSettings, true);
            }

            EditorGUILayout.Space();

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();


            ////Vertical
            GUILayout.Label("Terrain Low Poly Mesh Generator", EditorStyles.boldLabel);


            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.Space();
            ManagerSettings.terrainPath =
                EditorGUILayout.TextField("Terrain create folder", ManagerSettings.terrainPath);
            ManagerSettings.ambientLightColor = EditorGUILayout.ColorField(new GUIContent("Ambient color"),
                ManagerSettings.ambientLightColor, true, false, true);
            EditorGUILayout.Space();

            ManagerSettings.terrainPrefixName =
                EditorGUILayout.TextField("Terrain Mesh Name", ManagerSettings.terrainPrefixName);
            ManagerSettings.terrainLod =
                EditorGUILayout.IntSlider("Terrain lod", ManagerSettings.terrainLod, 0, 8);
            EditorGUILayout.Space();
            ManagerSettings.basemapResolution = Mathf.NextPowerOfTwo(
                EditorGUILayout.IntSlider("Basemap Resolution", ManagerSettings.basemapResolution, 128, 4096));
            ManagerSettings.useBaseMap =
                EditorGUILayout.Toggle("Use base map", ManagerSettings.useBaseMap);
            ManagerSettings.useSmoothness =
                EditorGUILayout.Toggle("Use smoothness map", ManagerSettings.useSmoothness);
#if NM_URP
            ManagerSettings.useMaskSmoothnessURP = EditorGUILayout.Toggle("Use mask map", ManagerSettings.useMaskSmoothnessURP);

#endif


            ManagerSettings.terrainNormalDetails = Mathf.NextPowerOfTwo(
                EditorGUILayout.IntSlider("Normal Details", ManagerSettings.terrainNormalDetails, 1, 32));
            ManagerSettings.terrainNormalStrength = EditorGUILayout.Slider("Normal strength",
                ManagerSettings.terrainNormalStrength, 1, 10);
            ManagerSettings.useTerrainNormal = EditorGUILayout.Toggle("Create normal from shape",
                ManagerSettings.useTerrainNormal);
            ManagerSettings.useTextureNormal = EditorGUILayout.Toggle("Create normal from textures",
                ManagerSettings.useTextureNormal);


            EditorGUILayout.Space();
            ManagerSettings.yOffset = EditorGUILayout.FloatField("Y offset", ManagerSettings.yOffset);
            ManagerSettings.addVerticesDown =
                EditorGUILayout.Toggle("Add vertices down", ManagerSettings.addVerticesDown);
            if (ManagerSettings.addVerticesDown)
                ManagerSettings.verticesDownDistance = EditorGUILayout.Slider("Vertices down distance",
                    ManagerSettings.verticesDownDistance, 1, 100);


            EditorGUILayout.Space();


            SerializedProperty trainglesTreesMax = serializedManagerSettings.FindProperty("trainglesTreesMax");

            EditorGUILayout.PropertyField(trainglesTreesMax,
                new GUIContent("Trees Max Traingles", "Trees Max Traingles to Export"));

            listTerrainTrees.isExpanded = EditorGUILayout.Foldout(listTerrainTrees.isExpanded,
                new GUIContent("Tree Prototypes", "Tree Prototypes to generate mesh lod"));
            EditorGUI.indentLevel++;
            if (listTerrainTrees.isExpanded)
            {
                //EditorGUILayout.PropertyField(listTerrainTrees.FindPropertyRelative("Array.size"));
                for (int i = 0; i < listTerrainTrees.arraySize; i++)
                {
                    SerializedProperty scriptable = listTerrainTrees.GetArrayElementAtIndex(i);

                    EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));

                    if (scriptable.FindPropertyRelative("tree").objectReferenceValue != null)
                    {
                        scriptable.isExpanded = EditorGUILayout.Foldout(scriptable.isExpanded, new GUIContent(scriptable.FindPropertyRelative("tree").objectReferenceValue.name), false);
                        EditorGUILayout.PropertyField(scriptable.FindPropertyRelative("active"), new GUIContent(""),
                            GUILayout.ExpandWidth(false), GUILayout.MaxWidth(40));
                    }
                    else
                    {
                        GUIStyle style = new GUIStyle
                        {
                            richText = true
                        };
                        EditorGUILayout.LabelField($"<color=red><b>Missing Tree id: {i}</b></color>", style);
                    }

                    //EditorGUILayout.LabelField();

                    EditorGUILayout.EndHorizontal();
                    if (scriptable.isExpanded)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(scriptable.FindPropertyRelative("tree"));
                        EditorGUI.indentLevel--;
                        EditorGUILayout.Space();
                    }
                }
            }

            if (GUILayout.Button("Refresh tree prototypes"))
            {
                ManagerSettings.terrainTrees.Clear();
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Export selected Terrains data: shape and trees into LP Mesh"))
            {
                TerrainToMesh(true);
            }

            if (GUILayout.Button("Export selected Terrains data: shape into LP Mesh"))
            {
                TerrainToMesh();
            }


            if (GUILayout.Button("Export selected Terrains data: trees into LP Mesh"))
            {
                ExportTreesForSelectedTerrain();
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("Export all Terrains data: shape and trees into LP Mesh"))
            {
                TerrainToMesh(true, true);
            }

            if (GUILayout.Button("Export all Terrains data: shape into LP Mesh"))
            {
                TerrainToMesh(false, true);
            }


            if (GUILayout.Button("Export all Terrains data: trees into LP Mesh"))
            {
                ExportTreesForSelectedTerrain(true);
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Export all Terrains Rim"))
            {
                TerrainToMeshRim(true);
            }

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndScrollView();
            serializedManagerSettings.ApplyModifiedProperties();
        }

        /// <summary>
        /// Creates the settings gameObject.
        /// </summary>
        private void SceneChanged()
        {
            _currentScene = EditorSceneManager.GetActiveScene().path;

#if UNITY_2022_1_OR_NEWER && !Unity_2021_3_1 && !Unity_2021_3_2 && !Unity_2021_3_3 && !Unity_2021_3_4 && !Unity_2021_3_5 && !Unity_2021_3_6 && !Unity_2021_3_7 && !Unity_2021_3_8 && !Unity_2021_3_9 && !Unity_2021_3_10 && !Unity_2021_3_11 && !Unity_2021_3_12 && !Unity_2021_3_13 && !Unity_2021_3_14 && !Unity_2021_3_15 && !Unity_2021_3_16 && !Unity_2021_3_17
            ManagerSettings = FindAnyObjectByType<TerrainManagerSettings>();
#else
            ManagerSettings = FindObjectOfType<TerrainManagerSettings>();
#endif

            if (ManagerSettings != null) return;

            GameObject gameObject = new GameObject("_TerrainManagerSettings");
            ManagerSettings = gameObject.AddComponent<TerrainManagerSettings>();
            ManagerSettings.materialTemplate =
                AssetDatabase.GetBuiltinExtraResource<Material>("Default-Terrain-Standard.mat");
            gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSaveInBuild;
            gameObject.tag = "EditorOnly";
        }


        private Texture ExportBaseMap(Terrain terrainBase, string terrainName, out Texture mask)
        {
            mask = null;
            float baseMapDistance = terrainBase.basemapDistance;

            if (ManagerSettings.useBaseMap)
            {
                terrainBase.basemapDistance = 20000;
            }
            else
            {
                terrainBase.basemapDistance = 0;
            }

            _previewScene = EditorSceneManager.NewPreviewScene();
            Material sky = RenderSettings.skybox;
            float ambient = RenderSettings.ambientIntensity;
            AmbientMode ambientMode = RenderSettings.ambientMode;
            Color ambientColor = RenderSettings.ambientLight;
            if (SceneView.lastActiveSceneView.camera != null)
            {
                _lastScene = SceneView.lastActiveSceneView.camera.scene;
                SceneView.lastActiveSceneView.camera.scene = _previewScene;

                RenderSettings.skybox = null;
                RenderSettings.ambientIntensity = 0;
                RenderSettings.ambientMode = AmbientMode.Flat;
                RenderSettings.ambientLight = ManagerSettings.ambientLightColor;
            }


            Terrain terrain = Instantiate(terrainBase);
            terrain.drawTreesAndFoliage = false;
            GameObject go = terrain.gameObject;
            EditorSceneManager.MoveGameObjectToScene(go, _previewScene);
            go.transform.position = Vector3.zero;


#if !NM_URP && !NM_HDRP
            terrain.materialTemplate = new Material(Shader.Find("NatureManufacture Shaders/Terrain/StandardAlbedo"));
            //Debug.Log(ManagerSettings.ambientLightColor);
            terrain.materialTemplate.SetColor("_AmbientColor", ManagerSettings.ambientLightColor);
#endif

            GameObject cameraGo = new GameObject("PreviewCamera");
            EditorSceneManager.MoveGameObjectToScene(cameraGo, _previewScene);


            Camera cam = cameraGo.AddComponent<Camera>();


            cam.rect = new Rect(0, 0, 1, 1);
            cam.orthographic = true;
            cam.depthTextureMode = DepthTextureMode.Depth;


            cam.rect = new Rect(0, 0, 1, 1);

            Bounds currentBounds = terrain.terrainData.bounds;

            cam.transform.eulerAngles = new Vector3(0, 0, 0);


            cam.transform.position = currentBounds.center + Vector3.up * currentBounds.max.y + new Vector3(0, 1, 0);


            Selection.activeGameObject = cam.gameObject;


            cam.transform.eulerAngles = new Vector3(90, 0, 0);

            cam.nearClipPlane = 0.5f;
            cam.farClipPlane = cam.transform.position.y + 1000.0f;


            float aspectSize = terrain.terrainData.size.x / terrain.terrainData.size.z;
            cam.aspect = aspectSize;
            if (aspectSize < 1)
                aspectSize = 1;
            cam.orthographicSize = Mathf.Max((currentBounds.max.x - currentBounds.min.x) / 2.0f,
                (currentBounds.max.z - currentBounds.min.z) / 2.0f) / aspectSize;

            cam.scene = _previewScene;

            Debug.Log(aspectSize);

            RenderTexture rt = new RenderTexture(ManagerSettings.basemapResolution, ManagerSettings.basemapResolution, 32);
            cam.targetTexture = rt;
            Texture2D screenShot = new Texture2D(ManagerSettings.basemapResolution, ManagerSettings.basemapResolution, TextureFormat.ARGB32, false);

            cam.Render();

            RenderTexture.active = rt;
            screenShot.ReadPixels(
                new Rect(0, 0, ManagerSettings.basemapResolution, (int) (ManagerSettings.basemapResolution)), 0,
                0);


            cam.targetTexture = null;
            RenderTexture.active = null;
            DestroyImmediate(rt);


            DestroyImmediate(cameraGo);

            // EditorSceneManager.MoveGameObjectToScene(cameraGo, EditorSceneManager.GetActiveScene());
            DestroyImmediate(go);

            EditorUtility.SetDirty(this);
            AssetDatabase.Refresh();

            if (_previewScene != null)
            {
                EditorSceneManager.ClosePreviewScene(_previewScene);
                SceneView.lastActiveSceneView.camera.scene = _lastScene;
                RenderSettings.skybox = sky;

                RenderSettings.ambientIntensity = ambient;
                RenderSettings.ambientMode = ambientMode;
                RenderSettings.ambientLight = ambientColor;
            }

            terrainBase.basemapDistance = baseMapDistance;

#if NM_URP
            screenShot = ExportTextureSmoothness(terrainBase, terrainName, 4);

            if (ManagerSettings.useMaskSmoothnessURP)
            {
                Texture2D smoothnessTexture = ExportTextureSmoothness(terrainBase, terrainName, 1);
                Texture2D aoTexture = ExportTextureSmoothness(terrainBase, terrainName, 2);
                Texture2D metalicTexture = ExportTextureSmoothness(terrainBase, terrainName, 3);


                for (int x = 0; x < metalicTexture.width; x++)
                {
                    for (int y = 0; y < metalicTexture.height; y++)
                    {
                        Color metalicColor = metalicTexture.GetPixel(x, y);
                        Color aoColor = aoTexture.GetPixel(x, y);
                        Color smoothnesColor = smoothnessTexture.GetPixel(x, y);

                        metalicColor.g = aoColor.r;
                        metalicColor.b = 0;
                        metalicColor.a = smoothnesColor.r;

                        metalicTexture.SetPixel(x, y, metalicColor);
                    }
                }

                byte[] byteMask = metalicTexture.EncodeToPNG();

                string nameMask = ManagerSettings.terrainPath + "/T_" + terrainName + "_M.png";
                System.IO.File.WriteAllBytes(Application.dataPath + nameMask, byteMask);
                AssetDatabase.Refresh();


                TextureImporter importerMask = (TextureImporter) AssetImporter.GetAtPath("Assets" + nameMask);
                importerMask.wrapMode = TextureWrapMode.Clamp;
                importerMask.streamingMipmaps = true;
                importerMask.sRGBTexture = false;
                importerMask.anisoLevel = 8;
                importerMask.SaveAndReimport();
                mask = AssetDatabase.LoadAssetAtPath<Texture>("Assets" + nameMask);
            }
            else
                mask = null;
#endif

            if (ManagerSettings.useSmoothness)
            {
                Texture2D smoothnessTexture = ExportTextureSmoothness(terrainBase, terrainName);

                for (int x = 0; x < screenShot.width; x++)
                {
                    for (int y = 0; y < screenShot.height; y++)
                    {
                        Color smoothnesColor = smoothnessTexture.GetPixel(x, y);
                        Color basemapColor = screenShot.GetPixel(x, y);
                        basemapColor.a = smoothnesColor.r;

                        // screenShot.SetPixel(x, y, smoothnesColor);
                        screenShot.SetPixel(x, y, basemapColor);
                    }
                }
            }


            byte[] bytesAtlas = screenShot.EncodeToPNG();

            string name = ManagerSettings.terrainPath + "/T_" + terrainName + "_BC.png";
            System.IO.File.WriteAllBytes(Application.dataPath + name, bytesAtlas);
            AssetDatabase.Refresh();

            //Debug.Log($"smoothness map name {name}");
            TextureImporter importer = (TextureImporter) AssetImporter.GetAtPath("Assets" + name);
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.streamingMipmaps = true;
            importer.anisoLevel = 8;
            importer.SaveAndReimport();

            return AssetDatabase.LoadAssetAtPath<Texture>("Assets" + name);
        }


        private Texture ExportBaseMapHdrp(Terrain terrainBase, string terrainName, out Texture mask)
        {
            float baseMapDistance = terrainBase.basemapDistance;

            if (ManagerSettings.useBaseMap)
            {
                terrainBase.basemapDistance = 20000;
            }
            else
            {
                terrainBase.basemapDistance = 0;
            }


            _previewScene = EditorSceneManager.NewPreviewScene();

            Material sky = RenderSettings.skybox;
            float ambient = RenderSettings.ambientIntensity;
            AmbientMode ambientMode = RenderSettings.ambientMode;
            Color ambientColor = RenderSettings.ambientLight;

            if (SceneView.lastActiveSceneView.camera != null)
            {
                _lastScene = SceneView.lastActiveSceneView.camera.scene;
                SceneView.lastActiveSceneView.camera.scene = _previewScene;

                RenderSettings.skybox = null;
                RenderSettings.ambientIntensity = 0;
                RenderSettings.ambientMode = AmbientMode.Flat;
                RenderSettings.ambientLight = ManagerSettings.ambientLightColor;
            }

            GameObject light = new GameObject("lightNM");
            EditorSceneManager.MoveGameObjectToScene(light, _previewScene);
            light.transform.eulerAngles = new Vector3(90, 0, 0);

#if NM_HDRP
            var hdLight = light.AddHDLight(HDLightTypeAndShape.Directional);
            //hdLight.intensity = 6.283186f;
            hdLight.intensity = 3.141593f;

            hdLight.EnableColorTemperature(false);
            // hdLight.SetColor(Color.white, 6500);

#endif

            Terrain terrain = Instantiate(terrainBase);
            terrain.drawTreesAndFoliage = false;
            bool drawInstanced = terrain.drawInstanced;
            terrain.drawInstanced = false;


            bool blend = terrain.materialTemplate.HasProperty("_EnableHeightBlend") &&
                         terrain.materialTemplate.GetFloat("_EnableHeightBlend") > 0;
            float heightTransition = 0;
            if (blend)
            {
                heightTransition = terrain.materialTemplate.GetFloat("_HeightTransition");
            }

            terrain.materialTemplate = new Material(Shader.Find("HDRP/TerrainLitNM Normal"));

            terrain.materialTemplate.SetFloat("_metallicMap", -1);

#if NM_HDRP
            if (blend)
            {
                CoreUtils.SetKeyword(terrain.materialTemplate, "_TERRAIN_BLEND_HEIGHT", blend);
                terrain.materialTemplate.SetFloat("_HeightTransition", heightTransition);
            }
#endif


            GameObject go = terrain.gameObject;
            EditorSceneManager.MoveGameObjectToScene(go, _previewScene);
            go.transform.position = Vector3.zero;

            GameObject cameraGo = new GameObject("PreviewCamera");
            EditorSceneManager.MoveGameObjectToScene(cameraGo, _previewScene);


            Camera cam = cameraGo.AddComponent<Camera>();

#if NM_HDRP
            HDAdditionalCameraData cameraData = cameraGo.AddComponent<HDAdditionalCameraData>();
            cameraData.clearColorMode = HDAdditionalCameraData.ClearColorMode.Color;

            FrameSettings frameSettings = cameraData.renderingPathCustomFrameSettings;
            cameraData.customRenderingSettings = true;

            FrameSettingsOverrideMask frameSettingsOverrideMask =
 cameraData.renderingPathCustomFrameSettingsOverrideMask;


            frameSettingsOverrideMask.mask[(uint)FrameSettingsField.Postprocess] = true;
            frameSettings.SetEnabled(FrameSettingsField.Postprocess, false);

            frameSettingsOverrideMask.mask[(uint)FrameSettingsField.ExposureControl] = true;
            frameSettings.SetEnabled(FrameSettingsField.ExposureControl, false);

            frameSettingsOverrideMask.mask[(uint)FrameSettingsField.DirectSpecularLighting] = true;
            frameSettings.SetEnabled(FrameSettingsField.DirectSpecularLighting, false);

            frameSettingsOverrideMask.mask[(uint)FrameSettingsField.SkyReflection] = true;
            frameSettings.SetEnabled(FrameSettingsField.SkyReflection, false);

            cameraData.renderingPathCustomFrameSettings = frameSettings;
            cameraData.renderingPathCustomFrameSettingsOverrideMask = frameSettingsOverrideMask;
#endif


            cam.rect = new Rect(0, 0, 1, 1);
            cam.orthographic = true;
            cam.depthTextureMode = DepthTextureMode.Depth;


            cam.rect = new Rect(0, 0, 1, 1);

            Bounds currentBounds = terrain.terrainData.bounds;

            cam.transform.eulerAngles = new Vector3(0, 0, 0);


            cam.transform.position = currentBounds.center - cam.transform.forward * (currentBounds.max.y + 300);

            GameObject centerObject = new GameObject("Center");
            EditorSceneManager.MoveGameObjectToScene(centerObject, _previewScene);

            centerObject.transform.position = currentBounds.center;
            cam.transform.parent = centerObject.transform;
            centerObject.transform.eulerAngles = new Vector3(90, 0, 0);

            cam.nearClipPlane = 0.5f;
            cam.farClipPlane = cam.transform.position.y + 300.0f;

            float aspectSize = terrain.terrainData.size.x / terrain.terrainData.size.z;
            cam.aspect = aspectSize;
            if (aspectSize < 1)
                aspectSize = 1;
            cam.orthographicSize = Mathf.Max((currentBounds.max.x - currentBounds.min.x) / 2.0f,
                (currentBounds.max.z - currentBounds.min.z) / 2.0f) / aspectSize;

            cam.scene = _previewScene;

            RenderTexture rt = new RenderTexture(ManagerSettings.basemapResolution,
                ManagerSettings.basemapResolution, 32);
            cam.targetTexture = rt;
            Texture2D screenShot = new Texture2D(ManagerSettings.basemapResolution,
                ManagerSettings.basemapResolution, TextureFormat.ARGB32, false);

            cam.Render();

            RenderTexture.active = rt;
            screenShot.ReadPixels(
                new Rect(0, 0, ManagerSettings.basemapResolution, ManagerSettings.basemapResolution), 0,
                0);

            cam.targetTexture = null;
            RenderTexture.active = null;
            DestroyImmediate(rt);


            cam.transform.parent = null;
            DestroyImmediate(centerObject);
            DestroyImmediate(terrain);
            DestroyImmediate(cameraGo);

            EditorUtility.SetDirty(this);
            AssetDatabase.Refresh();

            if (_previewScene != null)
            {
                EditorSceneManager.ClosePreviewScene(_previewScene);
                SceneView.lastActiveSceneView.camera.scene = _lastScene;
                RenderSettings.skybox = sky;

                RenderSettings.ambientIntensity = ambient;
                RenderSettings.ambientMode = ambientMode;
                RenderSettings.ambientLight = ambientColor;
            }

            terrainBase.drawInstanced = drawInstanced;
            terrainBase.basemapDistance = baseMapDistance;

            byte[] bytesAtlas = screenShot.EncodeToPNG();

            string name = ManagerSettings.terrainPath + "/T_" + terrainName + "_BC.png";
            System.IO.File.WriteAllBytes(Application.dataPath + name, bytesAtlas);
            AssetDatabase.Refresh();

            //Debug.Log("Assets" + name);
            TextureImporter importer = (TextureImporter) AssetImporter.GetAtPath("Assets" + name);
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.streamingMipmaps = true;
            importer.anisoLevel = 8;
            importer.SaveAndReimport();

            GC.Collect();


            if (ManagerSettings.useSmoothness)
            {
                Texture2D metalicTexture = ExportTextureSmoothnessHdrp(terrainBase, terrainName, 1);
                Texture2D aoTexture = ExportTextureSmoothnessHdrp(terrainBase, terrainName, 2);
                Texture2D smoothnessTexture = ExportTextureSmoothnessHdrp(terrainBase, terrainName, 3);


                for (int x = 0; x < metalicTexture.width; x++)
                {
                    for (int y = 0; y < metalicTexture.height; y++)
                    {
                        Color metalicColor = metalicTexture.GetPixel(x, y);
                        Color aoColor = aoTexture.GetPixel(x, y);
                        Color smoothnesColor = smoothnessTexture.GetPixel(x, y);

                        metalicColor.g = aoColor.r;
                        metalicColor.b = 0;
                        metalicColor.a = smoothnesColor.r;

                        metalicTexture.SetPixel(x, y, metalicColor);
                    }
                }

                byte[] byteMask = metalicTexture.EncodeToPNG();

                string nameMask = ManagerSettings.terrainPath + "/T_" + terrainName + "_M.png";
                System.IO.File.WriteAllBytes(Application.dataPath + nameMask, byteMask);
                AssetDatabase.Refresh();


                TextureImporter importerMask = (TextureImporter) AssetImporter.GetAtPath("Assets" + nameMask);
                importerMask.wrapMode = TextureWrapMode.Clamp;
                importerMask.streamingMipmaps = true;
                importerMask.sRGBTexture = false;
                importerMask.anisoLevel = 8;
                importerMask.SaveAndReimport();

                mask = AssetDatabase.LoadAssetAtPath<Texture>("Assets" + nameMask);
            }
            else
                mask = null;

            return AssetDatabase.LoadAssetAtPath<Texture>("Assets" + name);
        }

        private Texture2D ExportTextureNormalmapHdrp(Terrain terrainBase, string terrainName)
        {
            _previewScene = EditorSceneManager.NewPreviewScene();
            Material sky = RenderSettings.skybox;
            float ambient = RenderSettings.ambientIntensity;
            AmbientMode ambientMode = RenderSettings.ambientMode;
            Color ambientColor = RenderSettings.ambientLight;
            if (SceneView.lastActiveSceneView.camera != null)
            {
                _lastScene = SceneView.lastActiveSceneView.camera.scene;
                SceneView.lastActiveSceneView.camera.scene = _previewScene;

                RenderSettings.skybox = null;
                RenderSettings.ambientIntensity = 0;
                RenderSettings.ambientMode = AmbientMode.Flat;
                RenderSettings.ambientLight = ManagerSettings.ambientLightColor;
            }


            Terrain terrain = Instantiate(terrainBase);
            terrain.drawTreesAndFoliage = false;
            bool drawInstanced = terrain.drawInstanced;
            terrain.drawInstanced = false;

            bool blend = terrain.materialTemplate.HasProperty("_EnableHeightBlend") && (terrain.materialTemplate.GetFloat("_EnableHeightBlend") > 0);
            float heightTransition = 0;
            if (blend)
            {
                heightTransition = terrain.materialTemplate.GetFloat("_HeightTransition");
            }

            terrain.materialTemplate = new Material(Shader.Find("HDRP/TerrainLitNM Normal"));

#if NM_HDRP
            if (blend)
            {
                CoreUtils.SetKeyword(terrain.materialTemplate, "_TERRAIN_BLEND_HEIGHT", blend);
                terrain.materialTemplate.SetFloat("_HeightTransition", heightTransition);
            }
#endif

            GameObject go = terrain.gameObject;
            EditorSceneManager.MoveGameObjectToScene(go, _previewScene);
            go.transform.position = Vector3.zero;


            GameObject light = new GameObject("light");
            EditorSceneManager.MoveGameObjectToScene(light, _previewScene);
            light.transform.eulerAngles = new Vector3(90, 0, 0);

#if NM_HDRP
            var hdLight = light.AddHDLight(HDLightTypeAndShape.Directional);
            hdLight.intensity = 3.141593f;
#endif

            GameObject cameraGo = new GameObject("PreviewCamera");
            EditorSceneManager.MoveGameObjectToScene(cameraGo, _previewScene);


            Camera cam = cameraGo.AddComponent<Camera>();
#if NM_HDRP
            HDAdditionalCameraData cameraData = cameraGo.AddComponent<HDAdditionalCameraData>();
            cameraData.clearColorMode = HDAdditionalCameraData.ClearColorMode.None;
            FrameSettings frameSettings = cameraData.renderingPathCustomFrameSettings;

            FrameSettingsOverrideMask frameSettingsOverrideMask =
 cameraData.renderingPathCustomFrameSettingsOverrideMask;
            cameraData.customRenderingSettings = true;

            frameSettingsOverrideMask.mask[(uint)FrameSettingsField.Postprocess] = true;
            frameSettings.SetEnabled(FrameSettingsField.Postprocess, false);

            frameSettingsOverrideMask.mask[(uint)FrameSettingsField.ExposureControl] = true;
            frameSettings.SetEnabled(FrameSettingsField.ExposureControl, false);

            frameSettingsOverrideMask.mask[(uint)FrameSettingsField.DirectSpecularLighting] = true;
            frameSettings.SetEnabled(FrameSettingsField.DirectSpecularLighting, false);

            cameraData.renderingPathCustomFrameSettings = frameSettings;
            cameraData.renderingPathCustomFrameSettingsOverrideMask = frameSettingsOverrideMask;
#endif

            cam.rect = new Rect(0, 0, 1, 1);
            cam.orthographic = true;
            cam.depthTextureMode = DepthTextureMode.Depth;


            cam.rect = new Rect(0, 0, 1, 1);

            Bounds currentBounds = terrain.terrainData.bounds;

            cam.transform.eulerAngles = new Vector3(0, 0, 0);


            cam.transform.position = currentBounds.center - cam.transform.forward * currentBounds.max.y;

            GameObject centerObject = new GameObject("Center");
            centerObject.transform.position = currentBounds.center;
            cam.transform.parent = centerObject.transform;
            centerObject.transform.eulerAngles = new Vector3(90, 0, 0);

            cam.nearClipPlane = 0.5f;
            cam.farClipPlane = cam.transform.position.y + 10.0f;


            float aspectSize = terrain.terrainData.size.x / terrain.terrainData.size.z;
            cam.aspect = aspectSize;
            if (aspectSize < 1)
                aspectSize = 1;
            cam.orthographicSize = Mathf.Max((currentBounds.max.x - currentBounds.min.x) / 2.0f,
                (currentBounds.max.z - currentBounds.min.z) / 2.0f) / aspectSize;

            cam.scene = _previewScene;

            RenderTexture rt = new RenderTexture(ManagerSettings.basemapResolution,
                ManagerSettings.basemapResolution, 32);
            cam.targetTexture = rt;
            Texture2D screenShot = new Texture2D(ManagerSettings.basemapResolution,
                ManagerSettings.basemapResolution, TextureFormat.ARGB32, false);

            cam.Render();

            RenderTexture.active = rt;
            screenShot.ReadPixels(
                new Rect(0, 0, ManagerSettings.basemapResolution, ManagerSettings.basemapResolution), 0,
                0);

            cam.targetTexture = null;
            RenderTexture.active = null;
            DestroyImmediate(rt);


            //byte[] bytesAtlas = screenShot.EncodeToPNG();

            //string name = terrainManagerSettings.terrainPath + "/T_" + terrainName + "_BC.png";
            //System.IO.File.WriteAllBytes(Application.dataPath + name, bytesAtlas);

            cam.transform.parent = null;
            DestroyImmediate(centerObject);
            DestroyImmediate(cameraGo);

            EditorUtility.SetDirty(this);
            AssetDatabase.Refresh();

            if (_previewScene != null)
            {
                EditorSceneManager.ClosePreviewScene(_previewScene);
                SceneView.lastActiveSceneView.camera.scene = _lastScene;
                RenderSettings.skybox = sky;

                RenderSettings.ambientIntensity = ambient;
                RenderSettings.ambientMode = ambientMode;
                RenderSettings.ambientLight = ambientColor;
            }

            terrainBase.drawInstanced = drawInstanced;


            return screenShot;
        }

        private Texture2D ExportTextureNormalmap(Terrain terrainBase, string terrainName)
        {
            _previewScene = EditorSceneManager.NewPreviewScene();
            Material sky = RenderSettings.skybox;
            float ambient = RenderSettings.ambientIntensity;
            AmbientMode ambientMode = RenderSettings.ambientMode;
            Color ambientColor = RenderSettings.ambientLight;
            if (SceneView.lastActiveSceneView.camera != null)
            {
                _lastScene = SceneView.lastActiveSceneView.camera.scene;
                SceneView.lastActiveSceneView.camera.scene = _previewScene;

                RenderSettings.skybox = null;
                RenderSettings.ambientIntensity = 0;
                RenderSettings.ambientMode = AmbientMode.Flat;
                RenderSettings.ambientLight = ManagerSettings.ambientLightColor;
            }


            Terrain terrain = Instantiate(terrainBase);
            terrain.drawTreesAndFoliage = false;
            bool drawInstanced = terrain.drawInstanced;
            terrain.drawInstanced = false;

#if NM_URP
            terrain.materialTemplate = new Material(Shader.Find("NatureManufacture Shaders/Terrain Lit Normal"));

            terrain.materialTemplate.SetFloat("_NMNormal", 0);
#else
            terrain.materialTemplate = new Material(Shader.Find("NatureManufacture Shaders/Terrain/Standard"));
#endif

            // Debug.Log(terrain.drawInstanced);


            GameObject go = terrain.gameObject;
            EditorSceneManager.MoveGameObjectToScene(go, _previewScene);
            go.transform.position = Vector3.zero;

            GameObject cameraGo = new GameObject("PreviewCamera");
            EditorSceneManager.MoveGameObjectToScene(cameraGo, _previewScene);


            Camera cam = cameraGo.AddComponent<Camera>();


            cam.rect = new Rect(0, 0, 1, 1);
            cam.orthographic = true;
            cam.depthTextureMode = DepthTextureMode.Depth;


            cam.rect = new Rect(0, 0, 1, 1);

            Bounds currentBounds = terrain.terrainData.bounds;

            cam.transform.eulerAngles = new Vector3(0, 0, 0);


            cam.transform.position = currentBounds.center - cam.transform.forward * currentBounds.max.y;

            GameObject centerObject = new GameObject("Center");
            centerObject.transform.position = currentBounds.center;
            cam.transform.parent = centerObject.transform;
            centerObject.transform.eulerAngles = new Vector3(90, 0, 0);

            cam.nearClipPlane = 0.5f;
            cam.farClipPlane = cam.transform.position.y + 10.0f;


            float aspectSize = terrain.terrainData.size.x / terrain.terrainData.size.z;
            cam.aspect = aspectSize;
            if (aspectSize < 1)
                aspectSize = 1;
            cam.orthographicSize = Mathf.Max((currentBounds.max.x - currentBounds.min.x) / 2.0f,
                (currentBounds.max.z - currentBounds.min.z) / 2.0f) / aspectSize;

            cam.scene = _previewScene;

            RenderTexture rt = new RenderTexture(ManagerSettings.basemapResolution,
                ManagerSettings.basemapResolution, 32);
            cam.targetTexture = rt;
            Texture2D screenShot = new Texture2D(ManagerSettings.basemapResolution,
                ManagerSettings.basemapResolution, TextureFormat.ARGB32, false);

            cam.Render();

            RenderTexture.active = rt;
            screenShot.ReadPixels(
                new Rect(0, 0, ManagerSettings.basemapResolution, ManagerSettings.basemapResolution), 0,
                0);

            cam.targetTexture = null;
            RenderTexture.active = null;
            DestroyImmediate(rt);


            ////
            //byte[] bytesAtlas = screenShot.EncodeToPNG();

            //string name = terrainManagerSettings.terrainPath + "/T_" + terrainName + "_BCRob.png";
            //System.IO.File.WriteAllBytes(Application.dataPath + name, bytesAtlas);
            ////

            cam.transform.parent = null;
            DestroyImmediate(centerObject);
            DestroyImmediate(cameraGo);

            EditorUtility.SetDirty(this);
            AssetDatabase.Refresh();

            if (_previewScene != null)
            {
                EditorSceneManager.ClosePreviewScene(_previewScene);
                SceneView.lastActiveSceneView.camera.scene = _lastScene;
                RenderSettings.skybox = sky;

                RenderSettings.ambientIntensity = ambient;
                RenderSettings.ambientMode = ambientMode;
                RenderSettings.ambientLight = ambientColor;
            }

            terrainBase.drawInstanced = drawInstanced;

            return screenShot;
        }

        private Texture2D ExportTextureSmoothness(Terrain terrainBase, string terrainName, int type = 1)
        {
            _previewScene = EditorSceneManager.NewPreviewScene();
            Material sky = RenderSettings.skybox;
            float ambient = RenderSettings.ambientIntensity;
            AmbientMode ambientMode = RenderSettings.ambientMode;
            Color ambientColor = RenderSettings.ambientLight;
            if (SceneView.lastActiveSceneView.camera != null)

            {
                _lastScene = SceneView.lastActiveSceneView.camera.scene;
                SceneView.lastActiveSceneView.camera.scene = _previewScene;

                RenderSettings.skybox = null;
                RenderSettings.ambientIntensity = 0;
                RenderSettings.ambientMode = AmbientMode.Flat;
                RenderSettings.ambientLight = ManagerSettings.ambientLightColor;
            }


            Terrain terrain = Instantiate(terrainBase);
            terrain.drawTreesAndFoliage = false;
            bool drawInstanced = terrain.drawInstanced;
            terrain.drawInstanced = false;


#if NM_URP
            terrain.materialTemplate = new Material(Shader.Find("NatureManufacture Shaders/Terrain Lit Normal"));

            terrain.materialTemplate.SetFloat("_NMNormal", type);
#else
            terrain.materialTemplate =
                new Material(Shader.Find("NatureManufacture Shaders/Terrain/StandardSmoothness"));
#endif


            GameObject go = terrain.gameObject;
            EditorSceneManager.MoveGameObjectToScene(go, _previewScene);
            go.transform.position = Vector3.zero;

            GameObject cameraGo = new GameObject("PreviewCamera");
            EditorSceneManager.MoveGameObjectToScene(cameraGo, _previewScene);


            Camera cam = cameraGo.AddComponent<Camera>();


            cam.rect = new Rect(0, 0, 1, 1);
            cam.orthographic = true;
            cam.depthTextureMode = DepthTextureMode.Depth;


            cam.rect = new Rect(0, 0, 1, 1);

            Bounds currentBounds = terrain.terrainData.bounds;

            cam.transform.eulerAngles = new Vector3(0, 0, 0);


            cam.transform.position = currentBounds.center - cam.transform.forward * currentBounds.max.y;

            GameObject centerObject = new GameObject("Center");
            centerObject.transform.position = currentBounds.center;
            cam.transform.parent = centerObject.transform;
            centerObject.transform.eulerAngles = new Vector3(90, 0, 0);

            cam.nearClipPlane = 0.5f;
            cam.farClipPlane = cam.transform.position.y + 10.0f;

            float aspectSize = terrain.terrainData.size.x / terrain.terrainData.size.z;
            cam.aspect = aspectSize;
            if (aspectSize < 1)
                aspectSize = 1;
            cam.orthographicSize = Mathf.Max((currentBounds.max.x - currentBounds.min.x) / 2.0f,
                (currentBounds.max.z - currentBounds.min.z) / 2.0f) / aspectSize;

            cam.scene = _previewScene;

            RenderTexture rt = new RenderTexture(ManagerSettings.basemapResolution,
                ManagerSettings.basemapResolution, 32);
            cam.targetTexture = rt;
            Texture2D screenShot = new Texture2D(ManagerSettings.basemapResolution,
                ManagerSettings.basemapResolution, TextureFormat.ARGB32, false);

            cam.Render();

            RenderTexture.active = rt;
            screenShot.ReadPixels(
                new Rect(0, 0, ManagerSettings.basemapResolution, ManagerSettings.basemapResolution), 0,
                0);

            cam.targetTexture = null;
            RenderTexture.active = null;
            DestroyImmediate(rt);


            //byte[] bytesAtlas = screenShot.EncodeToPNG();

            //string name = terrainManagerSettings.terrainPath + "/T_" + terrainName + "_BC.png";
            //System.IO.File.WriteAllBytes(Application.dataPath + name, bytesAtlas);

            cam.transform.parent = null;
            DestroyImmediate(centerObject);
            DestroyImmediate(cameraGo);

            EditorUtility.SetDirty(this);
            AssetDatabase.Refresh();

            if (_previewScene != null)
            {
                EditorSceneManager.ClosePreviewScene(_previewScene);
                SceneView.lastActiveSceneView.camera.scene = _lastScene;
                RenderSettings.skybox = sky;

                RenderSettings.ambientIntensity = ambient;
                RenderSettings.ambientMode = ambientMode;
                RenderSettings.ambientLight = ambientColor;
            }

            terrainBase.drawInstanced = drawInstanced;

            return screenShot;
        }

        private Texture2D ExportTextureSmoothnessHdrp(Terrain terrainBase, string terrainName, int type)
        {
            _previewScene = EditorSceneManager.NewPreviewScene();
            Material sky = RenderSettings.skybox;
            float ambient = RenderSettings.ambientIntensity;
            AmbientMode ambientMode = RenderSettings.ambientMode;
            Color ambientColor = RenderSettings.ambientLight;
            if (SceneView.lastActiveSceneView.camera != null)

            {
                _lastScene = SceneView.lastActiveSceneView.camera.scene;
                SceneView.lastActiveSceneView.camera.scene = _previewScene;

                RenderSettings.skybox = null;
                RenderSettings.ambientIntensity = 0;
                RenderSettings.ambientMode = AmbientMode.Flat;
                RenderSettings.ambientLight = ManagerSettings.ambientLightColor;
            }


            GameObject light = new GameObject("light");
            EditorSceneManager.MoveGameObjectToScene(light, _previewScene);
            light.transform.eulerAngles = new Vector3(90, 0, 0);

#if NM_HDRP
            var hdLight = light.AddHDLight(HDLightTypeAndShape.Directional);
            hdLight.intensity = 3.141593f;
#endif

            Terrain terrain = Instantiate(terrainBase);
            terrain.drawTreesAndFoliage = false;
            bool drawInstanced = terrain.drawInstanced;
            terrain.drawInstanced = false;

            bool blend = terrain.materialTemplate.HasProperty("_EnableHeightBlend") &&
                         terrain.materialTemplate.GetFloat("_EnableHeightBlend") > 0;
            float heightTransition = 0;
            if (blend)
            {
                heightTransition = terrain.materialTemplate.GetFloat("_HeightTransition");
            }


            terrain.materialTemplate = new Material(Shader.Find("HDRP/TerrainLitNM Normal"));
            terrain.materialTemplate.SetFloat("_metallicMap", type);

#if NM_HDRP || NM_URP
            if (blend)
            {
                CoreUtils.SetKeyword(terrain.materialTemplate, "_TERRAIN_BLEND_HEIGHT", blend);
                terrain.materialTemplate.SetFloat("_HeightTransition", heightTransition);
            }
#endif

            GameObject go = terrain.gameObject;
            EditorSceneManager.MoveGameObjectToScene(go, _previewScene);
            go.transform.position = Vector3.zero;

            GameObject cameraGo = new GameObject("PreviewCamera");
            EditorSceneManager.MoveGameObjectToScene(cameraGo, _previewScene);


            Camera cam = cameraGo.AddComponent<Camera>();
#if NM_HDRP
            HDAdditionalCameraData cameraData = cameraGo.AddComponent<HDAdditionalCameraData>();
            cameraData.clearColorMode = HDAdditionalCameraData.ClearColorMode.None;
            FrameSettings frameSettings = cameraData.renderingPathCustomFrameSettings;

            FrameSettingsOverrideMask frameSettingsOverrideMask =
 cameraData.renderingPathCustomFrameSettingsOverrideMask;
            cameraData.customRenderingSettings = true;

            frameSettingsOverrideMask.mask[(uint)FrameSettingsField.Postprocess] = true;
            frameSettings.SetEnabled(FrameSettingsField.Postprocess, false);


            frameSettingsOverrideMask.mask[(uint)FrameSettingsField.ExposureControl] = true;
            frameSettings.SetEnabled(FrameSettingsField.ExposureControl, false);


            frameSettingsOverrideMask.mask[(uint)FrameSettingsField.DirectSpecularLighting] = true;
            frameSettings.SetEnabled(FrameSettingsField.DirectSpecularLighting, false);

            cameraData.renderingPathCustomFrameSettings = frameSettings;
            cameraData.renderingPathCustomFrameSettingsOverrideMask = frameSettingsOverrideMask;
#endif


            cam.rect = new Rect(0, 0, 1, 1);
            cam.orthographic = true;
            cam.depthTextureMode = DepthTextureMode.Depth;


            cam.rect = new Rect(0, 0, 1, 1);

            Bounds currentBounds = terrain.terrainData.bounds;

            cam.transform.eulerAngles = new Vector3(0, 0, 0);


            cam.transform.position = currentBounds.center - cam.transform.forward * currentBounds.max.y;

            GameObject centerObject = new GameObject("Center");
            centerObject.transform.position = currentBounds.center;
            cam.transform.parent = centerObject.transform;
            centerObject.transform.eulerAngles = new Vector3(90, 0, 0);

            cam.nearClipPlane = 0.5f;
            cam.farClipPlane = cam.transform.position.y + 10.0f;


            float aspectSize = terrain.terrainData.size.x / terrain.terrainData.size.z;
            cam.aspect = aspectSize;
            if (aspectSize < 1)
                aspectSize = 1;
            cam.orthographicSize = Mathf.Max((currentBounds.max.x - currentBounds.min.x) / 2.0f,
                (currentBounds.max.z - currentBounds.min.z) / 2.0f) / aspectSize;

            cam.scene = _previewScene;

            RenderTexture rt = new RenderTexture(ManagerSettings.basemapResolution,
                ManagerSettings.basemapResolution, 32);
            cam.targetTexture = rt;
            Texture2D screenShot = new Texture2D(ManagerSettings.basemapResolution,
                ManagerSettings.basemapResolution, TextureFormat.ARGB32, false);

            cam.Render();

            RenderTexture.active = rt;
            screenShot.ReadPixels(
                new Rect(0, 0, ManagerSettings.basemapResolution, ManagerSettings.basemapResolution), 0,
                0);

            cam.targetTexture = null;
            RenderTexture.active = null;
            DestroyImmediate(rt);


            cam.transform.parent = null;
            DestroyImmediate(centerObject);
            DestroyImmediate(cameraGo);

            EditorUtility.SetDirty(this);
            AssetDatabase.Refresh();

            if (_previewScene != null)
            {
                EditorSceneManager.ClosePreviewScene(_previewScene);
                SceneView.lastActiveSceneView.camera.scene = _lastScene;
                RenderSettings.skybox = sky;

                RenderSettings.ambientIntensity = ambient;
                RenderSettings.ambientMode = ambientMode;
                RenderSettings.ambientLight = ambientColor;
            }

            terrainBase.drawInstanced = drawInstanced;

            return screenShot;
        }

        private void TerrainToMeshRim(bool allTerrains = false)
        {
            _meshFiltersToFix.Clear();
            if (!Directory.Exists("Assets/" + ManagerSettings.terrainPath))
            {
                Directory.CreateDirectory("Assets/" + ManagerSettings.terrainPath);
            }

            Terrain[] terrains = Selection.GetFiltered<Terrain>(SelectionMode.TopLevel);

            if (allTerrains)
                terrains = Terrain.activeTerrains;

            int idTerrain = 0;
            int countTerrain = terrains.Length;
            foreach (var terrain in terrains)
            {
                EditorUtility.DisplayProgressBar("Terrain mesh generation",
                    "Exporting terrain " + idTerrain + "/" + countTerrain + "\n Exporting mesh",
                    idTerrain / (float) countTerrain);
                TerrainData terrainData = terrain.terrainData;
                float[,] heightmapData = terrainData.GetHeights(0, 0, terrainData.heightmapResolution,
                    terrainData.heightmapResolution);

                float sizeX = terrainData.size.x;
                float sizeY = terrainData.size.y;
                float sizeZ = terrainData.size.z;
                float terrainTowidth = (1 / sizeX * (terrainData.heightmapResolution - 1));
                float terrainToheight = (1 / sizeZ * (terrainData.heightmapResolution - 1));

                Vector3 position = Vector3.zero;

                int lod = 1;

                Vector4[,] positionArray = new Vector4[heightmapData.GetLength(0) + 2, heightmapData.GetLength(1) + 2];
                int addxz = 1;


                for (int x = 0; x < heightmapData.GetLength(0); x += lod)
                {
                    //List<Vector3> positionArrayRow = new List<Vector3>();
                    for (int z = 0; z < heightmapData.GetLength(1); z += lod)
                    {
                        position.x = z / (float) terrainToheight; //, polygonHeight
                        position.y = heightmapData[x, z] * sizeY;
                        position.z = x / (float) terrainTowidth;


                        positionArray[x / lod + addxz, z / lod + addxz] =
                            new Vector4(position.x, position.y, position.z);


                        if (x == 0)
                            positionArray[0, z / lod + addxz] = new Vector4(position.x,
                                position.y - ManagerSettings.verticesDownDistance, position.z);

                        if (x == heightmapData.GetLength(0) - 1)
                            positionArray[x / lod + 2, z / lod + addxz] = new Vector4(position.x,
                                position.y - ManagerSettings.verticesDownDistance, position.z);

                        if (z == 0)
                            positionArray[x / lod + 1, 0] = new Vector4(position.x,
                                position.y - ManagerSettings.verticesDownDistance, position.z);

                        if (z == heightmapData.GetLength(1) - 1)
                            positionArray[x / lod + addxz, z / lod + 2] = new Vector4(position.x,
                                position.y - ManagerSettings.verticesDownDistance, position.z);

                        if (x == 0 && z == 0)
                            positionArray[0, 0] = new Vector4(position.x,
                                position.y - ManagerSettings.verticesDownDistance, position.z);

                        if (x == 0 && z == heightmapData.GetLength(1) - 1)
                            positionArray[0, z / lod + 2] = new Vector4(position.x,
                                position.y - ManagerSettings.verticesDownDistance, position.z);

                        if (x == heightmapData.GetLength(0) - 1 && z == 0)
                            positionArray[x / lod + 2, 0] = new Vector4(position.x,
                                position.y - ManagerSettings.verticesDownDistance, position.z);

                        if (x == heightmapData.GetLength(0) - 1 && z == heightmapData.GetLength(1) - 1)
                            positionArray[x / lod + 2, z / lod + 2] = new Vector4(position.x,
                                position.y - ManagerSettings.verticesDownDistance, position.z);
                    }
                    //positionArray.Add(positionArrayRow);
                }


                Mesh meshTerrain = new Mesh();
                meshTerrain.indexFormat = IndexFormat.UInt32;
                List<Vector3> vertices = new List<Vector3>();
                List<Vector3> normals = new List<Vector3>();
                List<Vector2> uvs = new List<Vector2>();
                List<int> triangles = new List<int>();

                Vector3 normal;
                Vector3 vert;


                int id = 0;
                Vector2 uv;
                for (int x = 0; x < positionArray.GetLength(0); x++)
                {
                    for (int z = 0; z < positionArray.GetLength(1); z++)
                    {
                        if (x > 1 && x < positionArray.GetLength(0) - 2 && z > 1 && z < positionArray.GetLength(1) - 2)
                            continue;


                        vert = positionArray[x, z];
                        positionArray[x, z].w = id;
                        id++;
                        vertices.Add(vert);

                        uv = new Vector2(vert.x / (float) sizeX, vert.z / (float) sizeZ);
                        if (uv.x > 0.99)
                            uv.x = 1;
                        if (uv.y > 0.99)
                            uv.y = 1;
                        if (uv.x < 0.01)
                            uv.x = 0;
                        if (uv.y < 0.01)
                            uv.y = 0;
                        uvs.Add(uv);


                        normal = terrainData.GetInterpolatedNormal(vert.x / (float) sizeX, vert.z / (float) sizeZ);

                        if (x == 0)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x + 1, z].x / (float) sizeX,
                                positionArray[x + 1, z].z / (float) sizeZ);

                        if (x == positionArray.GetLength(0) - 1)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x - 1, z].x / (float) sizeX,
                                positionArray[x - 1, z].z / (float) sizeZ);

                        if (z == 0)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x, z + 1].x / (float) sizeX,
                                positionArray[x, z + 1].z / (float) sizeZ);

                        if (z == positionArray.GetLength(1) - 1)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x, z - 1].x / (float) sizeX,
                                positionArray[x, z - 1].z / (float) sizeZ);

                        if (x == 0 && z == 0)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x + 1, z + 1].x / (float) sizeX,
                                positionArray[x + 1, z + 1].z / (float) sizeZ);

                        if (x == 0 && z == positionArray.GetLength(1) - 1)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x + 1, z - 1].x / (float) sizeX,
                                positionArray[x, z - 1].z / (float) sizeZ);

                        if (x == positionArray.GetLength(0) - 1 && z == 0)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x - 1, z + 1].x / (float) sizeX,
                                positionArray[x - 1, z + 1].z / (float) sizeZ);

                        if (x == positionArray.GetLength(0) - 1 && z == heightmapData.GetLength(1) - 1)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x - 1, z - 1].x / (float) sizeX,
                                positionArray[x - 1, z - 1].z / (float) sizeZ);


                        if (x == 1)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x + 2, z].x / (float) sizeX,
                                positionArray[x + 2, z].z / (float) sizeZ);

                        if (x == positionArray.GetLength(0) - 2)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x - 2, z].x / (float) sizeX,
                                positionArray[x - 2, z].z / (float) sizeZ);

                        if (z == 1)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x, z + 2].x / (float) sizeX,
                                positionArray[x, z + 2].z / (float) sizeZ);

                        if (z == positionArray.GetLength(1) - 2)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x, z - 2].x / (float) sizeX,
                                positionArray[x, z - 2].z / (float) sizeZ);

                        if (x == 1 && z == 1)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x + 2, z + 2].x / (float) sizeX,
                                positionArray[x + 2, z + 2].z / (float) sizeZ);

                        if (x == 1 && z == positionArray.GetLength(1) - 2)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x + 2, z - 2].x / (float) sizeX,
                                positionArray[x, z - 2].z / (float) sizeZ);

                        if (x == positionArray.GetLength(0) - 2 && z == 1)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x - 2, z + 2].x / (float) sizeX,
                                positionArray[x - 2, z + 2].z / (float) sizeZ);

                        if (x == positionArray.GetLength(0) - 2 && z == heightmapData.GetLength(1) - 2)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x - 2, z - 2].x / (float) sizeX,
                                positionArray[x - 2, z - 2].z / (float) sizeZ);


                        normals.Add(normal);
                    }
                }


                int rowPositionCount = positionArray.GetLength(1);

                Debug.Log(positionArray.GetLength(0) + " " + positionArray.GetLength(1));

                for (int i = 0; i < positionArray.GetLength(1) - 1; i++)
                {
                    for (int j = 0; j < positionArray.GetLength(0) - 1; j++)
                    {
                        if (j > 0 && j < positionArray.GetLength(0) - 2 && i > 0 && i < positionArray.GetLength(1) - 2)
                            continue;


                        triangles.Add((int) positionArray[j, i].w);
                        triangles.Add((int) positionArray[(j + 1), i].w);
                        triangles.Add((int) positionArray[j, (i + 1)].w);

                        triangles.Add((int) positionArray[(j + 1), i].w);
                        triangles.Add((int) positionArray[(j + 1), (i + 1)].w);
                        triangles.Add((int) positionArray[j, (i + 1)].w);
                    }
                }


                //if (j > 0 && j < positionArray.GetLength(1) - 2 && i > 0 && i < positionArray.GetLength(0) - 2)
                //    continue;

                meshTerrain.SetVertices(vertices);
                meshTerrain.SetTriangles(triangles, 0);
                meshTerrain.SetUVs(0, uvs);
                meshTerrain.SetNormals(normals);
                //meshTerrain.RecalculateNormals();
                meshTerrain.RecalculateTangents();
                meshTerrain.RecalculateBounds();

                EditorUtility.DisplayProgressBar("Terrain mesh generation",
                    $"Exporting terrain {idTerrain}/{countTerrain}\n Exporting textures",
                    (idTerrain + 0.5f) / (float) countTerrain);
                string terrainName = ManagerSettings.terrainPrefixName + terrain.gameObject.name;
                GameObject meshGo = new GameObject(terrainName);

                MeshFilter meshfilter = meshGo.AddComponent<MeshFilter>();
                MeshRenderer meshRenderer = meshGo.AddComponent<MeshRenderer>();
                Material terrainMaterial = new Material(Shader.Find("Standard"));
                
                

                if (RenderPipelineManager.currentPipeline == null)
                {
                    terrainMaterial = new Material(Shader.Find("Standard"));
                }
                else
                {
                    var srpType = GraphicsSettings.defaultRenderPipeline.GetType().ToString();

                    if (srpType.Contains("HDRenderPipelineAsset"))
                    {
                        terrainMaterial = new Material(Shader.Find("HDRP/TerrainLit"));
                    }
                    else if (srpType.Contains("UniversalRenderPipelineAsset") ||
                             srpType.Contains("LightweightRenderPipelineAsset"))
                    {
                        terrainMaterial = new Material(Shader.Find("LWRP/TerrainLit"));
                    }
                }


                MaterialPropertyBlock block = new MaterialPropertyBlock();

                terrain.GetSplatMaterialPropertyBlock(block);


                Texture mask;

                Texture basemap = ExportBaseMap(terrain, terrainName, out mask);


                terrainMaterial.SetTexture("_MainTex", basemap);

                Texture texture = ExportNormalMapHeightMap(terrain, terrainName);

                terrainMaterial.SetTexture("_BumpMap", texture);

                terrainMaterial.SetInt("_SmoothnessTextureChannel", 1);
                terrainMaterial.SetFloat("_Glossiness", .2f);
                terrainMaterial.SetFloat("_GlossMapScale", 0.5f);
                terrainMaterial.SetFloat("_Metallic", .0750f);

                terrainMaterial.EnableKeyword("_NORMALMAP");


#if NM_URP
                terrainMaterial.SetFloat("_Smoothness", 1f);
                UnityEditor.Rendering.Universal.ShaderGUI.LitGUI.SetMaterialKeywords(terrainMaterial);
                //CoreUtils.SetKeyword(terrainMaterial, "_SPECULAR_SETUP", false);

                //CoreUtils.SetKeyword(terrainMaterial, "_METALLICSPECGLOSSMAP", true);
#endif


                string name = ManagerSettings.terrainPath + "/M_" + terrainName + ".mat";
                string path = "Assets/" + name;

                AssetDatabase.CreateAsset(terrainMaterial, path);
                terrainMaterial = AssetDatabase.LoadAssetAtPath<Material>(path);

                meshRenderer.sharedMaterial = terrainMaterial;

                meshGo.transform.position = terrain.transform.position;
                meshfilter.sharedMesh = meshTerrain;


                _meshFiltersToFix.Add(meshfilter);

                AssetDatabase.Refresh();

                idTerrain++;
            }

            EditorUtility.ClearProgressBar();

            Debug.Log("Clear fix normals");
            FixMeshNormals(_meshFiltersToFix);


            string pathMesh;
            foreach (var item in _meshFiltersToFix)
            {
                name = ManagerSettings.terrainPath + item.name + ".asset";
                pathMesh = "Assets/" + name;
                AssetDatabase.CreateAsset(item.sharedMesh, pathMesh);
            }

            AssetDatabase.SaveAssets();
            System.GC.Collect();
        }


        private void TerrainToMesh(bool exportTrees = false, bool allTerrains = false)
        {
            _meshFiltersToFix.Clear();
            if (!Directory.Exists("Assets/" + ManagerSettings.terrainPath))
            {
                Directory.CreateDirectory("Assets/" + ManagerSettings.terrainPath);
            }

            Terrain[] terrains = Selection.GetFiltered<Terrain>(SelectionMode.TopLevel);

            if (allTerrains)
                terrains = Terrain.activeTerrains;

            int idTerrain = 0;
            int countTerrain = terrains.Length;
            foreach (var terrain in terrains)
            {
                EditorUtility.DisplayProgressBar("Terrain mesh generation",
                    "Exporting terrain " + idTerrain + "/" + countTerrain + "\n Exporting mesh",
                    idTerrain / (float) countTerrain);
                TerrainData terrainData = terrain.terrainData;
                float[,] heightmapData = terrainData.GetHeights(0, 0, terrainData.heightmapResolution,
                    terrainData.heightmapResolution);

                float sizeX = terrainData.size.z;
                float sizeY = terrainData.size.y;
                float sizeZ = terrainData.size.x;
                float terrainTowidth = (1 / sizeX * (terrainData.heightmapResolution - 1));
                float terrainToheight = (1 / sizeZ * (terrainData.heightmapResolution - 1));

                Vector3 position = Vector3.zero;

                int lod = (int) Mathf.Pow(2, ManagerSettings.terrainLod);

                Vector3[,] positionArray =
                    new Vector3[heightmapData.GetLength(0) / lod + (lod == 1 ? -1 : 0) + 1 +
                                (ManagerSettings.addVerticesDown ? 2 : 0), heightmapData.GetLength(1) / lod +
                                                                           (lod == 1 ? -1 : 0) + 1 + (ManagerSettings.addVerticesDown ? 2 : 0)];
                int addxz = +(ManagerSettings.addVerticesDown ? 1 : 0);


                for (int x = 0; x < heightmapData.GetLength(0); x += lod)
                {
                    //List<Vector3> positionArrayRow = new List<Vector3>();
                    for (int z = 0; z < heightmapData.GetLength(1); z += lod)
                    {
                        position.x = z / (float) terrainToheight; //, polygonHeight
                        position.y = heightmapData[x, z] * sizeY;
                        position.z = x / (float) terrainTowidth;


                        positionArray[x / lod + addxz, z / lod + addxz] =
                            new Vector4(position.x, position.y, position.z);


                        if (ManagerSettings.addVerticesDown)
                        {
                            if (x == 0)
                                positionArray[0, z / lod + addxz] = new Vector4(position.x,
                                    position.y - ManagerSettings.verticesDownDistance, position.z);

                            if (x == heightmapData.GetLength(0) - 1)
                                positionArray[x / lod + 2, z / lod + addxz] = new Vector4(position.x,
                                    position.y - ManagerSettings.verticesDownDistance, position.z);

                            if (z == 0)
                                positionArray[x / lod + 1, 0] = new Vector4(position.x,
                                    position.y - ManagerSettings.verticesDownDistance, position.z);

                            if (z == heightmapData.GetLength(1) - 1)
                                positionArray[x / lod + addxz, z / lod + 2] = new Vector4(position.x,
                                    position.y - ManagerSettings.verticesDownDistance, position.z);

                            if (x == 0 && z == 0)
                                positionArray[0, 0] = new Vector4(position.x,
                                    position.y - ManagerSettings.verticesDownDistance, position.z);

                            if (x == 0 && z == heightmapData.GetLength(1) - 1)
                                positionArray[0, z / lod + 2] = new Vector4(position.x,
                                    position.y - ManagerSettings.verticesDownDistance, position.z);

                            if (x == heightmapData.GetLength(0) - 1 && z == 0)
                                positionArray[x / lod + 2, 0] = new Vector4(position.x,
                                    position.y - ManagerSettings.verticesDownDistance, position.z);

                            if (x == heightmapData.GetLength(0) - 1 && z == heightmapData.GetLength(1) - 1)
                                positionArray[x / lod + 2, z / lod + 2] = new Vector4(position.x,
                                    position.y - ManagerSettings.verticesDownDistance, position.z);
                        }
                    }
                    //positionArray.Add(positionArrayRow);
                }


                Mesh meshTerrain = new Mesh();
                meshTerrain.indexFormat = IndexFormat.UInt32;
                List<Vector3> vertices = new List<Vector3>();
                List<Vector3> normals = new List<Vector3>();
                List<Vector2> uvs = new List<Vector2>();
                List<int> triangles = new List<int>();

                Vector3 normal;
                Vector3 vert;

                //Debug.Log(sizeX);
                Vector2 uv;
                for (int x = 0; x < positionArray.GetLength(0); x++)
                {
                    for (int z = 0; z < positionArray.GetLength(1); z++)
                    {
                        vert = positionArray[x, z];
                        vertices.Add(vert);
                        uv = new Vector2(vert.x / (float) sizeZ, vert.z / (float) sizeX);
                        //if (uv.x > 0.99)
                        //    uv.x = 1;
                        //if (uv.y > 0.99)
                        //    uv.y = 1;
                        //if (uv.x < 0.01)
                        //    uv.x = 0;
                        //if (uv.y < 0.01)
                        //    uv.y = 0;
                        uvs.Add(uv);


                        normal = terrainData.GetInterpolatedNormal(vert.x / (float) sizeX, vert.z / (float) sizeZ);

                        if (x == 0)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x + 1, z].x / (float) sizeX,
                                positionArray[x + 1, z].z / (float) sizeZ);

                        if (x == positionArray.GetLength(0) - 1)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x - 1, z].x / (float) sizeX,
                                positionArray[x - 1, z].z / (float) sizeZ);

                        if (z == 0)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x, z + 1].x / (float) sizeX,
                                positionArray[x, z + 1].z / (float) sizeZ);

                        if (z == positionArray.GetLength(1) - 1)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x, z - 1].x / (float) sizeX,
                                positionArray[x, z - 1].z / (float) sizeZ);

                        if (x == 0 && z == 0)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x + 1, z + 1].x / (float) sizeX,
                                positionArray[x + 1, z + 1].z / (float) sizeZ);

                        if (x == 0 && z == positionArray.GetLength(1) - 1)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x + 1, z - 1].x / (float) sizeX,
                                positionArray[x, z - 1].z / (float) sizeZ);

                        if (x == positionArray.GetLength(0) - 1 && z == 0)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x - 1, z + 1].x / (float) sizeX,
                                positionArray[x - 1, z + 1].z / (float) sizeZ);

                        if (x == positionArray.GetLength(0) - 1 && z == heightmapData.GetLength(1) - 1)
                            normal = terrainData.GetInterpolatedNormal(positionArray[x - 1, z - 1].x / (float) sizeX,
                                positionArray[x - 1, z - 1].z / (float) sizeZ);


                        if (ManagerSettings.addVerticesDown)
                        {
                            if (x == 1)
                                normal = terrainData.GetInterpolatedNormal(positionArray[x + 2, z].x / (float) sizeX,
                                    positionArray[x + 2, z].z / (float) sizeZ);

                            if (x == positionArray.GetLength(0) - 2)
                                normal = terrainData.GetInterpolatedNormal(positionArray[x - 2, z].x / (float) sizeX,
                                    positionArray[x - 2, z].z / (float) sizeZ);

                            if (z == 1)
                                normal = terrainData.GetInterpolatedNormal(positionArray[x, z + 2].x / (float) sizeX,
                                    positionArray[x, z + 2].z / (float) sizeZ);

                            if (z == positionArray.GetLength(1) - 2)
                                normal = terrainData.GetInterpolatedNormal(positionArray[x, z - 2].x / (float) sizeX,
                                    positionArray[x, z - 2].z / (float) sizeZ);

                            if (x == 1 && z == 1)
                                normal = terrainData.GetInterpolatedNormal(
                                    positionArray[x + 2, z + 2].x / (float) sizeX,
                                    positionArray[x + 2, z + 2].z / (float) sizeZ);

                            if (x == 1 && z == positionArray.GetLength(1) - 2)
                                normal = terrainData.GetInterpolatedNormal(
                                    positionArray[x + 2, z - 2].x / (float) sizeX,
                                    positionArray[x, z - 2].z / (float) sizeZ);

                            if (x == positionArray.GetLength(0) - 2 && z == 1)
                                normal = terrainData.GetInterpolatedNormal(
                                    positionArray[x - 2, z + 2].x / (float) sizeX,
                                    positionArray[x - 2, z + 2].z / (float) sizeZ);

                            if (x == positionArray.GetLength(0) - 2 && z == heightmapData.GetLength(1) - 2)
                                normal = terrainData.GetInterpolatedNormal(
                                    positionArray[x - 2, z - 2].x / (float) sizeX,
                                    positionArray[x - 2, z - 2].z / (float) sizeZ);
                        }


                        normals.Add(normal);
                    }
                }

                int rowPositionCount = positionArray.GetLength(1);
                for (int i = 0; i < positionArray.GetLength(1) - 1; i++)
                {
                    for (int j = 0; j < positionArray.GetLength(0) - 1; j++)
                    {
                        triangles.Add(j + i * rowPositionCount);
                        triangles.Add(j + (i + 1) * rowPositionCount);
                        triangles.Add((j + 1) + i * rowPositionCount);

                        triangles.Add((j + 1) + i * rowPositionCount);
                        triangles.Add(j + (i + 1) * rowPositionCount);
                        triangles.Add((j + 1) + (i + 1) * rowPositionCount);
                    }
                }

                meshTerrain.SetVertices(vertices);
                meshTerrain.SetTriangles(triangles, 0);
                meshTerrain.SetUVs(0, uvs);
                meshTerrain.SetNormals(normals);
                //meshTerrain.RecalculateNormals();
                meshTerrain.RecalculateTangents();
                meshTerrain.RecalculateBounds();

                EditorUtility.DisplayProgressBar("Terrain mesh generation",
                    "Exporting terrain " + idTerrain + "/" + countTerrain + "\n Exporting textures",
                    (idTerrain + 0.5f) / (float) countTerrain);
                string terrainName = ManagerSettings.terrainPrefixName + terrain.gameObject.name;
                GameObject meshGo = new GameObject(terrainName);

                MeshFilter meshfilter = meshGo.AddComponent<MeshFilter>();
                MeshRenderer meshRenderer = meshGo.AddComponent<MeshRenderer>();

                Texture texture = null;
                if (ManagerSettings.useTerrainNormal || ManagerSettings.useTextureNormal)
                {
                    texture = ExportNormalMapHeightMap(terrain, terrainName);
                }

                Material terrainMaterial = new Material(Shader.Find("Standard"));

                if (RenderPipelineManager.currentPipeline == null)
                {
                    terrainMaterial = new Material(Shader.Find("Standard"));
                    Texture mask;
                    Texture basemap = ExportBaseMap(terrain, terrainName, out mask);
                    terrainMaterial.SetTexture("_MainTex", basemap);


                    if (ManagerSettings.useTerrainNormal || ManagerSettings.useTextureNormal)
                    {
                        terrainMaterial.SetTexture("_BumpMap", texture);
                    }

                    terrainMaterial.SetInt("_SmoothnessTextureChannel", 1);
                    terrainMaterial.SetFloat("_Glossiness", .2f);
                    terrainMaterial.SetFloat("_GlossMapScale", 0.5f);
                    terrainMaterial.SetFloat("_Metallic", .0750f);
                    terrainMaterial.SetFloat("_Smoothness", 1f);
                    terrainMaterial.EnableKeyword("_NORMALMAP");
                    terrainMaterial.EnableKeyword("_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A");
                }
                else
                {
                    var srpType = GraphicsSettings.defaultRenderPipeline.GetType().ToString();

                    if (srpType.Contains("HDRenderPipelineAsset"))
                    {
                        terrainMaterial = new Material(Shader.Find("HDRP/Lit"));
                        Texture mask;
                        Texture basemap = ExportBaseMapHdrp(terrain, terrainName, out mask);
                        terrainMaterial.SetTexture("_BaseColorMap", basemap);
                        terrainMaterial.SetTexture("_MaskMap", mask);
                        terrainMaterial.SetFloat("_Metallic", 1f);
                        terrainMaterial.SetFloat("_MetallicRemapMin", 0f);
                        terrainMaterial.SetFloat("_MetallicRemapMax", 1f);


                        if (ManagerSettings.useTerrainNormal || ManagerSettings.useTextureNormal)
                        {
                            terrainMaterial.SetTexture("_NormalMap", texture);
                        }

                        terrainMaterial.EnableKeyword("_NORMALMAP");
                        terrainMaterial.EnableKeyword("_MASKMAP");
                    }
                    else if (srpType.Contains("UniversalRenderPipelineAsset") ||
                             srpType.Contains("LightweightRenderPipelineAsset"))
                    {
                        terrainMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                        Texture mask;
                        Texture basemap = ExportBaseMap(terrain, terrainName, out mask);

                        Debug.Log(basemap);

                        terrainMaterial.SetTexture("_BaseMap", basemap);
                        terrainMaterial.SetTexture("_MainTex", basemap);

                        terrainMaterial.SetTexture("_MetallicGlossMap", mask);
                        terrainMaterial.SetTexture("_OcclusionMap", mask);

                        if (ManagerSettings.useTerrainNormal || ManagerSettings.useTextureNormal)
                        {
                            terrainMaterial.SetTexture("_BumpMap", texture);
                        }

                        if (ManagerSettings.useMaskSmoothnessURP)
                            terrainMaterial.SetInt("_SmoothnessTextureChannel", 0);
                        else
                            terrainMaterial.SetInt("_SmoothnessTextureChannel", 1);

                        terrainMaterial.SetFloat("_Glossiness", .2f);
                        terrainMaterial.SetFloat("_GlossMapScale", 0.5f);
                        terrainMaterial.SetFloat("_Metallic", .0750f);
                        terrainMaterial.EnableKeyword("_NORMALMAP");


#if NM_URP
                        terrainMaterial.SetFloat("_Smoothness", 1f);
                        UnityEditor.Rendering.Universal.ShaderGUI.LitGUI.SetMaterialKeywords(terrainMaterial);

                        //CoreUtils.SetKeyword(terrainMaterial, "_SPECULAR_SETUP", false);

                        //CoreUtils.SetKeyword(terrainMaterial, "_METALLICSPECGLOSSMAP", true);


#endif
                    }
                }

                MaterialPropertyBlock block = new MaterialPropertyBlock();

                terrain.GetSplatMaterialPropertyBlock(block);


                string name = ManagerSettings.terrainPath + "/M_" + terrainName + ".mat";
                string path = "Assets/" + name;

                AssetDatabase.CreateAsset(terrainMaterial, path);
                terrainMaterial = AssetDatabase.LoadAssetAtPath<Material>(path);

                meshRenderer.sharedMaterial = terrainMaterial;

                meshGo.transform.position =
                    terrain.transform.position + new Vector3(0, ManagerSettings.yOffset, 0);
                meshfilter.sharedMesh = meshTerrain;

                if (exportTrees)
                {
                    EditorUtility.DisplayProgressBar("Terrain mesh generation",
                        "Exporting terrain " + idTerrain + "/" + countTerrain + "\n Exporting trees",
                        (idTerrain + 0.75f) / (float) countTerrain);


                    List<GameObject> trees = ExportTrees(terrain);
                    foreach (var treeBatcher in trees)
                    {
                        treeBatcher.transform.parent = meshGo.transform;
                        treeBatcher.transform.localPosition = Vector3.zero;
                    }
                }

                _meshFiltersToFix.Add(meshfilter);

                AssetDatabase.Refresh();

                idTerrain++;
            }

            EditorUtility.ClearProgressBar();

            FixMeshNormals(_meshFiltersToFix);


            string pathMesh;
            foreach (var item in _meshFiltersToFix)
            {
                name = ManagerSettings.terrainPath + item.name + ".asset";
                pathMesh = "Assets/" + name;
                AssetDatabase.CreateAsset(item.sharedMesh, pathMesh);
            }

            AssetDatabase.SaveAssets();
            System.GC.Collect();
        }

        private void FixMeshNormals(List<MeshFilter> meshFiltersToFix)
        {
            MeshFilter meshFilter;
            Mesh mesh;
            Vector3[] vertices;
            Vector3[] normals;
            Bounds meshBounds;

            Matrix4x4 matrix4;

            MeshFilter meshFilterSecond;
            Mesh meshSecond;
            Vector3[] verticesSecond;
            Vector3[] normalsSecond;
            Bounds meshBoundsSecond;
            Matrix4x4 matrix4Second;
            Vector3 pos;
            Vector4 posRob;
            Vector3 pos2;
            double editorTime = EditorApplication.timeSinceStartup;

            try
            {
                bool cancel = false;
                AssetDatabase.StartAssetEditing();


                for (int i = 0; i < meshFiltersToFix.Count - 1; i++)
                {
                    meshFilter = meshFiltersToFix[i];

                    mesh = meshFilter.sharedMesh;
                    vertices = mesh.vertices;
                    normals = mesh.normals;
                    meshBounds = meshFilter.GetComponent<Renderer>().bounds;
                    matrix4 = meshFilter.transform.localToWorldMatrix;


                    cancel = EditorUtility.DisplayCancelableProgressBar("Terrain mesh generation",
                        "Normals calculating " + i + "/" + meshFiltersToFix.Count,
                        (i) / (float) meshFiltersToFix.Count);

                    for (int j = i + 1; j < meshFiltersToFix.Count; j++)
                    {
                        meshFilterSecond = meshFiltersToFix[j];

                        meshBoundsSecond = meshFilterSecond.GetComponent<Renderer>().bounds;
                        Vector3 baseSize = meshBoundsSecond.size;

                        meshBoundsSecond.size = baseSize * 0.99f;
                        bool outer = meshBounds.Intersects(meshBoundsSecond);
                        meshBoundsSecond.size = baseSize * 1.01f;
                        bool inner = meshBounds.Intersects(meshBoundsSecond);


                        if (!outer && inner)
                        {
                            meshSecond = meshFilterSecond.sharedMesh;
                            verticesSecond = meshSecond.vertices;
                            normalsSecond = meshSecond.normals;
                            matrix4Second = meshFilterSecond.transform.localToWorldMatrix;


                            for (int v1 = 0; v1 < vertices.Length; v1++)
                            {
                                posRob = vertices[v1];
                                posRob.w = 1;
                                pos = matrix4 * posRob;

                                if (meshBoundsSecond.SqrDistance(pos) < 1)
                                {
                                    for (int v2 = 0; v2 < verticesSecond.Length; v2++)
                                    {
                                        if (v2 % 1000 == 0)
                                            cancel = EditorUtility.DisplayCancelableProgressBar(
                                                "Terrain mesh generation",
                                                "Normals calculating " + i + "/" + meshFiltersToFix.Count,
                                                (i + v1 / (float) vertices.Length) / (float) meshFiltersToFix.Count);

                                        posRob = verticesSecond[v2];
                                        posRob.w = 1;
                                        pos2 = matrix4Second * posRob;
                                        if ((pos - pos2).sqrMagnitude < 0.01f)
                                        {
                                            normalsSecond[v2] = normals[v1];
                                        }

                                        if (cancel)
                                            break;
                                    }

                                    meshSecond.normals = normalsSecond;
                                }

                                if (cancel)
                                    break;
                            }
                        }

                        if (cancel)
                            break;
                    }

                    mesh.RecalculateTangents();

                    if (cancel)
                        break;
                }

                EditorUtility.ClearProgressBar();
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }
        }


        private Texture2D ExportNormalMapHeightMap(Terrain terrain, string terrainName)
        {
            int heightMapResolution = terrain.terrainData.heightmapResolution;
            float[,] rawHeights = terrain.terrainData.GetHeights(0, 0, heightMapResolution, heightMapResolution);

            int myIndex = 0;
            float min = float.MaxValue;
            float max = float.MinValue;
            float height = 0;

            for (int y = 0; y < heightMapResolution; y++)
            {
                for (int x = 0; x < heightMapResolution; x++)
                {
                    rawHeights[y, x] = Mathf.Clamp01(rawHeights[y, x]);
                    height = rawHeights[y, x];


                    if (height > max)
                        max = height;
                    if (height < min)
                        min = height;

                    myIndex++;
                }
            }

            if (max > MaxHeightTerrain)
                MaxHeightTerrain = max;
            if (min < MinHeightTerrain)
                MinHeightTerrain = min;

            for (int y = 0; y < heightMapResolution; y++)
            {
                for (int x = 0; x < heightMapResolution; x++)
                {
                    //Debug.Log($"{rawHeights[y, x]}  {minHeightTerrain} {maxHeightTerrain}");
                    rawHeights[y, x] = 1 - (rawHeights[y, x] - MinHeightTerrain) /
                        (float) (MaxHeightTerrain - MinHeightTerrain);
                }
            }


            string name = ManagerSettings.terrainPath + "/T_" + terrainName + "_N.png";
            string path = Application.dataPath + name;


            var extension = Path.GetExtension(path);


            Texture2D normalTerrain = WorldStreamer2.TerrainManagerUtils.GetNormalMap(rawHeights, 20 * ManagerSettings.terrainNormalStrength);


            Texture2D normalTextures = null;

            if (RenderPipelineManager.currentPipeline == null)
            {
                normalTextures = ExportTextureNormalmap(terrain, terrainName);
            }
            else
            {
                var srpType = GraphicsSettings.defaultRenderPipeline.GetType().ToString();

                if (srpType.Contains("HDRenderPipelineAsset"))
                {
                    normalTextures = ExportTextureNormalmapHdrp(terrain, terrainName);
                }
                else if (srpType.Contains("UniversalRenderPipelineAsset") ||
                         srpType.Contains("LightweightRenderPipelineAsset"))
                {
                    normalTextures = ExportTextureNormalmap(terrain, terrainName);
                }
            }


            Texture2D normalFinal = new Texture2D(normalTextures.width, normalTextures.height);

            float resize = normalTextures.width / (float) normalTerrain.width;


            Vector4 colorNormalTexture = Vector4.zero;

            Vector2 blueCount;

            for (int x = 0; x < normalTextures.width; x++)
            {
                for (int y = 0; y < normalTextures.height; y++)
                {
                    if (ManagerSettings.useTerrainNormal && ManagerSettings.useTextureNormal)
                    {
                        Vector4 colorNormal =
                            normalTerrain.GetPixel(Mathf.FloorToInt(x / resize), Mathf.FloorToInt(y / resize));

                        colorNormalTexture = normalTextures.GetPixel(x, y);


                        colorNormalTexture = WorldStreamer2.TerrainManagerUtils.LinearLightAddSub(colorNormalTexture, colorNormal);

                        //colorNormalTexture.z = colorNormal.z;

                        //colorNormalTexture.z = Mathf.Sqrt(1 - Mathf.Clamp01(colorNormalTexture.x * colorNormalTexture.x + colorNormalTexture.y * colorNormalTexture.y)) * 0.5f + 0.5f;
                        blueCount = new Vector2(colorNormalTexture.x, colorNormalTexture.y);
                        blueCount = blueCount * 2 - Vector2.one;
                        colorNormalTexture.z = Mathf.Clamp01(Vector2.Dot(blueCount, blueCount));
                        colorNormalTexture.z = Mathf.Sqrt(Mathf.Sqrt(1 - colorNormalTexture.z));

                        //colorNormalTexture.Normalize();
                        colorNormalTexture.w = 1;
                    }

                    if (!ManagerSettings.useTerrainNormal && ManagerSettings.useTextureNormal)
                    {
                        colorNormalTexture = normalTextures.GetPixel(x, y);

                        blueCount = new Vector2(colorNormalTexture.x, colorNormalTexture.y);
                        blueCount = blueCount * 2 - Vector2.one;
                        colorNormalTexture.z = Mathf.Clamp01(Vector2.Dot(blueCount, blueCount));
                        colorNormalTexture.z = Mathf.Sqrt(Mathf.Sqrt(1 - colorNormalTexture.z));

                        //colorNormalTexture = LinearLightAddSub(colorNormalTexture, colorNormalTexture);
                    }

                    if (ManagerSettings.useTerrainNormal && !ManagerSettings.useTextureNormal)
                    {
                        colorNormalTexture =
                            normalTerrain.GetPixel(Mathf.FloorToInt(x / resize), Mathf.FloorToInt(y / resize));

                        blueCount = new Vector2(colorNormalTexture.x, colorNormalTexture.y);
                        blueCount = blueCount * 2 - Vector2.one;
                        colorNormalTexture.z = Mathf.Clamp01(Vector2.Dot(blueCount, blueCount));
                        colorNormalTexture.z = Mathf.Sqrt(Mathf.Sqrt(1 - colorNormalTexture.z));
                    }

                    normalFinal.SetPixel(x, y, colorNormalTexture);
                }
            }


            byte[]
                pngData = normalFinal
                    .EncodeToPNG(); // GetNormalMap(rawHeights, 20 * terrainManagerSettings.terrainNormalStrength).EncodeToPNG();
            //byte[] pngData = normalTextures.EncodeToPNG();// GetNormalMap(rawHeights, 20 * terrainManagerSettings.terrainNormalStrength).EncodeToPNG();


            File.WriteAllBytes(path, pngData);
            //Debug.Log(path);

            AssetDatabase.Refresh();
            TextureImporter importer = (TextureImporter) AssetImporter.GetAtPath("Assets/" + name);
            importer.textureType = TextureImporterType.NormalMap;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.mipmapFilter = TextureImporterMipFilter.BoxFilter;
            importer.streamingMipmaps = true;
            importer.anisoLevel = 8;
            importer.SaveAndReimport();
            //importer.mipMapsPreserveCoverage = true;
            //importer.alphaTestReferenceValue = 0.85f;

            //importer.convertToNormalmap = true;
            //importer.normalmapFilter = TextureImporterNormalFilter.Sobel;
            //importer.heightmapScale = 0.2f;


            AssetDatabase.ImportAsset("Assets/" + name, ImportAssetOptions.ForceUpdate);

            AssetDatabase.Refresh();

            return (Texture2D) AssetDatabase.LoadAssetAtPath("assets" + name, typeof(Texture2D));
        }


        private void SetInstancing(bool instanced, bool allTerrains = false)
        {
            Terrain[] terrains = Selection.GetFiltered<Terrain>(SelectionMode.TopLevel);
            if (allTerrains)
                terrains = Terrain.activeTerrains;
            foreach (var terrain in terrains)
            {
                terrain.drawInstanced = instanced;
            }
        }


        public void ExportTreesForSelectedTerrain(bool allTerrains = false)
        {
            Terrain[] terrains = Selection.GetFiltered<Terrain>(SelectionMode.TopLevel);
            if (allTerrains)
                terrains = Terrain.activeTerrains;

            foreach (var terrain in terrains)
            {
                ExportTrees(terrain);
            }
        }


        public List<GameObject> ExportTrees(Terrain terrain)
        {
            if (!Directory.Exists("Assets/" + ManagerSettings.terrainPath))
            {
                Directory.CreateDirectory("Assets/" + ManagerSettings.terrainPath);
            }

            if (terrain == null)
                return null;

            List<GameObject> treePrototypes = new List<GameObject>();
            foreach (var item in ManagerSettings.terrainTrees)
            {
                if (item.active)
                    treePrototypes.Add(item.tree);
            }

            List<GameObject> treesBatchList = new List<GameObject>();


            bool backFace = Physics.queriesHitBackfaces;
            Physics.queriesHitBackfaces = true;

            TerrainData data = terrain.terrainData;
            float width = data.size.x;
            float height = data.size.z;
            float y = data.size.y;

            GameObject[] trees = new GameObject[data.treePrototypes.Length];
            string pathMesh;

            // Debug.Log("Exporting " + data.treeInstances.Length + " trees");
            List<TreeInstance> treeInstanceNew = new List<TreeInstance>();
            for (int tID = 0; tID < data.treePrototypes.Length; tID++)
            {
                if (!treePrototypes.Contains(data.treePrototypes[tID].prefab))
                    continue;

                //Debug.Log(tID);

                List<MeshFilter> meshFilters = new List<MeshFilter>();
                foreach (TreeInstance tree in data.treeInstances)
                {
                    if (tree.prototypeIndex == tID)
                    {
                        Vector3 position = new Vector3(tree.position.x * width, tree.position.y * y,
                            tree.position.z * height); // + _terrain.transform.position;
                        bool treeInRange = true;


                        if (treeInRange)
                        {
                            //Debug.Log(treeInRange);
                            if (trees[tree.prototypeIndex] == null)
                            {
                                GameObject treePrefab = data.treePrototypes[tree.prototypeIndex].prefab;
                                LODGroup lOdGroup = treePrefab.GetComponent<LODGroup>();
                                if (lOdGroup != null)
                                {
                                    LOD[] lods = lOdGroup.GetLODs();
                                    trees[tree.prototypeIndex] = lods[lods.Length - 1].renderers[0].gameObject;
                                }
                                else
                                {
                                    trees[tree.prototypeIndex] = treePrefab;
                                }
                            }

                            if (trees[tID].GetComponent<MeshRenderer>())
                            {
                                GameObject treeG = (GameObject) GameObject.Instantiate(trees[tree.prototypeIndex]);
                                //Debug.Log(treeG.name);
                                treeG.transform.position = position;
                                treeG.transform.rotation = Quaternion.Euler(0, tree.rotation, 0);
                                treeG.transform.localScale =
                                    new Vector3(tree.widthScale, tree.heightScale, tree.widthScale);
                                meshFilters.Add(treeG.GetComponent<MeshFilter>());
                            }
                        }
                    }
                }


                if (trees[tID] != null && trees[tID].GetComponent<MeshRenderer>() != null)
                {
                    //List<Matrix4x4> matrices = new List<Matrix4x4>();

                    Material treeMaterial = Instantiate(trees[tID].GetComponent<MeshRenderer>().sharedMaterial);

                    string shaderName = treeMaterial.shader.name;
                    if (shaderName.Contains("NatureManufacture") && shaderName.Contains("Cross"))
                    {
                        // Debug.Log(RenderPipelineManager.currentPipeline);
                        if (RenderPipelineManager.currentPipeline == null)
                        {
                            if (shaderName.Contains("Snow"))
                                treeMaterial.shader = Shader.Find("NatureManufacture Shaders/Trees/Cross Snow WS");
                            else
                                treeMaterial.shader = Shader.Find("NatureManufacture Shaders/Trees/Cross WS");
                        }
                        else
                        {
                            var srpType = GraphicsSettings.defaultRenderPipeline.GetType().ToString();
                            if (srpType.Contains("HDRenderPipelineAsset"))
                            {
                                if (shaderName.Contains("Snow"))
                                    treeMaterial.shader = Shader.Find("NatureManufacture/HDRP/Foliage/Cross Snow WS");
                                else
                                    treeMaterial.shader = Shader.Find("NatureManufacture/HDRP/Foliage/Cross WS");
                            }
                            else if (srpType.Contains("UniversalRenderPipelineAsset") ||
                                     srpType.Contains("LightweightRenderPipelineAsset"))
                            {
                                if (shaderName.Contains("Snow"))
                                    treeMaterial.shader = Shader.Find("NatureManufacture/URP/Foliage/Cross Snow WS");
                                else
                                    treeMaterial.shader = Shader.Find("NatureManufacture/URP/Foliage/Cross WS");
                            }
                        }
                    }

                    name = ManagerSettings.terrainPath + data.treePrototypes[tID].prefab.name + "_" +
                           treeMaterial.name + ".mat";
                    pathMesh = "Assets/" + name;

                    Material mat = AssetDatabase.LoadAssetAtPath<Material>(pathMesh);
                    if (mat == null)
                        AssetDatabase.CreateAsset(treeMaterial, pathMesh);


                    //Debug.Log(parent.name);

                    List<CombineInstance> combine = new List<CombineInstance>();
                    int id = 0;
                    int i = 0;
                    int meshVertsCount = 0;
                    while (i < meshFilters.Count)
                    {
                        CombineInstance combineInstance = new CombineInstance();
                        combineInstance.mesh = meshFilters[i].sharedMesh;
                        meshVertsCount += combineInstance.mesh.vertexCount;
                        combineInstance.transform = meshFilters[i].transform.localToWorldMatrix;
                        combine.Add(combineInstance);
                        //matrices.Add(meshFilters[i].transform.localToWorldMatrix);
                        i++;

                        if (meshVertsCount > ManagerSettings.trainglesTreesMax)
                        {
                            GameObject parent = new GameObject(terrain.name + "_Tree_Batch_" +
                                                               data.treePrototypes[tID].prefab.name + "_" + id);
                            parent.transform.position = terrain.transform.position;
                            treesBatchList.Add(parent);

                            MeshFilter filter = parent.AddComponent<MeshFilter>();

                            Mesh combinedAllMesh = new Mesh();
                            if (meshVertsCount > 65000)
                                combinedAllMesh.indexFormat = IndexFormat.UInt32;

                            combinedAllMesh.CombineMeshes(combine.ToArray(), true, true);
                            filter.sharedMesh = combinedAllMesh;

                            name = ManagerSettings.terrainPath + filter.name + "_" + id + ".asset";
                            pathMesh = "Assets/" + name;
                            AssetDatabase.CreateAsset(filter.sharedMesh, pathMesh);

                            MeshRenderer meshRenderer = parent.AddComponent<MeshRenderer>();

                            meshRenderer.sharedMaterial = treeMaterial;
                            Undo.RegisterCreatedObjectUndo(parent, "Create object trees");
                            id++;
                            combine.Clear();
                            meshVertsCount = 0;
                        }
                    }

                    if (meshVertsCount > 0)
                    {
                        GameObject parent = new GameObject(terrain.name + "_Tree_Batch_" +
                                                           data.treePrototypes[tID].prefab.name + "_" + id);
                        parent.transform.position = terrain.transform.position;
                        treesBatchList.Add(parent);

                        MeshFilter filter = parent.AddComponent<MeshFilter>();

                        Mesh combinedAllMesh = new Mesh();
                        if (meshVertsCount > 65000)
                            combinedAllMesh.indexFormat = IndexFormat.UInt32;

                        combinedAllMesh.CombineMeshes(combine.ToArray(), true, true);
                        filter.sharedMesh = combinedAllMesh;

                        name = ManagerSettings.terrainPath + filter.name + "_" + id + ".asset";
                        pathMesh = "Assets/" + name;
                        AssetDatabase.CreateAsset(filter.sharedMesh, pathMesh);


                        MeshRenderer meshRenderer = parent.AddComponent<MeshRenderer>();

                        // Debug.Log(trees[tID].name + " " + trees[tID].GetComponent<MeshRenderer>().sharedMaterial.name);
                        meshRenderer.sharedMaterial = treeMaterial;
                        Undo.RegisterCreatedObjectUndo(parent, "Create object trees");
                    }

                    i = 0;
                    while (i < meshFilters.Count)
                    {
                        DestroyImmediate(meshFilters[i].gameObject);
                        i++;
                    }
                }
                else if (trees[tID] != null)
                {
                    Debug.Log($"Tree {trees[tID].name} is incompatible with tree exporter");
                }
            }

            AssetDatabase.SaveAssets();
            Physics.queriesHitBackfaces = backFace;
            return treesBatchList;
        }
    }
}