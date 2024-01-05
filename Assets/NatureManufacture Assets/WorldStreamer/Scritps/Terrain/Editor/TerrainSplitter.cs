// /**
//  * Created by Pawel Homenko on  11/2023
//  */

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.TerrainTools;

namespace WorldStreamer2
{
    public static class TerrainSplitter
    {
        private const float SplitProgressParts = 5f;
        private static int _managerSettingsSplitSize;
        private static TerrainManagerSettings _terrainManagerSettings;
        private static string _progressCaption;
        private static float _progressTerrains;
        private static string _progressTextTerrainCount;

        //enum type for painting on terrain
        private enum PaintingType
        {
            Heightmap,
            Hole,
            Texture,
        }


        public static void SplitTerrain(TerrainManagerSettings managerSettings, bool allTerrains = false)
        {
            _terrainManagerSettings = managerSettings;
            CreateDirectoryIfNotExists();
            Undo.SetCurrentGroupName("Split Terrain");
            _managerSettingsSplitSize = _terrainManagerSettings.splitSize;

            _terrainManagerSettings.terrainsCount = _managerSettingsSplitSize * _managerSettingsSplitSize;
            Terrain[] terrains = GetTerrains(allTerrains);
            ProcessTerrains(terrains);
            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        private static Terrain[] GetTerrains(bool allTerrains)
        {
            return allTerrains ? Terrain.activeTerrains : Selection.GetFiltered<Terrain>(SelectionMode.TopLevel);
        }

        private static void CreateDirectoryIfNotExists()
        {
            string path = "Assets/" + _terrainManagerSettings.terrainsDataPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static void ProcessTerrains(IReadOnlyCollection<Terrain> terrains)
        {
            int currentObjectIndex = 0;
            foreach (var terrain in terrains)
            {
                currentObjectIndex++;
                if (terrain == null) continue;

                TerrainData terrainData = terrain.terrainData;
                _progressCaption = $"Splitting terrain {terrain.name} ({currentObjectIndex} of {terrains.Count})";

                for (int i = 0; i < _terrainManagerSettings.terrainsCount; i++)
                {
                    ProcessEachTerrainSplit(i, terrain, terrainData);
                }

                AssetDatabase.SaveAssets();

                EditorUtility.ClearProgressBar();
            }
        }

        private static void ProcessEachTerrainSplit(int i, Terrain terrain, TerrainData terrainData)
        {
            int xI = (i % _managerSettingsSplitSize);
            int yI = (i / _managerSettingsSplitSize);

            _progressTextTerrainCount = $"Generating terrain split {i}/terrainsCount";
            _progressTerrains = i / (float)_terrainManagerSettings.terrainsCount;

            EditorUtility.DisplayProgressBar(_progressCaption, _progressTextTerrainCount, _progressTerrains);

            var newTerrainData = CreateNewTerrain(terrain, terrainData, i, xI, yI, out Terrain newTerrain);

            GetTerrainBrushTransform(terrain, terrainData, xI, yI, out var mat, out var brushXForm, out var brushChunkXForm);

            //if (ProcessTerrainHeight(terrainData, xI, yI, newTerrainData, out int startX, out int startY)) return;
            if (ProcessTerrainHeightGPU(terrain, terrainData, newTerrain, mat, brushXForm, brushChunkXForm)) return;

            if (ProcessTerrainHoleGPU(terrain, terrainData, newTerrain, mat, brushXForm, brushChunkXForm)) return;

            //if (ProcessTerrainHole(terrainData, 0, 0, xI, yI, newTerrainData)) return;

            if (ProcessTerrainAlphamapsGPU(terrain, terrainData, newTerrain, mat, brushXForm, brushChunkXForm)) return;

            if (ProcessTerrainDetails(terrainData, newTerrainData, xI, yI)) return;

            if (ProcessTerrainTrees(terrainData, xI, yI, _progressTextTerrainCount, newTerrainData)) return;


            terrain.gameObject.SetActive(false);

            GC.Collect();
        }

        private static TerrainData CreateNewTerrain(Terrain terrain, TerrainData terrainData, int i, int xI, int yI, out Terrain newTerrain)
        {
            TerrainData newTerrainData = new TerrainData
            {
                name = $"{terrain.name} {xI}_{yI}"
            };
            GameObject newTerrainGo = Terrain.CreateTerrainGameObject(newTerrainData);

            RegisterCreatedObjects(newTerrainData, newTerrainGo);

            newTerrainGo.name = $"{terrain.name} {xI}_{yI}";

            newTerrain = AssignTerrainData(newTerrainGo, newTerrainData);

            TerrainSettingsSetter.SetTerrainSettings(newTerrain, _terrainManagerSettings);

            SetNewTerrainPosition(i, terrain, terrainData, newTerrainGo);

            newTerrain.materialTemplate = terrain.materialTemplate;

            CreateAssetForNewTerrain(newTerrain, _terrainManagerSettings, newTerrainData);

            SetTerrainBaseData(terrainData, newTerrainData);
            return newTerrainData;
        }


        private static bool ProcessTerrainHoleGPU(Terrain terrain, TerrainData terrainData, Terrain newTerrain, Material mat, BrushTransform brushXForm, BrushTransform brushChunkXForm)
        {
            PaintTerrain(terrain, newTerrain, mat, brushXForm, brushChunkXForm, PaintingType.Hole);

            return false;
        }

        private static bool ProcessTerrainHeightGPU(Terrain terrain, TerrainData terrainData, Terrain newTerrain, Material mat, BrushTransform brushXForm, BrushTransform brushChunkXForm)
        {
            PaintTerrain(terrain, newTerrain, mat, brushXForm, brushChunkXForm, PaintingType.Heightmap);

            return false;
        }


        private static bool ProcessTerrainAlphamapsGPU(Terrain terrain, TerrainData terrainData, Terrain newTerrain, Material mat, BrushTransform brushXForm, BrushTransform brushChunkXForm)
        {
            int alphamapId = 0;
            foreach (var terrainLayer in terrainData.terrainLayers)
            {
                PaintTerrain(terrain, newTerrain, mat, brushXForm, brushChunkXForm, PaintingType.Texture, terrainLayer);

                string info = $"{_progressTextTerrainCount} (Processing splat {alphamapId})";
                float currentProgress = (alphamapId / (float)terrainData.alphamapLayers);
                if (ShowCancelableProgressBar(info, 2, currentProgress)) return true;
                alphamapId++;
            }

            return false;
        }

        private static void PaintTerrain(Terrain terrain, Terrain newTerrain, Material mat, BrushTransform brushXForm, BrushTransform brushChunkXForm, PaintingType paintingType, TerrainLayer terrainLayer = null)
        {
            PaintContext paintContext = BeginPaint(terrain, brushXForm, paintingType, terrainLayer);

            TerrainPaintUtility.SetupTerrainToolMaterialProperties(paintContext, brushXForm, mat);
            RenderTexture temp = RenderTexture.GetTemporary(paintContext.sourceRenderTexture.descriptor);

            Graphics.Blit(paintContext.sourceRenderTexture, temp);
            Graphics.Blit(paintContext.sourceRenderTexture, paintContext.destinationRenderTexture);

            EndPainting(paintingType, paintContext);

            paintContext = BeginPaint(newTerrain, brushChunkXForm, paintingType, terrainLayer);


            TerrainPaintUtility.SetupTerrainToolMaterialProperties(paintContext, brushChunkXForm, mat);

            Graphics.Blit(temp, paintContext.destinationRenderTexture);

            EndPainting(paintingType, paintContext);

            RenderTexture.ReleaseTemporary(temp);
        }

        private static void EndPainting(PaintingType paintingType, PaintContext paintContext)
        {
            switch (paintingType)
            {
                case PaintingType.Heightmap:
                    TerrainPaintUtility.EndPaintHeightmap(paintContext, "Terrain Paint");
                    break;
                case PaintingType.Hole:
                    TerrainPaintUtility.EndPaintHoles(paintContext, "Terrain Paint");
                    break;
                default:
                    TerrainPaintUtility.EndPaintTexture(paintContext, "Terrain Paint");
                    break;
            }
        }

        private static PaintContext BeginPaint(Terrain terrain, BrushTransform brushXForm, PaintingType paintingType, TerrainLayer terrainLayer = null)
        {
            PaintContext paintContext;
            switch (paintingType)
            {
                case PaintingType.Heightmap:
                    paintContext = TerrainPaintUtility.BeginPaintHeightmap(terrain, brushXForm.GetBrushXYBounds(), 0, false);
                    break;
                case PaintingType.Hole:
                    paintContext = TerrainPaintUtility.BeginPaintHoles(terrain, brushXForm.GetBrushXYBounds(), 0, false);
                    break;
                default:
                    paintContext = TerrainPaintUtility.BeginPaintTexture(terrain, brushXForm.GetBrushXYBounds(), terrainLayer, 0, false);
                    break;
            }

            return paintContext;
        }

        private static void GetTerrainBrushTransform(Terrain terrain, TerrainData terrainData, int xI, int yI, out Material mat, out BrushTransform brushXForm, out BrushTransform brushChunkXForm)
        {
            mat = TerrainPaintUtility.GetBuiltinPaintMaterial();

            Vector3 terrainPosition = Vector3.zero; // terrain.GetPosition();
            Vector3 terrainSizeChunk = terrainData.size / _managerSettingsSplitSize;

            Vector2 chunkSize = new(terrainSizeChunk.x, terrainSizeChunk.z);


            Vector2 brushBasePosition = new(terrainPosition.x, terrainPosition.z);
            Vector2 brushPosition = new Vector2(terrainPosition.x, terrainPosition.z) + new Vector2(yI * terrainSizeChunk.x, xI * terrainSizeChunk.z);

            Rect brushRect = new(brushPosition, chunkSize);
            brushXForm = BrushTransform.FromRect(brushRect);

            Rect brushChunkRect = new(brushBasePosition, chunkSize);
            brushChunkXForm = BrushTransform.FromRect(brushChunkRect);
        }


        private static bool ProcessTerrainDetails(TerrainData terrainData, TerrainData newTerrainData, int xI, int yI)
        {
            int startX;
            int startY;
            newTerrainData.SetDetailResolution(terrainData.detailResolution / _managerSettingsSplitSize, terrainData.detailResolutionPerPatch);

            int detailShift = terrainData.detailResolution / _managerSettingsSplitSize;

            for (int d = 0; d < terrainData.detailPrototypes.Length; d++)
            {
                string info = $"{_progressTextTerrainCount} (Processing detail {d})";
                float currentProgress = (d / (float)terrainData.detailPrototypes.Length);
                if (ShowCancelableProgressBar(info, 3, currentProgress)) return true;

                int[,] terrainDetail = terrainData.GetDetailLayer(0, 0,
                    terrainData.detailResolution, terrainData.detailResolution, d);

                int[,] partDetail = new int[detailShift, detailShift];


                startX = startY = 0;


                for (int x = startX; x < detailShift; x++)
                {
                    for (int y = startY; y < detailShift; y++)
                    {
                        int detail = terrainDetail[x + detailShift * xI, y + detailShift * yI];
                        partDetail[x, y] = detail;
                    }
                }

                newTerrainData.SetDetailLayer(0, 0, d, partDetail);
            }

            return false;
        }

        private static void SetTerrainBaseData(TerrainData terrainData, TerrainData newTerrainData)
        {
#if !UNITY_2021
            newTerrainData.SetDetailScatterMode(terrainData.detailScatterMode);
#endif

            newTerrainData.terrainLayers = terrainData.terrainLayers;
            newTerrainData.detailPrototypes = terrainData.detailPrototypes;
            newTerrainData.treePrototypes = terrainData.treePrototypes;

            newTerrainData.heightmapResolution = terrainData.heightmapResolution / _managerSettingsSplitSize;

            newTerrainData.size = new Vector3(terrainData.size.x / _managerSettingsSplitSize, terrainData.size.y, terrainData.size.z / _managerSettingsSplitSize);
        }

        private static void SetNewTerrainPosition(int i, Terrain terrain, TerrainData terrainData, GameObject newTerrainGo)
        {
            float spaceShiftX = terrainData.size.z / _managerSettingsSplitSize;
            float spaceShiftY = terrainData.size.x / _managerSettingsSplitSize;

            float xWShift = (i % _managerSettingsSplitSize) * spaceShiftX;
            // ReSharper disable once PossibleLossOfFraction
            float zWShift = (i / _managerSettingsSplitSize) * spaceShiftY;

            Vector3 parentPosition = terrain.GetPosition();
            var position = newTerrainGo.transform.position;
            position = parentPosition + new Vector3(position.x + zWShift, position.y, position.z + xWShift);
            newTerrainGo.transform.position = position;
        }


        private static bool ProcessTerrainTrees(TerrainData terrainData, int xI, int yI, string progressText, TerrainData newTerrainData)
        {
            float size = 1 / (float)_managerSettingsSplitSize;
            float sizeCheckX = size * xI;
            float sizeCheckX1 = size * (xI + 1);

            float sizeCheckY = size * yI;
            float sizeCheckY1 = size * (yI + 1);


            List<TreeInstance> treeInstancesList = new();

            TreeInstance ti;
            TreeInstance[] treeInstances = terrainData.treeInstances;

            for (int t = 0; t < treeInstances.Length; t++)
            {
                if (t % 1000 == 0)
                {
                    string info = $"{progressText} (Processing trees {t}/{treeInstances.Length})";
                    float currentProgress = ((float)t / treeInstances.Length);
                    if (ShowCancelableProgressBar(info, 4, currentProgress)) return true;
                }

                ti = treeInstances[t];

                if (!(ti.position.x >= sizeCheckY) || !(ti.position.x <= sizeCheckY1) ||
                    !(ti.position.z >= sizeCheckX) || !(ti.position.z <= sizeCheckX1)) continue;

                ti.position = new Vector3((ti.position.x * _managerSettingsSplitSize) % 1,
                    ti.position.y, (ti.position.z * _managerSettingsSplitSize) % 1);

                treeInstancesList.Add(ti);
            }

            //Debug.Log("end");

            newTerrainData.SetTreeInstances(treeInstancesList.ToArray(), true);
            return false;
        }

        private static bool ShowCancelableProgressBar(string info, int partNumber, float currentProgress)
        {
            float terrainsCountProgress = _progressTerrains + (partNumber / SplitProgressParts + currentProgress / SplitProgressParts) / _terrainManagerSettings.terrainsCount;

            bool cancel = EditorUtility.DisplayCancelableProgressBar(_progressCaption, info, terrainsCountProgress);

            if (!cancel) return false;

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
            EditorUtility.ClearProgressBar();
            return true;
        }

        private static void RegisterCreatedObjects(TerrainData td, GameObject tgo)
        {
            Undo.RegisterCreatedObjectUndo(td, "Create terrain data");
            Undo.RegisterCreatedObjectUndo(tgo, "Create terrain split");
        }

        private static Terrain AssignTerrainData(GameObject tgo, TerrainData td)
        {
            Terrain newTerrain = tgo.GetComponent<Terrain>();
            newTerrain.terrainData = td;

            newTerrain.gameObject.AddComponent<TerrainCullingSystem>();

            return newTerrain;
        }

        private static void CreateAssetForNewTerrain(Terrain terrain, TerrainManagerSettings managerSettings, TerrainData terrainData)
        {
            AssetDatabase.CreateAsset(terrainData, "Assets" + managerSettings.terrainsDataPath + terrain.name + ".asset");
        }
    }
}