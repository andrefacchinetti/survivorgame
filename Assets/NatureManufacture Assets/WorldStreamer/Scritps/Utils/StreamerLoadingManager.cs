using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WorldStreamer2
{
    public class StreamerLoadingManager
    {
        private enum SceneType
        {
            Scene,
            SceneSplit
        }

        private struct SceneToLoad
        {
            public SceneType SceneType;
            public string SceneName;
            public SceneSplit SceneSplit;
        }

        public Streamer Streamer;

        private List<Scene> _scenesToUnload = new();


        public int ScenesToUnloadCount => _scenesToUnload.Count;
        private List<SceneToLoad> _scenesToLoad = new();
        public int ScenesToLoadCount => _scenesToLoad.Count;
        private List<AsyncOperation> _asyncOperations = new();
        public int AsyncOperationsCount => _asyncOperations.Count;

        private LoadingState _loadingState = LoadingState.Loading;


        private enum LoadingState
        {
            Loading,
            Unloading
        }

        private bool _operationStarted;

        public void Update()
        {
            //Debug.Log($"_operationStarted {_operationStarted}");
            if (_operationStarted)
                return;

            //Debug.Log($"_loadingState {_loadingState}");

            //Debug.Log(_asyncOperations.Count);
            if (_asyncOperations.Count > 0)
                return;

            //Debug.Log($"_loadingState {_loadingState} {_scenesToLoad.Count} {_scenesToUnload.Count} {_asyncOperations.Count}");

            if (_loadingState == LoadingState.Unloading)
            {
                if (_scenesToLoad.Count > 0)
                {
                    _loadingState = LoadingState.Loading;
                }
                else if (_scenesToUnload.Count > 0)
                {
                    // Debug.Log("scenesToUnload " + scenesToUnload.Count + " " + asyncOperations.Count);
                    _operationStarted = true;
                    Streamer.StartCoroutine(UnloadAsync());
                    //Unload();
                }
            }

            if (_loadingState == LoadingState.Loading)
            {
                if (_scenesToLoad.Count > 0)
                {
                    // Debug.Log("scenesToLoad " + scenesToLoad.Count + " " + asyncOperations.Count);
                    _operationStarted = true;
                    //Load();
                    Streamer.StartCoroutine(LoadAsync());
                }

                if (_scenesToLoad.Count == 0)
                    _loadingState = LoadingState.Unloading;
            }
        }


        private IEnumerator LoadAsync()
        {
            // yield return new WaitForSeconds(0.5f);

            //Debug.Log("scenesToLoad " + scenesToLoad.Count);
            for (int i = 0; i < _scenesToLoad.Count; i++)
            {
                int sceneID = SceneManager.sceneCount;

                AsyncOperation asyncOperation;
                if (_scenesToLoad[i].SceneType == SceneType.SceneSplit)
                {
                    SceneSplit split = _scenesToLoad[i].SceneSplit;
                    asyncOperation = SceneManager.LoadSceneAsync(split.sceneName, LoadSceneMode.Additive);

                    asyncOperation.completed += (operation) =>
                    {
                        SceneLoadComplete(sceneID, split);
                        OnOperationDone(operation);
                    };
                }
                else
                {
                    asyncOperation = SceneManager.LoadSceneAsync(_scenesToLoad[i].SceneName, LoadSceneMode.Additive);
                    asyncOperation.completed += OnOperationDone;
                }


                _asyncOperations.Add(asyncOperation);
                yield return null;
            }

            _scenesToLoad.Clear();
            //Debug.Log("Load finished " + asyncOperations.Count);
            _operationStarted = false;
        }


        private void SceneLoadComplete(int sceneID, SceneSplit split)
        {
            //Debug.Log(SceneManager.GetSceneAt(sceneID).name);

            Streamer.StartCoroutine(SceneLoadCompleteAsync(sceneID, split));
        }

        private IEnumerator SceneLoadCompleteAsync(int sceneID, SceneSplit split)
        {
            yield return null;
            Streamer.OnSceneLoaded(SceneManager.GetSceneAt(sceneID), split);
        }

        private void OnOperationDone(AsyncOperation asyncOperation)
        {
            //Debug.Log($"onOperationDone {asyncOperation.isDone} {_asyncOperations.Count}");
            Streamer.StartCoroutine(RemoveAsyncOperation(asyncOperation));
        }

        private IEnumerator RemoveAsyncOperation(AsyncOperation asyncOperation)
        {
            yield return null;
            yield return null;
            _asyncOperations.Remove(asyncOperation);
        }


        private IEnumerator UnloadAsync()
        {
            yield return null;

            //Debug.Log(asyncOperations.Count);
            for (int i = 0; i < _scenesToUnload.Count; i++)
            {
                _scenesToUnload[i].GetRootGameObjects()[0].name = $" (Unloading) {_scenesToUnload[i].GetRootGameObjects()[0].name}";
                AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(_scenesToUnload[i]);
                asyncOperation.completed += OnOperationDone;
                _asyncOperations.Add(asyncOperation);
                //Debug.Log($" UnloadAsync {_scenesToUnload[i].name} {_asyncOperations.Count}");

                yield return null;
            }

            _scenesToUnload.Clear();


            //Debug.Log("Unload finished " + _asyncOperations.Count);
            _operationStarted = false;
        }

        public void UnloadSceneAsync(Scene scene)
        {
            //Debug.Log($" UnloadSceneAsync {scene.name}");
            _scenesToUnload.Add(scene);
        }

        public void LoadSceneAsync(SceneSplit split)
        {
            //Debug.Log($" LoadSceneAsync {split.sceneName} ");
            _scenesToLoad.Add(new SceneToLoad() {SceneType = SceneType.SceneSplit, SceneSplit = split});
        }

        public void LoadSceneAsync(string sceneName)
        {
            //Debug.Log($" LoadSceneAsync sceneName {sceneName} ");
            _scenesToLoad.Add(new SceneToLoad() {SceneType = SceneType.Scene, SceneName = sceneName});
        }
    }
}