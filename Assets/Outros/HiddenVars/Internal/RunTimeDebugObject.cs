//  HiddenVars - RunTimeDebugObject

//  This class only exists when running application in Unity Editor.


#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Leguar.HiddenVars.Internal {

	public class RunTimeDebugObject : MonoBehaviour {

		public const string OBJECT_NAME="HiddenVars EditorOnly RunTimeDebug";

		private static RunTimeDebugObject instance;
		private static bool destroyedByUser=false;
		private static int nextId=1;

		internal static RunTimeDebugObject getOrCreateInstance() {
			if (destroyedByUser) {
				return null;
			}
			if (instance==null) {
				GameObject go=new GameObject(OBJECT_NAME);
				GameObject.DontDestroyOnLoad(go);
				go.hideFlags = HideFlags.HideInHierarchy;
				instance=go.AddComponent<RunTimeDebugObject>();
			}
			return instance;
		}

		internal static RunTimeDebugObject getInstance() {
			return instance;
		}

		internal static bool isDestroyedByUser() {
			return destroyedByUser;
		}

		private Dictionary<int,RunTimeDebugContent> debugContents;
		private bool showAllItems;
		private bool needRefresh;

		void Awake() {
			debugContents=new Dictionary<int,RunTimeDebugContent>();
			showAllItems=false;
			needRefresh=false;
		}

		void Update() {
			if (needRefresh) {
				EditorUtility.SetDirty(this);
				needRefresh=false;
			}
		}

		// If this game object gets destroyed (for example deleted from hierarchy during runtime), not creating new one as debug data wouldn't match real data
		void OnDestroy() {
			destroyedByUser=true;
			instance=null;
		}

		public bool isShowAllItems() {
			return showAllItems;
		}

		public void setShowAllItems() {
			showAllItems=true;
			needRefresh=true;
		}

		public Dictionary<int,RunTimeDebugContent> getContentForDebug() {
			return debugContents;
		}

		internal int addDebugDictionary(string debugName) {
			int debugId=nextId++;
			if (debugName==null) {
				debugName="Unnamed "+debugId;
			}
			debugContents[debugId]=new RunTimeDebugContent(debugName);
			needRefresh=true;
			return debugId;
		}

		internal void setDebugDictionaryValue(int debugId, string key, object value) {
			debugContents[debugId].getContent()[key]=value;
			needRefresh=true;
		}

		internal void removeDebugDictionaryValue(int debugId, string key) {
			debugContents[debugId].getContent().Remove(key);
			needRefresh=true;
		}

		internal void clearDebugDictionary(int debugId) {
			debugContents[debugId].getContent().Clear();
			needRefresh=true;
		}

		internal void removeDebugDictionary(int debugId) {
			debugContents.Remove(debugId);
			needRefresh=true;
		}

	}

}

#endif
