//  HiddenVars - RunTimeDebug Window


using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Leguar.HiddenVars.Internal {

	public class RunTimeDebugWindow : EditorWindow {

		private static GUIStyle titleStyle;

		private Vector2 scrollPosition=Vector2.zero;
		private string filterText="";

		[MenuItem("Window/HiddenVars Runtime")]
		static void Init() {
			RunTimeDebugWindow window=(RunTimeDebugWindow)(GetWindow(typeof(RunTimeDebugWindow)));
#if UNITY_5 || UNITY_2017
			window.titleContent = new GUIContent("HiddenVars");
#elif UNITY_2018_1_OR_NEWER
			Texture2D icon = (Texture2D)(AssetDatabase.LoadAssetAtPath("Assets/HiddenVars/Internal/Editor/window-icon.png", typeof(Texture2D)));
			window.titleContent = new GUIContent("HiddenVars Runtime",icon);
#else
			window.title = "HiddenVars";
#endif
		}

		void OnInspectorUpdate() {
			if (Application.isPlaying) {
				this.Repaint();
			}
		}

		void OnGUI() {

			GUILayout.Space(10);

			if (!Application.isPlaying) {
				EditorGUILayout.LabelField("Application is not running.");
				return;
			}

			RunTimeDebugObject runTimeDebugObject = null;
			GameObject runTimeDebugGameObject = GameObject.Find(RunTimeDebugObject.OBJECT_NAME);
			if (runTimeDebugGameObject != null) {
				runTimeDebugObject = runTimeDebugGameObject.GetComponent<RunTimeDebugObject>();
			}
			if (runTimeDebugObject == null) {
				EditorGUILayout.LabelField("No active HiddenVars instances");
				return;
			}

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Filter by name:");
			filterText=GUILayout.TextField(filterText,GUILayout.MinWidth(150f));
			EditorGUILayout.EndHorizontal();

			scrollPosition=EditorGUILayout.BeginScrollView(scrollPosition);
			drawContent(runTimeDebugObject,(string.IsNullOrEmpty(filterText)?null:filterText.ToLower()));
			EditorGUILayout.EndScrollView();

		}

		public static void drawContent(RunTimeDebugObject runTimeDebugObject, string filter) {
			
			if (titleStyle == null) {
				titleStyle = new GUIStyle();
				titleStyle.fontStyle = FontStyle.Bold;
			}

			Dictionary<int,RunTimeDebugContent> allContent=runTimeDebugObject.getContentForDebug();

			EditorGUILayout.Separator();

			int instanceCount=allContent.Count;
			if (instanceCount==0) {
				EditorGUILayout.LabelField("No active HiddenVars instances");
				return;
			}

			List<int> instancesToShow = new List<int>();
			foreach (int debugId in allContent.Keys) {
				if (filter != null) {
					string name = allContent[debugId].getName();
					if (filter != null) {
						if (!name.ToLower().Contains(filter)) {
							continue;
						}
					}
				}
				instancesToShow.Add(debugId);
			}
			int filteredInstanceCount=instancesToShow.Count;

			EditorGUILayout.LabelField("Active HiddenVars instances: "+instanceCount);
			if (filter != null) {
				if (filteredInstanceCount == 0) {
					EditorGUILayout.LabelField("(none matching the filter)");
				} else if (filteredInstanceCount < instanceCount) {
					EditorGUILayout.LabelField("(showing "+filteredInstanceCount+" matching the filter)");
				} else {
					EditorGUILayout.LabelField("(all matches the filter)");
				}
			}

			bool showAllItems=runTimeDebugObject.isShowAllItems();
			int outputInstanceCount=0;
			int outputRowCount=0;

			foreach (int debugId in instancesToShow) {
				if (!showAllItems && (outputInstanceCount>=150 || outputRowCount>=1500)) {
					break;
				}
				outputInstanceCount++;
				EditorGUILayout.Separator();
				Dictionary<string,object> contentDict=allContent[debugId].getContent();
				int count=contentDict.Count;
				EditorGUILayout.LabelField(allContent[debugId].getName()+" ("+count+" value"+(count!=1?"s":"")+")",titleStyle);
				outputRowCount++;
				foreach (string key in contentDict.Keys) {
					object value=contentDict[key];
					EditorGUILayout.LabelField(getKeyString(key),getValueString(value));
					outputRowCount++;
				}
			}

			if (outputInstanceCount<filteredInstanceCount) {
				EditorGUILayout.Separator();
				if (GUILayout.Button("Show all instances ("+(filteredInstanceCount-outputInstanceCount)+" more)")) {
					runTimeDebugObject.setShowAllItems();
				}
			}

		}

		private static string getKeyString(string key) {
			if (key==null) {
				return "(null key)"; // This should not happen
			}
			if (key.Length==0) {
				return "(empty key string)";
			}
			return key;
		}

		private static string getValueString(object value) {
			if (value==null) {
				return "(null value)"; // This should not happen
			}
			if (value is int) {
				return (value.ToString()+" (int)");
			}
			if (value is long) {
				return (value.ToString()+" (long)");
			}
			if (value is float) {
				return (value.ToString()+" (float)");
			}
			if (value is double) {
				return (value.ToString()+" (double)");
			}
			if (value is bool) {
				return (value.ToString()+" (bool)");
			}
			if (value is string) {
				string str=(string)(value);
				int len=str.Length;
				if (len==0) {
					return "(empty string)";
				}
				StringBuilder sb=new StringBuilder();
				if (len<=40) {
					sb.Append(str);
				} else {
					sb.Append(str,0,25).Append(" ... ").Append(str,len-10,10);
				}
				sb.Append(" (").Append(len).Append(" char");
				if (len!=1) {
					sb.Append('s');
				}
				sb.Append(')');
				return sb.ToString();
			}
			if (value is byte[]) {
				byte[] array=(byte[])(value);
				int len=array.Length;
				if (len==0) {
					return "(empty byte array)";
				}
				StringBuilder sb=new StringBuilder();
				if (len<=20) {
					for (int n=0; n<len; n++) {
						sb.Append(array[n]);
						if (n<len-1) {
							sb.Append(',');
						}
					}
				} else {
					for (int n=0; n<10; n++) {
						sb.Append(array[n]).Append(',');
					}
					sb.Append(" ... ");
					for (int n=len-5; n<len; n++) {
						sb.Append(array[n]);
						if (n<len-1) {
							sb.Append(',');
						}
					}
				}
				sb.Append(" (").Append(len).Append(" byte");
				if (len!=1) {
					sb.Append('s');
				}
				sb.Append(')');
				return sb.ToString();
			}
			if (value is int[]) {
				int[] array=(int[])(value);
				int len=array.Length;
				if (len==0) {
					return "(empty int array)";
				}
				StringBuilder sb=new StringBuilder();
				if (len<=15) {
					for (int n=0; n<len; n++) {
						sb.Append(array[n]);
						if (n<len-1) {
							sb.Append(',');
						}
					}
				} else {
					for (int n=0; n<7; n++) {
						sb.Append(array[n]).Append(',');
					}
					sb.Append(" ... ");
					for (int n=len-3; n<len; n++) {
						sb.Append(array[n]);
						if (n<len-1) {
							sb.Append(',');
						}
					}
				}
				sb.Append(" (").Append(len).Append(" int");
				if (len!=1) {
					sb.Append('s');
				}
				sb.Append(')');
				return sb.ToString();
			}
			if (value is long[]) {
				long[] array=(long[])(value);
				int len=array.Length;
				if (len==0) {
					return "(empty long array)";
				}
				StringBuilder sb=new StringBuilder();
				if (len<=5) {
					for (int n=0; n<len; n++) {
						sb.Append(array[n]);
						if (n<len-1) {
							sb.Append(',');
						}
					}
				} else {
					for (int n=0; n<3; n++) {
						sb.Append(array[n]).Append(',');
					}
					sb.Append(" ... ");
					for (int n=len-1; n<len; n++) {
						sb.Append(array[n]);
						if (n<len-1) {
							sb.Append(',');
						}
					}
				}
				sb.Append(" (").Append(len).Append(" long");
				if (len!=1) {
					sb.Append('s');
				}
				sb.Append(')');
				return sb.ToString();
			}
			if (value is float[]) {
				float[] array=(float[])(value);
				int len=array.Length;
				if (len==0) {
					return "(empty float array)";
				}
				StringBuilder sb=new StringBuilder();
				if (len<=10) {
					for (int n=0; n<len; n++) {
						sb.Append(array[n]);
						if (n<len-1) {
							sb.Append(',');
						}
					}
				} else {
					for (int n=0; n<5; n++) {
						sb.Append(array[n]).Append(',');
					}
					sb.Append(" ... ");
					for (int n=len-3; n<len; n++) {
						sb.Append(array[n]);
						if (n<len-1) {
							sb.Append(',');
						}
					}
				}
				sb.Append(" (").Append(len).Append(" float");
				if (len!=1) {
					sb.Append('s');
				}
				sb.Append(')');
				return sb.ToString();
			}
			if (value is double[]) {
				double[] array=(double[])(value);
				int len=array.Length;
				if (len==0) {
					return "(empty double array)";
				}
				StringBuilder sb=new StringBuilder();
				if (len<=5) {
					for (int n=0; n<len; n++) {
						sb.Append(array[n]);
						if (n<len-1) {
							sb.Append(',');
						}
					}
				} else {
					for (int n=0; n<3; n++) {
						sb.Append(array[n]).Append(',');
					}
					sb.Append(" ... ");
					for (int n=len-1; n<len; n++) {
						sb.Append(array[n]);
						if (n<len-1) {
							sb.Append(',');
						}
					}
				}
				sb.Append(" (").Append(len).Append(" double");
				if (len!=1) {
					sb.Append('s');
				}
				sb.Append(')');
				return sb.ToString();
			}
			if (value is bool[]) {
				bool[] array=(bool[])(value);
				int len=array.Length;
				if (len==0) {
					return "(empty bool array)";
				}
				StringBuilder sb=new StringBuilder();
				if (len<=20) {
					for (int n=0; n<len; n++) {
						sb.Append(array[n]);
						if (n<len-1) {
							sb.Append(',');
						}
					}
				} else {
					for (int n=0; n<10; n++) {
						sb.Append(array[n]).Append(',');
					}
					sb.Append(" ... ");
					for (int n=len-5; n<len; n++) {
						sb.Append(array[n]);
						if (n<len-1) {
							sb.Append(',');
						}
					}
				}
				sb.Append(" (").Append(len).Append(" bool");
				if (len!=1) {
					sb.Append('s');
				}
				sb.Append(')');
				return sb.ToString();
			}
			return ("Unknown value type: "+value.GetType().ToString());
		}
		
	}

}
