//  HiddenVars - RunTimeDebug Editor

//  THis class is obsolete and replcade by RunTimeDebugWindow


using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Leguar.HiddenVars.Internal {

	[CustomEditor(typeof(RunTimeDebugObject))]
	public class RunTimeDebugEditor : Editor {

		public override void OnInspectorGUI() {
			GUILayout.Space(5);
			RunTimeDebugObject runTimeDebugObject=(RunTimeDebugObject)(this.target); // There always should be only one object
			RunTimeDebugWindow.drawContent(runTimeDebugObject,null);
		}
		
	}

}
